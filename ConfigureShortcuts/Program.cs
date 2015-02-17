using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

/* Install or uninstall writelog shortcuts.
 * Two things to do:
 * set/remove writelog.ini entries
 * copy/delete dll file
 * 
 * Assumes the root namespace in the assembly matches the name of its containing dll
 */

namespace ConfigureShortcuts
{
    class Program
    {
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                Console.WriteLine("useage ConfigureShortcuts <filename-without-.dll> <classname> [/uninstall]");
                return;
            }

            bool install = true;
            if (args.Length == 3)
            {
                if (String.Compare(args[2], "/uninstall", true) == 0)
                    install = false;
                else if (String.Compare(args[2], "/install", true) == 0)
                    install = true;
                else
                {
                    Console.WriteLine("Third parameter \"" + args[2] + "\" must be either /uninstall or /install");
                    return;
                }
            }

            String assembly = args[0];
            String classname = args[1];
            const int sbs = 512;
            StringBuilder sb = new StringBuilder(sbs);
            GetPrivateProfileString("Install", "Directory", "", sb, sbs, "writelog.ini");

            if (sb.Length == 0)
            {
                Console.WriteLine("ConfigureShortcuts failed to read writelog install directory");
                return;
            }

            String programs = sb.ToString() + "Programs";

            if (install)
            {
                // command line to cmd.exe. what a mess
                string cpyargs =
                    "/c \"CopyShortcutAssembly.exe /install \"" + 
                    assembly + ".dll\" \"" + programs + "\"\"";

                System.Diagnostics.Process p =
                    System.Diagnostics.Process.Start("cmd.exe", cpyargs);
                p.WaitForExit();

                WritePrivateProfileString("Configuration", "ExtShortcutAssembly", assembly, "writelog.ini");
                WritePrivateProfileString("Configuration", "ExtShortcutClass", 
                    assembly + "." + classname, "writelog.ini");
            }
            else
            {
                WritePrivateProfileString("Configuration", "ExtShortcutAssembly", null, "writelog.ini");
                WritePrivateProfileString("Configuration", "ExtShortcutClass", null, "writelog.ini");

                string cpyargs =
                    "/c \"CopyShortcutAssembly.exe /uninstall \"" + 
                    assembly + ".dll\" \"" + programs + "\"";

                System.Diagnostics.Process p =
                    System.Diagnostics.Process.Start("cmd.exe", cpyargs);
                p.WaitForExit();
            }
        }
    }
}
