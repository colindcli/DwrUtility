#if NETFULL
using System;
using System.Threading.Tasks;

namespace DwrUtility
{
    /// <summary>
    /// 缓存父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CacheObj<T>
        //此处T不能为值类型，所以限制为class
        where T : class
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
        protected CacheObj(string cacheKey, TimeSpan? timeSpan = null)
        {
            CacheKey = cacheKey;
            TimeSpan = timeSpan ?? TimeSpan.FromHours(6);
        }

        /// <summary>
        /// 获取数据方法
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public abstract T GetData();

        /// <summary>
        /// 获取缓存后数据 (如果没有缓存，读取数据后缓存)
        /// </summary>
        /// <returns></returns>
        public T GetCache()
        {
            var flag = CacheHelper.GetCache<T>(CacheKey, out var obj);

            if (flag)
            {
                return obj;
            }

            //
            obj = GetData();
            SetCache(obj);

            return obj;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="obj"></param>
        public bool SetCache(T obj)
        {
            if (obj == null)
            {
                return false;
            }

            return CacheHelper.SetCache(CacheKey, obj, TimeSpan);
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
            var obj = GetData();
            return SetCache(obj);
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
