#if NETFULL
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DwrUtility.Test
{
    [TestClass]
    public class ConfigHelperTest
    {
        private static readonly string Root =
            Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/config/");

        private static readonly string Temp = $"{DwrUtilitySetting.Root}/Temp/";

        public ConfigHelperTest()
        {
            if (!Directory.Exists(Temp))
            {
                Directory.CreateDirectory(Temp);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var scource = $"{Root}data_1.config";
            var key = "test";
            var value = "ok";
            var value2 = "update";

            var path = $"{Temp}{Guid.NewGuid()}.config";
            File.Copy(scource, path, true);

            var b1 = string.IsNullOrWhiteSpace(ConfigHelper.GetAppSetting(path, key));

            var b2 = ConfigHelper.UpsetAppSetting(path, key, value);
            var b3 = ConfigHelper.GetAppSetting(path, key) == value;

            var b4 = ConfigHelper.UpdateAppSetting(path, key, value2);
            var b5 = ConfigHelper.GetAppSetting(path, key) == value2;

            ConfigHelper.RemoveAppSetting(path, key);
            var b6 = string.IsNullOrWhiteSpace(ConfigHelper.GetAppSetting(path, key));

            Assert.IsTrue(b1 && b2 && b3 && b4 && b5 && b6);

            File.Delete(path);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var scource = $"{Root}data_1.config";
            var name = "test";
            var value = "ok";
            var value2 = "update";

            var path = $"{Temp}{Guid.NewGuid()}.config";
            File.Copy(scource, path, true);

            var b1 = string.IsNullOrWhiteSpace(ConfigHelper.GetConnectionString(path, name));

            var b2 = ConfigHelper.UpsetConnectionString(path, name, value);
            var b3 = ConfigHelper.GetConnectionString(path, name) == value;

            var b4 = ConfigHelper.UpdateConnectionString(path, name, value2);
            var b5 = ConfigHelper.GetConnectionString(path, name) == value2;

            ConfigHelper.RemoveConnectionString(path, name);
            var b6 = string.IsNullOrWhiteSpace(ConfigHelper.GetConnectionString(path, name));

            Assert.IsTrue(b1 && b2 && b3 && b4 && b5 && b6);

            File.Delete(path);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var scource = $"{Root}data_2.config";
            var key = "test";
            var value = "ok";

            var path = $"{Temp}{Guid.NewGuid()}.config";
            File.Copy(scource, path, true);

            var b1 = ConfigHelper.UpdateAppSetting(path, key, value);
            Assert.IsTrue(!b1);

            var b2 = ConfigHelper.UpsetAppSetting(path, key, value);
            Assert.IsTrue(b2);

            //验证
            var text = File.ReadAllText(path);
            Assert.IsTrue(text.Contains("版本") && text.Contains(key) && text.Contains(value));

            File.Delete(path);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var scource = $"{Root}data_3.config";
            var key = "conn";
            var value = "ok";

            var path = $"{Temp}{Guid.NewGuid()}.config";
            File.Copy(scource, path, true);

            var b1 = ConfigHelper.UpdateConnectionString(path, key, value);
            Assert.IsTrue(!b1);

            var b2 = ConfigHelper.UpsetConnectionString(path, key, value);
            Assert.IsTrue(b2);

            //验证
            var text = File.ReadAllText(path);
            Assert.IsTrue(text.Contains("版本") && text.Contains(key) && text.Contains(value));

            File.Delete(path);
        }
    }
}
#endif
