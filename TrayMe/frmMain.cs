// frmMain.sc
// By Joe Esposito

// !!!!! TODO !!!!!
// * Get icon to work with all windows [icon/traybar version ?]
// * Check for top-level window .. work with ONLY top-level windows! (not for WinEye, though)
// - Get to work with multiple windows
// - Themable (windows themes)


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using System.IO;
using System.Reflection;



namespace TrayMe
{
  /// <summary> Main TrayMe Form Class. </summary>
  public class frmMain : System.Windows.Forms.Form
  {
    
    #region Class Variables
    
    private Cursor m_curTarget = null;
    private bool bTargeting = false;
    private IntPtr hCurrentTarget = IntPtr.Zero;
    private bool m_bSubclassed = false;
    
    #region Controls
    
    private Timer tmrCheck;
    private ImageList imlPics;
    private NotifyIcon nfiNotifyIcon;
    private Button btnAbout;
    private Button btnHide;
    private Button btnClose;
    private Label lblFinderTool;
    private PictureBox picTarget;
    private GroupBox gpbTrayMe;
    private Label lblHandle;
    private TextBox txtHandle;
    private Label lblCaption;
    private TextBox txtCaption;
    private Label lblUni;
    private TextBox txtUni;
    private Button btnAttach;
    private CheckBox chkTopmost;
    
    private ContextMenu mnuTray;
    private MenuItem mnuTrayStatus;
    private MenuItem mnuTrayExit;
    
    private System.ComponentModel.IContainer components;
    
    #endregion
    
    #endregion
    
    #region Class Construction
    
    public frmMain()
    {
      // Required for Windows Form Designer support
      InitializeComponent();
    }
    
