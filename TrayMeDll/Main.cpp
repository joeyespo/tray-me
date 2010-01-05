// TrayMe Dll
// By Joe Esposito
// 2004-07-26, 2010-01-01
// 
// HookInjEx by Robert Kuster
// 
// Note:
//   Use this at your own risk. No responsibilities for
//   possible damages of even functionality can be taken.
// 


#include "Main.h"


// Local Defines
// --------------

#ifdef _WIN64
# define SAFE_LONG_PTR  LONG_PTR
#else
# define SAFE_LONG_PTR  LONG
#endif

// Disable size [32/64 bit] conversion warnings
#pragma warning( disable : 4311 )
#pragma warning( disable : 4312 )



// Application Defines
// --------------------

#define ID_TRAYME_TRAYEDWINDOW    1001



// Shared Variables
// -----------------

// Notice: seen by all instances of this Dll

#pragma data_seg( ".shared" )

volatile bool g_bSubclassed = false;
UINT WM_TRAYME_HOOKEX = NULL;
UINT WM_TRAYME_TRAYNOTIFY = NULL;
HWND g_hWnd = NULL;
HHOOK g_hHook = NULL;
HMENU g_hMenu = NULL;
int g_nHotKeyAtom = 0;
int g_nHotKey = MOD_CONTROL | MOD_SHIFT;			// TODO: Make customizable
char g_chHotKey = 'M';

#pragma data_seg()

#pragma comment( linker, "/SECTION:.shared,RWS" )



// Global Variables
// -----------------

HINSTANCE  g_hDll;
bool g_bInTray = false;

// Version info
bool bWin9x_Known = false;
bool bWin9x = false;

// New & old window procedure of the subclassed window
WNDPROC g_OldProc = NULL;  
LRESULT CALLBACK NewProc( HWND, UINT, WPARAM, LPARAM );



// Local Function Declarations
//-----------------------------

bool LocalUnmapDll( bool bIndirectUnmap );

void GetVersionWin9x();
bool DoTrayContextMenu( HWND hWnd );
bool ShowTrayedWindow( HWND hWnd );
bool FreeLibraryAndExit();
DWORD WINAPI DllTerminationProc( LPVOID lpParameter );



// Global Functions
// -----------------

BOOL WINAPI IsSubclassed()
{
	return ( ( g_bSubclassed )?( TRUE ):( FALSE ) );
}



// Entry Point of Dll
// -------------------

BOOL APIENTRY DllMain( HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved )
{
	MessageBeep( MB_ICONASTERISK );

	if( ul_reason_for_call == DLL_PROCESS_ATTACH )
	{
		// TODO: Not supported in CE
		DisableThreadLibraryCalls( g_hDll = (HINSTANCE)hModule );

		// TODO: Not supported in CE
		// Register Ansi windows message
		if( WM_TRAYME_HOOKEX == NULL )
			WM_TRAYME_HOOKEX = RegisterWindowMessageA( "TRAYME@@WM_TRAYME_HOOKEX" );
		if( WM_TRAYME_TRAYNOTIFY == NULL )
			WM_TRAYME_TRAYNOTIFY = RegisterWindowMessageA( "TRAYME@@WM_TRAYME_TRAYNOTIFY" );
	}
	else if( ul_reason_for_call == DLL_PROCESS_DETACH )
	{
		if( g_hWnd )
		{
			if ( IsWindow( g_hWnd ) == FALSE )
				g_hWnd = NULL;
		}

		if( !g_hWnd )
			g_bSubclassed = false;
	}

	return TRUE;
}



//-------------------------------------------------------------
// HookProc
// Notice:
// - executed by the instance of "HookInjEx.dll" mapped into window;
// 
// When called from InjectDll():
//    - Sublasses the window
//    - Removes the hook, but the DLL stays in the remote process
//      though, because we increased its reference count via LoadLibray
//      (this way we disturb the target process as litle as possible)
// 
// When called from UnmapDll:
//    - Restores the old window procedure for the window
//    - Reduces the reference count of the DLL (via FreeLibrary)
//    - Removes the hook, so the DLL is unmapped
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

