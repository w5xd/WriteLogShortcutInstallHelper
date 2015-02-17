using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * This program must run asAdministrator because it copies into WriteLog's installation directory.
 * The app.manifest assures that.
 * It just copies (or deletes) one file
 */

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            const int ThreadWait = 2000;
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: CopyShortcutAssembly <[/install] | [/uninstall]> <dll-file-path> <writelog-install-dir>");
                System.Threading.Thread.Sleep(ThreadWait);
                return;
            }

            bool install = true;

            if (String.Compare(args[0], "/install", true) == 0)
                install = true;
            else if (String.Compare(args[0], "/uninstall", true) == 0)
                install = false;
            else
            {
                Console.WriteLine("Invalid first argument \"" + args[0] + "\" must be either /uninstall or /install");
                System.Threading.Thread.Sleep(ThreadWait);
                return;
            }

            string src = args[1];
            string tgt = args[2];

            System.IO.FileAttributes fa = System.IO.File.GetAttributes(tgt);
            if ((fa & System.IO.FileAttributes.Directory) !=
                    System.IO.FileAttributes.Directory)
            {
                Console.WriteLine("CopyShortcutAssembly target " + tgt + " must be directory");
                System.Threading.Thread.Sleep(ThreadWait);
                return;
            }

            try
            {
                System.IO.FileInfo fit = new System.IO.FileInfo(tgt);

                if (install)
                {
                    System.IO.FileInfo fis = new System.IO.FileInfo(src);

                    if (!System.IO.File.Exists(src))
                    {
                        Console.WriteLine("CopyShortcutAssembly file " + src + " not found");
                        System.Threading.Thread.Sleep(ThreadWait);
                        return;
                    }

                    string target = fit.FullName + "\\" + fis.Name;
                    Console.WriteLine("Copying to " + target);
                    System.IO.File.Copy(fis.FullName, target, true);
                }
                else
                {
                    string target = fit.FullName + "\\" + src;
                    Console.WriteLine("Deleting " + target);
                    if (System.IO.File.Exists(target))
                        System.IO.File.Delete(target);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            System.Threading.Thread.Sleep(ThreadWait);
        }
    }
}
