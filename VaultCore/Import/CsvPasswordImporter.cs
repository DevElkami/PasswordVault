using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using VaultCore.Models;

namespace VaultCore.Import;

/// <summary>
/// Reads password exports from browsers and password managers. The columns are detected from the header:
/// <list type="bullet">
/// <item>Firefox: url, username, password</item>
/// <item>Chrome / Edge: name, url, username, password, note</item>
/// <item>Bitwarden: name, login_uri, login_username, login_password</item>
/// </list>
/// </summary>
public static class CsvPasswordImporter
{
    private static readonly string[] UrlColumns = ["url", "login_uri", "uri", "website"];
    private static readonly string[] UserColumns = ["username", "login_username", "login", "user", "email"];
    private static readonly string[] PasswordColumns = ["password", "login_password"];
    private static readonly string[] NameColumns = ["name", "title"];

    /// <exception cref="InvalidDataException">No password column in the header.</exception>
    public static List<MyPassword> Read(Stream stream)
    {
        using StreamReader reader = new(stream);
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim().ToLowerInvariant(),
            MissingFieldFound = null,
            BadDataFound = null,
        };
        using CsvReader csv = new(reader, config);

        if (!csv.Read() || !csv.ReadHeader() || csv.HeaderRecord is null)
            return [];

        string[] header = [.. csv.HeaderRecord.Select(h => h.Trim().ToLowerInvariant())];
        int url = Find(header, UrlColumns);
        int user = Find(header, UserColumns);
        int password = Find(header, PasswordColumns);
        int name = Find(header, NameColumns);

        if (password < 0)
            throw new InvalidDataException("No password column found. Supported exports: Firefox, Chrome, Edge, Bitwarden.");

        List<MyPassword> result = [];
        while (csv.Read())
        {
            MyPassword entry = new()
            {
                Data = Field(csv, url),
                UserName = Field(csv, user),
                Password = Field(csv, password),
                Keyword = Field(csv, name),
            };

            if (entry.Password.Length > 0 || entry.UserName.Length > 0)
                result.Add(entry);
        }

        return result;
    }

    private static int Find(string[] header, string[] candidates)
    {
        foreach (string candidate in candidates)
        {
            int index = Array.IndexOf(header, candidate);
            if (index >= 0)
                return index;
        }
        return -1;
    }

    private static string Field(CsvReader csv, int index) =>
        index >= 0 && csv.TryGetField(index, out string? value) ? value ?? "" : "";
}