LRESULT HookProc( int code, WPARAM wParam, LPARAM lParam )
{
	char lib_name [MAX_PATH];
	bool bInjectMessage;

	// Failsafe
	if( g_hWnd == NULL )
		return CallNextHookEx( g_hHook, code, wParam, lParam );
	if( IsWindow( g_hWnd ) == 0 )
		return CallNextHookEx( g_hHook, code, wParam, lParam );

	// Get window version
	if( !bWin9x_Known )
	{
		GetVersionWin9x();
		bWin9x_Known = true;
	}

	// Get hook value
	if (bWin9x)
		bInjectMessage = (((LPMSG)lParam)->message == WM_TRAYME_HOOKEX);
	else
		bInjectMessage = (((LPCWPSTRUCT)lParam)->message == WM_TRAYME_HOOKEX);

	if( bInjectMessage )
	{
		bool bHook;

		// Get hook value
		if( bWin9x )
			bHook = ( ( (LPMSG)lParam )->wParam != 0 );
		else
			bHook = ( ( (LPCWPSTRUCT)lParam )->wParam != 0 );

		// Set window procedure
		if( bHook )
		{
			// Unhook the hook
			UnhookWindowsHookEx( g_hHook );
		  
			// already subclassed?
			if( g_bSubclassed )
				return CallNextHookEx( g_hHook, code, wParam, lParam );
		  
			// Increase the reference count of the DLL (via LoadLibrary), so it's NOT unmapped once the hook is removed
			GetModuleFileNameA(g_hDll, lib_name, MAX_PATH);
			if( !LoadLibraryA( lib_name ) )
				return CallNextHookEx( g_hHook, code, wParam, lParam );
		  
			// Subclass window
			if( IsWindowUnicode( g_hWnd ) == 0 )
			{
				if( ( g_OldProc = (WNDPROC)SetWindowLongPtrA( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)NewProc ) ) == NULL )
				{
					FreeLibrary( g_hDll );
					return CallNextHookEx( g_hHook, code, wParam, lParam );
				}
			}
			else
			{
				if ( ( g_OldProc = (WNDPROC)SetWindowLongPtrW( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)NewProc ) ) == NULL )
				{
					FreeLibrary( g_hDll );
					return CallNextHookEx( g_hHook, code, wParam, lParam );
				}
			}
		  
		  
			// Register hotkey
			g_nHotKeyAtom = GlobalAddAtom( "TrayMe HotKey" );
			if( g_nHotKeyAtom != 0 )
				RegisterHotKey( g_hWnd, g_nHotKeyAtom, g_nHotKey, g_chHotKey );
		  
			// Success
			MessageBeep( MB_OK );
			g_bSubclassed = true;
		}
		else
		{
			bool bIndirectUnhook;
		  
			if( bWin9x )
			{
				if( !bHook )
					bIndirectUnhook = ( ( (LPMSG)lParam )->lParam != 0 );
			}
			else
			{
				if( !bHook )
					bIndirectUnhook = ( ( (LPCWPSTRUCT)lParam )->lParam != 0 );
			}
		  
			// Unhook the hook
			UnhookWindowsHookEx( g_hHook );
		  
			// Unregister hotkey
			if( g_nHotKeyAtom != 0 )
			{
				UnregisterHotKey( g_hWnd, g_nHotKeyAtom );
				GlobalDeleteAtom( g_nHotKeyAtom );
				g_nHotKeyAtom = 0;
			}
		  
			// Unmap Dll
			if( !g_bSubclassed )
				return NULL;
		  
			// Show window (if not already shown)
			ShowTrayedWindow( g_hWnd );
		  
			// If failed to restore old window procedure => don't unmap the DLL either.
			// Why? Because then process would call "unmapped" NewProc and crash!!
			if( IsWindowUnicode( g_hWnd ) == 0 )
			{
				if( (WNDPROC)SetWindowLongPtrA( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)g_OldProc ) == 0 )
					return CallNextHookEx( g_hHook, code, wParam, lParam );
			}
			else
			{
				if( (WNDPROC)SetWindowLongPtrW( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)g_OldProc ) == 0 )
					return CallNextHookEx( g_hHook, code, wParam, lParam );
			}
		  
			// Success
			g_bSubclassed = false;
			MessageBeep( MB_OK );
		  
			if( bIndirectUnhook )
				FreeLibraryAndExit();
			else
				FreeLibrary( g_hDll );
		}
	}

	return CallNextHookEx( g_hHook, code, wParam, lParam );
}



