using System;
using System.Windows.Forms;

namespace SimpleText
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] bootArgs = Environment.GetCommandLineArgs();
            AppDelegate mainProcess = new AppDelegate(bootArgs);
            mainProcess.Run(bootArgs);
        }

    }
}
