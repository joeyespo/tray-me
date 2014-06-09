// TrayMe.h
// By Joe Esposito


#pragma once

#include <windows.h>

#define HOOKDLL_API __declspec(dllexport)

// Public functions
HOOKDLL_API BOOL WINAPI IsSubclassed();
HOOKDLL_API BOOL WINAPI InjectDll(HWND hWnd);
HOOKDLL_API BOOL WINAPI UnmapDll();
