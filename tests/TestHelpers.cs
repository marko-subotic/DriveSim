using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    internal class TestHelpers
    {
        public static double maxError = .0001;
        public static void AssertDouble(double value1, double value2)
        {
            Assert.IsTrue(Math.Abs(value2 - value1) <= maxError, value1.ToString() + ")(" + value2.ToString());
        }
        public static void AssertDoubleArray(double[] value1, double[] value2)
        {
            for(int i = 0; i < value1.Length; i++)
            {
                AssertDouble(value1[i], value2[i]);
            }
        }

    }
}
