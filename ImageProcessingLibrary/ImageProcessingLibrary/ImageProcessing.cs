using System.Drawing;
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
        private string dllNativPath = System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\..\\Debug\\";

        public ImageProcessing()
        {
            Directory.SetCurrentDirectory(dllNativPath);
        }

        public Bitmap greyScale(Bitmap greyBitMap)
        {
            greyScaleProcesing(greyBitMap);
            return greyBitMap;
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

        public Bitmap greyScaleAsync(Bitmap greyBitMap)
        {
            greyScaleProcesing(greyBitMap);
            return greyBitMap;
        }

        public Bitmap loadImageFromPath(string path)
        {
            if (path.Length > 0)
            {
                return new Bitmap(path);
            }
            return null;
        }

        unsafe public Bitmap nativCppGreyScale(Bitmap bmp)
        {
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
            return bmp;
        }

        [DllImport("DlLNativCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HelloWorld();

        [DllImport("DlLNativCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte*  GreyScaleNative(byte* array, int size);
    }

}
