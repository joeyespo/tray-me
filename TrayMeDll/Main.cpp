/***************************************************************
Module name: HookInjEx_DLL.cpp
Copyright (c) 2003 Robert Kuster

Notice:  If this code works, it was written by Robert Kuster.
    Else, I don't know who wrote it.

    Use it on your own risk. No responsibilities for
    possible damages of even functionality can be taken.
***************************************************************/


// !!!!! ToDo
// 
// - Option: When window is shown .. 'unhide' tray icon
// 
// !!!!!


#include "Main.h"



//-------------------------------------------------------------
// Defines
// 

#define pCW ((CWPSTRUCT*)lParam)
#define pMSG ((MSG*)lParam)
#define ID_TRAYME_TRAYEDWINDOW    1001



//-------------------------------------------------------------
// shared data 
// Notice:  seen by both: the instance of "HookInjEx.dll" mapped
//      into window as well as by the instance
//      of "HookInjEx.dll" mapped into our "HookInjEx.exe"

#pragma data_seg (".shared")

volatile bool g_bSubclassed = false;
UINT WM_TRAYME_HOOKEX = 0;
UINT WM_TRAYME_TRAYNOTIFY = 0;
HWND g_hWnd = 0;
HHOOK g_hHook = 0;

#pragma data_seg ()

#pragma comment(linker,"/SECTION:.shared,RWS")



//-------------------------------------------------------------
// global variables (unshared!)
//
HINSTANCE  hDll;
bool g_bInTray = false;

// Version info
bool bWin9x_Known = false;
bool bWin9x = false;

// New & old window procedure of the subclassed window
WNDPROC g_OldProc = NULL;  
LRESULT CALLBACK NewProc ( HWND, UINT, WPARAM, LPARAM );



//-------------------------------------------------------------
// global functions
//

BOOL WINAPI IsSubclassed ()
{ return (( g_bSubclassed )?( TRUE ):( FALSE )); }



//-------------------------------------------------------------
// DllMain
//

BOOL APIENTRY DllMain (HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
  ::MessageBeep(MB_ICONASTERISK);
  
  if (ul_reason_for_call == DLL_PROCESS_ATTACH)
  {
    // !!!!! Not supported in CE
    ::DisableThreadLibraryCalls(hDll = (HINSTANCE)hModule);
    
    // Register Ansi windows message   !!!!! Not supported in CE
    if (WM_TRAYME_HOOKEX == NULL) WM_TRAYME_HOOKEX = ::RegisterWindowMessageA("TRAYME@@WM_TRAYME_HOOKEX");
    if (WM_TRAYME_TRAYNOTIFY == NULL) WM_TRAYME_TRAYNOTIFY = ::RegisterWindowMessageA("TRAYME@@WM_TRAYME_TRAYNOTIFY");
  }
  else if (ul_reason_for_call == DLL_PROCESS_DETACH)
  {
    if (g_hWnd)
    { if (IsWindow(g_hWnd) == FALSE) g_hWnd = NULL; }
    
    if (!g_hWnd) g_bSubclassed = false;
  }
  
  return TRUE;
}


//-------------------------------------------------------------
// IsVersionWin9x
//

void GetVersionWin9x ()
{
  OSVERSIONINFO osvi;
  
  // Get version info
  osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
  GetVersionEx(&osvi);
  
  // Get Win9x value
  bWin9x = ((osvi.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS) && (osvi.dwMajorVersion == 4));
}



//-------------------------------------------------------------
// HookProc
// Notice:
// - executed by the instance of "HookInjEx.dll" mapped into window;
// 
// When called from InjectDll:
//    - sublasses the window;
//    - removes the hook, but the DLL stays in the remote process
//    though, because we increased its reference count via LoadLibray
//    (this way we disturb the target process as litle as possible);
// 
// When called from UnmapDll:
//    - restores the old window procedure for the window;
//    - reduces the reference count of the DLL (via FreeLibrary);
//    - removes the hook, so the DLL is unmapped;
// 
//    Also note, that the DLL isn't unmapped immediately after the
//    call to UnhookWindowsHookEx, but in the near future
//    (right after finishing with the current message).
//    Actually it's obvious why: windows can NOT unmap the 
//    DLL in the middle of processing a meesage, because the code
//    in the hook procedure is still required. 
//    That's why we can change the order LoadLibrary/FreeLibrary &
//    UnhookWindowsHookEx are called.
// 
//    FreeLibrary, in contrast, unmapps the DLL imeditaley if the 
//    reference count reaches zero.
// 

