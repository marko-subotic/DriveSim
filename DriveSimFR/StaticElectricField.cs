using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DriveSimFR
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
        private readonly int width;
        private readonly int height;
        private Vector[,,] fieldVectors;
        private ArrayList charges;
        private double vectorScale = 50;

        public StaticElectricField(int width, int height, int resolution_width, int resolution_height)
        {
            this.width = width;
            this.height = height;
            this.resolution_width = resolution_width;
            this.resolution_height = resolution_height;
            fieldVectors = new Vector[resolution_width, resolution_height, 2];
            charges = new ArrayList();
            for (int r = 0; r < fieldVectors.GetLength(0); r++)
            {
                for (int c = 0; c < fieldVectors.GetLength(1); c++)
                {
                    Vector location = new Vector((r + 1) * width / ((double)resolution_width), (c + 1) * height / ((double)resolution_height));
                    fieldVectors[r, c, 0] = location;
                }
            }
        }

        public void addCharge(PointCharge inCharge)
        {
            charges.Add(inCharge);
            for (int r = 0; r < resolution_width; r++)
            {
                for (int c = 0; c < resolution_height; c++)
                {
                    Vector location = fieldVectors[r,c,0];
                    fieldVectors[r, c, 1] = new Vector();
                    foreach (PointCharge charge in charges)
                    {
                        fieldVectors[r, c, 1] += Utils.unitVectorFromTheta(Utils.angleToVector(location, charge.location)) * charge.charge / charge.location.dist(location);
                    }
                    fieldVectors[r, c, 1] *= vectorScale/fieldVectors[r, c, 1].dist();
                }
            }
        }

        /*
         * Returns a deep copy of the electric fields, where the first Vector is the location of the field, the second Vector
         * is a normalized vector in the direction of the electric field at that Vector.
         */
        public Vector[,,] getElectricFields()
        {
            Vector[,,] deepCopy = new Vector[fieldVectors.GetLength(0), fieldVectors.GetLength(1), fieldVectors.GetLength(2)];
            for(int r = 0; r < deepCopy.GetLength(0); r++)
            {
                for(int c = 0; c< deepCopy.GetLength(1); c++)
                {
                    for(int i = 0; i < deepCopy.GetLength(2); i++)
                    {
                        deepCopy[r, c, i] = fieldVectors[r, c, i];
                    }
                }
            }
            return deepCopy;
        }

        public ArrayList getCharges()
        {
            return charges;
        }
    }

}
