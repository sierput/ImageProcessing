using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageProcessingLibrary
{
    public class TimerProcessing
    {
        private long time;

        public Bitmap imageProcessingTime(Func<Bitmap, Bitmap> grayScale, Bitmap c)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Bitmap bitMap = grayScale.Invoke(c);
            sw.Stop();
            Time = sw.ElapsedMilliseconds;
            return bitMap;
        }
        public long Time { get => time; set => time = value; }
    }
}
