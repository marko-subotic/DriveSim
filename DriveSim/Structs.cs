using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSim
{
    public struct Point
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point(double m)
        {
            this.x = m;
            this.y = m;
        }
        public Point(Point input)
        {
            this.x = input.x;
            this.y = input.y;
        }
        
        public static Point avg(Point p1, Point p2)
        {
            return new Point((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x+p2.x, p1.y+p2.y);
        }

        public double distO()
        {
            return Math.Sqrt(x*x + y*y);
        }
    }

}
