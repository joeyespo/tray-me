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
    
    [DllImport("HookInjEx.dll", EntryPoint="IsSubclassed", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int IsSubclassed ();
    
    [DllImport("HookInjEx.dll", EntryPoint="InjectDll", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int InjectDll ( IntPtr hWnd );
    
    [DllImport("HookInjEx.dll", EntryPoint="UnmapDll", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int UnmapDll ();
    
    #endregion
    
    
    
    #region Public Functions
    
    // Public Functions
    // -----------------
    
    // Returns whether or not it is currently subclassed
    public bool IsHooked ()
    { return (IsSubclassed() != 0); }
    
    // Hooks (subclasses) the window
    public bool HookTrayWindow (IntPtr hWnd, IntPtr hIcon, string strToolTip)
    {
      if (IsSubclassed() == 0)
      { InjectDll(hWnd); }
      else
      { UnmapDll(); }
      
      return (IsSubclassed() != 0);
      
      /*
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
      */
    }
    
    #endregion
    
    
    
    #region Private Window Procedure Function
    
    // Private Window Procedure Function
    // ----------------------------------
    
    /*
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
    */
    
    #endregion

  }
}
