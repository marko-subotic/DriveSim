using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DriveSim
{

    /*
     * This class will display the electric field lines of a particular coordinate system given a
     * set of point charges.
     * 
     */
    internal class StaticElectricField
    {
        private readonly int resolution_width;
        private readonly int resolution_height;
        private readonly int width;
        private readonly int height;
        private Point[,,] fieldVectors;
        private ArrayList charges;

        public StaticElectricField(int width, int height, int resolution_width, int resolution_height)
        {
            this.width= width;
            this.height= height;
            this.resolution_width= resolution_width;
            this.resolution_height= resolution_height;
            fieldVectors = new Point[resolution_width, resolution_height,2];
            charges = new ArrayList();

        }

        public void addCharge(PointCharge charge)
        {
            charges.Add(charge);
            for(int r = 0; r < resolution_width; r++)
            {
                for(int c = 0; c < resolution_height; c++)
                {
                    Point location = new Point((r+1)/(resolution_width + 1), (c + 1) / (resolution_height + 1));
                    for(int i = 0; i < charges.Count; i++)
                    {
                        fieldVectors[r, c, 1] += new Point(1) * charges[i].charge / charges[i].location.dist(location);
                    }
                }
            }
        }
    }

}
