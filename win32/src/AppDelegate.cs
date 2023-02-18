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
    public class AppDelegate : WindowsFormsApplicationBase
    {
        // Record of opened windows
        readonly IDictionary<string, WindowEditor> openWindows = new Dictionary<string, WindowEditor>();

        // Shared Classes
        IniFile appConfig = null;
        readonly TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));

        // Shared settings
        internal bool darkModeEnabled = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "1") == 0;
        internal bool wordWrapEnabled = true;
        internal Font editorFont = new Font("Arial", 12, FontStyle.Regular);

        public AppDelegate(string[] bootArgs= null)
        {
            this.IsSingleInstance= true;
            this.StartupNextInstance += HandleNewInstance;

            LoadSettings();

            string openFile = (bootArgs != null && bootArgs.Length > 1) ? bootArgs[1] : null;
            CreateWindowEditor(openFile);
        }

        void HandleNewInstance(object sender, StartupNextInstanceEventArgs e)
        {
            string openFile = e.CommandLine.Count > 1 ? e.CommandLine[1] : null;

            CreateWindowEditor(openFile);
        }

        protected override void OnCreateMainForm()
        {
            MainForm = openWindows[openWindows.Keys.First()];
        }

        public void LoadSettings()
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

        public void WriteSetting(string key, string value)
        {
            if (appConfig == null)
            {
                return;
            }
            appConfig.Write(key, value);
        }

        // Window creation and close functions

        public string CreateWindowEditor(string openFile = null)
        {
            WindowEditor newWindowEditor = new WindowEditor(this);
            string newWindowId = newWindowEditor.GetWindowId();

            openWindows.Add(newWindowId, newWindowEditor);
            newWindowEditor.Show();
            if (openFile != null)
            {
                newWindowEditor.ReadFile(openFile);
            }
            newWindowEditor.TextBoxFocus();
            this.MainForm = newWindowEditor;
            return newWindowId;
        }

        public void RemoveWindowEditor(string windowId)
        {
            openWindows.Remove(windowId);
            if (openWindows.Count < 1)
            {
                Application.Exit();
            }
            else
            {
                MainForm = openWindows.ToList()[0].Value;
            }
        }

        public void CloseAllWindows()
        {
            foreach (var oW in openWindows.ToList())
            {
                oW.Value.Close();
            }
        }


        // Toggles for shared settings

        public void SetEdtiorWordWrap(bool enabled)
        {
            wordWrapEnabled = enabled;
            WriteSetting("wordWrapEnabled", wordWrapEnabled ? "true" : "false");
            foreach (var oW in openWindows)
            {
                oW.Value.SetWordWrap(enabled);
            }
        }

        public void SetEditorDarkMode(bool enabled)
        {
            darkModeEnabled = enabled;
            foreach (var oW in openWindows)
            {
                oW.Value.SetDarkTheme(enabled);
            }
        }

        public void SetEditorFont(Font selectedFont)
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
