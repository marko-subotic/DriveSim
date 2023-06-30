using DriveSimFR;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSim.Utils
{
    public class DrawingUtils
    {
        public static Rect alignCenter(Rect rect)
        {
            rect.location.x -= rect.size.x / 2;
            rect.location.y -= rect.size.y / 2;
            return rect;
        }
        public static Rect alignCenter(double x1, double y1, double x2, double y2)
        {
            return new Rect(x1 - x2 / 2, y1 - y2 / 2, x2, y2);
        }

        public static void drawButton(SKCanvas canvas, SKPaint rectPaint, SKPaint textPaint, MyButton button)
        {
            canvas.DrawRect(MathUtils.rectToSK(button.rectangle), rectPaint);
            canvas.DrawText(button.text, MathUtils.vecToPt(button.rectangle.center), textPaint);
        }
    }
}
