using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TrayMe
{
    /// <summary>
    /// Represents the main form of the TrayMe application.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        #region Form Target Event Handlers

        /// <summary>
        /// Handles the MouseDown event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            // Set capture image and cursor
            picTarget.Image = imageList.Images[1];
            picTarget.Cursor = targetCursor;

            // Set capture
            Win32.SetCapture( picTarget.Handle );

            // Begin targeting
            isTargetingWindow = true;
            targetedWindowHandle = IntPtr.Zero;

            // Show info   FIX: Put into function for mousemove & mousedown
            ShowWindowInfo( picTarget.Handle, true );
        }

        /// <summary>
        /// Handles the MouseUp event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            IntPtr hWnd;
            IntPtr hTemp;

            // End targeting
            isTargetingWindow = false;

            // Unhighlight window
            if( targetedWindowHandle != IntPtr.Zero )
                Win32Ex.HighlightWindow( targetedWindowHandle );
            targetedWindowHandle = IntPtr.Zero;

            // Reset capture image and cursor
            picTarget.Cursor = Cursors.Default;
            picTarget.Image = imageList.Images[0];

            // Get screen coords from client coords and window handle
            hWnd = Win32Ex.WindowFromPoint( picTarget.Handle, e.X, e.Y );

            // Get owner
            while( ( hTemp = Win32.GetParent( hWnd ) ) != IntPtr.Zero )
                hWnd = hTemp;

            ShowWindowInfo( hWnd, true );

            // Release capture
            Win32.SetCapture( IntPtr.Zero );
        }

        /// <summary>
        /// Handles the MouseMove event of the picTarget control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void picTarget_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            IntPtr hWnd;
            IntPtr hTemp;
            Win32.POINTAPI pt;

            // FIX: Draw border around EVERY window

            pt.x = e.X;
            pt.y = e.Y;
            Win32.ClientToScreen( picTarget.Handle, ref pt );

            // Make sure targeting before highlighting windows
            if( !isTargetingWindow )
                return;

            // Get screen coords from client coords and window handle
            hWnd = Win32Ex.WindowFromPoint( picTarget.Handle, e.X, e.Y );

            // Get real window
            if( hWnd != IntPtr.Zero )
            {
                hTemp = IntPtr.Zero;

                while( true )
                {
                    Win32.MapWindowPoints( hTemp, hWnd, ref pt, 1 );
                    hTemp = (IntPtr)Win32.ChildWindowFromPoint( hWnd, pt.x, pt.y );
                    if( hTemp == IntPtr.Zero )
                        break;
                    if( hWnd == hTemp )
                        break;
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
            while( ( hTemp = Win32.GetParent( hWnd ) ) != IntPtr.Zero )
                hWnd = hTemp;

            // Show info
            ShowWindowInfo( hWnd, true );

            // Highlight valid window
            HighlightValidWindow( hWnd, Handle );

        }

        #endregion

        #region Tray Menu Event Handlers

        /// <summary>
        /// Handles the Click event of the menuTrayMenuExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menuTrayMenuExit_Click( object sender, System.EventArgs e )
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the menuTrayMenuStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menuTrayMenuStatus_Click( object sender, System.EventArgs e )
        {
            ShowTrayStatus();
        }

        /// <summary>
        /// Handles the DoubleClick event of the notifyIcon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void notifyIcon_DoubleClick( object sender, System.EventArgs e )
        {
            ShowTrayStatus();
        }

        #endregion

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load( object sender, System.EventArgs e )
        {
            try
            {
                using( MemoryStream ms = new MemoryStream( Properties.Resources.curTarget ) )
                    targetCursor = new Cursor( ms );
            }
            catch( Exception x )
            {
                // Show error
                MessageBox.Show( this, "Could not load the \"Target\" cursor." + Environment.NewLine + Environment.NewLine + x.ToString(), "TrayMe Error", MessageBoxButtons.OK, MessageBoxIcon.Error );

                // Attempt to use backup cursor
                if( targetCursor == null )
                    targetCursor = Cursors.Cross;
            }


            // Set default values
            picTarget.Image = imageList.Images[0];

            // Check tray
            UpdateStatus();
        }

        /// <summary>
        /// Handles the Activated event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainForm_Activated( object sender, System.EventArgs e )
        {
            if( !notifyIcon.Visible )
                notifyIcon.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the buttonAbout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonAbout_Click( object sender, System.EventArgs e )
        {
            AboutForm about = new AboutForm();
            about.ShowDialog( this );
            about.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the buttonAttach control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonAttach_Click( object sender, System.EventArgs e )
        {
            if( IsAttached() )
                DoDetach();
            else
                DoAttach();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkTopmost control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void checkTopmost_CheckedChanged( object sender, System.EventArgs e )
        {
            TopMost = checkTopmost.Checked;
        }

        /// <summary>
        /// Handles the TextChanged event of the textHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void textHandle_TextChanged( object sender, System.EventArgs e )
        {
            IntPtr hWnd;

            try
            {
                hWnd = (IntPtr)Convert.ToInt32( textHandle.Text, 16 );
            }
            catch
            {
                hWnd = IntPtr.Zero;
            }

            ShowWindowInfo( hWnd, false );
        }

        /// <summary>
        /// Handles the Click event of the buttonClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonClose_Click( object sender, System.EventArgs e )
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the buttonHide control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonHide_Click( object sender, System.EventArgs e )
        {
            Hide();
        }

        /// <summary>
        /// Handles the Tick event of the timerCheck control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerCheck_Tick( object sender, System.EventArgs e )
        {
            UpdateStatus();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether a window is currently attached.
        /// </summary>
        /// <returns>true, if a window is currently attached; otherwise, false.</returns>
        public bool IsAttached()
        {
            return ( new TrayMeClass() ).IsHooked();
        }

        /// <summary>
        /// Determines whether a window is currently attached.
        /// </summary>
        /// <returns>true, if there is no window attached or the attached window is now detached; otherwise, false.</returns>
        public bool DoDetach()
        {
            // Check for trayed window
            TrayMeClass trayMe = new TrayMeClass();
            if( trayMe.IsHooked() )
            {
                if( trayMe.HookTrayWindow( IntPtr.Zero, IntPtr.Zero ) == true )
                {
                    MessageBox.Show( this, "Could not unhook currently trayed window.", "TrayMe Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }

                UpdateStatus();
            }

            return true;
        }

        /// <summary>
        /// Attaches to the provided window handle.
        /// </summary>
        /// <returns>true if the window is now attached; otherwise, false.</returns>
        public bool DoAttach()
        {
            IntPtr hWnd;

            // Failsafe
            if( !UpdateStatus() )
                return false;

            // Validate input
            try
            {
                hWnd = (IntPtr)Convert.ToInt32( textHandle.Text, 16 );
            }
            catch
            {
                hWnd = IntPtr.Zero;
            }
            if( Win32.IsWindow( hWnd ) == 0 )
            {
                MessageBox.Show( this, "Please enter a valid handle.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Information );
                return false;
            }

            // Attach new window
            return DoAttach( hWnd );
        }

        /// <summary>
        /// Attaches to the specified window handle.
        /// </summary>
        /// <param name="hWnd">The handle to the window to attach to.</param>
        /// <returns>true if the window is now attached; otherwise, false.</returns>
        public bool DoAttach( IntPtr hWnd )
        {
            // Failsafe
            if( !UpdateStatus() )
                return false;
            if( hWnd == IntPtr.Zero )
                return false;

            // Detach old window
            if( !DoDetach() )
                return false;

            // Tray the window
            TrayMeClass trayMe = new TrayMeClass();
            if( trayMe.HookTrayWindow( hWnd, Icon.Handle ) )
                MessageBox.Show( this, "Could not hook the window with the provided handle.", "TrayMe Error", MessageBoxButtons.OK, MessageBoxIcon.Error );

            // Update status
            UpdateStatus();
            return true;
        }

        #endregion

        /// <summary>
        /// Shows the information of the window with the specified handle.
        /// </summary>
        private void ShowWindowInfo( IntPtr hWnd, bool handle )
        {
            if( ( Win32.IsWindow( hWnd ) == 0 ) || ( Win32Ex.IsRelativeWindow( hWnd, Handle, true ) ) )
            {
                if( handle )
                    textHandle.Text = "";
                textCaption.Text = "";
                textCharset.Text = "";
                return;
            }

            // Set window information
            if( handle )
                textHandle.Text = Convert.ToString( hWnd.ToInt32(), 16 ).ToUpper().PadLeft( 8, '0' );
            textCaption.Text = Win32Ex.GetWindowText( hWnd );
            textCharset.Text = ( ( Win32.IsWindowUnicode( hWnd ) != 0 ) ? ( "Unicode" ) : ( "Ansi" ) );
        }

        /// <summary>
        /// Highlights the specified window, if valid.
        /// </summary>
        private void HighlightValidWindow( IntPtr hWnd, IntPtr hOwner )
        {
            // Check for valid highlight
            if( targetedWindowHandle == hWnd )
                return;

            // Check for relative
            if( Win32Ex.IsRelativeWindow( hWnd, hOwner, true ) )
            {
                // Unhighlight last window
                if( targetedWindowHandle != IntPtr.Zero )
                {
                    Win32Ex.HighlightWindow( targetedWindowHandle );
                    targetedWindowHandle = IntPtr.Zero;
                }

                return;
            }

            // Unhighlight last window
            Win32Ex.HighlightWindow( targetedWindowHandle );

            // Set as current target
            targetedWindowHandle = hWnd;

            // Highlight window
            Win32Ex.HighlightWindow( hWnd );
        }

        /// <summary>
        /// Updates the status and returns true if this was successful.
        /// </summary>
        private bool UpdateStatus()
        {
            TrayMeClass trayMe = new TrayMeClass();

            try
            {
                if( trayMe.IsHooked() )
                {
                    if( !isWindowTrayed )
                    {
                        buttonTrayMe.Text = "Un-&Tray Me";
                        isWindowTrayed = true;
                    }
                }
                else
                {
                    if( isWindowTrayed )
                    {
                        buttonTrayMe.Text = "&Tray Me!";
                        isWindowTrayed = false;
                    }
                }

                if( !buttonTrayMe.Enabled )
                    buttonTrayMe.Enabled = true;

                return true;
            }
            catch( DllNotFoundException )
            {
                if( buttonTrayMe.Enabled )
                {
                    buttonTrayMe.Enabled = false;
                    buttonTrayMe.Text = "Missing Dll";
                }
            }
            catch( EntryPointNotFoundException )
            {
                if( buttonTrayMe.Enabled )
                {
                    buttonTrayMe.Enabled = false;
                    buttonTrayMe.Text = "Bad Dll";
                }
            }

            return false;
        }

        /// <summary>
        /// Shows the tray status.
        /// </summary>
        private void ShowTrayStatus()
        {
            Show();
            Focus();
        }

        private Cursor targetCursor = null;
        private bool isWindowTrayed = false;
        private bool isTargetingWindow = false;
        private IntPtr targetedWindowHandle = IntPtr.Zero;
    }
}
