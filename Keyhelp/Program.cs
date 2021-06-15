using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Colorful;
using Console = Colorful.Console;

namespace Keyhelp
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentExe = new Executable(fullPath: null);

            while (true)
            {
                var foregroundExe = GetForegroundExe();
                if (!foregroundExe.Equals(currentExe))
                {
                    currentExe = foregroundExe;

                    Console.Clear();

                    ShowBanner(currentExe);

                    ShowHelp(currentExe);
                }

                Thread.Sleep(1000);
            }
        }

        private static void ShowHelp(Executable foregroundExe)
        {
            var helpFile = $"{foregroundExe.Filename}.md";
            var helpText = "";
            if (File.Exists(helpFile))
                helpText = File.ReadAllText(helpFile);

            if (string.IsNullOrWhiteSpace(helpText)) return;

            var styleSheet = new StyleSheet(Color.White);
            styleSheet.AddStyle("</kbd>[+]", Color.Gray, match => " + ");
            styleSheet.AddStyle("</?kbd>", Color.Gray, match => "");

            Console.WriteLineStyled(helpText, styleSheet);
        }

        private static void ShowBanner(Executable exe)
        {
            if (string.IsNullOrWhiteSpace(exe?.Filename)) return;

            var figlet = new Figlet(FigletFont.Load("ogre.flf"));
            
            Console.WriteLine(figlet.ToAscii(exe.FilenameShort), Color.GreenYellow);
        }

        private static Executable GetForegroundExe()
        {
            return new(GetProcessPath(PInvoke.GetForegroundWindow()));
        }

        private static string GetProcessPath(IntPtr hWnd)
        {
            try
            {
                PInvoke.GetWindowThreadProcessId(hWnd, out var pid);
                if (pid == 0) return null;
                var proc = Process.GetProcessById((int) pid);
                return proc?.MainModule?.FileName;
            }
            catch
            {
                return null;
            }
        }
    }
}