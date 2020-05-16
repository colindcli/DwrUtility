using System;
using System.Threading;
using System.Threading.Tasks;

namespace DwrUtility
{
    /// <summary>
    /// 一直循环执行方法
    /// </summary>
    public class WhileHelper
    {
        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        public static void StartAsync(bool executeImmediately, int millisecondsTimeout, Action action)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        public static void StartAsync<T>(bool executeImmediately, int millisecondsTimeout, Action<T> action, T t1)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public static void StartAsync<T1, T2>(bool executeImmediately, int millisecondsTimeout, Action<T1, T2> action, T1 t1, T2 t2)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1, t2);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        public static void StartAsync<T1, T2, T3>(bool executeImmediately, int millisecondsTimeout, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1, t2, t3);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        public static void StartAsync<T1, T2, T3, T4>(bool executeImmediately, int millisecondsTimeout, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1, t2, t3, t4);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        public static void StartAsync<T1, T2, T3, T4, T5>(bool executeImmediately, int millisecondsTimeout, Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1, t2, t3, t4, t5);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }

        /// <summary>
        /// 一直循环执行方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="executeImmediately">是否马上执行</param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="action"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <param name="t6"></param>
        public static void StartAsync<T1, T2, T3, T4, T5, T6>(bool executeImmediately, int millisecondsTimeout, Action<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (!executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    try
                    {
                        action?.Invoke(t1, t2, t3, t4, t5, t6);
                    }
                    catch (Exception ex)
                    {
                        DwrUtilitySetting.Log?.Invoke(ex);
                    }
                    if (executeImmediately)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                }
            });
        }
    }
}
