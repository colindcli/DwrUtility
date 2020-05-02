using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 任务扩展
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// 批量运行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<Task> RunTask<T>(this List<T> list, Action<T> action)
        {
            return TaskHelper.RunTask(list, action);
        }

        /// <summary>
        /// 批量运行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<Task<TReturn>> RunTask<T, TReturn>(this List<T> list, Func<T, TReturn> func)
        {
            return TaskHelper.RunTask(list, func);
        }

        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        public static Task<T> SetTimeout<T>(this Task<T> task, int millisecondsDelay)
        {
            return TaskHelper.SetTimeout(task, millisecondsDelay);
        }

        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值（返回值和是否超时字段）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        public static Task<TimeoutResult<T>> SetTimeoutResult<T>(this Task<T> task, int millisecondsDelay)
        {
            return TaskHelper.SetTimeoutResult(task, millisecondsDelay);
        }

        /// <summary>
        /// 运行多个线程，定时返回结果，直到完成为止；action(int, List)(已完成总数，在这批waitTime内完成的任务)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action">(int, List)(已完成总数，这在等待时间内完成的线程)</param>
        public static void TaskStatus<T>(this List<Task<T>> tasks, int waitTimeMilliseconds, Action<int, List<T>> action)
        {
            TaskHelper.TaskStatus(tasks, waitTimeMilliseconds, action);
        }

        /// <summary>
        /// 运行多个线程，定时返回结果，直到完成为止；action(int)(已完成总数)
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action">已完成总数</param>
        public static void TaskStatus(this List<Task> tasks, int waitTimeMilliseconds, Action<int> action)
        {
            TaskHelper.TaskStatus(tasks, waitTimeMilliseconds, action);
        }

        /// <summary>
        /// 运行单个线程，定时返回结果，直到完成为止；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action"></param>
        public static void TaskStatus<T>(this Task<T> task, int waitTimeMilliseconds, Action action)
        {
            TaskHelper.TaskStatus(task, waitTimeMilliseconds, action);
        }

        /// <summary>
        /// 运行单个线程，定时返回结果，直到完成为止；
        /// </summary>
        /// <param name="task"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action"></param>
        public static void TaskStatus(this Task task, int waitTimeMilliseconds, Action action)
        {
            TaskHelper.TaskStatus(task, waitTimeMilliseconds, action);
        }

        /// <summary>
        /// 获取各线程任务运行时间
        /// </summary>
        /// <returns>返回个线程运行时间，如果运行失败返回-1</returns>
        public static long GetTaskTime(this Task task)
        {
            var res = TaskHelper.GetTaskTime(new List<Task>() { task });
            if (res.Count > 0)
            {
                return res[0];
            }

            return -1;
        }

        /// <summary>
        /// 获取各线程任务运行时间
        /// </summary>
        /// <returns>返回个线程运行时间，如果运行失败返回-1</returns>
        public static long GetTaskTime<T>(this Task<T> task)
        {
            var res = TaskHelper.GetTaskTime(new List<Task>() { task });
            if (res.Count > 0)
            {
                return res[0];
            }

            return -1;
        }

        /// <summary>
        /// 获取各线程任务运行时间
        /// </summary>
        /// <returns>按顺序返回个线程运行时间</returns>
        public static List<long> GetTaskTime(this List<Task> tasks)
        {
            return TaskHelper.GetTaskTime(tasks);
        }

        /// <summary>
        /// 获取各线程任务运行时间
        /// </summary>
        /// <returns>按顺序返回个线程运行时间</returns>
        public static List<long> GetTaskTime<T>(this List<Task<T>> tasks)
        {
            var items = tasks.Cast<Task>().ToList();
            return TaskHelper.GetTaskTime(items);
        }
    }
}
