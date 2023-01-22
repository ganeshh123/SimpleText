using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using TextBox = System.Windows.Forms.TextBox;
using Microsoft.Win32;

namespace SimpleText
{
    public partial class WindowEditor : Form
    {

        private readonly string windowId = Guid.NewGuid().ToString();

        // Data Structures
        private readonly Stack<string> undoActions = new Stack<string>();
        private readonly Stack<string> redoActions = new Stack<string>();
        private readonly int defaultFontSize = 12;
        private string openFilePath = null;
        private string openFileInitialText = "";
        private bool fileModified = false;
        public Color fgColor = Color.Black;
        public Color bgColor = Color.White;


        public WindowEditor()
        {
            InitializeComponent();
            this.Text = $"New File - SimpleText";
            setDarkTheme(Program.darkModeEnabled);
            setWordWrap(Program.wordWrapEnabled);

        }

        // Get/Set Functions
        public string getWindowId()
        {
            return windowId;
        }

        // Window Load Actions
        private void WindowEditor_Load(object sender, EventArgs e)
        {
            undoActions.Push(openFileInitialText);
            zoomInToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Plus";
            zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Minus";
        }

        // Load and Save Text Data
        private void ReadFile(string filePath){
            FileStream fileToOpen = File.OpenRead(filePath);
            string fileContent = new StreamReader(fileToOpen, Encoding.UTF8).ReadToEnd();
            fileToOpen.Close();

            openFilePath = filePath;
            openFileInitialText = fileContent;
            this.Text = $"{Path.GetFileName(filePath)} - SimpleText";

            textBoxEditor.Text = fileContent;
            TextBoxCursorToEnd();
            UpdateFileModified();

            undoActions.Clear();
            redoActions.Clear();
            undoActions.Push(openFileInitialText);
        }

        private void WriteFile(string filePath)
        {
            FileStream fileToSave = File.OpenWrite(filePath);
            fileToSave.SetLength(0);

            byte[] fileContent = new UTF8Encoding(true).GetBytes(textBoxEditor.Text);
            fileToSave.Write(fileContent, 0, fileContent.Length);
            fileToSave.Close();
        }

        private void UpdateFileModified()
        {
            fileModified = !String.Equals(textBoxEditor.Text, openFileInitialText);
            if (fileModified)
            {
                if (openFilePath != null)
                {
                    this.Text = $"{Path.GetFileName(openFilePath)}* - SimpleText";
                }        
            }
            else if (this.Text.Contains("*") && openFilePath != null)
            {
                    this.Text = $"{Path.GetFileName(openFilePath)} - SimpleText";
            }
        }

        private void PushToUndoActions()
        {
            // Don't push if text hasn't changed
            if(undoActions.Count > 0 && String.Equals(undoActions.Peek(), textBoxEditor.Text))
            {
                return;
            }

            undoActions.Push(textBoxEditor.Text);

            if (undoActions.Count > 1 && undoToolStripMenuItem.Enabled == false)
            {
                undoToolStripMenuItem.Enabled = true;
            }
        }

        internal void setWordWrap(bool enabled)
        {
            textBoxEditor.WordWrap = enabled;
            wordWrapToolStripMenuItem.Checked = enabled;
            if (enabled)
            {
                textBoxEditor.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                textBoxEditor.ScrollBars = ScrollBars.Both;
            }
        }

        internal void setDarkTheme(bool enabled)
        {
            darkThemeToolStripMenuItem.Checked = enabled;
            if (enabled)
            {
                fgColor = Color.FromArgb(195, 204, 219);
                bgColor = Color.FromArgb(40, 44, 52);
            }
            else
            {
                fgColor = Color.Black;
                bgColor = Color.White;
            }

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.BackColor = bgColor;
                tb.ForeColor = fgColor;
            }

            foreach (MenuStrip mS in this.Controls.OfType<MenuStrip>())
            {
                mS.BackColor = bgColor;
                mS.ForeColor = fgColor;

                foreach (ToolStripMenuItem sectionMenu in mS.Items)
                {
                    sectionMenu.BackColor = bgColor;
                    sectionMenu.ForeColor = fgColor;

                    void menuItemHoverColorChange(object sender, EventArgs e)
                    {
                        if (Program.darkModeEnabled == false)
                        {
                            return;
                        }
                        ToolStripMenuItem item = (ToolStripMenuItem)sender;
                        item.ForeColor = bgColor;
                        item.BackColor = fgColor;
                    };
                    void menuItemHoverColorReset(object sender, EventArgs e)
                    {
                        ToolStripMenuItem item = (ToolStripMenuItem)sender;
                        item.BackColor = bgColor;
                        item.ForeColor = fgColor;
                    }

                    sectionMenu.MouseEnter += menuItemHoverColorChange;
                    sectionMenu.DropDownOpened += menuItemHoverColorChange;
                    sectionMenu.MouseLeave += delegate (object sender, EventArgs e)
                    {
                        ToolStripMenuItem item = (ToolStripMenuItem)sender;
                        if (item.DropDown.Visible == false)
                        {
                            menuItemHoverColorReset(sender, e);
                        }
                    };
                    sectionMenu.DropDownClosed += menuItemHoverColorReset;
                }
            }
        }

        private void TextBoxCursorToEnd()
        {
            textBoxEditor.SelectionStart = textBoxEditor.Text.Length;
            textBoxEditor.SelectionLength = 0;
        }

