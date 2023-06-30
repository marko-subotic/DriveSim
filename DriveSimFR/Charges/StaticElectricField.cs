using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using DriveSimFR;
using DriveSim.Utils;

namespace DriveSim.Charges
{

    /*
     * This class will display the electric field lines of a particular coordinate system given a
     * set of Vector charges.
     * 
     */
    internal class StaticElectricField
    {
        private readonly int resolution_width;
        private readonly int resolution_height;
        private readonly int resolution_circle;
        private readonly int width;
        private readonly int height;
        private Vector[,,] fieldVectors;
        private ArrayList charges;
        private double vectorScale = 50;
        private ArrayList fieldLines;
        private readonly double startDist = 2;
        private readonly double step_size = .5;
        public StaticElectricField(int width, int height, int resolution_width, int resolution_height, int resolution_circle)
        {
            this.width = width;
            this.height = height;
            this.resolution_width = resolution_width;
            this.resolution_height = resolution_height;
            this.resolution_circle = resolution_circle;
            fieldVectors = new Vector[resolution_width, resolution_height, 2];
            fieldLines = new ArrayList();
            charges = new ArrayList();
            for (int r = 0; r < fieldVectors.GetLength(0); r++)
            {
                for (int c = 0; c < fieldVectors.GetLength(1); c++)
                {
                    Vector location = new Vector((r + 1) * width / (double)resolution_width, (c + 1) * height / (double)resolution_height);
                    fieldVectors[r, c, 0] = location;
                }
            }
        }

        //adds a charge to the system, and calculates the vector at each sampled point of the screen.
        //kind of a slope field solution.
        public void addChargeSlopeField(PointCharge inCharge)
        {
            addCharge(inCharge);
            for (int r = 0; r < resolution_width; r++)
            {
                for (int c = 0; c < resolution_height; c++)
                {
                    Vector location = fieldVectors[r, c, 0];
                    fieldVectors[r, c, 1] = netField(location);
                    fieldVectors[r, c, 1] *= vectorScale / fieldVectors[r, c, 1].dist();
                }
            }
        }

        //calculates lines of force starting from each charge, calculating the next point with a given
        //step size untill the line leaves the screen. Euler's method solution.
        public void addChargeEulers(PointCharge inCharge)
        {
            addCharge(inCharge);
            for (int j = 0; j < charges.Count; j++)
            {
                PointCharge charge = (PointCharge)charges[j];
                fieldLines[j] = new ArrayList();
                ArrayList chargeList = (ArrayList)fieldLines[j];
                for (int i = 0; i < resolution_circle; i++)
                {
                    double theta = (double)i * 2 * Math.PI / resolution_circle;
                    chargeList.Add(new LinkedList<Vector>());
                    Vector location = charge.location + MathUtils.unitVectorFromTheta(theta) * startDist;
                    LinkedList<Vector> currentTrail = (LinkedList<Vector>)chargeList[i];
                    while (inBounds(location, charge))
                    {
                        currentTrail.AddLast(location);
                        Vector net_field = netField(location);
                        location += net_field * step_size / net_field.dist() * (charge.charge > 0 ? 1 : -1);
                    }
                }
            }
        }

        /*
         * This is a helper method called at the beginning of each addCharge method, no matter of the
         * method used, charges must be kept track of globaly.
         */
        private void addCharge(PointCharge inCharge)
        {
            charges.Add(inCharge);
            fieldLines.Add(new ArrayList());
        }

        /*
         * Will clear away all charges and lines holding anything. essentially a reset.
         */
        public void clearCharges()
        {
            fieldLines.Clear();
            charges.Clear();
            for (int i = 0; i < fieldVectors.GetLength(0); i++)
            {
                for (int j = 0; j < fieldVectors.GetLength(1); j++)
                {
                    fieldVectors[i, j, 0] = new Vector();
                    fieldVectors[i, j, 1] = new Vector();
                }
            }
        }
        /*
         * This method returns the net vector of all charges at a given point. (not normalized)
         */
        private Vector netField(Vector point)
        {
            Vector net = new Vector();
            foreach (PointCharge charge in charges)
            {
                net += MathUtils.unitVectorFromTheta(MathUtils.angleToVector(point, charge.location)) * charge.charge / charge.location.dist(point);
            }
            return net;
        }


        /*
         * This method checks if a given vector is inside the screen size
         */
        private bool inBounds(Vector vector, PointCharge inCharge)
        {
            if (vector.x < 0 || vector.x > width) return false;
            if (vector.y < 0 || vector.y > height) return false;
            foreach (PointCharge charge in charges)
            {
                if (charge.location != inCharge.location)
                {
                    if (vector.dist(charge.location) < 15) return false;

                }
            }
            return true;
        }
        /*
         * Returns a deep copy of the electric fields, where the first Vector is the location of the field, the second Vector
         * is a normalized vector in the direction of the electric field at that Vector.
         */
        public Vector[,,] getElectricFields()
        {
            Vector[,,] deepCopy = new Vector[fieldVectors.GetLength(0), fieldVectors.GetLength(1), fieldVectors.GetLength(2)];
            for (int r = 0; r < deepCopy.GetLength(0); r++)
            {
                for (int c = 0; c < deepCopy.GetLength(1); c++)
                {
                    for (int i = 0; i < deepCopy.GetLength(2); i++)
                    {
                        deepCopy[r, c, i] = fieldVectors[r, c, i];
                    }
                }
            }
            return deepCopy;
        }

        public ArrayList getElectricLines()
        {
            return fieldLines;
        }

        public ArrayList getCharges()
        {
            return charges;
        }
    }

}
