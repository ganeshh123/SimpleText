using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace SimpleText
{
    internal static class Program
    {

        // Container for managing open windows
        static readonly IDictionary<string, WindowEditor> openWindows = new Dictionary<string, WindowEditor>();

        // Configuration
        static IniFile appConfig = null;

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
            LoadSettings();
            CreateWindowEditor();
            Application.Run();
        }

        public static void LoadSettings()
        {
            string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimpleText");
            Directory.CreateDirectory(userDataFolder);
            string configFilePath = Path.Combine(userDataFolder, "SimpleTextSettings.ini");
            if(File.Exists(configFilePath))
            {
                appConfig = new IniFile(configFilePath);

                if (appConfig.KeyExists("wordWrapEnabled"))
                {
                    string valueStr = appConfig.Read("wordWrapEnabled");
                    wordWrapEnabled = valueStr == "true" || (valueStr != "false" && wordWrapEnabled);
                }
            }
            else
            {
                File.Create(configFilePath).Close();
            }
        }

        public static void WriteSetting(string key, string value)
        {
            if(appConfig == null)
            {
                return;
            }
            appConfig.Write(key, value);
        }

        // Window creation and close functions

        public static void CreateWindowEditor()
        {
            WindowEditor newWindowEditor= new WindowEditor();
            string newWindowId = newWindowEditor.GetWindowId();

            openWindows.Add(newWindowId, newWindowEditor);
            Debug.WriteLine(newWindowEditor);
            newWindowEditor.Show();
        }

        public static void RemoveWindowEditor(string windowId)
        {
            openWindows.Remove(windowId);
            if(openWindows.Count < 1)
            {
                Application.Exit();
            }
        }

        public static void CloseAllWindows()
        {
            foreach (var oW in openWindows.ToList())
            {
                oW.Value.Close();
            }
        }


        // Toggles for shared settings

        public static void SetEdtiorWordWrap(bool enabled)
        {
            wordWrapEnabled = enabled;
            WriteSetting("wordWrapEnabled", wordWrapEnabled ? "true" : "false");
            foreach(var oW in openWindows)
            {
                oW.Value.SetWordWrap(enabled);
            }
        }

        public static void SetEditorDarkMode(bool enabled)
        {
            darkModeEnabled = enabled;
            foreach (var oW in openWindows)
            {
                oW.Value.SetDarkTheme(enabled);
            }
        }

    }
}
