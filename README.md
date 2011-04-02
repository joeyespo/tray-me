TrayMe
------

A small application that allows you send a window to the notification area.
Targeting a window is as easy as drag and drop, as inspired by Spy++.


Usage
-----

Run the executable and target a window, or enter the window's handle, then
click "Tray Me!" This will hook into the process of the window. When you
"close" that window, it will become hidden and its icon will appear in the
notification area.

The TrayMe application can be hidden and even closed, and the hooked window
will remain hooked. To stop sending the window to the notification area, either
right-click its icon there and select "Un-Tray Me" or click the "Un-Tray Me"
button in the TrayMe application.

The application also accepts command line arguments for automatic hooking.

Usage:
TrayMe.exe [options] [target [cmd]]

Options:
 -h  Displays this help message.
 -s  Show window; do not close to tray.
 -q  Quiet mode; display no command line errors.
 -x  Do not exit when trayed by command line.

Here, target is the path to an executable to run and automatically tray,
and cmd is any command line arguments to pass to it.


Compatibility
-------------

This is known to work on Windows 98 and XP. Currently, only one window is
trayable at a time. However, more than one can be trayed by having multiple
copies of TrayMe.exe and the accompanying DLL in your file system.
