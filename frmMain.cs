// frmMain.sc
// By Joe Esposito

// TODO: Themable (windows themes)


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.IO;
using System.Reflection;



namespace TrayMe
{
  /// <summary> Main TrayMe Form Class. </summary>
  public class frmMain : System.Windows.Forms.Form
  {
    
    #region Form Variables
    
    // Form Variables
    // ---------------
    
    // Resources
    private Cursor m_curTarget = null;
    private bool bTargeting = false;
    private IntPtr hCurrentTarget = IntPtr.Zero;
    
    // Controls
    private ImageList imlPics;
    private TextBox txtHandle;
    private Label lblHandle;
    private Button btnAttach;
    private TextBox txtToolTip;
    private Label lblToolTip;
    private TextBox txtCaption;
    private Label lblCaption;
    private PictureBox picTarget;
    
    #endregion
    
    
    
    #region Internal Information

    #region Internal Private Members
    private System.ComponentModel.IContainer components;
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
    
    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
      this.txtHandle = new System.Windows.Forms.TextBox();
      this.lblHandle = new System.Windows.Forms.Label();
      this.btnAttach = new System.Windows.Forms.Button();
      this.txtToolTip = new System.Windows.Forms.TextBox();
      this.lblToolTip = new System.Windows.Forms.Label();
      this.picTarget = new System.Windows.Forms.PictureBox();
      this.lblCaption = new System.Windows.Forms.Label();
      this.txtCaption = new System.Windows.Forms.TextBox();
      this.imlPics = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // txtHandle
      // 
      this.txtHandle.Location = new System.Drawing.Point(128, 152);
      this.txtHandle.MaxLength = 8;
      this.txtHandle.Name = "txtHandle";
      this.txtHandle.Size = new System.Drawing.Size(68, 20);
      this.txtHandle.TabIndex = 1;
      this.txtHandle.Text = "";
      this.txtHandle.TextChanged += new System.EventHandler(this.txtHandle_TextChanged);
      // 
      // lblHandle
      // 
      this.lblHandle.Location = new System.Drawing.Point(76, 156);
      this.lblHandle.Name = "lblHandle";
      this.lblHandle.Size = new System.Drawing.Size(48, 16);
      this.lblHandle.TabIndex = 0;
      this.lblHandle.Text = "H&andle:";
      // 
      // btnAttach
      // 
      this.btnAttach.Location = new System.Drawing.Point(204, 304);
      this.btnAttach.Name = "btnAttach";
      this.btnAttach.Size = new System.Drawing.Size(100, 29);
      this.btnAttach.TabIndex = 2;
      this.btnAttach.Text = "&Tray Me!";
      this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
      // 
      // txtToolTip
      // 
      this.txtToolTip.Location = new System.Drawing.Point(76, 276);
      this.txtToolTip.Name = "txtToolTip";
      this.txtToolTip.Size = new System.Drawing.Size(228, 20);
      this.txtToolTip.TabIndex = 1;
      this.txtToolTip.Text = "My ToolTip";
      // 
      // lblToolTip
      // 
      this.lblToolTip.Location = new System.Drawing.Point(76, 260);
      this.lblToolTip.Name = "lblToolTip";
      this.lblToolTip.Size = new System.Drawing.Size(228, 13);
      this.lblToolTip.TabIndex = 0;
      this.lblToolTip.Text = "&ToolTip:";
      // 
      // picTarget
      // 
      this.picTarget.BackColor = System.Drawing.Color.White;
      this.picTarget.Location = new System.Drawing.Point(80, 108);
      this.picTarget.Name = "picTarget";
      this.picTarget.Size = new System.Drawing.Size(31, 28);
      this.picTarget.TabIndex = 3;
      this.picTarget.TabStop = false;
      this.picTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseUp);
      this.picTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseMove);
      this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseDown);
      // 
      // lblCaption
      // 
      this.lblCaption.Location = new System.Drawing.Point(76, 180);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new System.Drawing.Size(48, 16);
      this.lblCaption.TabIndex = 0;
      this.lblCaption.Text = "&Caption:";
      // 
      // txtCaption
      // 
      this.txtCaption.Location = new System.Drawing.Point(128, 176);
      this.txtCaption.Name = "txtCaption";
      this.txtCaption.ReadOnly = true;
      this.txtCaption.Size = new System.Drawing.Size(176, 20);
      this.txtCaption.TabIndex = 1;
      this.txtCaption.Text = "";
      // 
      // imlPics
      // 
      this.imlPics.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      this.imlPics.ImageSize = new System.Drawing.Size(31, 28);
      this.imlPics.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlPics.ImageStream")));
      this.imlPics.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // frmMain
      // 
      this.AcceptButton = this.btnAttach;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(376, 366);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.picTarget,
                                                                  this.btnAttach,
                                                                  this.lblHandle,
                                                                  this.txtHandle,
                                                                  this.txtToolTip,
                                                                  this.lblToolTip,
                                                                  this.lblCaption,
                                                                  this.txtCaption});
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "frmMain";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "TrayMe - Inactive";
      this.Load += new System.EventHandler(this.frmMain_Load);
      this.ResumeLayout(false);

    }
    #endregion

    #endregion
    
    
    
    #region Entry Point of Application
    
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      Application.Run(new frmMain());
    }
    
    #endregion
    
    
    
    #region Form Events
    
    #region Form Creation Events

    // Form Creation Events
    // ---------------------
    
    private void frmMain_Load(object sender, System.EventArgs e)
    {
      Assembly asm = Assembly.GetExecutingAssembly();
      
      // Create Resources
      // -----------------
      
      try
      {
        // Load cursors
        m_curTarget = new Cursor(asm.GetManifestResourceStream("TrayMe.curTarget.cur"));
      }
      catch (Exception x )
      {
        // Show error
        MessageBox.Show("Failed to load cursors.\n\n" + x.ToString());
        
        // Attempt to use backup cursor
        if (m_curTarget == null) m_curTarget = Cursors.Cross;
      }
      
      
      // Set default values
      picTarget.Image = imlPics.Images[0];
    }
    
    #endregion
    
    #region Form Member Events
    
    // Form Member Events
    // -------------------
    
    private void btnAttach_Click(object sender, System.EventArgs e)
    {
      IntPtr hWnd;
      TrayMeClass objTrayMe = new TrayMeClass();
      
      try
      { hWnd = (IntPtr)Convert.ToInt32(txtHandle.Text, 16); }
      catch
      { hWnd = IntPtr.Zero; }
      
      if (Win32.IsWindow(hWnd) == 0)
      { MessageBox.Show(this, "Enter a valid handle.", "TrayMe - Handle Error"); return; }
      
      // Tray the window
      objTrayMe.HookTrayWindow(hWnd, this.Icon.Handle, txtToolTip.Text);
    }
    
    private void txtHandle_TextChanged(object sender, System.EventArgs e)
    {
      IntPtr hWnd;
      
      try
      { hWnd = (IntPtr)Convert.ToInt32(txtHandle.Text, 16); }
      catch { hWnd = IntPtr.Zero; }
      
      txtCaption.Text = Win32Ex.GetWindowText(hWnd);
    }
    
    #endregion
    
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
    }
    
    private void picTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      IntPtr hWnd;
      
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
      
      // Set window information
      txtHandle.Text = Convert.ToString(hWnd.ToInt32(), 16).ToUpper().PadLeft(8, '0');
      
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
        
        /* Win32.ScreenToClient(hWnd, ref pt);
        Win32.MapWindowPoints(IntPtr.Zero, hWnd, ref pt, 2);
        if ((hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y)) != IntPtr.Zero) 
        {
          hWnd = hTemp;
        }
        // */
      }
      
      // Highlight valid window
      HighlightValidWindow(hWnd, this.Handle);
      
    }
    
    #endregion
    
    #endregion
    
    #region Form Functions
    
    // Form Functions
    // ---------------
    
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
    
    #endregion
    
  }
}
