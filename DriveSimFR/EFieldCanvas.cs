using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSimFR
{
    public class EFieldCanvas : Canvas
    {
        public EFieldCanvas()
        {
            surfaceDims = new SKImageInfo(500, 500);
        }

        public EFieldCanvas(int width, int height)
        {
            surfaceDims = new SKImageInfo(width, height);
        }
    }
}
