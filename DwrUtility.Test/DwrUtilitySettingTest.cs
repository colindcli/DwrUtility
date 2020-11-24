using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class DwrUtilitySettingTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var debug = "Release";
#if DEBUG
            debug = "Debug";
#endif

#if NETFRAMEWORK
            Assert.IsTrue(DwrUtilitySetting.Root == $"E:\\Workspace\\Shared\\Nuget\\DwrUtility\\DwrUtility.Test\\bin\\{debug}\\net46");
#else
            Assert.IsTrue(DwrUtilitySetting.Root == $"E:\\Workspace\\Shared\\Nuget\\DwrUtility\\DwrUtility.Test\\bin\\{debug}\\netcoreapp3.0");
#endif
            var root = DwrUtilitySetting.Root;
            DwrUtilitySetting.Root = "123";
            var b = DwrUtilitySetting.Root == "123";
            DwrUtilitySetting.Root = root;

            Assert.IsTrue(b);
        }
    }
}
