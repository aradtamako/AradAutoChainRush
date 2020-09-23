using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace AradAutoChainRush
{
    class Program
    {
        const string GameProcessName = "ARAD";

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private static Bitmap Bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        private static Graphics Graphics = Graphics.FromImage(Bmp);
        private static InputSimulator InputSimulator = new InputSimulator();

        static void PressKey(VirtualKeyCode keyCode)
        {
            InputSimulator.Keyboard.KeyDown(keyCode);
            Thread.Sleep(100);
            InputSimulator.Keyboard.KeyUp(keyCode);
        }

        static void Main(string[] args)
        {
            var pid = Process.GetProcessesByName(GameProcessName).FirstOrDefault().Id;
            var hWnd = Process.GetProcessesByName(GameProcessName).FirstOrDefault().MainWindowHandle;

            if (hWnd != IntPtr.Zero)
            {
                GetClientRect(hWnd, out var rect);

                if (rect.Width != 1600 || rect.Height != 900)
                {
                    Console.WriteLine("アラド戦記の解像度を1600x900に設定して下さい。");
                    return;
                }

                MoveWindow(hWnd, 0, 0, rect.Width, rect.Height, true);
            }

            Microsoft.VisualBasic.Interaction.AppActivate(pid);
            Thread.Sleep(1000);

            while (true)
            {
                Graphics.CopyFromScreen(956, 330, 0, 0, Bmp.Size, CopyPixelOperation.SourceCopy);
                var pixel = Bmp.GetPixel(0, 0);

                if (pixel.R == 255 && pixel.G == 255)
                {
                    PressKey(VirtualKeyCode.SPACE);
                    Console.WriteLine("press");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
