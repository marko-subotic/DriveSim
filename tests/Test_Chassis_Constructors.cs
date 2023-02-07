using DriveSimFR;

namespace tests
{

    //Tests for the chassis constructor given a set of wheel positions and directions
    //should create the two end points outside of the first average.
    [TestClass]
    public class Test_Chassis_Constructors
    {
        int num_wheels = 4;
        Vector[,] wheelPosAssert;
        double[] wheelDir;
        int radius = 5;
        Vector position = new Vector();

        /*
         * Helper method that will just return a vector array with all of the first values of a sub-array
         */
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
        public void TestChassisConstructor_CustomTankDrive()
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
                    Assert.IsTrue(testerWheelPos[r, c] == wheelPosAssert[r, c], testerWheelPos[r, c] + " " + wheelPosAssert[r, c]);
                }
            }
        }

        [TestMethod]
        public void TestChassisConstructor_Custom_XDrive_Normal()
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
        public void TestChassisConstructor_Custom_XDrive_Angle()
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

        [TestMethod]
        public void TestChassisConstructor_TankDrive()
        {
            //Given
            //Given
            double rT = Math.Sqrt(2) / 2 *radius;
            double rTP = rT + .5;
            double rTM = rT - .5;
            int strokeWidth = 1;
            //wheel positions start at top left and go cw
            wheelPosAssert = new Vector[4, 3]
            {
                {new Vector(-rT, rT), new Vector(-rT, rTP), new Vector(-rT, rTM)},
                {new Vector(rT,rT), new Vector(rT, rTP), new Vector(rT, rTM)},
                {new Vector(rT, -rT), new Vector(rT, -rTM), new Vector(rT, -rTP)},
                {new Vector(-rT,-rT), new Vector(-rT, -rTM), new Vector(-rT, -rTP)},
            };
            Vector[] headerLine = new Vector[] { new Vector(), new Vector(0, radius) };
            Vector[] bodyAssert = new Vector[4]
                {new Vector(-rT+strokeWidth, rT), new Vector(rT-strokeWidth,rT), new Vector(rT-strokeWidth, -rT), new Vector(-rT+strokeWidth,-rT) };
            Vector[] wheelPos = getWheelPos(wheelPosAssert);
            wheelDir = new double[4] { 0, 0, 0, 0 };
            TankChassis tester = new TankChassis(radius, position, strokeWidth, radius);
            Vector[,] testerWheelPos = tester.getWheelPositions();
            Vector[] testerBody = tester.getBody();
            Vector[] testerHeader = tester.getHeaderLine();

            //then
            for (int r = 0; r < testerWheelPos.GetLength(0); r++)
            {
                Assert.IsTrue(bodyAssert[r] == testerBody[r],"body: " + bodyAssert[r] + ")( " + testerBody[r]);
                for (int c = 0; c < testerWheelPos.GetLength(1); c++)
                {
                    Assert.IsTrue(testerWheelPos[r, c] == wheelPosAssert[r, c], r + ", " + c+ " wheels: " + testerWheelPos[r, c] + " " + wheelPosAssert[r, c]);
                }
            }
            for(int i = 0; i < testerHeader.Length; i++)
            {
                Assert.IsTrue(testerHeader[i] == headerLine[i], "header line: " + headerLine[i] + " " + testerHeader[i]);
            }
        }

        [TestMethod]
        public void TestChassisConstructor_XDrive()
        {
            //Given
            //Given
            double rT = Math.Sqrt(2) / 2*radius;
            double rTP = rT + .5 * rT/radius;
            double rTM = rT - .5 * rT/radius;
            int strokeWidth = 1;
            //wheel positions start at left and go cw
            wheelPosAssert = new Vector[4, 3]
            {
                {new Vector(-rT, rT), new Vector(-rTM, rTP), new Vector(-rTP, rTM)},
                {new Vector(rT,rT), new Vector(rTP, rTM), new Vector(rTM, rTP), },
                {new Vector(rT, -rT), new Vector(rTM, -rTP), new Vector(rTP, -rTM), },
                {new Vector(-rT,-rT), new Vector(-rTP, -rTM), new Vector(-rTM, -rTP)} ,
            };
            Vector[] headerLine = new Vector[] { new Vector(), new Vector(0, radius) };
            Vector[] bodyAssert = new Vector[4]
                {new Vector(-rT+strokeWidth, rT), new Vector(rT-strokeWidth,rT), new Vector(rT-strokeWidth, -rT), new Vector(-rT+strokeWidth,-rT) };
            Vector[] wheelPos = getWheelPos(wheelPosAssert);
            wheelDir = new double[4] { 7 * Math.PI / 4, 5 * Math.PI / 4, 3 * Math.PI / 4, Math.PI / 4 };
            XChassis tester = new XChassis(radius, position, strokeWidth, radius);
            Vector[,] testerWheelPos = tester.getWheelPositions();
            Vector[] testerBody = tester.getBody();
            Vector[] testerHeader = tester.getHeaderLine();

            //then
            for (int r = 0; r < testerWheelPos.GetLength(0); r++)
            {
                Assert.IsTrue(bodyAssert[r] == testerBody[r], "body: " + bodyAssert[r] + " " + testerBody[r]);
                for (int c = 0; c < testerWheelPos.GetLength(1); c++)
                {
                    Assert.IsTrue(testerWheelPos[r, c] == wheelPosAssert[r, c], r + ", " + c + " wheels: " + testerWheelPos[r, c] + " " + wheelPosAssert[r, c]);
                }
            }
            for (int i = 0; i < testerHeader.Length; i++)
            {
                Assert.IsTrue(testerHeader[i] == headerLine[i], "header line: " + headerLine[i] + " " + testerHeader[i]);
            }
        }
    }
}