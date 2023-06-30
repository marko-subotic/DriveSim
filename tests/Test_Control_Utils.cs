using DriveSim.Utils;
using DriveSimFR;
using static System.Net.Mime.MediaTypeNames;

namespace tests
{

    //Tests for the chassis constructor given a set of wheel positions and directions
    //should create the two end points outside of the first average.
    [TestClass]
    public class Test_Control_Utils
    {
        const int num_wheels = 4;
        double[] wheelPowAssert;
        double heading = 0;

        [TestMethod]
        public void TestBeyblade_Single_50_0Heading_Forward()
        {
            double rS = .50;
            double rT = Math.Sqrt(2) / 2;
            heading = 0;
            int input = (int) (1 * ControlUtils.JOYSTICK_MAX);
            double plus = rS + rT * (1 - rS);
            double minus = rS - rT * (1 - rS);
            wheelPowAssert = new double[num_wheels]{ plus, minus, minus, plus };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(input, 0, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);
            
        }

        [TestMethod]
        public void TestBeyblade_Single_50_0Heading_Backward()
        {
            double rT = Math.Sqrt(2) / 2;
            double rS = .50;
            heading = 0;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            double plus = rS + rT * (1 - rS);
            double minus = rS - rT * (1 - rS);
            wheelPowAssert = new double[num_wheels] { minus, plus, plus, minus };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(-input, 0, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);

        }

        [TestMethod]
        public void TestBeyblade_Single_50_0Heading_Right()
        {
            double rT = Math.Sqrt(2) / 2;
            double rS = .50;
            heading = 0;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            double plus = rS + rT * (1 - rS);
            double minus = rS - rT * (1 - rS);
            wheelPowAssert = new double[num_wheels] { plus, plus, minus, minus };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(0, input, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);
        }

        [TestMethod]
        public void TestBeyblade_Single_50_0Heading_Left()
        {
            double rT = Math.Sqrt(2) / 2;
            double rS = .50;
            heading = 0;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            double plus = rS + rT * (1 - rS);
            double minus = rS - rT * (1 - rS);
            wheelPowAssert = new double[num_wheels] { minus, minus, plus, plus };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(0, -input, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);

        }

        [TestMethod]
        public void TestBeyblade_Single_50_PI4Heading_Forward()
        {
            double rS = .50;
            heading = Math.PI/4;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            wheelPowAssert = new double[num_wheels] { 1, .5, 0, .5 };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(input, 0, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);
        }

        [TestMethod]
        public void TestBeyblade_Single_50_PI4Heading_Backward()
        {
            double rS = .50;
            heading = Math.PI / 4;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            wheelPowAssert = new double[num_wheels] { 0, .5, 1, .5 };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(-input, 0, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);
        }

        [TestMethod]
        public void TestBeyblade_Single_50_PI4Heading_Right()
        {
            double rS = .50;
            heading = Math.PI / 4;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            wheelPowAssert = new double[num_wheels] { .5, 1, .5, 0 };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(0, input, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);

        }

        [TestMethod]
        public void TestBeyblade_Single_50_PI4Heading_Left()
        {
            double rS = .50;
            heading = Math.PI / 4;
            int input = (int)(1 * ControlUtils.JOYSTICK_MAX);
            wheelPowAssert = new double[num_wheels] { .5, 0, .5, 1 };
            //when
            double[] wheelPows = ControlUtils.wheelPowsFromJoyStickBeyblade(0, -input, rS, heading);
            //then
            TestHelpers.AssertDoubleArray(wheelPows, wheelPowAssert);

        }
    }
}