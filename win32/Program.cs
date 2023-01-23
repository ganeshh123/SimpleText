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

        // Container for managing open windows
        static IDictionary<string, WindowEditor> openWindows = new Dictionary<string, WindowEditor>();

        // Shared setting flags
        internal static bool darkModeEnabled = (int) Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "1") == 0;
        internal static bool wordWrapEnabled = true;

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

        // Window creation and close functions

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

        public static void closeAllWindows()
        {
            foreach (var oW in openWindows.ToList())
            {
                oW.Value.Close();
            }
        }


        // Toggles for shared settings

        public static void setEdtiorWordWrap(bool enabled)
        {
            wordWrapEnabled = enabled;
            foreach(var oW in openWindows)
            {
                oW.Value.setWordWrap(enabled);
            }
        }

        public static void setEditorDarkMode(bool enabled)
        {
            darkModeEnabled = enabled;
            foreach (var oW in openWindows)
            {
                oW.Value.setDarkTheme(enabled);
            }
        }

    }
}