    /// <summary> Clean up any resources being used. </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
      this.imlPics = new System.Windows.Forms.ImageList(this.components);
      this.tmrCheck = new System.Windows.Forms.Timer(this.components);
      this.nfiNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.mnuTray = new System.Windows.Forms.ContextMenu();
      this.mnuTrayStatus = new System.Windows.Forms.MenuItem();
      this.mnuTrayExit = new System.Windows.Forms.MenuItem();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnHide = new System.Windows.Forms.Button();
      this.gpbTrayMe = new System.Windows.Forms.GroupBox();
      this.chkTopmost = new System.Windows.Forms.CheckBox();
      this.picTarget = new System.Windows.Forms.PictureBox();
      this.btnAttach = new System.Windows.Forms.Button();
      this.lblHandle = new System.Windows.Forms.Label();
      this.txtHandle = new System.Windows.Forms.TextBox();
      this.lblCaption = new System.Windows.Forms.Label();
      this.txtCaption = new System.Windows.Forms.TextBox();
      this.lblUni = new System.Windows.Forms.Label();
      this.txtUni = new System.Windows.Forms.TextBox();
      this.lblFinderTool = new System.Windows.Forms.Label();
      this.btnAbout = new System.Windows.Forms.Button();
      this.gpbTrayMe.SuspendLayout();
      this.SuspendLayout();
      // 
      // imlPics
      // 
      this.imlPics.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      this.imlPics.ImageSize = new System.Drawing.Size(31, 28);
      this.imlPics.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlPics.ImageStream")));
      this.imlPics.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // tmrCheck
      // 
      this.tmrCheck.Enabled = true;
      this.tmrCheck.Tick += new System.EventHandler(this.tmrCheck_Tick);
      // 
      // nfiNotifyIcon
      // 
      this.nfiNotifyIcon.ContextMenu = this.mnuTray;
      this.nfiNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("nfiNotifyIcon.Icon")));
      this.nfiNotifyIcon.Text = "TrayMe";
      this.nfiNotifyIcon.DoubleClick += new System.EventHandler(this.nfiNotifyIcon_DoubleClick);
      // 
      // mnuTray
      // 
      this.mnuTray.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                            this.mnuTrayStatus,
                                                                            this.mnuTrayExit});
      // 
      // mnuTrayStatus
      // 
      this.mnuTrayStatus.DefaultItem = true;
      this.mnuTrayStatus.Index = 0;
      this.mnuTrayStatus.Text = "&Status...";
      this.mnuTrayStatus.Click += new System.EventHandler(this.mnuTrayStatus_Click);
      // 
      // mnuTrayExit
      // 
      this.mnuTrayExit.Index = 1;
      this.mnuTrayExit.Text = "&Exit";
      this.mnuTrayExit.Click += new System.EventHandler(this.mnuTrayExit_Click);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(276, 92);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(100, 29);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Cl&ose";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnHide
      // 
      this.btnHide.Location = new System.Drawing.Point(276, 56);
      this.btnHide.Name = "btnHide";
      this.btnHide.Size = new System.Drawing.Size(100, 29);
      this.btnHide.TabIndex = 2;
      this.btnHide.Text = "&Hide";
      this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
      // 
      // gpbTrayMe
      // 
      this.gpbTrayMe.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.chkTopmost,
                                                                            this.picTarget,
                                                                            this.btnAttach,
                                                                            this.lblHandle,
                                                                            this.txtHandle,
                                                                            this.lblCaption,
                                                                            this.txtCaption,
                                                                            this.lblUni,
                                                                            this.txtUni,
                                                                            this.lblFinderTool});
      this.gpbTrayMe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.gpbTrayMe.Location = new System.Drawing.Point(8, 8);
      this.gpbTrayMe.Name = "gpbTrayMe";
      this.gpbTrayMe.Size = new System.Drawing.Size(260, 192);
      this.gpbTrayMe.TabIndex = 0;
      this.gpbTrayMe.TabStop = false;
      // 
      // chkTopmost
      // 
      this.chkTopmost.Checked = true;
      this.chkTopmost.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkTopmost.Location = new System.Drawing.Point(20, 148);
      this.chkTopmost.Name = "chkTopmost";
      this.chkTopmost.Size = new System.Drawing.Size(116, 16);
      this.chkTopmost.TabIndex = 7;
      this.chkTopmost.Text = "Topmost";
      this.chkTopmost.CheckedChanged += new System.EventHandler(this.chkTopmost_CheckedChanged);
      // 
      // picTarget
      // 
      this.picTarget.BackColor = System.Drawing.Color.White;
      this.picTarget.Location = new System.Drawing.Point(108, 28);
      this.picTarget.Name = "picTarget";
      this.picTarget.Size = new System.Drawing.Size(31, 28);
      this.picTarget.TabIndex = 13;
      this.picTarget.TabStop = false;
      this.picTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseUp);
      this.picTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseMove);
      this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseDown);
      // 
      // btnAttach
      // 
      this.btnAttach.Location = new System.Drawing.Point(144, 148);
      this.btnAttach.Name = "btnAttach";
      this.btnAttach.Size = new System.Drawing.Size(100, 28);
      this.btnAttach.TabIndex = 8;
      this.btnAttach.Text = "&Tray Me!";
      this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
      // 
      // lblHandle
      // 
      this.lblHandle.Location = new System.Drawing.Point(16, 72);
      this.lblHandle.Name = "lblHandle";
      this.lblHandle.Size = new System.Drawing.Size(48, 16);
      this.lblHandle.TabIndex = 1;
      this.lblHandle.Text = "H&andle:";
      // 
      // txtHandle
      // 
      this.txtHandle.Location = new System.Drawing.Point(68, 68);
      this.txtHandle.MaxLength = 8;
      this.txtHandle.Name = "txtHandle";
      this.txtHandle.Size = new System.Drawing.Size(72, 20);
      this.txtHandle.TabIndex = 2;
      this.txtHandle.Text = "";
      this.txtHandle.TextChanged += new System.EventHandler(this.txtHandle_TextChanged);
      // 
      // lblCaption
      // 
      this.lblCaption.Location = new System.Drawing.Point(16, 96);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new System.Drawing.Size(48, 16);
      this.lblCaption.TabIndex = 3;
      this.lblCaption.Text = "&Caption:";
      // 
      // txtCaption
      // 
      this.txtCaption.Location = new System.Drawing.Point(68, 92);
      this.txtCaption.Name = "txtCaption";
      this.txtCaption.ReadOnly = true;
      this.txtCaption.Size = new System.Drawing.Size(176, 20);
      this.txtCaption.TabIndex = 4;
      this.txtCaption.Text = "";
      // 
      // lblUni
      // 
      this.lblUni.Location = new System.Drawing.Point(16, 120);
      this.lblUni.Name = "lblUni";
      this.lblUni.Size = new System.Drawing.Size(48, 16);
      this.lblUni.TabIndex = 5;
      this.lblUni.Text = "C&harset:";
      // 
      // txtUni
      // 
      this.txtUni.Location = new System.Drawing.Point(68, 116);
      this.txtUni.Name = "txtUni";
      this.txtUni.ReadOnly = true;
      this.txtUni.Size = new System.Drawing.Size(176, 20);
      this.txtUni.TabIndex = 6;
      this.txtUni.Text = "";
      // 
      // lblFinderTool
      // 
      this.lblFinderTool.Location = new System.Drawing.Point(16, 36);
      this.lblFinderTool.Name = "lblFinderTool";
      this.lblFinderTool.Size = new System.Drawing.Size(84, 16);
      this.lblFinderTool.TabIndex = 0;
      this.lblFinderTool.Text = "&Finder Tool:";
      // 
      // btnAbout
      // 
      this.btnAbout.Location = new System.Drawing.Point(276, 12);
      this.btnAbout.Name = "btnAbout";
      this.btnAbout.Size = new System.Drawing.Size(100, 29);
      this.btnAbout.TabIndex = 1;
      this.btnAbout.Text = "&About...";
      this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
      // 
      // frmMain
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(386, 212);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.gpbTrayMe,
                                                                  this.btnClose,
                                                                  this.btnHide,
                                                                  this.btnAbout});
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "frmMain";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "TrayMe - Inactive";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.frmMain_Load);
      this.Activated += new System.EventHandler(this.frmMain_Activated);
      this.gpbTrayMe.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    
    #endregion
    
    #endregion
    
    
    #region Entry Point of Application
    
    /// <summary> The main entry point for the application. </summary>
    [STAThread]
    static void Main (string [] args)
    {
      frmMain form = new frmMain();
      
      // !!!!! Load settings
      
      // Attach to app
      if (args.Length > 0)
      {
        bool bExit   = false;
        bool bNoHide = false;
        bool bQuiet  = false;
        
        // Get cmdline options
        int i = 0;
        for (i = 0; i < args.Length; i++)
        {
          if (args[i].Length == 0) continue;
          if (args[i][0] != '-') break;
          
          // Get individual options
          switch (args[i].ToLower())
          {
            case "-h":
              MessageBox.Show(form, "Usage: " + 
                Path.GetFileName(Application.ExecutablePath) + " [options] target cmd\n\n" +
                "Options:\n" +
                " -h  --  displays this help message\n" +
                " -s  --  show window; do not close to tray\n" +
                " -q  --  quiet mode; display no command line errors\n" +
                " -x  --  exit when trayed\n" +
                "\n" +
                "target  --  the target application to tray\n" +
                "cmd     --  the command line to be passed to the target\n",
                "TrayMe Command Line Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
              break;
            case "-q": bQuiet  = true; break;
            case "-s": bNoHide = true; break;
            case "-x": bExit   = true; break;
          }
        }
        
        if (i < args.Length)
        {
          // Start app process and wait for idle input
          ProcessStartInfo psi = new ProcessStartInfo(args[i], (( (i + 1) < args.Length )?( string.Join(" ", args, (i + 1), (args.Length - 1)) ):( "" )) );
          if (!bNoHide) psi.WindowStyle = ProcessWindowStyle.Minimized;
          Process app = Process.Start(psi);
          app.WaitForInputIdle();
          app.Refresh();
          
          // Attach to app's main window
          if (app.HasExited)
          { if (!bQuiet) MessageBox.Show(form, "Application exited before it could be trayed.", "TrayMe"); }
          else
          {
            IntPtr hWnd = app.MainWindowHandle;
            
            if (hWnd == IntPtr.Zero)
            { if (!bQuiet) MessageBox.Show(form, "Could not find application's main window.", "TrayMe"); }
            else
            {
              form.DoAttach(hWnd);
              if (!bNoHide) Win32.SendMessage(hWnd, Win32.WM_CLOSE, 0, 0);
            }
          }
          
          // Exit if done
          if (bExit)
          {
            form.Close();
            return;
          }
        }
      }
      
      Application.Run(form);
    }
    
    #endregion
    
    
    
    private void frmMain_Load(object sender, System.EventArgs e)
    {
      // Create Resources
      // -----------------
      
      try
      {
        // Load cursors
        Assembly asm = Assembly.GetExecutingAssembly();
        m_curTarget = new Cursor(asm.GetManifestResourceStream("TrayMe.curTarget.cur"));
      }
      catch (Exception x )
      {
        // Show error
        MessageBox.Show(this, "Failed to load cursors.\n\n" + x.ToString(), "TrayMe");
        
        // Attempt to use backup cursor
        if (m_curTarget == null) m_curTarget = Cursors.Cross;
      }
      
      
      // Set default values
      picTarget.Image = imlPics.Images[0];
      
      // Check tray
      CheckTrayStatus();
    }
    
    private void frmMain_Activated(object sender, System.EventArgs e)
    { if (!nfiNotifyIcon.Visible) nfiNotifyIcon.Visible = true; }
    
    
    
    private void btnAbout_Click(object sender, System.EventArgs e)
    {
      frmAbout about = new frmAbout();
      about.ShowDialog(this);
      about.Dispose();
    }
    
    private void btnAttach_Click(object sender, System.EventArgs e)
    {
      if (IsAttached())
        DoDetach();
      else
        DoAttach();
    }
    
    private void chkTopmost_CheckedChanged(object sender, System.EventArgs e)
    { TopMost = chkTopmost.Checked; }
    
    private void txtHandle_TextChanged(object sender, System.EventArgs e)
    {
      IntPtr hWnd;
      
      try
      { hWnd = (IntPtr)Convert.ToInt32(txtHandle.Text, 16); }
      catch { hWnd = IntPtr.Zero; }
      
      ShowWindowInfo(hWnd, false);
    }
    
    private void btnClose_Click(object sender, System.EventArgs e)
    { this.Close(); }

    private void btnHide_Click(object sender, System.EventArgs e)
    {
      this.Hide();
    }
    
    private void tmrCheck_Tick(object sender, System.EventArgs e)
    { CheckTrayStatus(); }
    
    #region Form Target Events
    
    // Target Finder Events
    // ---------------------
    
    private void picTarget_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      // Set capture image and cursor
      picTarget.Image = imlPics.Images[1];
      picTarget.Cursor = m_curTarget;
      
      // Set capture
      Win32.SetCapture(picTarget.Handle);
      
      // Begin targeting
      bTargeting = true;
      hCurrentTarget = IntPtr.Zero;
      
      // Show info   FIX: Put into function for mousemove & mousedown
      ShowWindowInfo(picTarget.Handle, true);
    }
    
    private void picTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      IntPtr hWnd;
      IntPtr hTemp;
      
      // End targeting
      bTargeting = false;
      
      // Unhighlight window
      if (hCurrentTarget != IntPtr.Zero) Win32Ex.HighlightWindow(hCurrentTarget);
      hCurrentTarget = IntPtr.Zero;
      
      // Reset capture image and cursor
      picTarget.Cursor = Cursors.Default;
      picTarget.Image = imlPics.Images[0];
      
      // Get screen coords from client coords and window handle
      hWnd = Win32Ex.WindowFromPoint(picTarget.Handle, e.X, e.Y);
      
      // Get owner
      while ((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero) hWnd = hTemp;
      
      ShowWindowInfo(hWnd, true);
      
      // Release capture
      Win32.SetCapture(IntPtr.Zero);
    }

    private void picTarget_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      IntPtr hWnd;
      IntPtr hTemp;
      Win32.POINTAPI pt;
      
      // FIX: Draw border around EVERY window
      
      pt.x = e.X; pt.y = e.Y;
      Win32.ClientToScreen(picTarget.Handle, ref pt);
      
      // Make sure targeting before highlighting windows
      if (!bTargeting) return;
      
      // Get screen coords from client coords and window handle
      hWnd = Win32Ex.WindowFromPoint(picTarget.Handle, e.X, e.Y);
      
      // Get real window
      if (hWnd != IntPtr.Zero) {
        hTemp = IntPtr.Zero;
        
        while (true)
        {
          Win32.MapWindowPoints(hTemp, hWnd, ref pt, 1);
          hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y);
          if (hTemp == IntPtr.Zero) break;
          if (hWnd == hTemp) break;
          hWnd = hTemp;
        }
        
        /* FIX: Work with ALL windows
        Win32.ScreenToClient(hWnd, ref pt);
        Win32.MapWindowPoints(IntPtr.Zero, hWnd, ref pt, 2);
        if ((hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y)) != IntPtr.Zero) 
        {
          hWnd = hTemp;
        }
        // */
      }
      
      // Get owner
      while ((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero) hWnd = hTemp;
      
      // Show info
      ShowWindowInfo(hWnd, true);
      
      // Highlight valid window
      HighlightValidWindow(hWnd, this.Handle);
      
    }
    
    #endregion
    
    #region Form Tray Menu Events
    
    private void mnuTrayExit_Click(object sender, System.EventArgs e)
    { this.Close(); }

    private void mnuTrayStatus_Click(object sender, System.EventArgs e)
    { ShowTrayStatus(); }
    private void nfiNotifyIcon_DoubleClick(object sender, System.EventArgs e)
    { ShowTrayStatus(); }
    
    #endregion
    
    
    
    private bool IsAttached ()
    { return (new TrayMeClass()).IsHooked(); }
    
    private bool DoDetach ()
    {
      // Check for trayed window
      TrayMeClass objTrayMe = new TrayMeClass();
      if (objTrayMe.IsHooked())
      {
        if (objTrayMe.HookTrayWindow(IntPtr.Zero, IntPtr.Zero) == true)
        {
          MessageBox.Show(this, "Could not unhook tray window.", "TrayMe");
          return false;
        }
        
        CheckTrayStatus();
      }
      
      return true;
    }
    
    private bool DoAttach ()
    {
      IntPtr hWnd;
      
      // Failsafe
      if (!CheckTrayStatus()) return false;
      
      // Validate input
      try { hWnd = (IntPtr)Convert.ToInt32(txtHandle.Text, 16); }
      catch { hWnd = IntPtr.Zero; }
      if (Win32.IsWindow(hWnd) == 0)
      {
        MessageBox.Show(this, "Enter a valid handle.", "TrayMe");
        return false;
      }
      
      // Attach new window
      return DoAttach(hWnd);
    }
    private bool DoAttach (IntPtr hWnd)
    {
      // Failsafe
      if (!CheckTrayStatus()) return false;
      if (hWnd == IntPtr.Zero) return false;
      
      // Detach old window
      if (!DoDetach()) return false;
      
      // Tray the window
      TrayMeClass objTrayMe = new TrayMeClass();
      if (objTrayMe.HookTrayWindow(hWnd, this.Icon.Handle) == false)
      { MessageBox.Show(this, "Could not hook tray window.", "TrayMe"); }
      
      // Update status
      CheckTrayStatus();
      return true;
    }
    
    
    private void ShowWindowInfo (IntPtr hWnd, bool bHandle)
    {
      if ( (Win32.IsWindow(hWnd) == 0) || (Win32Ex.IsRelativeWindow(hWnd, this.Handle, true)) ) {
        if (bHandle) txtHandle.Text = "";
        txtCaption.Text = "";
        txtUni.Text = "";
        return;
      }
      
      // Set window information
      if (bHandle) txtHandle.Text = Convert.ToString(hWnd.ToInt32(), 16).ToUpper().PadLeft(8, '0');
      txtCaption.Text = Win32Ex.GetWindowText(hWnd);
      txtUni.Text = (( Win32.IsWindowUnicode(hWnd) != 0 )?( "Unicode" ):( "Ansi" ));
    }
    
    private void HighlightValidWindow (IntPtr hWnd, IntPtr hOwner)
    {
      // Check for valid highlight
      if (hCurrentTarget == hWnd) return;
      
      // Check for relative
      if (Win32Ex.IsRelativeWindow(hWnd, hOwner, true))
      {
        // Unhighlight last window
        if (hCurrentTarget != IntPtr.Zero)
        { Win32Ex.HighlightWindow(hCurrentTarget); hCurrentTarget = IntPtr.Zero; }
        
        return;
      }
      
      // Unhighlight last window
      Win32Ex.HighlightWindow(hCurrentTarget);
      
      // Set as current target
      hCurrentTarget = hWnd;
      
      // Highlight window
      Win32Ex.HighlightWindow(hWnd);
    }
    
    private bool CheckTrayStatus ()
    {
      TrayMeClass objTrayMe = new TrayMeClass();
      
      try
      {
        if (objTrayMe.IsHooked())
        { if (!m_bSubclassed) { btnAttach.Text = "Un-Tray Me"; m_bSubclassed = true; } }
        else  
        { if (m_bSubclassed) { btnAttach.Text = "Tray Me!"; m_bSubclassed = false; } }
        
        if (!btnAttach.Enabled)
          btnAttach.Enabled = true;
        
        return true;
      }
      catch (DllNotFoundException)
      {
        if (btnAttach.Enabled)
        {
          btnAttach.Enabled = false;
          btnAttach.Text = "Missing Dll";
        }
      }
      catch (EntryPointNotFoundException)
      {
        if (btnAttach.Enabled)
        {
          btnAttach.Enabled = false;
          btnAttach.Text = "Bad Dll";
        }
      }
      
      return false;
    }
    
    private void ShowTrayStatus ()
    {
      this.Show();
      this.Focus();
    }
    
  }
}
