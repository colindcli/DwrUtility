#if NETFULL
using DwrUtility.Registers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class RegisterHelperTest
    {
        /// <summary>
        /// 测试注册表
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var reg = new RegisterHelper(RegisterKeyType.HKEY_CURRENT_USER);
            var item = "DwrUtility";
            var key = "InstallDir";
            var value = "C:\\DwrUtility";

            var isExists = reg.IsExistItem(item);
            if (isExists)
            {
                reg.DeleteItem(item);
            }

            var b = reg.SetValue(item, key, value);

            Assert.IsTrue(b, "写入失败");

            var list = reg.GetValues(item);
            var name = list[0].Key;

            var obj = reg.GetValue(item, name);
            Assert.IsTrue(obj.ToString() == value);

            reg.DeleteName(item, name);

            var rows = reg.GetValues(item);
            Assert.IsTrue(rows.Count == 0);

            reg.DeleteItem(item);

            Assert.IsTrue(!reg.IsExistItem(item));
        }
    }
}

#endif