        // Actions to perform when user stops writing
        private void StoppedTyping()
        {
            UpdateFileModified();
            PushToUndoActions();
        }

        // Window Events
        private void WindowEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileModified == true)
            {
                string fileName = "New File";
                if (openFilePath != null)
                {
                    fileName = Path.GetFileName(openFilePath);
                }
                DialogResult saveRequested = MessageBox.Show($"Would you like to save changes to {fileName}?", "SimpleText", MessageBoxButtons.YesNo);
                if (saveRequested == DialogResult.Yes)
                {
                    e.Cancel = true;
                    SaveToolStripMenuItem_Click(sender, e);
                    this.Close();
                }
            }
            Program.removeWindowEditor(this.windowId);

        }


        // Text Box events
        private void TextBoxEditor_TextChanged(object sender, EventArgs e)
        {
            timerStoppedTyping.Stop();
            timerStoppedTyping.Start();
        }

        // Timer Events
        private void TimerStoppedTyping_Tick(object sender, EventArgs e)
        {
            timerStoppedTyping.Stop();
            StoppedTyping();
        }

        // Menu Bar Events
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*var info = new System.Diagnostics.ProcessStartInfo(Application.ExecutablePath);
            System.Diagnostics.Process.Start(info);*/

            Program.createWindowEditor();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = true,
                Filter = "Text Files|*.txt|All Files|*.*"
            };
            openFile.ShowDialog();

            if(openFile.FileName == "")
            {
                return;
            }

            ReadFile(openFile.FileName);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFilePath == null)
            {
                SaveAsToolStripMenuItem_Click(sender, null);
                return;
            }

            WriteFile(openFilePath);
            ReadFile(openFilePath);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = true,
                Filter = "Text File|*.txt|Custom|*.*",
                DefaultExt = ".txt",
                OverwritePrompt = true
            };
            saveFile.ShowDialog();

            if (saveFile.FileName == "")
            {
                return;
            }

            WriteFile(saveFile.FileName);
            ReadFile(saveFile.FileName);
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(undoActions.Count < 0)
            {
                return;
            }

            if(undoActions.Count == 1)
            {
                textBoxEditor.Text = undoActions.Peek();
            }
            else
            {
                redoActions.Push(undoActions.Pop());
                textBoxEditor.Text = undoActions.Pop();
            }
            TextBoxCursorToEnd();

            if (undoActions.Count == 1 && undoToolStripMenuItem.Enabled == true && String.Equals(openFileInitialText, textBoxEditor.Text))
            {
                undoToolStripMenuItem.Enabled = false;
            }

            if(redoActions.Count > 0 && redoToolStripMenuItem.Enabled == false)
            {
                redoToolStripMenuItem.Enabled = true;
            }
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoActions.Count < 1)
            {
                return;
            }

            textBoxEditor.Text = redoActions.Pop();
            TextBoxCursorToEnd();

            if (redoActions.Count < 1 && redoToolStripMenuItem.Enabled == true)
            {
                redoToolStripMenuItem.Enabled = false;
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Cut();
        }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Copy();
        }


        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cursorLocation = textBoxEditor.SelectionStart;
            if (textBoxEditor.SelectionLength < 1 && cursorLocation < textBoxEditor.Text.Length)
            {
                textBoxEditor.Text = textBoxEditor.Text.Remove(cursorLocation, 1);
                textBoxEditor.SelectionStart = cursorLocation;
                return;
            }
            textBoxEditor.Text = textBoxEditor.Text.Remove(cursorLocation, textBoxEditor.SelectionLength);
            textBoxEditor.SelectionLength = 0;
            textBoxEditor.SelectionStart = cursorLocation;
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.SelectAll();
        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontChooser = new FontDialog
            {
                Font = textBoxEditor.Font,
                ShowColor = false
            };
            if (fontChooser.ShowDialog() != DialogResult.Cancel)
            {
                textBoxEditor.Font = fontChooser.Font;
            }
        }

        private void WordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool enableWordWrap = !wordWrapToolStripMenuItem.Checked;
            textBoxEditor.WordWrap = enableWordWrap;
            Program.setEdtiorWordWrap(enableWordWrap);
        }

        private void ZoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Font = new Font(textBoxEditor.Font.FontFamily, textBoxEditor.Font.Size + 1);
        }

        private void ZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Font = new Font(textBoxEditor.Font.FontFamily, textBoxEditor.Font.Size - 1);
        }

        private void ResetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Font = new Font(textBoxEditor.Font.FontFamily, defaultFontSize);
        }

        private void DarkThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool enableDarkTheme = !darkThemeToolStripMenuItem.Checked;
            Program.setEditorDarkMode(enableDarkTheme);
        }

        private void ViewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ganeshh123.github.io/SimpleText/#usage");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*MessageBox.Show($"SimpleText {Application.ProductVersion}", "About SimpleText", MessageBoxButtons.OK);*/
            Form aboutWindow = new WindowAbout();
            aboutWindow.Show(this);
        }

        private void LicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ganeshh123/SimpleText/blob/main/LICENSE.MD");
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ganeshh123/SimpleText/releases/");
        }

        private void ReportBugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ganeshh123/SimpleText/issues");
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var proc in Process.GetProcesses())
            {
                // Check process list and set close window commands to any other instances
                if (proc.ProcessName == "SimpleText")
                {
                    proc.CloseMainWindow();
                }
            }
        }
    }
}
