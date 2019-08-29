using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// 代替Task.WaitAll()，抛出异常明细
        /// </summary>
        /// <param name="tasks"></param>
        public static void WaitAll(params Task[] tasks)
        {
            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception)
            {
                var exs = (from task in tasks where task.IsFaulted where task.Exception != null select task.Exception.ToString()).ToList();
                throw new Exception(string.Join("\r\n", exs));
            }
        }

        /// <summary>
        /// 获取各线程任务运行时间 (生成环境必须移除)
        /// </summary>
        /// <param name="sw">new Stopwatch()，且必须启动了</param>
        /// <param name="log"></param>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        //[Conditional("DEBUG")]
        internal static void GetTaskTime(Stopwatch sw, Action<string> log, params Expression<Func<Task>>[] memberExpression)
        {
            var ts = memberExpression.Select(item => item.Compile().Invoke()).ToList();
            var lts = new List<KeyValue<long, Task>>();
            while (ts.Count > 0)
            {
                Task.WaitAny(ts.ToArray());

                var lt = ts.Where(p => p.IsCompleted).ToList();
                ts.RemoveAll(p => p.IsCompleted);

                sw.Stop();
                var sc = sw.ElapsedMilliseconds;
                sw.Start();

                foreach (var it in lt)
                {
                    lts.Add(new KeyValue<long, Task>()
                    {
                        Key = sc,
                        Value = it
                    });
                }
            }
            sw.Stop();

            //计算名称
            var dict = new Dictionary<int, string>();
            foreach (var item in memberExpression)
            {
                var t = item.Compile().Invoke();
                var id = t.Id;
                var name = GetMemberName(item);
                dict.Add(id, name);
            }

            log?.Invoke(string.Join("\r\n", lts.Select(p => $"{dict[p.Value.Id]} => {p.Key}ms")));
        }

        /// <summary>
        /// 获取变量名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        private static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            var expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        internal static Task<T> SetTimeout<T>(Task<T> task, int millisecondsDelay)
        {
            return Task.Run(() =>
            {
                var completedTask = Task.WhenAny(task, Task.Delay(millisecondsDelay));
                if (completedTask.Result == task)
                {
                    return task.Result;
                }
                return default(T);
            });
        }

        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值（返回值和是否超时字段）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        internal static Task<TimeoutResult<T>> SetTimeoutResult<T>(Task<T> task, int millisecondsDelay)
        {
            return Task.Run(() =>
            {
                var completedTask = Task.WhenAny(task, Task.Delay(millisecondsDelay));
                if (completedTask.Result == task)
                {
                    return new TimeoutResult<T>()
                    {
                        Value = task.Result,
                        IsTimeout = false
                    };
                }

                return new TimeoutResult<T>()
                {
                    Value = default(T),
                    IsTimeout = true
                };
            });
        }
    }
}
