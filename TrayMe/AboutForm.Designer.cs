namespace TrayMe
{
    partial class AboutForm
    {
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel panelAbout;
        private System.Windows.Forms.LinkLabel linkHomepageLink;
        private System.Windows.Forms.Label labelHomepageTitle;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.Panel picDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelRevision;

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.labelVersion = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panelAbout = new System.Windows.Forms.Panel();
            this.linkHomepageLink = new System.Windows.Forms.LinkLabel();
            this.labelHomepageTitle = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.picDescription = new System.Windows.Forms.Panel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelRevision = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panelAbout.SuspendLayout();
            this.picDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 220);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(45, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "Version:";
            this.labelVersion.Resize += new System.EventHandler(this.labelVersion_Resize);
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(12, 8);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(32, 32);
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // panelAbout
            // 
            this.panelAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(89)))), ((int)(((byte)(117)))));
            this.panelAbout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelAbout.BackgroundImage")));
            this.panelAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAbout.Controls.Add(this.linkHomepageLink);
            this.panelAbout.Controls.Add(this.labelHomepageTitle);
            this.panelAbout.Controls.Add(this.labelAuthor);
            this.panelAbout.Controls.Add(this.picDescription);
            this.panelAbout.Location = new System.Drawing.Point(12, 48);
            this.panelAbout.Name = "panelAbout";
            this.panelAbout.Size = new System.Drawing.Size(284, 160);
            this.panelAbout.TabIndex = 2;
            // 
            // linkHomepageLink
            // 
            this.linkHomepageLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(228)))), ((int)(((byte)(164)))));
            this.linkHomepageLink.AutoSize = true;
            this.linkHomepageLink.BackColor = System.Drawing.Color.Transparent;
            this.linkHomepageLink.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.linkHomepageLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.484F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.linkHomepageLink.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(175)))), ((int)(((byte)(162)))));
            this.linkHomepageLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkHomepageLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(175)))), ((int)(((byte)(162)))));
            this.linkHomepageLink.Location = new System.Drawing.Point(184, 136);
            this.linkHomepageLink.Name = "linkHomepageLink";
            this.linkHomepageLink.Size = new System.Drawing.Size(95, 15);
            this.linkHomepageLink.TabIndex = 3;
            this.linkHomepageLink.TabStop = true;
            this.linkHomepageLink.Text = "joeyespo.com";
            this.linkHomepageLink.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(175)))), ((int)(((byte)(162)))));
            this.linkHomepageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHomepageLink_LinkClicked);
            this.linkHomepageLink.MouseEnter += new System.EventHandler(this.linkHomepageLink_MouseEnter);
            this.linkHomepageLink.MouseLeave += new System.EventHandler(this.linkHomepageLink_MouseLeave);
            // 
            // labelHomepageTitle
            // 
            this.labelHomepageTitle.AutoSize = true;
            this.labelHomepageTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelHomepageTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.484F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labelHomepageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(175)))), ((int)(((byte)(162)))));
            this.labelHomepageTitle.Location = new System.Drawing.Point(176, 118);
            this.labelHomepageTitle.Name = "labelHomepageTitle";
            this.labelHomepageTitle.Size = new System.Drawing.Size(103, 15);
            this.labelHomepageTitle.TabIndex = 2;
            this.labelHomepageTitle.Text = "uber-ware labs";
            // 
            // labelAuthor
            // 
            this.labelAuthor.BackColor = System.Drawing.Color.Transparent;
            this.labelAuthor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(175)))), ((int)(((byte)(162)))));
            this.labelAuthor.Location = new System.Drawing.Point(12, 80);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(264, 15);
            this.labelAuthor.TabIndex = 1;
            this.labelAuthor.Text = "Designed and Coded by Joe Esposito";
            this.labelAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picDescription
            // 
            this.picDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(44)))), ((int)(((byte)(58)))));
            this.picDescription.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picDescription.BackgroundImage")));
            this.picDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDescription.Controls.Add(this.labelDescription);
            this.picDescription.Location = new System.Drawing.Point(8, 8);
            this.picDescription.Name = "picDescription";
            this.picDescription.Size = new System.Drawing.Size(268, 64);
            this.picDescription.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDescription.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelDescription.Location = new System.Drawing.Point(4, 8);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(256, 48);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "[ Description ]";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(48, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(248, 16);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "[ Title ]";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(208, 220);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(84, 32);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "&Close";
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelRevision.Location = new System.Drawing.Point(68, 220);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new System.Drawing.Size(51, 13);
            this.labelRevision.TabIndex = 4;
            this.labelRevision.Text = "Revision:";
            this.labelRevision.Visible = false;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(306, 264);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelRevision);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.panelAbout);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panelAbout.ResumeLayout(false);
            this.panelAbout.PerformLayout();
            this.picDescription.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
