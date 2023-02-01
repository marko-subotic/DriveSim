using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSimFR
{
    public struct Vector
    {
        public double x;
        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector(double m)
        {
            this.x = m;
            this.y = m;
        }
        public Vector(Vector input)
        {
            this.x = input.x;
            this.y = input.y;
        }
        public Vector(Point input)
        {
            this.x = input.X;
            this.y = input.Y;
        }
        public static Vector avg(Vector p1, Vector p2)
        {
            return new Vector((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        }

        public static Vector operator +(Vector p1, Vector p2)
        {
            return new Vector(p1.x+p2.x, p1.y+p2.y);
        }
        public static Vector operator -(Vector p1, Vector p2)
        {
            return new Vector(p1.x - p2.x, p1.y - p2.y);
        }

        public static Vector operator *(Vector p1, double scalar)
        {
            return new Vector(p1.x * scalar, p1.y * scalar);
        }

        public static Vector operator /(Vector p1, double scalar)
        {
            return new Vector(p1.x / scalar, p1.y / scalar);
        }

        public double dist(Vector origin = new Vector())
        {
            return Math.Sqrt(Math.Pow(x-origin.x,2) + Math.Pow(y - origin.y, 2));
        }

    }

    public struct PointCharge
    {
        public Vector location;
        public double charge;

        public PointCharge(Vector location, double charge)
        {
            this.location = location;
            this.charge = charge;
        }
    }

}
