// Win32Ex.cs
// By Joe Esposito

using System;
using System.Drawing;


/// <summary> Win32 API. </summary>
public class Win32Ex : Win32
{
  
  public static string GetWindowText (IntPtr hWnd)
  {
    string strTemp;
    
    if (IsWindow(hWnd) == 0) return "";
    
    strTemp = new String(Convert.ToChar(32), (GetWindowTextLength(hWnd) + 1));
    GetWindowText(hWnd, strTemp, strTemp.Length);
    
    return strTemp;
  }
  
  public static void DoEvents ()
  { System.Windows.Forms.Application.DoEvents(); }
  
  // Retrieves the window from the client point
  public static IntPtr WindowFromPoint(IntPtr hClientWnd, int xPoint, int yPoint)
  {
    POINTAPI pt;
    
    pt.x = xPoint; pt.y = yPoint;
    Win32.ClientToScreen(hClientWnd, ref pt);
    
    return (IntPtr)Win32.WindowFromPoint(pt.x, pt.y);
  }
  
  public static bool HighlightWindow (IntPtr hWnd)
  {
    IntPtr hDC;                   // The DC of the window.
    RECT rt = new RECT();         // Rectangle area of the window.
    
    // Get the window DC of the window.
    if ((hDC = (IntPtr)GetWindowDC(hWnd)) == IntPtr.Zero) return false;
    
    // Get the screen coordinates of the rectangle of the window.
    GetWindowRect(hWnd, ref rt);
    rt.right -= rt.left; rt.left = 0;
    rt.bottom -= rt.top; rt.top = 0;
    
    // Draw a border in the DC covering the entire window area of the window.
    IntPtr hRgn = (IntPtr)CreateRectRgnIndirect(ref rt);
    GetWindowRgn(hWnd, hRgn);
    SetROP2(hDC, R2_NOT); FrameRgn(hDC, hRgn, (IntPtr)GetStockObject(WHITE_BRUSH), 3, 3);
    DeleteObject(hRgn);
    
    // Finally release the DC.
    ReleaseDC(hWnd, hDC);
    
    return true;
  }
  
  public static bool IsRelativeWindow (IntPtr hWnd, IntPtr hRelativeWindow, bool bProcessAncestor)
  {
    int dwProcess = new int(), dwProcessOwner = new int();
    int dwThread = new int(), dwThreadOwner = new int();;
    
    // Failsafe
    if (hWnd == IntPtr.Zero) return false;
    if (hRelativeWindow == IntPtr.Zero) return false;
    if (hWnd == hRelativeWindow) return true;
    
    // Get processes and threads
    dwThread = Win32.GetWindowThreadProcessId(hWnd, ref dwProcess);
    dwThreadOwner = Win32.GetWindowThreadProcessId(hRelativeWindow, ref dwProcessOwner);
    
    // Get relative info
    if (bProcessAncestor) return (dwProcess == dwProcessOwner);
    return (dwThread == dwThreadOwner);
  }
}
