using System;
using System.Collections.Generic;
using DwrUtility.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = "小飞兔整站下载";
            var keys = new List<string>()
            {
                "小飞兔",
                "下载"
            };

            var b = str.HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false);

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var str = "小飞兔整站下载";
            var b = str.IsContainChinese();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var str = "小飞兔整站下载V3.0";
            var b = str.CountChinese() == 7;
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var end1 = (int)'\u9fa5';
            var end2 = (int)'\u9fbb';

            var list = new List<string>();
            for (var i = end1; i <= end2; i++)
            {
                list.Add(((char)i).ToString());
            }

            var s = string.Join("", list);
            var b = s == "龥龦龧龨龩龪龫龬龭龮龯龰龱龲龳龴龵龶龷龸龹龺龻";

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void TestMethod5()
        {
            Assert.IsTrue("str".RightToLeftIndexOfChar(new[] { 't' }, out var outStr, false) && outStr == "tr");
            Assert.IsTrue("str".RightToLeftIndexOfChar(new[] { 'r' }, out outStr, false) && outStr == "r");
            Assert.IsTrue("str".RightToLeftIndexOfChar(new[] { 's' }, out outStr, false) && outStr == "str");
            Assert.IsTrue(!"str".RightToLeftIndexOfChar(null, out outStr, false) && outStr == null);
            Assert.IsTrue(!"".RightToLeftIndexOfChar(null, out outStr, false) && outStr == null);
            Assert.IsTrue("12ststr".RightToLeftIndexOfChar(new[] { 's' }, out outStr, false) && outStr == "str");

            Assert.IsTrue(!"12ststr".RightToLeftIndexOfChar(new[] { '/' }, out outStr, false) && outStr == null);
            Assert.IsTrue("12ststr".RightToLeftIndexOfChar(new[] { '/' }, out outStr, true) && outStr == "12ststr");
        }

        [TestMethod]
        public void TestMethod6()
        {
            Assert.IsTrue("str".LeftToRightIndexOfChar(new[] { 't' }, out var outStr, false) && outStr == "st");
            Assert.IsTrue("str".LeftToRightIndexOfChar(new[] { 'r' }, out outStr, false) && outStr == "str");
            Assert.IsTrue("str".LeftToRightIndexOfChar(new[] { 's' }, out outStr, false) && outStr == "s");
            Assert.IsTrue(!"str".LeftToRightIndexOfChar(null, out outStr, false) && outStr == null);
            Assert.IsTrue(!"".LeftToRightIndexOfChar(null, out outStr, false) && outStr == null);
            Assert.IsTrue("12ststr".LeftToRightIndexOfChar(new[] { 's' }, out outStr, false) && outStr == "12s");


            Assert.IsTrue("12ststr".LeftToRightIndexOfChar(new[] { '/' }, out outStr, true) && outStr == "12ststr");
            Assert.IsTrue(!"12ststr".LeftToRightIndexOfChar(new[] { '/' }, out outStr, false) && outStr == null);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var ls = string.Empty;
            var rs = string.Empty;

            Assert.IsTrue("123 45 6".SplitString(' ', Direction.LeftRight, DefaultValue.TrueAndValue, out ls, out rs) && ls == "123" && rs == "45 6");
            Assert.IsTrue("123 45 6".SplitString(' ', Direction.RightLeft, DefaultValue.TrueAndValue, out ls, out rs) && ls == "123 45" && rs == "6");
            Assert.IsTrue("123 45 6".SplitString(' ', Direction.LeftRight, DefaultValue.FalseAndNull, out ls, out rs) && ls == "123" && rs == "45 6");
            Assert.IsTrue("123 45 6".SplitString(' ', Direction.RightLeft, DefaultValue.FalseAndNull, out ls, out rs) && ls == "123 45" && rs == "6");

            Assert.IsTrue("123456".SplitString(' ', Direction.LeftRight, DefaultValue.TrueAndValue, out ls, out rs) && ls == "123456" && rs == "");
            Assert.IsTrue("123456".SplitString(' ', Direction.RightLeft, DefaultValue.TrueAndValue, out ls, out rs) && ls == "" && rs == "123456");
            Assert.IsTrue(!"123456".SplitString(' ', Direction.LeftRight, DefaultValue.FalseAndNull, out ls, out rs) && ls == null && rs == null);
            Assert.IsTrue(!"123456".SplitString(' ', Direction.RightLeft, DefaultValue.FalseAndNull, out ls, out rs) && ls == null && rs == null);

            Assert.IsTrue(" 123456".SplitString(' ', Direction.LeftRight, DefaultValue.TrueAndValue, out ls, out rs) && ls == "" && rs == "123456");
            Assert.IsTrue(" 123456".SplitString(' ', Direction.RightLeft, DefaultValue.TrueAndValue, out ls, out rs) && ls == "" && rs == "123456");
        }
    }
}
