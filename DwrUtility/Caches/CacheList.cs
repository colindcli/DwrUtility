#if NETFULL
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DwrUtility
{
    /// <summary>
    /// 缓存父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CacheList<T>
    {
        /// <summary>
        /// 缓存key值
        /// </summary>
        private string CacheKey { get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        private TimeSpan TimeSpan { get; }

        /// <summary>
        /// 缓存父类 (默认6小时过期)
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="timeSpan">默认6小时过期</param>
        protected CacheList(string cacheKey, TimeSpan? timeSpan = null)
        {
            CacheKey = cacheKey;
            TimeSpan = timeSpan ?? TimeSpan.FromHours(6);
        }

        /// <summary>
        /// 获取数据方法
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public abstract List<T> GetData();

        /// <summary>
        /// 获取缓存后数据 (如果没有缓存，读取数据后缓存)
        /// </summary>
        /// <returns></returns>
        public List<T> GetCache()
        {
            var list = CacheHelper.GetCache<List<T>>(CacheKey);

            if (list != null)
            {
                return list;
            }

            //
            list = GetData();
            SetCache(list);

            return list;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="list"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool SetCache(List<T> list)
        {
            if (list == null)
            {
                return false;
            }

            return CacheHelper.SetCache(CacheKey, list, TimeSpan);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public void RemoveCache()
        {
            CacheHelper.RemoveCache(CacheKey);
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool RefreshCache()
        {
            var list = CacheHelper.GetCache<List<T>>(CacheKey);
            return SetCache(list);
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public Task RefreshCacheAsync()
        {
            return Task.Run(RefreshCache);
        }
    }
}
#endif
