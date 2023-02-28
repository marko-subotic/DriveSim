using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSimFR
{
    public class XChassis : Chassis
    {
        Vector[] body;
        Vector[] bodyGlob;
        Vector[] headerLine;
        Vector[] headerLineGlob;
        int strokeWidth;
        public XChassis(double radius, Vector position, int strokeWidth, double WHEEL_PROP, int max_speed = 1, double k_fric_for = .2, double k_fric_lat = .2, double mass = 1) : base(radius, null, null, position, WHEEL_PROP, max_speed, k_fric_for, k_fric_lat, mass)
        {
            double rT = Math.Sqrt(2) / 2 * radius;
            double[] wheelDirections = new double[] { 7 * Math.PI / 4, 5 * Math.PI / 4, 3 * Math.PI / 4, Math.PI / 4 };
            Vector[] wheelPositions = new Vector[] {
                                            new Vector(-rT,rT), new Vector(rT,rT),
                                            new Vector(rT,-rT),new Vector(-rT,-rT),};
            base.constructor(wheelDirections, wheelPositions, position);
            this.strokeWidth = strokeWidth;
            body = new Vector[Chassis.NUM_WHEELS];
            for (int i = 0; i < body.Length; i++)
            {
                body[i] += wheelPositions[i] + new Vector((wheelPositions[i].x < 0 ? strokeWidth : -strokeWidth), 0);
            }
            headerLine = new Vector[2] { new Vector(), new Vector(0, radius) };
            headerLineGlob = new Vector[headerLine.Length];
            bodyGlob = new Vector[body.Length];
        }

        /*
         * Returns line on chassis pointing in direction of heading
         */
        public override Vector[] getHeaderLine()
        {
            for (int i = 0; i < headerLine.Length; i++)
            {
                headerLineGlob[i] = base.chassisToGlobal(headerLine[i]);
            }
            return headerLineGlob;
        }

        /*
         * Returns the array holding the points of the body
         */
        public override Vector[] getBody()
        {
            for (int i = 0; i < body.Length; i++)
            {
                bodyGlob[i] = base.chassisToGlobal(body[i]);
            }
            return bodyGlob;
        }

    }
}
