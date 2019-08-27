using System;
using System.Collections.Generic;

namespace DwrUtility
{
    /// <summary>
    /// 对象去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityComparer<T> : IEqualityComparer<T> where T : new()
    {
        private Func<T, T, bool> Func { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public EqualityComparer(Func<T, T, bool> func)
        {
            Func = func;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return Func.Invoke(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return 1;
        }
    }
}
