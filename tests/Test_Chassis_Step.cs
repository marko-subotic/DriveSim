using DriveSim.Utils;
using DriveSimFR;

namespace tests
{
    [TestClass]
    public class Test_Chassis_Step
    {
        int num_wheels = 4;
        int strokeWidth = 1;
        Vector position = new Vector();
        int radius = 5;
        int max_speed = 10;
        double k_lat = .2;
        double mass = 1;
        public Vector[] getWheelPos(Vector[,] wheelPos)
        {
            Vector[] rtrn = new Vector[num_wheels];
            for (int i = 0; i < num_wheels; i++)
            {
                rtrn[i] = wheelPos[i, 0];
            }
            return rtrn;
        }
        
        [TestMethod]
        public void TestTankStep_NoStartingVeloStraight()
        {
            //Given
            double step_size = 2;
            double K = 1/mass;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            double[] wheelPows = new double[] { 1, 1, 1, 1 };
            Vector expectedLinVelo = new Vector(0, num_wheels*K*wheelPows[0]*step_size);
            double expectedAngVelo = 0;
            Vector expectedPos = new Vector(0, .5 * K * wheelPows[0] * step_size * step_size*num_wheels);
            double expectedHead = 0;
            double wlSpd = expectedLinVelo.y;
            double[,] expectedWheelVelos = new double[,] { { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 } };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos(), expectedPos);
            Assert.IsTrue(Math.Abs(tester.getHeading() -expectedHead) < TestHelpers.maxError|| Math.Abs(tester.getHeading() - (expectedHead+Math.PI*2)) < TestHelpers.maxError, tester.getHeading().ToString() + " )( " + expectedHead.ToString());
            for (int i = 0; i < num_wheels; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestTankStep_NoStartingVeloSpin()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 2;
            double K = 1 / mass*rT;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            double[] wheelPows = new double[] { 1, -1, -1, 1 };
            double accel = -num_wheels * K * wheelPows[0] / radius;
            Vector expectedLinVelo = new Vector();
            double expectedAngVelo = accel*Math.Pow(step_size,1);
            Vector expectedPos = new Vector();
            double expectedHead = MathUtils.mod2PI((.5 * accel*Math.Pow(step_size,2)));
            double wlSpd = expectedAngVelo * -rT*radius;
            double[,] expectedWheelVelos = new double[,] { { wlSpd, wlSpd }, { -wlSpd, wlSpd }, { -wlSpd, -wlSpd }, { wlSpd, -wlSpd } };
            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos() , expectedPos);
            TestHelpers.AssertDouble(tester.getHeading(), expectedHead);
            for (int i = 0; i < num_wheels; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestTankStep_MaxStartingVeloStraight()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            double K = 1 / mass;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            tester.setHeading(Math.PI * 7 / 4);
            double[] wheelPows = new double[] { 1, 1, 1, 1 };
            double expectedHead = Math.PI * 7 / 4;
            Vector expectedLinVelo = (max_speed) * MathUtils.unitVectorFromTheta(expectedHead);
            double expectedAngVelo = 0;
            tester.setVelos(expectedLinVelo, expectedAngVelo);
            Vector expectedPos = (max_speed) * MathUtils.unitVectorFromTheta(expectedHead);
            double wlSpd = max_speed;
            double[,] expectedWheelVelos = new double[,] { { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 } };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos(), expectedPos);
            TestHelpers.AssertDouble(tester.getHeading(), expectedHead);
            for (int i = 0; i < num_wheels; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestXStep_MaxStartingVeloStraight()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            XChassis tester = new XChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            tester.setHeading(Math.PI * 7 / 4);
            double[] wheelPows = new double[] { 1, -1, -1, 1 };
            double expectedHead = Math.PI * 7 / 4;
            double spd = Math.Sqrt(Math.Pow(max_speed, 2)/(1+ k_lat* Math.Pow(max_speed, 2)));
            Vector expectedLinVelo = (1 / rT * spd) * MathUtils.unitVectorFromTheta(expectedHead);
            double expectedAngVelo = 0;
            tester.setVelos(expectedLinVelo, expectedAngVelo);
            Vector expectedPos = (expectedLinVelo.dist())*MathUtils.unitVectorFromTheta(expectedHead);
            double wlSpd = spd;
            double[,] expectedWheelVelos = new double[,] { { wlSpd, -wlSpd }, { -wlSpd, -wlSpd }, { -wlSpd, wlSpd }, { wlSpd, wlSpd } };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos(), expectedPos);
            TestHelpers.AssertDouble(tester.getHeading(), expectedHead);
            for (int i = 0; i < num_wheels; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestTankStep_HalfMaxVeloStraight()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            double accel = num_wheels / mass * (1 - Math.Pow(max_speed / 2, 2) / Math.Pow(max_speed, 2));
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            tester.setHeading(0);
            double[] wheelPows = new double[] { 1, 1, 1, 1 };
            double expectedHead = 0;
            double expectedAngVelo = 0;
            tester.setVelos(MathUtils.unitVectorFromTheta(expectedHead)*max_speed/2, expectedAngVelo);
            Vector expectedLinVelo = (max_speed / 2 + accel * step_size) * MathUtils.unitVectorFromTheta(expectedHead);
            Vector expectedPos = (max_speed/2*step_size+.5*accel*Math.Pow(step_size,2)) * MathUtils.unitVectorFromTheta(expectedHead);
            double wlSpd = expectedLinVelo.y;
            double[,] expectedWheelVelos = new double[,] { { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 }, { wlSpd, 0 } };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos(), expectedPos);
            TestHelpers.AssertDouble(tester.getHeading(), expectedHead);
            for (int i = 0; i < num_wheels; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }

        [TestMethod]
        public void TestTankStep_MaxSpinningVeloNoInput()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_lat, mass);
            tester.setHeading(0);
            double[] wheelPows = new double[] { 1, -1, -1, 1 };
            double spd = Math.Sqrt(Math.Pow(max_speed, 2) / (1 + k_lat * Math.Pow(max_speed, 2)));
            double expectedHead = MathUtils.mod2PI(-spd /radius/rT*step_size); //divide by sqrt2/2 to ensure that wheelspeed is spd
            double expectedAngVelo = -spd /rT/radius;
            tester.setVelos(new Vector(), expectedAngVelo);
            Vector expectedLinVelo = new Vector();
            Vector expectedPos = new Vector();
            double[,] expectedWheelVelos = new double[,] { { spd, spd }, { -spd,spd }, { -spd, -spd }, { spd, -spd } };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            TestHelpers.AssertDouble(tester.getAngVelo(), expectedAngVelo);
            Assert.AreEqual(tester.getPos(), expectedPos);
            TestHelpers.AssertDouble(tester.getHeading(), expectedHead);
            for (int i = 0; i < num_wheels; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    TestHelpers.AssertDouble(tester.getWheelVelos()[i, j], expectedWheelVelos[i, j]);
                }
            }
        }
    }
}