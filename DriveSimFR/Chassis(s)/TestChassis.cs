using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace DriveSimFR
{
    public class TestChassis : Chassis
    {
        
        public TestChassis(double[] wheelDirections, Vector[] wheelPositions, Vector position, int radius=1, double WHEELS_PROP=1) : base(radius, wheelDirections, wheelPositions, position, WHEELS_PROP)
        {
        }

        //Returns the array holding the points of the body
        public override Vector[] getBody()
        {
            return null;
        }

        //Return the array of the header line
        public override Vector[] getHeaderLine()
        {
            return null;
        }
    }
}
