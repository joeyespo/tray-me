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
    
    #region Dll Declarations
    
    // Variables and Constants
    // ------------------------
    
    [DllImport("TrayMe.dll", EntryPoint="IsSubclassed", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int IsSubclassed ();
    
    [DllImport("TrayMe.dll", EntryPoint="InjectDll", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int InjectDll ( IntPtr hWnd );
    
    [DllImport("TrayMe.dll", EntryPoint="UnmapDll", SetLastError=false, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.Winapi)]
    private static extern int UnmapDll ();
    
    #endregion
    
    
    // Returns whether or not it is currently subclassed
    public bool IsHooked ()
    { return (IsSubclassed() != 0); }
    
    // Hooks (subclasses) the window
    public bool HookTrayWindow (IntPtr hWnd, IntPtr hIcon)
    {
      if (IsSubclassed() == 0)
      { InjectDll(hWnd); }
      else
      { UnmapDll(); }
      
      return (IsSubclassed() != 0);
    }
  }
}
