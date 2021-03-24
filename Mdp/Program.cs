using System;
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
            Application.Run(new MainForm());
        }
    }
}