LRESULT HookProc (int code, WPARAM wParam, LPARAM lParam)
{
  char lib_name [MAX_PATH];
  bool bInjectMessage;
  bool bHook;
  
  // Failsafe
  if (g_hWnd == NULL) goto END;
  if (IsWindow(g_hWnd) == 0) goto END;
  
  // Get window version
  if (!bWin9x_Known)
  { GetVersionWin9x(); bWin9x_Known = true; }
  
  // Get hook value
  if (bWin9x)
  { bHook = (pMSG->lParam != 0); bInjectMessage = (pMSG->message == WM_TRAYME_HOOKEX); }
  else { bHook = (pCW->lParam != 0); bInjectMessage = (pCW->message == WM_TRAYME_HOOKEX); }
  
  // Set window procedure
  if ((bInjectMessage) && (bHook))
  {
    ::UnhookWindowsHookEx(g_hHook);
    
    // already subclassed?
    if (g_bSubclassed) goto END;
    
    // Increase the reference count of the DLL (via LoadLibrary), so it's NOT unmapped once the hook is removed
    ::GetModuleFileNameA(hDll, lib_name, MAX_PATH);
    if (!::LoadLibraryA(lib_name)) goto END;
    
    // Subclass window
    if (::IsWindowUnicode(g_hWnd) == 0) {
      if ((g_OldProc = (WNDPROC)::SetWindowLongA(g_hWnd, GWL_WNDPROC, (long)NewProc)) == NULL)
      { ::FreeLibrary(hDll); goto END; }
    }
    else {
      if ((g_OldProc = (WNDPROC)::SetWindowLongW(g_hWnd, GWL_WNDPROC, (long)NewProc)) == NULL)
      { ::FreeLibrary(hDll); goto END; }
    }
    
    // Success
    ::MessageBeep(MB_OK);
    g_bSubclassed = true;
  }
  else if (bInjectMessage)
  {
    ::UnhookWindowsHookEx(g_hHook);
    
    // Unmap Dll
    if (!g_bSubclassed) return NULL;
    
    // If failed to restore old window procedure => don't unmap the DLL either.
    // Why? Because then process would call "unmapped" NewProc and crash!!
    if (::IsWindowUnicode(g_hWnd) == 0) {
      SendMessageA(g_hWnd, WM_TRAYME_TRAYNOTIFY, ID_TRAYME_TRAYEDWINDOW, WM_LBUTTONDBLCLK);
      if ((WNDPROC)SetWindowLongA(g_hWnd, GWL_WNDPROC, (long)g_OldProc) == 0) goto END;
    }
    else {
      SendMessageW(g_hWnd, WM_TRAYME_TRAYNOTIFY, ID_TRAYME_TRAYEDWINDOW, WM_LBUTTONDBLCLK);
      if ((WNDPROC)SetWindowLongW(g_hWnd, GWL_WNDPROC, (long)g_OldProc) == 0) goto END;
    }
    
    // Success
    g_bSubclassed = false;
    ::MessageBeep(MB_OK);
    ::FreeLibrary(hDll);
  }

END:
  
  return ::CallNextHookEx(g_hHook, code, wParam, lParam);
}



//-------------------------------------------------------------
// InjectDll
// Notice: 
//  - injects "HookInjEx.dll" into window (via SetWindowsHookEx);
//  - subclasses the window (see HookProc for more details);
//
//    Parameters: - hWnd = window handle
//
//    Return value:  1 - success;
//            0 - failure;
//

