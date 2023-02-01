using DriveSimFR;

namespace tests
{
    [TestClass]
    public class Test_Chassis_Constructors
    {
        int num_wheels = 4;
        Vector[,] wheelPosAssert;
        double[] wheelDir;
        Vector position = new Vector();

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
        public void TestChassisConstructor_TankDrive()
        {
            //Given
            double rT = Math.Sqrt(2)/2;
            double rTP = rT + .5;
            double rTM = rT - .5;
            //wheel positions start at top left and go cw
            wheelPosAssert = new Vector[4, 3]
            {
                {new Vector(-rT, rT), new Vector(-rT, rTP), new Vector(-rT, rTM)},
                {new Vector(rT,rT), new Vector(rT, rTP), new Vector(rT, rTM)},
                {new Vector(rT, -rT), new Vector(rT, -rTM), new Vector(rT, -rTP)},
                {new Vector(-rT,-rT), new Vector(-rT, -rTM), new Vector(-rT, -rTP)},
            };
            Vector[] wheelPos = getWheelPos(wheelPosAssert);
            wheelDir = new double[4] { 0, 0, 0, 0 };
            TestChassis tester = new TestChassis(wheelDir, wheelPos, position);
            Vector[,] testerWheelPos = tester.getWheelPositions();
            
            
            //then
            for(int r = 0; r< testerWheelPos.GetLength(0); r++)
            {
                for(int c = 0; c < testerWheelPos.GetLength(1); c++)
                {
                    Assert.AreEqual(testerWheelPos[r,c], wheelPosAssert[r,c]);
                }
            }
        }

        [TestMethod]
        public void TestChassisConstructor_XDrive_Normal()
        {
            //wheel positions start at left and go cw
            wheelPosAssert = new Vector[4, 3]
            {
                {new Vector(-1, 0), new Vector(-1, .5), new Vector(-1, -.5)},
                {new Vector(0,1), new Vector(.5, 1), new Vector(-.5, 1), },
                {new Vector(1, 0), new Vector(1, -.5), new Vector(1, .5), },
                {new Vector(0,-1), new Vector(-.5, -1), new Vector(.5, -1)} ,
            };
            Vector[] wheelPos = getWheelPos(wheelPosAssert);
            //wheel directions start at  left and go cw
            wheelDir = new double[4] { 0, 3*Math.PI/2, Math.PI, Math.PI/2 };
            TestChassis tester = new TestChassis(wheelDir, wheelPos, position);
            Vector[,] testerWheelPos = tester.getWheelPositions();


            //then
            for (int r = 0; r < testerWheelPos.GetLength(0); r++)
            {
                for (int c = 0; c < testerWheelPos.GetLength(1); c++)
                {
                    Assert.IsTrue(testerWheelPos[r, c]==wheelPosAssert[r, c], testerWheelPos[r, c] + " " + wheelPosAssert[r, c]);
                }
            }
        }

        [TestMethod]
        public void TestChassisConstructor_XDrive_Angle()
        {
            //Given
            double rT = Math.Sqrt(2) / 2;
            double rTP = rT + .5*rT;
            double rTM = rT - .5*rT;
            //wheel positions start at left and go cw
            wheelPosAssert = new Vector[4, 3]
            {
                {new Vector(-rT, rT), new Vector(-rTM, rTP), new Vector(-rTP, rTM)},
                {new Vector(rT,rT), new Vector(rTP, rTM), new Vector(rTM, rTP), },
                {new Vector(rT, -rT), new Vector(rTM, -rTP), new Vector(rTP, -rTM), },
                {new Vector(-rT,-rT), new Vector(-rTP, -rTM), new Vector(-rTM, -rTP)} ,
            };
            Vector[] wheelPos = getWheelPos(wheelPosAssert);
            //wheel directions start at  left and go cw
            wheelDir = new double[4] { 7 * Math.PI/4 , 5 * Math.PI / 4, 3 * Math.PI / 4, Math.PI / 4 };
            TestChassis tester = new TestChassis(wheelDir, wheelPos, position);
            Vector[,] testerWheelPos = tester.getWheelPositions();


            //then
            for (int r = 0; r < testerWheelPos.GetLength(0); r++)
            {
                for (int c = 0; c < testerWheelPos.GetLength(1); c++)
                {
                    Assert.IsTrue(testerWheelPos[r, c]==wheelPosAssert[r, c], testerWheelPos[r, c]+ " " + wheelPosAssert[r, c]);
                }
            }
        }
    }
}