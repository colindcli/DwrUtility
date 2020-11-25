using System;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace DwrUtility
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static T GetCache<T>(string cacheKey)
        {
            return GetCache<T>(cacheKey, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="tryNumber"></param>
        /// <returns></returns>
        private static T GetCache<T>(string cacheKey, int tryNumber)
        {
            var objCache = HttpRuntime.Cache;
            var result = (T)objCache[cacheKey];
            if (result != null)
            {
                return result;
            }

            //保证并发写入还会命中
            if (tryNumber > 10)
            {
                return default(T);
            }

            if (tryNumber > 0)
            {
                Thread.Sleep(tryNumber * tryNumber * tryNumber);
            }

            return GetCache<T>(cacheKey, tryNumber + 1);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache<T>(string cacheKey, T objObject)
        {
            if (objObject == null)
            {
                return;
            }

            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存(缓存滑动过期时间)
        /// </summary>
        public static void SetCache<T>(string cacheKey, T objObject, TimeSpan timeout)
        {
            if (objObject == null)
            {
                return;
            }

            SetCache(cacheKey, objObject, DateTime.Now.Add(timeout));
        }

        /// <summary>
        /// 设置数据缓存(指定缓存到时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="absoluteExpiration">具体到期时间。5秒钟后过期：DateTime.Now.AddSeconds(5)</param>
        public static void SetCache<T>(string cacheKey, T objObject, DateTime absoluteExpiration)
        {
            if (objObject == null)
            {
                return;
            }

            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration.ToUniversalTime(), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key != null)
                {
                    cache.Remove(cacheEnum.Key.ToString());
                }
            }
        }
    }
}
