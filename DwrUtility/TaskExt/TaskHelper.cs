using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// 批量运行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<Task> RunTask<T>(List<T> list, Action<T> action)
        {
            return list.Select(li => Task.Run(() => action?.Invoke(li))).ToList();
        }

        /// <summary>
        /// 批量运行任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<Task<TReturn>> RunTask<T, TReturn>(List<T> list, Func<T, TReturn> func)
        {
            return list.Select(li => Task.Run(() => func.Invoke(li))).ToList();
        }

        /// <summary>
        /// 批量运行任务（限制线程大小）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="param"></param>
        public static void RunTaskLimite<T>(List<T> list, TaskParam<T> param)
        {
            if (param.Method == null)
            {
                throw new Exception("Func字段没赋值");
            }
            if (param.MaxThread < 1)
            {
                throw new Exception("MaxThread不能小于1");
            }

            //在2秒内没有返回值时再次通知
            var continuTime = 2000;

            var running = new List<Task>();
            var complete = 0;
            var record = new Dictionary<int, T>();
            foreach (var item in list)
            {
                var t = Task.Run(() => param.Method?.Invoke(item));
                record.Add(t.Id, item);
                running.Add(t);

                //进度通知
                param.Report?.Invoke(ReportTypeEnum.Start, complete, complete + running.Count, item, 0);

                if (param.Sleep > 0)
                {
                    Thread.Sleep(param.Sleep);
                    //处理已完成进度
                    complete = Complete(param, running, record, complete);
                }

                if (running.Count < param.MaxThread)
                {
                    continue;
                }

                //等待循环
                Wait(param, running, record, complete, continuTime);
                //处理已完成进度
                complete = Complete(param, running, record, complete);
            }

            //列表已经循环结束了，但任务还有等待完成的
            while (running.Count > 0)
            {
                //等待循环
                Wait(param, running, record, complete, continuTime);
                //处理已完成进度
                complete = Complete(param, running, record, complete);
            }
        }

        /// <summary>
        /// 等待循环
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="running"></param>
        /// <param name="record"></param>
        /// <param name="complete"></param>
        /// <param name="continuTime"></param>
        private static void Wait<T>(TaskParam<T> param, List<Task> running, Dictionary<int, T> record, int complete, int continuTime)
        {
            var newItem = record[running[0].Id];
            var num = 0;
            while (true)
            {
                //等待是每2秒返回一次通知，取正在进行中的第一个
                param.Report?.Invoke(ReportTypeEnum.Wait, complete, complete + running.Count, newItem, num);
                num++;
                var obj = Task.WhenAny(running.ToArray()).SetTimeoutResult2(continuTime);
                if (!obj.IsTimeout)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 处理已完成进度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="running"></param>
        /// <param name="record"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        private static int Complete<T>(TaskParam<T> param, List<Task> running, Dictionary<int, T> record, int complete)
        {
            var res = running.Where(p => p.IsCanceled || p.IsCompleted || p.IsFaulted).ToList();
            if (res.Count <= 0)
            {
                return complete;
            }

            complete += res.Count;
            foreach (var r in res)
            {
                running.Remove(r);
            }

            //进度通知
            param.Report?.Invoke(ReportTypeEnum.Finished, complete, complete + running.Count, record[res[0].Id], 0);
            return complete;
        }

        /// <summary>
        /// 批量运行任务（限制线程大小）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="list"></param>
        /// <param name="param"></param>
        public static List<KeyValue<T, TReturn>> RunTaskLimite<T, TReturn>(List<T> list, TaskParam<T, TReturn> param)
        {
            if (param.Method == null)
            {
                throw new Exception("Func字段没赋值");
            }
            if (param.MaxThread < 1)
            {
                throw new Exception("MaxThread不能小于1");
            }

            //在2秒内没有返回值时再次通知
            var continuTime = 2000;

            var running = new List<Task<TReturn>>();
            var complete = new List<Task<TReturn>>();
            var record = new Dictionary<int, T>();
            foreach (var item in list)
            {
                var t = Task.Run(() => param.Method.Invoke(item));
                record.Add(t.Id, item);
                running.Add(t);

                //进度通知
                param.Report?.Invoke(ReportTypeEnum.Start, complete.Count, complete.Count + running.Count, item, 0);

                if (param.Sleep > 0)
                {
                    Thread.Sleep(param.Sleep);
                    //处理已完成进度
                    Complete(param, running, record, complete);
                }

                if (running.Count < param.MaxThread)
                {
                    continue;
                }

                //等待循环
                Wait(param, running, record, complete, continuTime);
                //处理已完成进度
                Complete(param, running, record, complete);
            }

            //列表已经循环结束了，但任务还有等待完成的
            while (running.Count > 0)
            {
                //等待循环
                Wait(param, running, record, complete, continuTime);
                //处理已完成进度
                Complete(param, running, record, complete);
            }

            return record.Join(complete, p => p.Key, p => p.Id, (p, q) => new KeyValue<T, TReturn>()
            {
                Key = p.Value,
                Value = q.Result,
            }).ToList();
        }

        /// <summary>
        /// 等待循环
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="param"></param>
        /// <param name="running"></param>
        /// <param name="record"></param>
        /// <param name="complete"></param>
        /// <param name="continuTime"></param>
        private static void Wait<T, TReturn>(TaskParam<T, TReturn> param, List<Task<TReturn>> running, Dictionary<int, T> record, List<Task<TReturn>> complete, int continuTime)
        {
            var newItem = record[running[0].Id];
            var num = 0;
            while (true)
            {
                //等待是每2秒返回一次通知，取正在进行中的第一个
                param.Report?.Invoke(ReportTypeEnum.Wait, complete.Count, complete.Count + running.Count, newItem, num);
                num++;
                var obj = Task.WhenAny(running.ToArray()).SetTimeoutResult2(continuTime);
                if (!obj.IsTimeout)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 处理已完成进度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="param"></param>
        /// <param name="running"></param>
        /// <param name="record"></param>
        /// <param name="complete"></param>
        private static void Complete<T, TReturn>(TaskParam<T, TReturn> param, List<Task<TReturn>> running, Dictionary<int, T> record, List<Task<TReturn>> complete)
        {
            var res = running.Where(p => p.IsCanceled || p.IsCompleted || p.IsFaulted).ToList();
            if (res.Count <= 0)
            {
                return;
            }

            complete.AddRange(res);
            foreach (var r in res)
            {
                running.Remove(r);
            }

            //进度通知
            param.Report?.Invoke(ReportTypeEnum.Finished, complete.Count, complete.Count + running.Count, record[res[0].Id], 0);
        }

        /// <summary>
        /// 运行多个线程，定时返回结果，直到完成为止；action(int, List)(已完成总数，在这批waitTime内完成的任务)
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="waitTimeMilliseconds">等待多长时间通知一次消息</param>
        /// <param name="action">(int, List)(已完成总数，这在等待时间内完成的线程)</param>
        public static void TaskStatus<T>(List<Task<T>> tasks, int waitTimeMilliseconds, Action<int, List<T>> action)
        {
            var array = tasks.ToArray();
            var completes = new List<Task<T>>();
            while (true)
            {
                // ReSharper disable once CoVariantArrayConversion
                var b = Task.WaitAll(array, waitTimeMilliseconds);
                var rs = tasks.Where(p => p.IsCompleted || p.IsCanceled || p.IsFaulted).ToList();
                var news = (from r in rs
                            join c in completes on r.Id equals c.Id into temp
                            where temp == null || !temp.Any()
                            join task in tasks on r.Id equals task.Id
                            select task).ToList();
                completes.AddRange(news);
                action?.Invoke(completes.Count, news.Select(p => p.Result).ToList());
                if (b || tasks.Count == completes.Count)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 运行多个线程，定时返回结果，直到完成为止；action(int)(已完成总数)
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="waitTimeMilliseconds">等待多长时间通知一次消息</param>
        /// <param name="action">已完成总数</param>
        public static void TaskStatus(List<Task> tasks, int waitTimeMilliseconds, Action<int> action)
        {
            var array = tasks.ToArray();
            while (true)
            {
                // ReSharper disable once CoVariantArrayConversion
                var b = Task.WaitAll(array, waitTimeMilliseconds);
                var count = tasks.Count(p => p.IsCompleted || p.IsCanceled || p.IsFaulted);
                action?.Invoke(count);
                if (b || tasks.Count == count)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 运行单个线程，定时返回结果，直到完成为止；
        /// </summary>
        /// <param name="task"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action"></param>
        public static void TaskStatus(Task task, int waitTimeMilliseconds, Action action)
        {
            var array = new[] { task };
            while (true)
            {
                var b = Task.WaitAll(array, waitTimeMilliseconds);
                action?.Invoke();
                if (b)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 运行单个线程，定时返回结果，直到完成为止；
        /// </summary>
        /// <param name="task"></param>
        /// <param name="waitTimeMilliseconds">等待时间</param>
        /// <param name="action">返回次数</param>
        public static void TaskStatus(Task task, int waitTimeMilliseconds, Action<int> action)
        {
            var array = new[] { task };
            var num = 0;
            while (true)
            {
                var b = Task.WaitAll(array, waitTimeMilliseconds);
                num++;
                action?.Invoke(num);
                if (b)
                {
                    break;
                }
            }
        }

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
        /// 获取各线程任务运行时间
        /// </summary>
        /// <returns>按顺序返回个线程运行时间</returns>
        public static List<long> GetTaskTime(List<Task> ts)
        {
            var list = ts.Select(p => p).ToList();
            var sw = new Stopwatch();
            sw.Start();
            var res = new List<KeyValue<int, long>>();
            while (list.Count > 0)
            {
                Task.WaitAny(list.ToArray());
                var lt = list.Where(p => p.IsCompleted || p.IsCanceled || p.IsFaulted).ToList();
                list.RemoveAll(p => p.IsCompleted);
                sw.Stop();
                var sc = sw.ElapsedMilliseconds;
                sw.Start();

                res.AddRange(lt.Select(it => new KeyValue<int, long>()
                {
                    Key = it.Id,
                    Value = sc
                }));
            }
            sw.Stop();

            return ts.Join(res, p => p.Id, p => p.Key, (p, q) => q.Value).ToList();
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

        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值（返回值和是否超时字段）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        internal static TimeoutResult<T> SetTimeoutResult2<T>(Task<T> task, int millisecondsDelay)
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
        }
    }
}
