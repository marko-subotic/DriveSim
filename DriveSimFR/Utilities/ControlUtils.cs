using DriveSimFR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveSim.Utils
{
    public static class ControlUtils
    {
        public static readonly double JOYSTICK_MAX = 65540 / 2;
        public static readonly double JOYSTICK_DEADZONE = .4;

        /*
         * Returns wheelPowers to control a TankChassis from a tank-drive controller configuration
         */
        public static double[] wheelPowsFromJoyStickTank(int r, int l)
        {
            double[] pows = new double[] { l / JOYSTICK_MAX, r / JOYSTICK_MAX, r / JOYSTICK_MAX, l / JOYSTICK_MAX };
            for (int i = 0; i < pows.Length; i++)
            {
                if (Math.Abs(pows[i]) < JOYSTICK_DEADZONE)
                {
                    pows[i] = 0;
                }
            }
            return pows;
        }

        /*
         * Returns wheelPowers array to control a XChassis from a tank-drive-esque controller configuration.
         * Right and left joystick are used to control each sides motion, and the left joystick horizontal 
         * controls left/right motion.
         */
        public static double[] wheelPowsFromJoyStickX(int r, int l, int m)
        {
            m *= -1;
            double[] pows = new double[] { (l + m) / JOYSTICK_MAX, (m - r) / JOYSTICK_MAX, (-m - r) / JOYSTICK_MAX, (l - m) / JOYSTICK_MAX };
            for (int i = 0; i < pows.Length; i++)
            {
                if (Math.Abs(pows[i]) < JOYSTICK_DEADZONE)
                {
                    pows[i] = 0;
                }
                if (Math.Abs(pows[i]) > 1)
                {
                    if (pows[i] > 1)
                    {
                        pows[i] = 1;
                    }
                    else
                    {
                        pows[i] = -1;
                    }
                }
            }
            return pows;
        }

        /**
         * Returns wheel array to control XChassis where the chassis is constantly spinning at a specified rate.
         * @param lV: left joystick vertical value
         * @param lH: left joystick horizontal value
         * @param rS: percentage of maximum speed with which chassis rotates
         * @param cH: current heading of chassis (0 is straight up) in radians
         */
        public static double[] wheelPowsFromJoyStickBeyblade(int lV, int lH, double rS, double cH)
        {
            double[] pows = new double[4];
            if(Math.Sqrt(lV*lV+lH*lH)> JOYSTICK_DEADZONE)
            {
                double scale = 1 - rS;
                Vector globLeft = MathUtils.rotateVector(cH, MathUtils.unitVectorFromTheta(7 * Math.PI / 4));
                Vector globRight = MathUtils.rotateVector(cH, MathUtils.unitVectorFromTheta(5 * Math.PI / 4));

                double[,] matrix = new double[,]
                {
                {globLeft.x , globRight.x, lH/JOYSTICK_MAX},
                { globLeft.y, globRight.y, lV/JOYSTICK_MAX,},
                };
                MathUtils.gaussianElim(matrix);
                if (Math.Abs(matrix[0, 0]) < MathUtils.minDiff)
                {
                    MathUtils.swapRows(matrix, 0, 1);
                }
                pows = new double[4]{
                matrix[0,2]*scale, matrix[1,2]* scale,
                -matrix[0,2]*scale, -matrix[1,2]* scale,
                };
            }
            for (int i = 0; i < pows.Length; i++)
            {
                pows[i] += rS;
            }
            return pows;
        }
    }
}
