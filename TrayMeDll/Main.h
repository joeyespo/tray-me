/***************************************************************
Module name: HookInjEx_DLL.h
Copyright (c) 2003 Robert Kuster

Notice:	If this code works, it was written by Robert Kuster.
		Else, I don't know who wrote it.

		Use it on your own risk. No responsibilities for
		possible damages of even functionality can be taken.
***************************************************************/
#if !defined INJECT_EX__H
#define INJECT_EX__H

#include <windows.h>

#define HOOKDLL_API __declspec(dllexport)


HOOKDLL_API BOOL WINAPI IsSubclassed ();
HOOKDLL_API BOOL WINAPI InjectDll ( HWND hWnd );
HOOKDLL_API BOOL WINAPI UnmapDll ();

#endif // !defined(INJECT_EX__H)
