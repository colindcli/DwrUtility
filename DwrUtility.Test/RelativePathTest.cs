using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DwrUtility.Test
{
    [TestClass]
    public class RelativePathTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var basePath = @"C:\Users\Administrator\source\repos\DwrUtility";
            var fullPath = @"C:\Users\Administrator\source\repos";

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals("../repos"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var basePath = @"C:\Users\Administrator\";
            var fullPath = @"C:\Users\Administrator\source\repos";

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals(@"source/repos"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var basePath = @"C:\Users\Administrator";
            var fullPath = @"C:\Users\Administrator\source\repos";

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals(@"Administrator/source/repos"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            var basePath = @"C:/Users/Administrator";
            var fullPath = @"C:/Users/Administrator/source/repos";

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals(@"Administrator/source/repos"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            string basePath = null;
            var fullPath = @"C:/Users/Administrator/source/repos";

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals(@""));
        }

        [TestMethod]
        public void TestMethod6()
        {
            var basePath = @"C:/Users/Administrator";
            string fullPath = null;

            var relativePath = RelativeHelper.FullPathToRelativePath(basePath, fullPath);

            Assert.IsTrue(relativePath.IsEquals(@""));
        }
    }
}
