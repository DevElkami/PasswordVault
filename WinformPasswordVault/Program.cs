using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using ReaLTaiizor.Colors;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Manager;
using ReaLTaiizor.Util;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using VaultCore;

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
                CultureInfo culture = new CultureInfo("en");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
#else
                if (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag != "fr-FR")
                {
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
#endif
                // Theme                                
                MaterialSkinManager.Instance.EnforceBackcolorOnAllComponents = true;
                MaterialSkinManager.Instance.Theme = MaterialSkinManager.Themes.DARK;
                MaterialSkinManager.Instance.ColorScheme = new MaterialColorScheme(
                    ColorTranslator.FromHtml("0x37474F"),
                    MaterialSkinManager.Instance.BackdropColor/*ColorTranslator.FromHtml("0x263238")*/,
                    ColorTranslator.FromHtml("0x607D8B"),
                    Color.FromArgb(162, 101, 190),
                    MaterialTextShade.WHITE);

                // Run app
                MyVault myVault = new();
                //if (myVault.IsInitialized() == false)
                {
                    InitVaultForm initVaultForm = new(ref myVault);
                    if (initVaultForm.ShowDialog() != DialogResult.OK)
                        return;
                }

                FormAskPassword formAskPassword = new(ref myVault);
                if (formAskPassword.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(ref myVault));
                }
            }
            catch (Exception except)
            {
                LogManager.GetLogger(nameof(WinformPasswordVault)).Fatal(except.ToString());
            }
        }
    }
}
