using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TrayMe
{
    /// <summary>
    /// Represents a ...
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            // TODO: Load custom settings

            IntPtr hWndInitial = IntPtr.Zero;

            // Attach to app
            if (args.Length > 0)
            {
                bool noExit = false;
                bool show = false;
                bool quiet = false;
                var target = "";
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

                            case "-x":
                                noExit = true;
                                continue;
                            
                            default:
                                continue;
                        }
                    }

                    target = args[i];

                    while (Path.GetExtension(target).ToLower() == ".lnk")
                        target = (new System.ShellShortcut.ShellShortcut(target)).Path;

                    if (target == "")
                        continue;

                    cmd = i + 1 < args.Length
                        ? string.Join(" ", args, i + 1, args.Length - 1).Trim()
                        : "";

                    break;
                }

                if (target != null)
                {
                    if (Path.GetFullPath(target).ToLower() == Path.GetFullPath(Application.ExecutablePath).ToLower())
                    {
                        if (!quiet)
                            MessageBox.Show("Cannot tray self.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
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
                        }
                        else
                        {
                            // Wait for idle input .. wait for application to be done loading
                            process.WaitForInputIdle();
                            process.Refresh();

                            // Attach to app's main window
                            if (process.HasExited)
                            {
                                if (!quiet)
                                    MessageBox.Show("Application exited before it could be trayed.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {

                                IntPtr hWnd = process.MainWindowHandle;
                                if (hWnd == IntPtr.Zero)
                                {
                                    if (!quiet)
                                        MessageBox.Show("Could not find application's main window.", "TrayMe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    hWndInitial = hWnd;
                                    if (!show)
                                    {
                                        Win32.WINDOWPLACEMENT wp = new Win32.WINDOWPLACEMENT();
                                        Win32.GetWindowPlacement(hWnd, ref wp);

                                        // Hide (close) window only if still minimized
                                        if ((wp.showCmd == Win32.SW_MINIMIZE) || (wp.showCmd == Win32.SW_SHOWMINIMIZED))
                                            Win32.SendMessage(hWnd, Win32.WM_CLOSE, 0, 0);
                                    }
                                }
                            }
                        }
                    }
                }

                // Exit if done
                if (!noExit)
                    return;
            }

            using (MainForm form = new MainForm())
            {
                if (hWndInitial != IntPtr.Zero)
                    form.DoAttach(hWndInitial);
                Application.Run(form);
            }
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
            sb.AppendLine(" -h  --  displays this help message.");
            sb.AppendLine(" -s  --  show window; do not close to tray.");
            sb.AppendLine(" -q  --  quiet mode; display no command line errors.");
            sb.AppendLine(" -x  --  do not exit when trayed by command line.");
            sb.AppendLine();
            sb.AppendLine("target  --  the target application to tray.");
            sb.AppendLine("cmd     --  the command line to be passed to the target.");

            MessageBox.Show(sb.ToString(), "TrayMe Command Line Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
