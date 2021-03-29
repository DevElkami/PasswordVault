using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Mdp
{
    static class Program
    {
        /// <summary>
        /// Main point entry
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
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
    }
}
