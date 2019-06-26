using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace UdpServerHomeWork
{
    public enum CaptureMode
    {
        Screen, Window
    }

    public static class ScreenCapturer
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        public static void CaptureAndSave(string filename, CaptureMode mode = CaptureMode.Window, ImageFormat format = null)
        {
            ImageSave(filename, format, Capture(mode));
        }

        public static void CaptureAndSave(string filename, IntPtr handle, ImageFormat format = null)
        {
            ImageSave(filename, format, Capture(handle));
        }

        public static void CaptureAndSave(string filename, Control c, ImageFormat format = null)
        {
            ImageSave(filename, format, Capture(c));
        }

        public static Bitmap Capture(CaptureMode mode = CaptureMode.Window)
        {
            return Capture(mode == CaptureMode.Screen ? GetDesktopWindow() : GetForegroundWindow());
        }

        public static Bitmap Capture(Control c)
        {
            return Capture(c.Handle);
        }

        public static Bitmap Capture(IntPtr handle)
        {
            Rectangle bounds;
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);

            var result = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(result))
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

            return result;
        }

        public static Point CursorPosition;

        static void ImageSave(string filename, ImageFormat format, Image image)
        {
            format = format ?? ImageFormat.Png;
            if (!filename.Contains("."))
                filename = filename.Trim() + "." + format.ToString().ToLower();

            if (!filename.Contains(@"\"))
                filename = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Temp", filename);

            filename = filename.Replace("%NOW%", DateTime.Now.ToString("yyyy-MM-dd@hh.mm.ss"));
            image.Save(filename, format);
        }
    }
}
