using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        }

        // Events
        private void LinkLabelIconDesigner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jwhutchinson");
        }

        
    }
}
