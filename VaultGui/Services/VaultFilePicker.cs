namespace VaultGui.Services;

/// <summary>
/// File pickers for CSV exports and data files of version 3.x.
/// </summary>
public static class VaultFilePicker
{
    private static readonly FilePickerFileType CsvFiles = new(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        [DevicePlatform.Android] = new[] { "text/csv", "text/comma-separated-values", "application/csv", "text/plain" },
        [DevicePlatform.WinUI] = new[] { ".csv" },
    });

    /// <summary>
    /// Pick a 3.x data file (SimpleDb.sqlite, or whatever it was renamed to: any file can be chosen)
    /// and return its content, or null if cancelled.
    /// </summary>
    public static async Task<byte[]?> PickLegacyAsync()
    {
        FileResult? file = await FilePicker.Default.PickAsync(new PickOptions());
        if (file is null)
            return null;

        await using Stream stream = await file.OpenReadAsync();
        using MemoryStream memory = new();
        await stream.CopyToAsync(memory);
        return memory.ToArray();
    }

    /// <summary>Pick a CSV export and return an open stream, or null if cancelled.</summary>
    public static async Task<Stream?> PickCsvAsync()
    {
        FileResult? file = await FilePicker.Default.PickAsync(new PickOptions { FileTypes = CsvFiles });
        return file is null ? null : await file.OpenReadAsync();
    }
}