BOOL WINAPI InjectDll (HWND hWnd)
{
  static bool g_bIsSubclassing = false;
  
  // Failsafe
  if (g_bSubclassed) return TRUE;
  if (g_bIsSubclassing) return FALSE;
  g_bIsSubclassing = true;
  
  
  // Set global vars
  if (!::IsWindow(hWnd)) goto BAD;
  g_hWnd = hWnd;
  
  // Get window version
  if (!bWin9x_Known)
  { GetVersionWin9x(); bWin9x_Known = true; }
  
  // Hook window
  if (( g_hHook = ::SetWindowsHookExA(((bWin9x)?(WH_GETMESSAGE):(WH_CALLWNDPROC)), (HOOKPROC)HookProc, hDll, GetWindowThreadProcessId(hWnd, NULL)) ) == NULL)
    goto BAD;
  
  // By the time SendMessage returns, the window has already been subclassed
  if (bWin9x)
  {
    if (::IsWindowUnicode(g_hWnd))
    { while (!::PostMessageW(g_hWnd, WM_TRAYME_HOOKEX, 0, 1)); }
    else { while (!::PostMessageA(g_hWnd, WM_TRAYME_HOOKEX, 0, 1)); }
  }
  else
  {
    if (::IsWindowUnicode(g_hWnd))
    { ::SendMessageW(g_hWnd, WM_TRAYME_HOOKEX, 0, 1); }
    else { ::SendMessageA(g_hWnd, WM_TRAYME_HOOKEX, 0, 1); }
  }
  
  // Check for subclassing
  while (!g_bSubclassed);
  g_bIsSubclassing = false;
  
  return TRUE;
  

BAD:
  g_bIsSubclassing = false;
  return FALSE;
}


//-------------------------------------------------------------
// UnmapDll
// Notice: 
//  - restores the old window procedure for the window;
//  - unmapps the DLL from the remote process
//    (see HookProc for more details);
//
//    Return value:  1 - success;
//            0 - failure;
//

BOOL WINAPI UnmapDll ()
{
  static bool g_bIsUnSubclassing = false;
  
  // Failsafe
  if (!g_bSubclassed) return TRUE;
  if (g_bIsUnSubclassing) return FALSE;
  if (!IsWindow(g_hWnd)) { g_bSubclassed = false; return TRUE; }
  g_bIsUnSubclassing = true;
  
  // Get window version
  if (!bWin9x_Known) GetVersionWin9x();
  
  if (( g_hHook = ::SetWindowsHookExA(((bWin9x)?(WH_GETMESSAGE):(WH_CALLWNDPROC)), (HOOKPROC)HookProc, hDll, GetWindowThreadProcessId(g_hWnd,NULL)) ) == NULL)
    goto BAD;
  
  // By the time SendMessage returns, the window has already been subclassed
  if (bWin9x)
  {
    if (::IsWindowUnicode(g_hWnd))
    { while (!::PostMessageW(g_hWnd, WM_TRAYME_HOOKEX, 0, 0)); }
    else { while (!::PostMessageA(g_hWnd, WM_TRAYME_HOOKEX, 0, 0)); }
  }
  else
  {
    if (::IsWindowUnicode(g_hWnd))
    { ::SendMessageW(g_hWnd, WM_TRAYME_HOOKEX, 0, 0); }
    else { ::SendMessageA(g_hWnd, WM_TRAYME_HOOKEX, 0, 0); }
  }
  
  // Check for subclassing
  while (g_bSubclassed);
  g_bIsUnSubclassing = false;
  
  return TRUE;
  

BAD:
  g_bIsUnSubclassing = false;
  return FALSE;
}






//-------------------------------------------------------------
// DllTerminationProc
// Notice:  - Unmaps the Dll and exits the thread;
//  

DWORD WINAPI DllTerminationProc(LPVOID lpParameter)
{
  // Wait enough time for the Dll to stop being used
  Sleep(200);
  
  // No longer subclassed
  g_bSubclassed = false;
  
  // Double-beep for error
  ::MessageBeep(MB_OK);
  
  // Unmap DLL
  FreeLibraryAndExitThread(hDll, 0);
  return 0;
}






