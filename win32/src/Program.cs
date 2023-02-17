﻿using System;
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
            string[] args = Environment.GetCommandLineArgs();
            AppDelegate mainProcess = new AppDelegate();
            mainProcess.Run(args);
        }

    }
}
