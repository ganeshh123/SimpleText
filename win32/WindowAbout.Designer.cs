namespace SimpleText
{
    partial class WindowAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowAbout));
            this.LabelAppName = new System.Windows.Forms.Label();
            this.LabelAppVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.linkLabelIconDesigner = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelAppName
            // 
            this.LabelAppName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LabelAppName.AutoSize = true;
            this.LabelAppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelAppName.Location = new System.Drawing.Point(83, 79);
            this.LabelAppName.Name = "LabelAppName";
            this.LabelAppName.Size = new System.Drawing.Size(136, 29);
            this.LabelAppName.TabIndex = 0;
            this.LabelAppName.Text = "SimpleText";
            this.LabelAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelAppVersion
            // 
            this.LabelAppVersion.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LabelAppVersion.AutoSize = true;
            this.LabelAppVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelAppVersion.Location = new System.Drawing.Point(129, 108);
            this.LabelAppVersion.Name = "LabelAppVersion";
            this.LabelAppVersion.Size = new System.Drawing.Size(0, 20);
            this.LabelAppVersion.TabIndex = 1;
            this.LabelAppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(119, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAuthor.Location = new System.Drawing.Point(114, 164);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(75, 18);
            this.labelAuthor.TabIndex = 3;
            this.labelAuthor.Text = "Ganesh H";
            this.labelAuthor.Click += new System.EventHandler(this.LabelAuthor_Click);
            // 
            // linkLabelIconDesigner
            // 
            this.linkLabelIconDesigner.AutoSize = true;
            this.linkLabelIconDesigner.Location = new System.Drawing.Point(68, 138);
            this.linkLabelIconDesigner.Name = "linkLabelIconDesigner";
            this.linkLabelIconDesigner.Size = new System.Drawing.Size(167, 16);
            this.linkLabelIconDesigner.TabIndex = 4;
            this.linkLabelIconDesigner.TabStop = true;
            this.linkLabelIconDesigner.Text = "Icon by Joseph Hutchinson";
            this.linkLabelIconDesigner.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelIconDesigner_LinkClicked);
            // 
            // WindowAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 193);
            this.Controls.Add(this.linkLabelIconDesigner);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LabelAppVersion);
            this.Controls.Add(this.LabelAppName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::SimpleText.Properties.Resources.SimpleText;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowAbout";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About SimpleText";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WindowAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelAppName;
        private System.Windows.Forms.Label LabelAppVersion;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.LinkLabel linkLabelIconDesigner;
    }
}