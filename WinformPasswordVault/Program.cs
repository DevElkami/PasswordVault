using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WinformPasswordVault
{
    static class Program
    {
        /// <summary>
        /// Main point entry
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                String directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DevElkami", nameof(VaultCore));
                FileTarget logfile = new("logfile")
                {
                    FileName = Path.Combine(directory, "log.txt"),
                    CreateDirs = true
                };

                logfile.Layout = Layout.FromString("${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}");
                LoggingConfiguration config = new();
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile, "*");
                LogManager.Configuration = config;
            }
            catch (Exception except)
            {
                Debug.WriteLine(except.ToString());
                // No log, don't care
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.SetCompatibleTextRenderingDefault(false);

                // Quick translate
#if DEBUG
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
#else
            if (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag != "fr-FR")
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
#endif

                // Run app
                Application.Run(new MainForm());
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
            }
        }
    }
}
