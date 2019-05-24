using System;
using System.Globalization;
using DwrUtility.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class MathHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (var i = 0; i < 10; i++)
            {
                var d = decimal.Parse($"9.94{i}1");
                var val = d.ToRoundUp(2);
                var b = val == Convert.ToDecimal("9.95");
                Assert.IsTrue(b);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            for (var i = 0; i < 10; i++)
            {
                var d = decimal.Parse($"9.94{i}");
                var val = d.ToRoundUp(2);
                var b = val == (i == 0 ? Convert.ToDecimal("9.94") : Convert.ToDecimal("9.95"));
                Assert.IsTrue(b);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            for (var i = 0; i < 10; i++)
            {
                var d = decimal.Parse($"9{i}");
                var val = d.ToRoundUp(2);
                var b = val == d;
                Assert.IsTrue(b);
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            for (var i = 0; i < 10; i++)
            {
                var d = decimal.Parse($".98{i}");
                var val = d.ToRoundUp(2);
                var b = val == (i == 0 ? Convert.ToDecimal("0.98") : Convert.ToDecimal("0.99"));
                Assert.IsTrue(b);
            }
        }

        [TestMethod]
        public void TestMethod5()
        {
            for (var i = 0; i < 10; i++)
            {
                var d = decimal.Parse($".00{i}");
                var val = d.ToRoundUp(0);
                var b = val == (i == 0 ? Convert.ToDecimal("0") : Convert.ToDecimal("1"));
                Assert.IsTrue(b);
            }
        }

        [TestMethod]
        public void TestMethod6()
        {
            var b = true;
            var c = (decimal)0.0001;
            for (var i = 0; i < 100000; i++)
            {
                var n = (decimal)(i * 0.0001);
                var d = c + n;
                var v = d.ToRoundUp(2);

                var s = d.ToString(CultureInfo.InvariantCulture);
                var index = s.IndexOf(".", StringComparison.Ordinal);
                var v2 = Convert.ToDecimal(s.Substring(0, index + 3));

                var v3 = Convert.ToInt32(s.Substring(index + 3, s.Length - index - 3));
                if (v3 > 0)
                {
                    v2 = v2 + (decimal)0.01;
                }

                if (v != v2)
                {
                    b = false;
                    break;
                }
            }

            var b0 = b;
            var b1 = ((decimal)0).ToRoundUp(2) == 0;
            var b2 = ((decimal)-1.888).ToRoundUp(2) == (decimal)-1.88;
            var b3 = ((decimal)0.0002).ToRoundUp(2) == (decimal)0.01;
            var b4 = ((decimal)98).ToRoundUp(2) == 98;

            var success = b0 && b1 && b2 && b3 && b4;

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var b = true;
            var c = (decimal)0.0001;
            for (var i = 0; i < 100000; i++)
            {
                var n = (decimal)(i * 0.0001);

                var d = c + n;

                var v = d.ToRoundDown(2);

                var s = d.ToString(CultureInfo.InvariantCulture);
                var index = s.IndexOf(".", StringComparison.Ordinal);
                var v2 = Convert.ToDecimal(s.Substring(0, index + 3));

                if (v != v2)
                {
                    b = false;
                    break;
                }
            }

            var b0 = b;
            var b1 = ((decimal)0).ToRoundDown(2) == 0;
            var b2 = ((decimal)-1.888).ToRoundDown(2) == (decimal)-1.88;
            var b3 = ((decimal)0.0002).ToRoundDown(2) == 0;
            var b4 = ((decimal)98).ToRoundDown(2) == 98;

            var success = b0 && b1 && b2 && b3 && b4;

            Assert.IsTrue(success);
        }

    }
}
