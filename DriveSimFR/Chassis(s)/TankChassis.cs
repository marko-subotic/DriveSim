using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSimFR
{
    public class TankChassis : Chassis
    {
        //should be used to position the wheels
        Vector[] body;
        Vector[] headerLine;
        int strokeWidth;
        public TankChassis(double radius, Vector position, int strokeWidth, double WHEEL_PROP, int max_speed = 1, double k_fric = .2, double mass = 1) : base(radius, null, null, position,WHEEL_PROP, max_speed, k_fric, mass)
        {
            double rT = Math.Sqrt(2) / 2 * radius;
            double[] wheelDirections = new double[Chassis.NUM_WHEELS];
            Vector[] wheelPositions = new Vector[] {
                                            new Vector(-rT,rT), new Vector(rT,rT),
                                            new Vector(rT,-rT),new Vector(-rT,-rT),};
            base.constructor(wheelDirections, wheelPositions, position);
            this.strokeWidth = strokeWidth;
            body = new Vector[Chassis.NUM_WHEELS];
            for(int i = 0; i<body.Length; i++)
            {
                body[i] += wheelPositions[i] + new Vector((wheelPositions[i].x < 0 ? strokeWidth : -strokeWidth), 0);
            }
            headerLine = new Vector[2] { new Vector(), new Vector(0, radius) };
        }

        //Returns the array holding the points of the body
        public Vector[] getBody()
        {
            return body;
        }

        //Return the array of the header line
        public Vector[] getHeaderLine()
        {
            return headerLine;
        }

        

        
    }
}