//-------------------------------------------------------------
// InjectDll
// Notice: 
//  - Injects "TrayMeDll.dll" into window (via SetWindowsHookEx);
//  - subclasses the window (see HookProc for more details);
//
//    Parameters: - hWnd = window handle
//
//    Return value:  1 - success;
//            0 - failure;
//

BOOL WINAPI InjectDll( HWND hWnd )
{
	static bool g_bIsSubclassing = false;

	// Failsafe
	if( g_bSubclassed )
		return TRUE;
	if( g_bIsSubclassing )
		return FALSE;
	g_bIsSubclassing = true;


	// Set global vars
	if( !IsWindow( hWnd ) )
	{
		g_bIsSubclassing = false;
		return FALSE;
	}
	g_hWnd = hWnd;

	// Get window version
	if( !bWin9x_Known )
	{
		GetVersionWin9x();
		bWin9x_Known = true;
	}

	// Hook window
	if( ( g_hHook = SetWindowsHookExA( ( ( bWin9x )?( WH_GETMESSAGE ):( WH_CALLWNDPROC ) ), (HOOKPROC)HookProc, g_hDll, GetWindowThreadProcessId( hWnd, NULL ) ) ) == NULL )
	{
		g_bIsSubclassing = false;
		return FALSE;
	}

	// By the time SendMessage returns, the window has already been subclassed
	if( bWin9x )
	{
		if( IsWindowUnicode( g_hWnd ) )
		{
			while( !PostMessageW( g_hWnd, WM_TRAYME_HOOKEX, 1, 0 ) )
			{ }
		}
		else
		{
			while( !PostMessageA( g_hWnd, WM_TRAYME_HOOKEX, 1, 0 ) )
			{ }
		}
	}
	else
	{
		if( IsWindowUnicode( g_hWnd ) )
			SendMessageW( g_hWnd, WM_TRAYME_HOOKEX, 1, 0 );
		else
			SendMessageA( g_hWnd, WM_TRAYME_HOOKEX, 1, 0 );
	}

	// Check for subclassing
	while( !g_bSubclassed )
	{ }

	g_bIsSubclassing = false;
	return TRUE;
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

BOOL WINAPI UnmapDll()
{
	return ( ( LocalUnmapDll( false ) )?( TRUE ):( FALSE ) );
}

bool LocalUnmapDll( bool bIndirectUnmap )
{
	static bool g_bIsUnSubclassing = false;

	// Failsafe
	if( !g_bSubclassed )
		return TRUE;
	if( g_bIsUnSubclassing )
		return FALSE;
	if( !IsWindow( g_hWnd ))
	{
		g_bSubclassed = false;
		return TRUE;
	}
	g_bIsUnSubclassing = true;

	// Get window version
	if( !bWin9x_Known )
		GetVersionWin9x();

	if( ( g_hHook = SetWindowsHookExA( ( ( bWin9x )?( WH_GETMESSAGE ):( WH_CALLWNDPROC ) ), (HOOKPROC)HookProc, g_hDll, GetWindowThreadProcessId( g_hWnd, NULL ) ) ) == NULL )
	{
		g_bIsUnSubclassing = false;
		return FALSE;
	}

	// By the time SendMessage returns, the window has already been subclassed
	if( bWin9x )
	{
		if( IsWindowUnicode( g_hWnd ) )
		{
			while( !PostMessageW( g_hWnd, WM_TRAYME_HOOKEX, 0, ( ( bIndirectUnmap )?( 1 ):( 0 ) ) ) )
			{ }
		}
		else
		{
			while( !PostMessageA( g_hWnd, WM_TRAYME_HOOKEX, 0, ( ( bIndirectUnmap )?( 1 ):( 0 ) ) ) )
			{ }
		}
	}
	else
	{
		if( IsWindowUnicode( g_hWnd ) )
			SendMessageW( g_hWnd, WM_TRAYME_HOOKEX, 0, ( ( bIndirectUnmap )?( 1 ):( 0 ) ) );
		else
			SendMessageA( g_hWnd, WM_TRAYME_HOOKEX, 0, ( ( bIndirectUnmap )?( 1 ):( 0 ) ) );
	}

	// Check for subclassing
	while( g_bSubclassed )
	{ }

	g_bIsUnSubclassing = false;
	g_hWnd = NULL;
	return TRUE;
}



// Subclassed Windows Procedure
// -----------------------------

LRESULT CALLBACK NewProc (HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	LRESULT lResult;

	union
	{
		NOTIFYICONDATAA nidA;
		NOTIFYICONDATAW nidW;
	};

	union
	{
		CHAR chTempA [1024];
		WCHAR chTempW [1024];
	};


	// Test tray removal
	if( uMsg == WM_TRAYME_TRAYNOTIFY )
	{
		// Subclassed window
		if (wParam == ID_TRAYME_TRAYEDWINDOW)
		{
			switch(lParam)
			{
				case WM_RBUTTONUP:
					DoTrayContextMenu( hWnd );
					break;
		    
				case WM_LBUTTONDBLCLK:
					if( ShowTrayedWindow( hWnd ) )
						return TRUE;
					break;
			}
		}

		return 0;
	}
	else if( uMsg == WM_HOTKEY )
	{
		if( (int)wParam == g_nHotKeyAtom )
		{
			if( IsWindow( hWnd ) )
			{
				if( IsWindowVisible( hWnd ) )
					SendMessage( hWnd, WM_CLOSE, 0, 0 );
				else
					ShowTrayedWindow( hWnd );

				return 0;
			}
		}
	}
	else
	{
		switch (uMsg)
		{
			// Workaround to Outlook Express 'feature'
			case WM_DISPLAYCHANGE:
				if( IsWindowVisible( hWnd ) == FALSE )
					return 0;
				break;
			  
			case WM_SYSCOMMAND:
				if( wParam != SC_CLOSE )
					break;
			case WM_CLOSE:
				// Get window version
				if ( !bWin9x_Known )
				{
					GetVersionWin9x();
					bWin9x_Known = true;
				}
			    
				if( g_bInTray )
				{
					ShowWindow( hWnd, SW_HIDE );
					return 0;
				}
			    
				// Set up charset-specific info
				if( IsWindowUnicode( hWnd ) )
				{
					// Set up notify icon
					nidW.cbSize = sizeof (NOTIFYICONDATAW);
					nidW.hWnd = hWnd; nidW.uID = ID_TRAYME_TRAYEDWINDOW;
					nidW.uCallbackMessage = WM_TRAYME_TRAYNOTIFY;
					nidW.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
			      
					// Get text
					rsize_t chTempW_size = sizeof( chTempW );
					if( GetWindowTextW(hWnd, chTempW, (int)chTempW_size ) == 0) chTempW[0] = NULL;
						wcsncpy_s( nidW.szTip, chTempW, chTempW_size );
			      
					// Create/copy icon
					if( ( ( nidW.hIcon = (HICON)SendMessageW( hWnd, WM_GETICON, ( ( bWin9x ) ? ( ICON_SMALL ) : ( 2 ) ), 0 ) ) == NULL ) && ( ( nidW.hIcon = (HICON)GetClassLongW( hWnd, GCL_HICONSM ) ) == NULL ) )
						if ( ( ( nidW.hIcon = (HICON)SendMessageW( hWnd, WM_GETICON, ICON_BIG, 0 ) ) == NULL ) && ( ( nidW.hIcon = (HICON)GetClassLongW( hWnd, GCL_HICON ) ) == NULL ) )
							nidW.hIcon = (HICON)LoadImageW( NULL, MAKEINTRESOURCEW( 32512 ), IMAGE_ICON, 0, 0, LR_CREATEDIBSECTION );
			      
					// Add to System Tray
					if( Shell_NotifyIconW( NIM_ADD, &nidW ) == 0 )
						return 0;
				}
				else
				{
					// Set up notify icon
					nidA.cbSize = sizeof( NOTIFYICONDATAA );
					nidA.hWnd = hWnd; nidA.uID = ID_TRAYME_TRAYEDWINDOW;
					nidA.uCallbackMessage = WM_TRAYME_TRAYNOTIFY;
					nidA.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
			      
					// Get text
					size_t chTempA_size = sizeof( chTempA );
					if( GetWindowTextA( hWnd, chTempA, (int)chTempA_size ) == 0 )
						chTempA[0] = NULL;
					strncpy_s( nidA.szTip, chTempA, chTempA_size );
			      
					// Create/copy icon
					if( ( ( nidA.hIcon = (HICON)SendMessageA( hWnd, WM_GETICON, ( (bWin9x) ? (ICON_SMALL) : ( 2 ) ), 0 ) ) == NULL ) && ( ( nidA.hIcon = (HICON)GetClassLongA( hWnd, GCL_HICONSM ) ) == NULL ) )
						if( ( ( nidA.hIcon = (HICON)SendMessageA( hWnd, WM_GETICON, ICON_BIG, 0 ) ) == NULL ) && ( ( nidA.hIcon = (HICON)GetClassLongA( hWnd, GCL_HICON ) ) == NULL ) )
							nidA.hIcon = (HICON)LoadImageA( NULL, MAKEINTRESOURCEA( 32512 ), IMAGE_ICON, 0, 0, LR_CREATEDIBSECTION );
			      
					// Add to System Tray
					if( Shell_NotifyIconA( NIM_ADD, &nidA ) == 0 )
						return 0;
				}
			    
				// Hide window
				g_bInTray = true;
				ShowWindow( hWnd, SW_HIDE );
				return 0;
			  
			case WM_DESTROY:
				// Unregister hotkey
				if( g_nHotKeyAtom != 0 )
				{
					UnregisterHotKey(g_hWnd, g_nHotKeyAtom);
					GlobalDeleteAtom(g_nHotKeyAtom);
					g_nHotKeyAtom = 0;
				}
			    
				// Unsubclass window
				if( IsWindowUnicode( hWnd ) )
					SetWindowLongPtrW( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)g_OldProc );
				else
					SetWindowLongPtrA( g_hWnd, GWL_WNDPROC, (SAFE_LONG_PTR)g_OldProc );
			    
				// Handle current message
				lResult = CallWindowProc( g_OldProc, hWnd, uMsg, wParam, lParam );
			    
				// Unmap the library and exit in separate thread
				FreeLibraryAndExit();
			    
				// Successful destruction of window and Dll unmapping
				return lResult;
		}
	}

	return CallWindowProc( g_OldProc, hWnd, uMsg, wParam, lParam );
}



