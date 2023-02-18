using System;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleText
{
    public partial class WindowAbout : Form
    {
        public WindowAbout()
        {
            InitializeComponent();
        }

        // Setup
        private void WindowAbout_Load(object sender, EventArgs e)
        {
            WindowEditor ownerEditor = (WindowEditor)this.Owner;
            this.Location = ownerEditor.Location;

            this.BackColor = ownerEditor.bgColor;
            this.ForeColor = ownerEditor.fgColor;

            linkLabelIconDesigner.LinkColor = ownerEditor.fgColor;

            SetAppVersion();
        }

        private void SetAppVersion()
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            LabelAppVersion.Text = appVersion.Major + "." + appVersion.Minor + "." + appVersion.Build;
        }

        // Events
        private void LinkLabelIconDesigner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jwhutchinson");
        }

        private void LabelAuthor_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ganeshh123.github.io");
        }
    }
}
