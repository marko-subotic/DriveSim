using DriveSimFR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    //Test class to test functions in DriveSim.Utils class. mostly geometry functions
    [TestClass]
    public class Test_Utils
    {
        
        double minDif = .0001;
        [TestMethod]
        public void angleToVectorQ1()
        {
            Vector target = new Vector(0.5, Math.Sqrt(3) / 2);
            double dif = Math.Abs(Utils.angleToVector(target) - (11 * Math.PI / 6));


            Assert.IsTrue(dif < minDif, dif.ToString());
        }
        [TestMethod]
        public void angleToVectorQ2()
        {
            bool result = false;
            Vector target = new Vector(-0.5, Math.Sqrt(3) / 2);
            double dif = Math.Abs(Utils.angleToVector(target) - Math.PI / 6);

            Assert.IsTrue(dif < minDif, dif.ToString());
        }

        [TestMethod]
        public void angleToVectorQ3()
        {
            bool result = false;
            Vector target = new Vector(-0.5, Math.Sqrt(3) / -2);
            double dif = Math.Abs(Utils.angleToVector(target) - (5 * Math.PI / 6));
            Assert.IsTrue(dif < minDif, dif.ToString());
        }


        [TestMethod]
        public void angleToVectorQ4()
        {
            bool result = false;
            Vector target = new Vector(0.5, Math.Sqrt(3) / -2);
            double dif = Math.Abs(Utils.angleToVector(target) - (7 * Math.PI / 6));

            Assert.IsTrue(dif < minDif, dif.ToString());
        }

        public void unitVectorFromThetaQ1()
        {
            //given
            Vector target = new Vector(0.5, Math.Sqrt(3) / 2);
            Vector test = Utils.unitVectorFromTheta(11 * Math.PI / 6);

            //then
            Assert.IsTrue(test==target, test + " | " + target);
        }
        [TestMethod]
        public void unitVectorFromThetaQ2()
        {
            //given
            Vector target = new Vector(-0.5, Math.Sqrt(3) / 2);
            Vector test = Utils.unitVectorFromTheta(Math.PI / 6);

            //then
            Assert.IsTrue(test == target, test + " | " + target);
        }

        [TestMethod]
        public void unitVectorFromThetaQ3()
        {
            //given
            Vector target = new Vector(-0.5, Math.Sqrt(3) / -2);
            Vector test = Utils.unitVectorFromTheta(5*Math.PI / 6);

            //then
            Assert.IsTrue(test == target, test + " | " + target);

        }


        [TestMethod]
        public void unitVectorFromThetaQ4()
        {
            //given
            Vector target = new Vector(0.5, Math.Sqrt(3) / -2);
            Vector test = Utils.unitVectorFromTheta(7*Math.PI / 6);

            //then
            Assert.IsTrue(test == target, test + " | " + target);
        }

    }
}