// Local Functions
// ----------------

void GetVersionWin9x()
{
	OSVERSIONINFO osvi;

	// Get version info
	osvi.dwOSVersionInfoSize = sizeof( OSVERSIONINFO );
	GetVersionEx( &osvi );

	// Get Win9x value
	bWin9x = ( ( osvi.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS ) && ( osvi.dwMajorVersion == 4 ) );
}

bool DoTrayContextMenu( HWND hWnd )
{
	HMENU hMenu;

	if( ( hMenu= CreatePopupMenu() ) == NULL )
		return false;

	if( IsWindowUnicode( hWnd ) )
	{
		MENUITEMINFOW miiw;
		ZeroMemory(&miiw, sizeof(miiw));
		miiw.cbSize = sizeof(miiw);
		miiw.wID = (WORD)(1);
		if( bWin9x )
		{
			miiw.fMask = (MIIM_ID | MIIM_STATE | MIIM_TYPE);
			miiw.fType = (MFT_STRING);
			miiw.fState = (MFS_ENABLED | MFS_DEFAULT);
		}
		else
		{
			miiw.fMask = (MIIM_ID | MIIM_STATE | MIIM_STRING);
			miiw.fState = (MFS_ENABLED | MFS_DEFAULT);
		}
		miiw.dwTypeData = L"&Show";
		miiw.cch = 4;
		InsertMenuItemW(hMenu, 0, TRUE, &miiw);

		AppendMenuW(hMenu, (MF_SEPARATOR), 0, NULL);
		AppendMenuW(hMenu, (MF_STRING | MF_ENABLED), 2, L"Un&tray Me");
	}
	else
	{
		MENUITEMINFOA miia;
		ZeroMemory( &miia, sizeof( miia ) );
		miia.cbSize = sizeof( miia ); miia.wID = (WORD)( 1 );
		if ( bWin9x )
		{
			miia.fMask = MIIM_ID | MIIM_STATE | MIIM_TYPE;
			miia.fType = MFT_STRING;
			miia.fState = MFS_ENABLED | MFS_DEFAULT;
		}
		else
		{
			miia.fMask = MIIM_ID | MIIM_STATE | MIIM_STRING;
			miia.fState = MFS_ENABLED | MFS_DEFAULT;
		}
		miia.dwTypeData = "&Show";
		miia.cch = 4;
		InsertMenuItemA( hMenu, 0, TRUE, &miia );

		AppendMenuA( hMenu, MF_SEPARATOR, 0, NULL );
		AppendMenuA( hMenu, MF_STRING | MF_ENABLED, 2, "Un&tray Me" );
	}

	// HACK: Use menu trick to get rid of 'feature'
	POINT pt;
	GetCursorPos( &pt );
	INT nResult = TrackPopupMenuEx( hMenu, TPM_RETURNCMD | TPM_NONOTIFY | TPM_LEFTALIGN | TPM_TOPALIGN | TPM_RIGHTBUTTON, ( pt.x + 1), pt.y, hWnd, NULL );
	DestroyMenu( hMenu );

	// Get selection
	switch( nResult )
	{
		case 1:
			ShowTrayedWindow( hWnd );
			break;

		case 2:
			LocalUnmapDll( true );
			break;
	}

	// Success
	return ( nResult != 0 );
}