//-------------------------------------------------------------
// NewProc
// Notice:  - new window procedure for the window;
//      - it just swaps the left & right muse clicks;
//  

LRESULT CALLBACK NewProc (HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  LRESULT lResult;
  HANDLE hThread;
  
  union {
    NOTIFYICONDATAA nidA;
    NOTIFYICONDATAW nidW;
  };
  
  union {
    CHAR chTempA [1024];
    WCHAR chTempW [1024];
  };
  
  
  // Test tray removal
  if (uMsg == WM_TRAYME_TRAYNOTIFY)
  {
    
    // Subclassed window
    if (wParam == ID_TRAYME_TRAYEDWINDOW)
    {
      switch(lParam)
      {
        /* !!!!!
        case WM_RBUTTONUP:
          
          GetCursorPos(&pxy);
          
          // !!!!! Use menu trick to get rid of 'feature'
          TrackPopupMenuEx(hSysTrayPopup_Menu, (TPM_LEFTALIGN | TPM_TOPALIGN | TPM_RIGHTBUTTON), ((pxy.x) + 1), (pxy.y), hWnd, NULL);
          break;
        */
        
        case WM_LBUTTONDBLCLK:
          
          // Set up charset-specific info
          if (::IsWindowUnicode(hWnd)) {
            // Delete tray icon
            nidW.cbSize = sizeof (NOTIFYICONDATAW); // !!!!! NOTIFYICONDATA_V1_SIZE;
            nidW.hWnd = hWnd; nidW.uID = ID_TRAYME_TRAYEDWINDOW;
            if (::Shell_NotifyIconW(NIM_DELETE, &nidW) == 0) return false;
          }
          else {
            // Delete tray icon
            nidA.cbSize = sizeof (NOTIFYICONDATAA); // !!!!! NOTIFYICONDATA_V1_SIZE;
            nidA.hWnd = hWnd; nidA.uID = ID_TRAYME_TRAYEDWINDOW;
            if (::Shell_NotifyIconA(NIM_DELETE, &nidA) == 0) return false;
          }
          
          // Show window
          ::ShowWindow(hWnd, SW_SHOWNORMAL);
          g_bInTray = false;
          break;
      }
    }
    
    return 0;
    
  }
  else
  {
    
    switch (uMsg)
    {
      case WM_SYSCOMMAND:
        if (wParam != SC_CLOSE) break;
      case WM_CLOSE:
        
        // Get window version
        if (!bWin9x_Known)
        { GetVersionWin9x(); bWin9x_Known = true; }
        
        if (g_bInTray)
        { ::ShowWindow(hWnd, SW_HIDE); return 0; }

        // Set up charset-specific info
        if (::IsWindowUnicode(hWnd))
        {
          // Set up notify icon
          nidW.cbSize = sizeof (NOTIFYICONDATAW); // !!!!! NOTIFYICONDATAW_V1_SIZE;
          nidW.hWnd = hWnd; nidW.uID = ID_TRAYME_TRAYEDWINDOW;
          nidW.uCallbackMessage = WM_TRAYME_TRAYNOTIFY;
          nidW.uFlags = (NIF_ICON | NIF_MESSAGE | NIF_TIP);
          
          // Get text
          if (::GetWindowTextW(hWnd, chTempW, sizeof(chTempW)) == 0) chTempW[0] = NULL;
          wcscpy(nidW.szTip, chTempW);
          
          // !!!!! Create/copy icon
          if (((nidW.hIcon = (HICON)SendMessageW(hWnd, WM_GETICON, ((bWin9x)?(ICON_SMALL):(2)), 0)) == NULL) && ((nidW.hIcon = (HICON)GetClassLongW(hWnd, GCL_HICONSM)) == NULL))
            if (((nidW.hIcon = (HICON)SendMessageW(hWnd, WM_GETICON, ICON_BIG, 0)) == NULL) && ((nidW.hIcon = (HICON)GetClassLongW(hWnd, GCL_HICON)) == NULL))
              nidW.hIcon = (HICON)LoadImageW(NULL, MAKEINTRESOURCEW(32512), IMAGE_ICON, 0, 0, (LR_CREATEDIBSECTION));
          
          //if (((nidW.hIcon = (HICON)SendMessageW(hWnd, WM_GETICON, ((bWin9x)?(ICON_SMALL):(2)), 0)) == NULL) || ((nidW.hIcon = (HICON)GetClassLongW(hWnd, GCL_HICONSM)) == NULL))
          //  if (((nidW.hIcon = (HICON)SendMessageW(hWnd, WM_GETICON, ICON_BIG, 0)) == NULL) || ((nidW.hIcon = (HICON)GetClassLongW(hWnd, GCL_HICON)) == NULL))
          //    nidW.hIcon = (HICON)LoadImageW(NULL, MAKEINTRESOURCEW(32512), IMAGE_ICON, 0, 0, (LR_CREATEDIBSECTION));
          
          // Add to System Tray
          if (::Shell_NotifyIconW(NIM_ADD, &nidW) == 0) return 0;
        }
        else
        {
          // Set up notify icon
          nidA.cbSize = sizeof (NOTIFYICONDATAA); // !!!!! NOTIFYICONDATAA_V1_SIZE;
          nidA.hWnd = hWnd; nidA.uID = ID_TRAYME_TRAYEDWINDOW;
          nidA.uCallbackMessage = WM_TRAYME_TRAYNOTIFY;
          nidA.uFlags = (NIF_ICON | NIF_MESSAGE | NIF_TIP);
          
          // Get text
          if (::GetWindowTextA(hWnd, chTempA, sizeof(chTempA)) == 0) chTempA[0] = NULL;
          strcpy(nidA.szTip, chTempA);
          
          // !!!!! Create/copy icon
          if (((nidA.hIcon = (HICON)SendMessageA(hWnd, WM_GETICON, ((bWin9x)?(ICON_SMALL):(2)), 0)) == NULL) && ((nidA.hIcon = (HICON)GetClassLongA(hWnd, GCL_HICONSM)) == NULL))
            if (((nidA.hIcon = (HICON)SendMessageA(hWnd, WM_GETICON, ICON_BIG, 0)) == NULL) && ((nidA.hIcon = (HICON)GetClassLongA(hWnd, GCL_HICON)) == NULL))
              nidA.hIcon = (HICON)LoadImageA(NULL, MAKEINTRESOURCEA(32512), IMAGE_ICON, 0, 0, (LR_CREATEDIBSECTION));
          
          // Add to System Tray
          if (::Shell_NotifyIconA(NIM_ADD, &nidA) == 0) return 0;
        }
        
        // Hide window
        g_bInTray = true;
        ::ShowWindow(hWnd, SW_HIDE);
        return 0;
      
      case WM_DESTROY:
        
        // Unsubclass window
        if (::IsWindowUnicode(hWnd))
        { ::SetWindowLongW(g_hWnd, GWL_WNDPROC, (long)g_OldProc); }
        else { ::SetWindowLongA(g_hWnd, GWL_WNDPROC, (long)g_OldProc); }
        
        // Handle current message
        lResult = ::CallWindowProc(g_OldProc, hWnd, uMsg, wParam, lParam);
        
        // Create termination thread, then resume it and close the handle
        hThread = ::CreateThread(NULL, 0, DllTerminationProc, NULL, CREATE_SUSPENDED, 0);
        ::SetThreadPriority(hThread, THREAD_PRIORITY_IDLE);
        ::ResumeThread(hThread);
        ::CloseHandle(hThread);
        
        // Successful destruction of window and Dll unmapping
        return lResult;
    }
    
  }
  
  
  return ::CallWindowProc(g_OldProc, hWnd, uMsg, wParam, lParam);
}
