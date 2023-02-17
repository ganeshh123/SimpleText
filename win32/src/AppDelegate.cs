using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SimpleText
{
    class AppDelegate : WindowsFormsApplicationBase
    {
        // Record of opened windows
        static readonly IDictionary<string, WindowEditor> openWindows = new Dictionary<string, WindowEditor>();

        // Shared Static Classes
        static IniFile appConfig = null;
        static readonly TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));

        // Shared settings
        internal static bool darkModeEnabled = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "1") == 0;
        internal static bool wordWrapEnabled = true;
        internal static Font editorFont = new Font("Consolas", 12, FontStyle.Regular);

        public AppDelegate()
        {
            this.IsSingleInstance= true;
            this.StartupNextInstance += handleNewInstance;

            LoadSettings();
            CreateWindowEditor();
        }

        void handleNewInstance(object sender, StartupNextInstanceEventArgs e)
        {

        }

        protected override void OnCreateMainForm()
        {
            MainForm = openWindows[openWindows.Keys.First()];
        }

        public static void LoadSettings()
        {
            string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimpleText");
            Directory.CreateDirectory(userDataFolder);
            string configFilePath = Path.Combine(userDataFolder, "SimpleTextSettings.ini");
            if (File.Exists(configFilePath))
            {
                appConfig = new IniFile(configFilePath);

                if (appConfig.KeyExists("wordWrapEnabled"))
                {
                    string valueStr = appConfig.Read("wordWrapEnabled");
                    wordWrapEnabled = valueStr == "true" || (valueStr != "false" && wordWrapEnabled);
                }
                if (appConfig.KeyExists("editorFont"))
                {
                    string valueStr = appConfig.Read("editorFont");
                    editorFont = (Font)fontConverter.ConvertFromString(valueStr);
                }
            }
            else
            {
                File.Create(configFilePath).Close();
            }
        }

        public static void WriteSetting(string key, string value)
        {
            if (appConfig == null)
            {
                return;
            }
            appConfig.Write(key, value);
        }

        // Window creation and close functions

        public static string CreateWindowEditor(string openFile = null)
        {
            WindowEditor newWindowEditor = new WindowEditor();
            string newWindowId = newWindowEditor.GetWindowId();

            openWindows.Add(newWindowId, newWindowEditor);
            if (openFile != null)
            {
                newWindowEditor.ReadFile(openFile);
            }
            newWindowEditor.Show();
            return newWindowId;
        }

        public static void RemoveWindowEditor(string windowId)
        {
            openWindows.Remove(windowId);
            if (openWindows.Count < 1)
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
            foreach (var oW in openWindows)
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

        public static void SetEditorFont(Font selectedFont)
        {
            editorFont = selectedFont;
            string selectedFontString = fontConverter.ConvertToString(editorFont);
            WriteSetting("editorFont", selectedFontString);
            foreach (var oW in openWindows)
            {
                oW.Value.SetFont(editorFont);
            }
        }
    }
}
