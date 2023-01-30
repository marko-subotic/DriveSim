using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSim
{
    public class Utils
    {
        /*
     * Similar to atan2 function, but returns an angle according to theta = 0
     * meaning the point is directly above the origin and positive direction of theta being ccw.
     * **/
        public static double angleToPoint(Point target, Point origin = new Point())
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
         * returns cross product of two vectors
         */
    }
}
