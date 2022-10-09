using System.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace UmaWglSongTool.Utility
{
    public class CaptureHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public void GetWindowCaptureByName(string ProcessName, string fileName)
        {
            Rect NotepadRect = GetWindowPosition(ProcessName);
            var bit = GetWindowCapture(NotepadRect);
            bit = Threshold(bit);
            bit.Save(fileName);
        }

        private Bitmap GetWindowCapture(Rect rect)
        {
            int Width = rect.Right - rect.Left;
            int Height = Convert.ToInt32((rect.Bottom - rect.Top) * 0.12);
            int X = rect.Left;
            int Y = rect.Top + Convert.ToInt32((rect.Bottom - rect.Top) * 0.73); ;

            Bitmap myImage = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(myImage);
            graphics.CopyFromScreen(new System.Drawing.Point(X, Y), new System.Drawing.Point(0, 0), new System.Drawing.Size(Width, Height));
            IntPtr dc1 = graphics.GetHdc();
            graphics.ReleaseHdc(dc1);

            return myImage;
        }

        private Rect GetWindowPosition(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            Process target = processes[0];
            IntPtr ptr = target.MainWindowHandle;
            Rect NotepadRect = new Rect();
            GetWindowRect(ptr, ref NotepadRect);
            return NotepadRect;
        }

        //图片二值化
        public static Bitmap Threshold(Bitmap bit)
        {
            Mat src = bit.ToMat();
            Cv2.CvtColor(src, src, ColorConversionCodes.BGRA2GRAY);
            Mat result = src.Threshold(50, 200, ThresholdTypes.Otsu);
            return result.ToBitmap();
        }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }


        //public Bitmap GetWindowCapture(IntPtr hWnd)
        //{
        //    IntPtr hscrdc = GetWindowDC(hWnd);
        //    RECT windowRect = new RECT();
        //    GetWindowRect(hWnd, ref windowRect);
        //    int width = (int)(windowRect.Right - windowRect.Left);
        //    int height = (int)(windowRect.Bottom - windowRect.Top);

        //    IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);
        //    IntPtr hmemdc = CreateCompatibleDC(hscrdc);
        //    SelectObject(hmemdc, hbitmap);
        //    PrintWindow(hWnd, hmemdc, 0);
        //    Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
        //    DeleteDC(hscrdc);//删除用过的对象
        //    DeleteDC(hmemdc);//删除用过的对象
        //    return bmp;
        //}

        //public void CaptureImage(string fromImagePath, int offsetX, int offsetY, string toImagePath, int width, int height)
        //{
        //    //原图片文件
        //    Image fromImage = Image.FromFile(fromImagePath);
        //    //创建新图位图
        //    Bitmap bitmap = new Bitmap(width, height);
        //    //创建作图区域
        //    Graphics graphic = Graphics.FromImage(bitmap);
        //    //截取原图相应区域写入作图区
        //    graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
        //    //从作图区生成新图
        //    Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
        //    //保存图片
        //    saveImage.Save(toImagePath, ImageFormat.Png);
        //    //释放资源   
        //    saveImage.Dispose();
        //    graphic.Dispose();
        //    bitmap.Dispose();
        //}
    }
}
