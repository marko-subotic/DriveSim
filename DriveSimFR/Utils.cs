using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace DriveSimFR
{
    public class Utils
    {
        /*
     * Similar to atan2 function, but returns an angle according to theta = 0
     * meaning the Vector is directly above the origin and positive direction of theta being ccw.
     * **/
        public static double angleToVector(Vector target, Vector origin = new Vector())
        {
            target -= origin;
            if (target.y == 0)
            {
                if (target.x == 0)
                {
                    throw new Exception("Illegal Argument");
                }
                else if (target.x > 0)
                {
                    return Math.PI * 3 / 2;
                }
                else if (target.x < 0)
                {
                    return Math.PI / 2;
                }
            }
            double startAngle = Math.Abs(Math.Atan(target.x / target.y));
            if (target.y > 0)
            {
                if (target.x <= 0)
                {
                    return startAngle;
                }
                else
                {
                    return 2 * Math.PI - startAngle;
                }
            }
            else
            {
                if (target.x >= 0)
                {
                    return Math.PI + startAngle;
                }
                else
                {
                    return Math.PI - startAngle;
                }
            }
        }

        /*
         * Generates a unit vector given a theta, where 0 would give [0,1] (Vectoring straight up).
         * CCW is positive.
         */
        public static Vector unitVectorFromTheta(double theta)
        {
            return new Vector(-Math.Sin(theta), Math.Cos(theta));

        }

        public static SKPoint vecToPt(Vector vector)
        {
            return new SKPoint((int)vector.x, (int)vector.y);
        }

        public static SKRect rectToSK(Rect rect)
        {
            return new SKRect((float)rect.location.x, (float)rect.location.y, (float)(rect.location.x + rect.size.x), (float)(rect.location.y + rect.size.y));
        }

        public static double mod2PI(double mod)
        {
            while (mod < 0 || mod > 2 * Math.PI)
            {
                mod += 2 * Math.PI * (mod < 0 ? 1 : -1);
            }
            return mod;
        }

        public static Vector rotateVector(double theta, Vector pointRotating , Vector pointOfRotation  = new Vector())
        {
            double cosT = Math.Cos(theta);
            double sinT = Math.Sin(theta);

            double xTemp = pointRotating.x - pointOfRotation.x;
            double yTemp = pointRotating.y - pointOfRotation.y;
            pointOfRotation.x += (xTemp* cosT) - (yTemp* sinT);
            pointOfRotation.y += (xTemp* sinT) + (yTemp* cosT);

            return pointOfRotation;
         }
        /*
         * returns cross product of two vectors
         */
    }
}
