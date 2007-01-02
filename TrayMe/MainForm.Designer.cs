namespace TrayMe
{
    partial class MainForm
    {
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelFinderTool;
        private System.Windows.Forms.PictureBox picTarget;
        private System.Windows.Forms.GroupBox groupTrayMe;
        private System.Windows.Forms.Label labelHandle;
        private System.Windows.Forms.TextBox textHandle;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.TextBox textCaption;
        private System.Windows.Forms.Label labelCharset;
        private System.Windows.Forms.TextBox textCharset;
        private System.Windows.Forms.Button buttonTrayMe;
        private System.Windows.Forms.CheckBox checkTopmost;
        private System.Windows.Forms.ContextMenu menuTrayMenu;
        private System.Windows.Forms.MenuItem menuTrayMenuStatus;
        private System.Windows.Forms.MenuItem menuTrayMenuExit;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.imageList = new System.Windows.Forms.ImageList( this.components );
            this.timerCheck = new System.Windows.Forms.Timer( this.components );
            this.notifyIcon = new System.Windows.Forms.NotifyIcon( this.components );
            this.menuTrayMenu = new System.Windows.Forms.ContextMenu();
            this.menuTrayMenuStatus = new System.Windows.Forms.MenuItem();
            this.menuTrayMenuExit = new System.Windows.Forms.MenuItem();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonHide = new System.Windows.Forms.Button();
            this.groupTrayMe = new System.Windows.Forms.GroupBox();
            this.checkTopmost = new System.Windows.Forms.CheckBox();
            this.picTarget = new System.Windows.Forms.PictureBox();
            this.buttonTrayMe = new System.Windows.Forms.Button();
            this.labelHandle = new System.Windows.Forms.Label();
            this.textHandle = new System.Windows.Forms.TextBox();
            this.labelCaption = new System.Windows.Forms.Label();
            this.textCaption = new System.Windows.Forms.TextBox();
            this.labelCharset = new System.Windows.Forms.Label();
            this.textCharset = new System.Windows.Forms.TextBox();
            this.labelFinderTool = new System.Windows.Forms.Label();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.groupTrayMe.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.picTarget ) ).BeginInit();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject( "imageList.ImageStream" ) ) );
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName( 0, "" );
            this.imageList.Images.SetKeyName( 1, "" );
            // 
            // timerCheck
            // 
            this.timerCheck.Enabled = true;
            this.timerCheck.Tick += new System.EventHandler( this.timerCheck_Tick );
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenu = this.menuTrayMenu;
            this.notifyIcon.Icon = ( (System.Drawing.Icon)( resources.GetObject( "notifyIcon.Icon" ) ) );
            this.notifyIcon.Text = "TrayMe";
            this.notifyIcon.DoubleClick += new System.EventHandler( this.notifyIcon_DoubleClick );
            // 
            // menuTrayMenu
            // 
            this.menuTrayMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuTrayMenuStatus,
            this.menuTrayMenuExit} );
            // 
            // menuTrayMenuStatus
            // 
            this.menuTrayMenuStatus.DefaultItem = true;
            this.menuTrayMenuStatus.Index = 0;
            this.menuTrayMenuStatus.Text = "&Status...";
            this.menuTrayMenuStatus.Click += new System.EventHandler( this.menuTrayMenuStatus_Click );
            // 
            // menuTrayMenuExit
            // 
            this.menuTrayMenuExit.Index = 1;
            this.menuTrayMenuExit.Text = "&Exit";
            this.menuTrayMenuExit.Click += new System.EventHandler( this.menuTrayMenuExit_Click );
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point( 276, 92 );
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size( 100, 29 );
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Cl&ose";
            this.buttonClose.Click += new System.EventHandler( this.buttonClose_Click );
            // 
            // buttonHide
            // 
            this.buttonHide.Location = new System.Drawing.Point( 276, 56 );
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size( 100, 29 );
            this.buttonHide.TabIndex = 2;
            this.buttonHide.Text = "&Hide";
            this.buttonHide.Click += new System.EventHandler( this.buttonHide_Click );
            // 
            // groupTrayMe
            // 
            this.groupTrayMe.Controls.Add( this.checkTopmost );
            this.groupTrayMe.Controls.Add( this.picTarget );
            this.groupTrayMe.Controls.Add( this.buttonTrayMe );
            this.groupTrayMe.Controls.Add( this.labelHandle );
            this.groupTrayMe.Controls.Add( this.textHandle );
            this.groupTrayMe.Controls.Add( this.labelCaption );
            this.groupTrayMe.Controls.Add( this.textCaption );
            this.groupTrayMe.Controls.Add( this.labelCharset );
            this.groupTrayMe.Controls.Add( this.textCharset );
            this.groupTrayMe.Controls.Add( this.labelFinderTool );
            this.groupTrayMe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupTrayMe.Location = new System.Drawing.Point( 8, 8 );
            this.groupTrayMe.Name = "groupTrayMe";
            this.groupTrayMe.Size = new System.Drawing.Size( 260, 192 );
            this.groupTrayMe.TabIndex = 0;
            this.groupTrayMe.TabStop = false;
            // 
            // checkTopmost
            // 
            this.checkTopmost.Checked = true;
            this.checkTopmost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTopmost.Location = new System.Drawing.Point( 20, 148 );
            this.checkTopmost.Name = "checkTopmost";
            this.checkTopmost.Size = new System.Drawing.Size( 116, 16 );
            this.checkTopmost.TabIndex = 7;
            this.checkTopmost.Text = "Topmost";
            this.checkTopmost.CheckedChanged += new System.EventHandler( this.checkTopmost_CheckedChanged );
            // 
            // picTarget
            // 
            this.picTarget.BackColor = System.Drawing.Color.White;
            this.picTarget.Location = new System.Drawing.Point( 108, 28 );
            this.picTarget.Name = "picTarget";
            this.picTarget.Size = new System.Drawing.Size( 31, 28 );
            this.picTarget.TabIndex = 13;
            this.picTarget.TabStop = false;
            this.picTarget.MouseMove += new System.Windows.Forms.MouseEventHandler( this.picTarget_MouseMove );
            this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler( this.picTarget_MouseDown );
            this.picTarget.MouseUp += new System.Windows.Forms.MouseEventHandler( this.picTarget_MouseUp );
            // 
            // buttonTrayMe
            // 
            this.buttonTrayMe.Location = new System.Drawing.Point( 144, 148 );
            this.buttonTrayMe.Name = "buttonTrayMe";
            this.buttonTrayMe.Size = new System.Drawing.Size( 100, 28 );
            this.buttonTrayMe.TabIndex = 8;
            this.buttonTrayMe.Text = "&Tray Me!";
            this.buttonTrayMe.Click += new System.EventHandler( this.buttonAttach_Click );
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point( 16, 72 );
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size( 48, 16 );
            this.labelHandle.TabIndex = 1;
            this.labelHandle.Text = "H&andle:";
            // 
            // textHandle
            // 
            this.textHandle.Location = new System.Drawing.Point( 68, 68 );
            this.textHandle.MaxLength = 8;
            this.textHandle.Name = "textHandle";
            this.textHandle.Size = new System.Drawing.Size( 72, 20 );
            this.textHandle.TabIndex = 2;
            this.textHandle.TextChanged += new System.EventHandler( this.textHandle_TextChanged );
            // 
            // labelCaption
            // 
            this.labelCaption.Location = new System.Drawing.Point( 16, 96 );
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size( 48, 16 );
            this.labelCaption.TabIndex = 3;
            this.labelCaption.Text = "&Caption:";
            // 
            // textCaption
            // 
            this.textCaption.Location = new System.Drawing.Point( 68, 92 );
            this.textCaption.Name = "textCaption";
            this.textCaption.ReadOnly = true;
            this.textCaption.Size = new System.Drawing.Size( 176, 20 );
            this.textCaption.TabIndex = 4;
            // 
            // labelCharset
            // 
            this.labelCharset.Location = new System.Drawing.Point( 16, 120 );
            this.labelCharset.Name = "labelCharset";
            this.labelCharset.Size = new System.Drawing.Size( 48, 16 );
            this.labelCharset.TabIndex = 5;
            this.labelCharset.Text = "C&harset:";
            // 
            // textCharset
            // 
            this.textCharset.Location = new System.Drawing.Point( 68, 116 );
            this.textCharset.Name = "textCharset";
            this.textCharset.ReadOnly = true;
            this.textCharset.Size = new System.Drawing.Size( 176, 20 );
            this.textCharset.TabIndex = 6;
            // 
            // labelFinderTool
            // 
            this.labelFinderTool.Location = new System.Drawing.Point( 16, 36 );
            this.labelFinderTool.Name = "labelFinderTool";
            this.labelFinderTool.Size = new System.Drawing.Size( 84, 16 );
            this.labelFinderTool.TabIndex = 0;
            this.labelFinderTool.Text = "Finder Tool:";
            // 
            // buttonAbout
            // 
            this.buttonAbout.Location = new System.Drawing.Point( 276, 12 );
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size( 100, 29 );
            this.buttonAbout.TabIndex = 1;
            this.buttonAbout.Text = "&About...";
            this.buttonAbout.Click += new System.EventHandler( this.buttonAbout_Click );
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonTrayMe;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size( 386, 212 );
            this.Controls.Add( this.groupTrayMe );
            this.Controls.Add( this.buttonClose );
            this.Controls.Add( this.buttonHide );
            this.Controls.Add( this.buttonAbout );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TrayMe - Inactive";
            this.TopMost = true;
            this.Load += new System.EventHandler( this.MainForm_Load );
            this.Activated += new System.EventHandler( this.MainForm_Activated );
            this.groupTrayMe.ResumeLayout( false );
            this.groupTrayMe.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.picTarget ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion
    }
}