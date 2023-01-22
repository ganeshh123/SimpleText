using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace SimpleText
{
    internal static class Program
    {

        static IDictionary<string, WindowEditor> openWindows = new Dictionary<string, WindowEditor>();
        static bool darkModeEnabled = (int) Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "1") == 0;
        static bool wordWrapEnabled = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*Application.Run(new WindowEditor());*/
            createWindowEditor();
            Application.Run();
        }

        // Window Create/Remove

        public static void createWindowEditor()
        {
            WindowEditor newWindowEditor= new WindowEditor();
            string newWindowId = newWindowEditor.getWindowId();

            openWindows.Add(newWindowId, newWindowEditor);
            Debug.WriteLine(newWindowEditor);
            newWindowEditor.Show();
        }

        public static void removeWindowEditor(string windowId)
        {
            openWindows.Remove(windowId);
            if(openWindows.Count < 1)
            {
                Application.Exit();
            }
        }


        // Shared Settings
        public static void setEdtiorWordWrap(bool enabled)
        {
            foreach(var oW in openWindows)
            {
                oW.Value.setWordWrap(enabled);
            }
        }

    }
}
