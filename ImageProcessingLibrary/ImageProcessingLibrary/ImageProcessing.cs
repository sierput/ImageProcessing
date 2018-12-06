using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace ImageProcessingLibrary
{
    unsafe public class ImageProcessing
    {
        private Bitmap bitMap;
        private long time;
        private long timeNativ;
        public static string IMAGEGREY = "grey";
        public static string IMAGEGREYASYN = "greyAsyn";
        public static string IMAGENATIV = "greyScaleNativ";

        public ImageProcessing(string path)
        {
            loadImage(path);
        }

        public void grayScale()
        {
            Bitmap greyBitMap = (Bitmap)bitMap.Clone();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            greyScaleProcesing(greyBitMap);
            sw.Stop();
            time = sw.ElapsedMilliseconds;
            greyBitMap.Save(IMAGEGREY);
        }

        private void greyScaleProcesing(Bitmap greyBitMap)
        {
            for (int x = 0; x < greyBitMap.Width; x++)
            {
                for (int y = 0; y < greyBitMap.Height; y++)
                {
                    Color color = greyBitMap.GetPixel(x, y);

                    // L = 0.2126·R + 0.7152·G + 0.0722·B 
                    double L = 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B;
                    greyBitMap.SetPixel(x, y, Color.FromArgb(System.Convert.ToInt32(L), System.Convert.ToInt32(L), System.Convert.ToInt32(L)));
                }
            }
        }

        public Bitmap greyScaleAsyn()
        {
            Bitmap greyBitMap = (Bitmap)bitMap.Clone();
            greyScaleProcesing(greyBitMap);
            return greyBitMap;
        }

        private void loadImage(string path)
        {
            if (path.Length > 0 || bitMap != null)
            {
                bitMap = new Bitmap(path);
            }
        }

        unsafe public void nativCppGreyScale()
        {
            Bitmap bmp = (Bitmap)bitMap.Clone();
            Stopwatch sw = new Stopwatch();
            sw.Start();  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            byte[] copy = new byte[rgbValues.Length];
            fixed (byte* inBuf = rgbValues)
            {
                byte* outBuf = GreyScaleNative(inBuf, bytes);
                 for (int i = 0; i < rgbValues.Length; i++)
                    rgbValues[i] = outBuf[i];
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            sw.Stop();
            TimeNativ = sw.ElapsedMilliseconds;
            bmp.Save("greyScaleNativ");
        }

        [DllImport("C:\\Users\\msierputowski\\source\\repos\\WpfApp1\\Debug\\DlLNativCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HelloWorld();

        [DllImport("C:\\Users\\msierputowski\\source\\repos\\WpfApp1\\Debug\\DlLNativCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte*  GreyScaleNative(byte* array, int size);

        public Bitmap BitMapValue { get => bitMap; set => bitMap = value; }
        public long Time { get => time; set => time = value; }
        public long TimeNativ { get => timeNativ; set => timeNativ = value; }
    }

}
