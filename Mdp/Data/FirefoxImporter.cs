using firepwd.Cryptography;
using firepwd.net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mdp
{
    /// <summary>
    /// Class helper to retrieve data in firefox password vault
    /// </summary>
    public class FirefoxImporter
    {
        public static List<MyPassword> Import()
        {
            bool loginsFound = false;
            bool signonsFound = false;
            byte[] privateKey = new byte[24];
            string signonsFile = string.Empty, loginsFile = string.Empty;
            string filePath = string.Empty;
            DBHelper dbh = new DBHelper();
            Converts conv = new Converts();
            DataTable dt = new DataTable();
            Asn1Der asn = new Asn1Der();
            List<MyPassword> lp = new List<MyPassword>();

            String profileDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles");
            List<string> dirs = Directory.GetDirectories(profileDir).ToList();

            foreach (string dir in dirs)
            {
                // Check if files exists
                String[] files = Directory.GetFiles(dir, "signons.sqlite");
                if (files.Length > 0)
                {
                    filePath = dir;
                    signonsFile = files[0];
                    signonsFound = true;
                }

                // find logins.json file
                files = Directory.GetFiles(dir, "logins.json");
                if (files.Length > 0)
                {
                    filePath = dir;
                    loginsFile = files[0];
                    loginsFound = true;
                }

                if (!loginsFound && !signonsFound)
                    continue;

                if (String.IsNullOrEmpty(filePath))
                    continue;

                // Check if files exists
                if (!File.Exists(Path.Combine(filePath, "key3.db")))
                    privateKey = dbh.CheckKey4DB(dir, String.Empty, false);
                else
                    privateKey = dbh.CheckKey3DB(dir, String.Empty, false);

                if (privateKey == null || privateKey.Length == 0)
                    continue;

                FFLogins ffLoginData;
                if (signonsFound)
                {
                    using (SQLiteConnection cnn = new SQLiteConnection("Data Source=" + Path.Combine(filePath, "signons.sqlite")))
                    {
                        cnn.Open();
                        SQLiteCommand mycommand = new SQLiteCommand(cnn);
                        mycommand.CommandText = "select hostname, encryptedUsername, encryptedPassword, guid, encType from moz_logins;";
                        SQLiteDataReader reader = mycommand.ExecuteReader();
                        dt.Load(reader);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        Asn1DerObject user = asn.Parse(Convert.FromBase64String(row["encryptedUsername"].ToString()));
                        Asn1DerObject pwd = asn.Parse(Convert.FromBase64String(row["encryptedPassword"].ToString()));
                        string hostname = row["hostname"].ToString();
                        string decryptedUser = TripleDESHelper.DESCBCDecryptor(privateKey, user.objects[0].objects[1].objects[1].Data, user.objects[0].objects[2].Data);
                        string decryptedPwd = TripleDESHelper.DESCBCDecryptor(privateKey, pwd.objects[0].objects[1].objects[1].Data, pwd.objects[0].objects[2].Data);

                        string username = Regex.Replace(decryptedUser, @"[^\u0020-\u007F]", "");
                        string password = Regex.Replace(decryptedPwd, @"[^\u0020-\u007F]", "");

                        lp.Add(new MyPassword { Data = hostname, UserName = username, Password = password.ToSecureString() });
                    }
                }
                if (loginsFound)
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(filePath, "logins.json")))
                    {
                        String json = sr.ReadToEnd();
                        ffLoginData = JsonConvert.DeserializeObject<FFLogins>(json);
                    }

                    foreach (LoginData loginData in ffLoginData.logins)
                    {
                        Asn1DerObject user = asn.Parse(Convert.FromBase64String(loginData.encryptedUsername));
                        Asn1DerObject pwd = asn.Parse(Convert.FromBase64String(loginData.encryptedPassword));
                        string hostname = loginData.hostname;
                        string decryptedUser = TripleDESHelper.DESCBCDecryptor(privateKey, user.objects[0].objects[1].objects[1].Data, user.objects[0].objects[2].Data);
                        string decryptedPwd = TripleDESHelper.DESCBCDecryptor(privateKey, pwd.objects[0].objects[1].objects[1].Data, pwd.objects[0].objects[2].Data);

                        string username = Regex.Replace(decryptedUser, @"[^\u0020-\u007F]", "");
                        string password = Regex.Replace(decryptedPwd, @"[^\u0020-\u007F]", "");

                        lp.Add(new MyPassword { Data = hostname, UserName = username, Password = password.ToSecureString() });
                    }
                }
            }

            return lp;
        }
    }
}
