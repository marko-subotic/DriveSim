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
        double k_fric = .2;
        double mass = 1;
        double maxError = .0001;
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
            double K = max_speed * k_fric / mass;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_fric, mass);
            double[] wheelPows = new double[] { 1, 1, 1, 1 };
            Vector expectedLinVelo = new Vector(0, num_wheels*K*wheelPows[0]*step_size);
            double expectedAngVelo = 0;
            Vector expectedPos = new Vector(0, .5 * K * wheelPows[0] * step_size * step_size*num_wheels);
            double expectedHead = 0;
            double wlSpd = expectedLinVelo.y;
            double[] expectedWheelVelos = new double[] { wlSpd, wlSpd, wlSpd, wlSpd };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            Assert.IsTrue(Math.Abs(tester.getAngVelo() - expectedAngVelo) < maxError, tester.getAngVelo().ToString() + " )( " + expectedAngVelo.ToString());
            Assert.AreEqual(tester.getPos(), expectedPos);
            Assert.IsTrue(Math.Abs(tester.getHeading() -expectedHead) < maxError|| Math.Abs(tester.getHeading() - (expectedHead+Math.PI*2)) < maxError, tester.getHeading().ToString() + " )( " + expectedHead.ToString());
            for (int i =0;i<num_wheels;i++)
            {
                Assert.IsTrue(Math.Abs(tester.getWheelVelos()[i]- expectedWheelVelos[i])<maxError, tester.getWheelVelos()[i].ToString() + "  )( " + expectedWheelVelos[i].ToString());
            }
        }

        [TestMethod]
        public void TestTankStep_NoStartingVeloSpin()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 2;
            double K = max_speed * k_fric / mass*rT;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_fric, mass);
            double[] wheelPows = new double[] { 1, -1, -1, 1 };
            Vector expectedLinVelo = new Vector();
            double expectedAngVelo = -num_wheels*K*wheelPows[0]*Math.Pow(step_size,1)/radius;
            Vector expectedPos = new Vector();
            double expectedHead = Utils.mod2PI(-(.5 * K * wheelPows[0]/radius*Math.Pow(step_size,2)*num_wheels));
            double wlSpd = expectedAngVelo * -rT*radius;
            double[] expectedWheelVelos = new double[] { wlSpd, -wlSpd, -wlSpd, wlSpd };
            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            Assert.IsTrue(Math.Abs(tester.getAngVelo() -expectedAngVelo) < maxError, tester.getAngVelo().ToString() + " )( " + expectedAngVelo.ToString());
            Assert.AreEqual(tester.getPos() , expectedPos);
            Assert.IsTrue(Math.Abs(tester.getHeading() - expectedHead) < maxError, tester.getHeading().ToString() + " )( " + expectedHead.ToString());
            for (int i = 0; i < num_wheels; i++)
            {
                Assert.IsTrue(Math.Abs(tester.getWheelVelos()[i] - expectedWheelVelos[i]) < maxError, tester.getWheelVelos()[i].ToString() + "  )( " + expectedWheelVelos[i].ToString());
            }
        }

        [TestMethod]
        public void TestTankStep_MaxStartingVeloStraight()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            double K = max_speed * k_fric / mass;
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius, max_speed, k_fric, mass);
            tester.setHeading(Math.PI * 7 / 4);
            double[] wheelPows = new double[] { 1, 1, 1, 1 };
            double expectedHead = Math.PI * 7 / 4;
            Vector expectedLinVelo = (max_speed) * Utils.unitVectorFromTheta(expectedHead);
            double expectedAngVelo = 0;
            tester.setVelos(expectedLinVelo, expectedAngVelo);
            Vector expectedPos = (max_speed) * Utils.unitVectorFromTheta(expectedHead);
            double wlSpd = max_speed;
            double[] expectedWheelVelos = new double[] { wlSpd, wlSpd, wlSpd, wlSpd };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            Assert.IsTrue(Math.Abs(tester.getAngVelo() - expectedAngVelo) < maxError, tester.getAngVelo().ToString() + " )( " + expectedAngVelo.ToString());
            Assert.AreEqual(tester.getPos(), expectedPos);
            Assert.IsTrue(Math.Abs(tester.getHeading() - expectedHead) < maxError, tester.getHeading().ToString() + " )( " + expectedHead.ToString());
            for (int i = 0; i < num_wheels; i++)
            {
                Assert.IsTrue(Math.Abs(tester.getWheelVelos()[i] - expectedWheelVelos[i]) < maxError, tester.getWheelVelos()[i].ToString() + "  )( " + expectedWheelVelos[i].ToString());
            }
        }

        [TestMethod]
        public void TestXStep_MaxStartingVeloStraight()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double step_size = 1;
            double K = max_speed * k_fric / mass;
            XChassis tester = new XChassis(radius, position, strokeWidth, radius, max_speed, k_fric, mass);
            tester.setHeading(Math.PI * 7 / 4);
            double[] wheelPows = new double[] { 1, -1, -1, 1 };
            double expectedHead = Math.PI * 7 / 4;
            Vector expectedLinVelo = (1 / rT * max_speed) * Utils.unitVectorFromTheta(expectedHead);
            double expectedAngVelo = 0;
            tester.setVelos(expectedLinVelo, expectedAngVelo);
            Vector expectedPos = (1/rT * max_speed)*Utils.unitVectorFromTheta(expectedHead);
            double wlSpd = max_speed;
            double[] expectedWheelVelos = new double[] { wlSpd, -wlSpd, -wlSpd, wlSpd };

            //when
            tester.inputWheelPowers(wheelPows);
            tester.step(step_size);

            //then
            Assert.AreEqual(tester.getLinVelo(), expectedLinVelo);
            Assert.IsTrue(Math.Abs(tester.getAngVelo() - expectedAngVelo) < maxError, tester.getAngVelo().ToString() + " )( " + expectedAngVelo.ToString());
            Assert.AreEqual(tester.getPos(), expectedPos);
            Assert.IsTrue(Math.Abs(tester.getHeading() - expectedHead) < maxError, tester.getHeading().ToString() + " )( " + expectedHead.ToString());
            for (int i = 0; i < num_wheels; i++)
            {
                Assert.IsTrue(Math.Abs(tester.getWheelVelos()[i] - expectedWheelVelos[i]) < maxError, tester.getWheelVelos()[i].ToString() + "  )( " + expectedWheelVelos[i].ToString());
            }
        }
    }
}