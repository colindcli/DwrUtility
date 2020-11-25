#if NETFRAMEWORK
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Test
{
    [TestClass]
    public class CacheHelperTest
    {
        [TestMethod]
        public void TestMethod1A()
        {
            var n = 0;
            for (var i = 0; i < 1000; i++)
            {
                var b = IsRobotIp("127.0.0.1", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36");
                if (b)
                {
                    break;
                }
                n++;
            }

            var m = 0;
            for (var i = 0; i < 1000; i++)
            {
                var b = IsRobotIp("127.0.0.2", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36");
                if (b)
                {
                    break;
                }
                m++;
            }

            var total = 31;
            Assert.IsTrue(n == total && m == total);
        }

        /// <summary>
        /// 是否机器人IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsRobotIp(string ip, string userAgent)
        {
            //如果在黑名单里
            var keyBlack = "BlackList";
            var b = CacheHelper.GetCache<List<string>>(keyBlack, out var blackIpList);
            if (b && blackIpList.Exists(p => p == ip))
            {
                return true;
            }

            //统计是否超出阈值
            var key = "Ips";
            var b2 = CacheHelper.GetCache<List<string>>(key, out var cacheIpList);

            //超出阈值
            if (b2 && cacheIpList.Count(p => p == ip) > 30)
            {
                //加入黑名单1小时
                if (blackIpList == null)
                {
                    blackIpList = new List<string>();
                }
                blackIpList.Add(ip);
                CacheHelper.SetCache(keyBlack, blackIpList, DateTime.Now.AddHours(3));

                return true;
            }

            if (cacheIpList == null)
            {
                cacheIpList = new List<string>();
            }
            cacheIpList.Add(ip);

            //没有超出阈值
            CacheHelper.SetCache(key, cacheIpList, DateTime.Now.AddSeconds(60));

            return false;
        }

        [TestMethod]
        public void TestMethod2()
        {
            var b = CacheHelper.GetCache<int>("TestInt", out var n);
            Assert.IsTrue(!b);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var b1 = CacheHelper.SetCache("TestInsert", 10);
            var b2 = CacheHelper.SetCache("TestInsert", 100);

            var b = CacheHelper.GetCache<int>("TestInsert", out var n);
            Assert.IsTrue(b1 && b2 && b && n == 100);
        }
    }
}

#endif
