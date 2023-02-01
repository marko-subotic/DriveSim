using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DriveSimFR
{
    public class TestChassis : Chassis
    {
        
        public TestChassis(double[] wheelDirections, Vector[] wheelPositions, Vector position, int radius=1, double WHEELS_PROP=1) : base(radius, wheelDirections, wheelPositions, position, WHEELS_PROP)
        {
        }
    }
}
