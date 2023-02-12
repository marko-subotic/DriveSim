using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public static Vector operator *(double scalar, Vector p1)
        {
            return new Vector(p1.x * scalar, p1.y * scalar);
        }

        public static Vector operator /(Vector p1, double scalar)
        {
            return new Vector(p1.x / scalar, p1.y / scalar);
        }
        public static bool operator ==(Vector p1, Vector p2)
        {
            return (Math.Abs(p1.x-p2.x)<.0001 && Math.Abs(p1.y - p2.y) < .0001);
        }
        public static bool operator !=(Vector p1, Vector p2)
        {
            return !(p1==p2);
        }

        public override string ToString()
        {
            return this.x.ToString() + ", "+ this.y.ToString();
        }

        public  bool Equals(Vector obj)
        {
            return this==obj;
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

    public struct Rect
    {
        public Vector location;
        public Vector size;
        public Vector center;

        public Rect(Vector location, Vector size)
        {
            this.location = location;
            this.size = size;
            center = new Vector(location+size/2);
        }
        public Rect(double x1, double y1, double x2, double y2)
        {
            if (x1 < 0 || y1 < 0)
            {
                throw new ArgumentException("coordinates less than 0");
            }
            this.location = new Vector(x1,y1);
            this.size = new Vector(x2, y2);
            center = new Vector(location + size / 2);
        }

        public static bool operator ==(Rect r1, Rect r2)
        {
            return r1.location == r2.location && r1.size == r2.size;
        }

        public static bool operator !=(Rect r1, Rect r2)
        {
            return !(r1 == r2);
        }

        public bool contains(Vector point)
        {
            if (point.x > this.location.x && point.x < this.location.x + this.size.x)
            {
                if (point.y > this.location.y && point.y < this.location.y + this.size.y)
                    return true;
            }
            return false;
        }
    }

    public struct MyButton
    {
        public Rect rectangle;
        public string text;

        public MyButton(Rect rectangle, string text)
        {
            this.rectangle = rectangle;
            this.text = text;
        }

        
    }

}
