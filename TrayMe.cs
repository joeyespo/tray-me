// TrayMe.cs
// By Joe Esposito


using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace TrayMe
{
	/// <summary> Class used for subclassing windows and adding them to the tray. </summary>
	public class TrayMeClass
	{
    
    #region Variables and Constants
    
    // Variables and Constants
    // ------------------------
  
    // Callback delegate function declarations
    private delegate int HookProc ( int nCode, int wParam, int lParam );
    
    [DllImport("user32", EntryPoint="SetWindowsHookEx", SetLastError=true, CharSet=CharSet.Auto, ExactSpelling=false, CallingConvention=CallingConvention.Winapi)]
    private static extern int SetWindowsHookEx ( int idHook, HookProc lpfn, IntPtr hmod, int dwThreadId );
    
    // Public constants
    public const int DEF_WM_SYSTRAYNOTIFY = (Win32.WM_USER+3);  // Default Tray Icon message
    public const int DEF_ID_SYSTRAYNOTIFY = (1001);             // Default Tray Icon ID
    
    // Internal variables
    private IntPtr m_hHook;                 // The previous window procedure
    internal IntPtr m_lpWndProc;            // The previous window procedure
    internal int m_uCallbackMessage;        // The windows message used for the callback
    internal int m_uSysTrayID;              // The Tray ID
    
    #endregion
    
    
    
    #region Public Functions
    
    // Public Functions
    // -----------------
    
    // Hooks (subclasses) the window
    public bool HookTrayWindow (IntPtr hWnd, IntPtr hIcon, string strToolTip)
    {
      int dwThread;
      int dwProcessId = new int();
      HookProc hp = new HookProc(HookCBTProc);
      
      // Local vars
      Win32.NOTIFYICONDATA nidTrayIcon = new Win32.NOTIFYICONDATA();
      
      
      // TODO: Subclass window
      dwThread = Win32.GetWindowThreadProcessId(hWnd, ref dwProcessId);
      
      m_lpWndProc = IntPtr.Zero;
      m_uCallbackMessage = DEF_WM_SYSTRAYNOTIFY;
      m_uSysTrayID = DEF_ID_SYSTRAYNOTIFY;
      m_hHook = (IntPtr)Win32.SetWindowsHookEx(Win32.WH_CALLWNDPROC, hp.Method.MethodHandle.Value, IntPtr.Zero, dwThread);
      
      Win32.UnhookWindowsHookEx(m_hHook);
      
      // Initialize tray icon
      nidTrayIcon.cbSize = Win32.NOTIFYICONDATA_V1_SIZE;
      nidTrayIcon.hWnd = hWnd;
      nidTrayIcon.uCallbackMessage = m_uCallbackMessage;
      nidTrayIcon.uID = m_uSysTrayID;
      nidTrayIcon.uFlags = (Win32.NIF_ICON | Win32.NIF_MESSAGE | Win32.NIF_TIP);
      // FIX: Get window's icon
      nidTrayIcon.hIcon = (IntPtr)Win32.SendMessage(hWnd, 0x007F, 2, 0); // !!!!!: use constants   ..  //(IntPtr)Win32.GetClassLong(hWnd, Win32.GCL_HICON);
      nidTrayIcon.szTip = strToolTip;
      
      // Add to System Tray .. TODO: Add XP features to icon .. Use "GetDllVersion()"
      if (Win32.Shell_NotifyIcon(Win32.NIM_ADD, ref nidTrayIcon) == 0) return false;
      
      
      // Success
      return true;
    }
    
    #endregion
    
    
    
    #region Private Window Procedure Function
    
    // Private Window Procedure Function
    // ----------------------------------
    
    private int HookCBTProc (int nCode, int wParam, int lParam)
    {
      if (nCode < 0) return Win32.CallNextHookEx(m_hHook, nCode, wParam, lParam);
      return 0;
    }
    
    private int TrayMeProc (IntPtr hWnd, int uMsg, int wParam, int lParam)
    {
      // Call subclassed window procedure
      return Win32.CallWindowProc(m_lpWndProc, hWnd, uMsg, wParam, lParam);
    }
    
    #endregion

  }
}