bool ShowTrayedWindow (HWND hWnd)
{
	union
	{
		NOTIFYICONDATAA nidA;
		NOTIFYICONDATAW nidW;
	};

	if ( !g_bInTray )
		return false;

	// Get window version
	if ( !bWin9x_Known )
	{
		GetVersionWin9x();
		bWin9x_Known = true;
	}

	// Set up charset-specific info
	if( IsWindowUnicode( hWnd ) )
	{
		// Delete tray icon
		nidW.cbSize = sizeof( NOTIFYICONDATAW );
		nidW.hWnd = hWnd; nidW.uID = ID_TRAYME_TRAYEDWINDOW;
		if( Shell_NotifyIconW( NIM_DELETE, &nidW ) == 0 )
			return false;
	}
	else
	{
		// Delete tray icon
		nidA.cbSize = sizeof( NOTIFYICONDATAA );
		nidA.hWnd = hWnd; nidA.uID = ID_TRAYME_TRAYEDWINDOW;
		if( Shell_NotifyIconA( NIM_DELETE, &nidA ) == 0 )
			return false;
	}

	// Show window
	ShowWindow( hWnd, SW_SHOWNORMAL );
	SetForegroundWindow( hWnd );
	g_bInTray = false;

	return true;
}

// Unmaps the Dll and creates a new thread for exiting
bool FreeLibraryAndExit()
{
	HANDLE hThread;

	// Create termination thread, then resume it and close the handle
	hThread = CreateThread( NULL, 0, DllTerminationProc, (LPVOID)g_hDll, CREATE_SUSPENDED, 0 );
	SetThreadPriority( hThread, THREAD_PRIORITY_IDLE );
	ResumeThread( hThread );
	CloseHandle( hThread );

	return true;
}

// Unmaps the Dll and exits the thread
DWORD WINAPI DllTerminationProc( LPVOID lpParameter )
{
	// TODO: WaitForSingleObject() ?
	// Wait enough time for the DLL to stop being used
	Sleep( 200 );

	// No longer subclassed
	g_bSubclassed = false;

	// Double-beep for error
	MessageBeep( MB_OK );

	// Unmap DLL
	FreeLibraryAndExitThread( (HMODULE)lpParameter, 0 );
	return 0;
}
