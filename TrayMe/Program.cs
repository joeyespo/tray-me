using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TrayMe
{
    /// <summary>
    /// Represents the TrayMe application.
    /// </summary>
    internal static class Program
    {
        delegate T TryFunction<T>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            // TODO: Load custom settings

            var hWnd = IntPtr.Zero;
            var andExit = true;

            // Parse arguments
            if (args.Length > 0)
            {
                var show = false;
                var quiet = false;
                var target = "";
                var title = "";
                var cmd = "";

                // Get cmdline options
                int i = 0;
                for (i = 0; i < args.Length; i++)
                {
                    if (args[i].Length == 0)
                        continue;

                    if (args[i][0] == '-')
                    {
                        // Get individual options
                        switch (args[i].ToLower())
                        {
                            case "-h":
                                ShowHelp();
                                continue;

                            case "-q":
                                quiet = true;
                                continue;

                            case "-s":
                                show = true;
                                continue;

                            case "-t":
                                if (i + 1 >= args.Length)
                                {
                                    MessageBox.Show("Argument '-t' requires a value.");
                                    return;
                                }
                                else
                                {
                                    i++;
                                    title = args[i];
                                }
                                continue;

                            case "-x":
                                andExit = false;
                                continue;
                            
                            default:
                                continue;
                        }
                    }

                    target = args[i++];
                    cmd = i < args.Length
                        ? string.Join(" ", args, i, args.Length - i).Trim()
                        : "";
                    break;
                }

                // Attach to app
                if (!string.IsNullOrEmpty(target))
                {
                    var process = RunProcess(target, cmd, show, quiet);
                    if (process != null)
                    {
                        hWnd = FindWindow(process, title, quiet);
                        if (hWnd != IntPtr.Zero)
                        {
                            if (!show)
                                HideWindow(hWnd);
                        }
                    }

                    // Exit if done
                    if (hWnd == IntPtr.Zero && andExit)
                        return;
                }
            }

            using (MainForm form = new MainForm())
            {
                // Attach and exit, or show application
                if (hWnd != IntPtr.Zero)
                {
                    form.DoAttach(hWnd);
                    if (andExit)
                        return;
                }

                Application.Run(form);
            }
        }

        static Process RunProcess(string target, string cmd, bool show, bool quiet)
        {
            if (!string.IsNullOrEmpty(target))
            {
                while (Path.GetExtension(target).ToLower() == ".lnk")
                    target = (new System.ShellShortcut.ShellShortcut(target)).Path;

                if (string.IsNullOrEmpty(target))
                {
                    if (!quiet)
                        MessageBox.Show("Could not follow shortcut.");
                    return null;
                }
            }

            if (Path.GetFullPath(target).ToLower() == Path.GetFullPath(Application.ExecutablePath).ToLower())
            {
                if (!quiet)
                    MessageBox.Show("Cannot tray self.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Start app process
            ProcessStartInfo startInfo = new ProcessStartInfo(target, cmd);
            if (!show)
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            Process process = null;
            try
            {
                process = Process.Start(startInfo);
            }
            catch (Win32Exception)
            {
            }
            if (process == null)
            {
                if (!quiet)
                    MessageBox.Show("Application could not be started.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Wait for idle input .. wait for application to be done loading
            process.WaitForInputIdle();
            process.Refresh();
            return process;
        }

        static IntPtr FindWindow(Process process, string title, bool quiet)
        {
            // Attach to app's main window
            if (process.HasExited)
            {
                if (!quiet)
                    MessageBox.Show("Application exited before it could be trayed.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return IntPtr.Zero;
            }

            if (!string.IsNullOrEmpty(title))
            {
                var hWnd = TryMany<IntPtr>(() => Win32Ex.FindWindowByPartialTitle(title));
                if (hWnd == IntPtr.Zero && !quiet)
                    MessageBox.Show("Could not find application's \"" + title + "\" window.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return hWnd;
            }

            var hMainWindow = process.MainWindowHandle;
            if (hMainWindow == IntPtr.Zero && !quiet)
                MessageBox.Show("Could not find application's main window.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return hMainWindow;
        }

        static void HideWindow(IntPtr hWnd)
        {
            Win32.WINDOWPLACEMENT wp = new Win32.WINDOWPLACEMENT();
            Win32.GetWindowPlacement(hWnd, ref wp);

            // Hide (close) window only if still minimized
            if ((wp.showCmd == Win32.SW_MINIMIZE) || (wp.showCmd == Win32.SW_SHOWMINIMIZED))
                Win32.SendMessage(hWnd, Win32.WM_CLOSE, 0, 0);
        }

        static T TryMany<T>(TryFunction<T> run, int tries = 5, int millisecondsTimeout = 1000)
        {
            for(int t = 0; t < tries; t++)
            {
                var result = run();
                if (!EqualityComparer<T>.Default.Equals(result, default(T)))
                    return result;
                Thread.Sleep(millisecondsTimeout);
            }

            return default(T);
        }

        /// <summary>
        /// Shows the help message.
        /// </summary>
        public static void ShowHelp()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Usage:");
            sb.Append(Path.GetFileName(Application.ExecutablePath));
            sb.AppendLine(" [options] target cmd");
            sb.AppendLine();

            sb.AppendLine("Options:");
            sb.AppendLine(" -h          -  displays this help message.");
            sb.AppendLine(" -s          -  show window; do not close to tray.");
            sb.AppendLine(" -t=<title>  -  tray title instead of main window.");
            sb.AppendLine(" -q          -  quiet mode; display no command line errors.");
            sb.AppendLine(" -x          -  do not exit when trayed by command line.");
            sb.AppendLine();
            sb.AppendLine("target  --  the target executable to tray.");
            sb.AppendLine("cmd     --  the command line to be passed to the target.");

            MessageBox.Show(sb.ToString(), "TrayMe Command Line Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
