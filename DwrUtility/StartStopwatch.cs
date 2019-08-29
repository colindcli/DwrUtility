using DwrUtility.TaskExt;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DwrUtility
{
    /// <summary>
    /// 代码计时
    /// </summary>
    public class StartStopwatch
    {
        private Stopwatch Sw { get; set; }
        private Action<object, Exception> Log { get; set; }

        /// <summary>
        /// 开始计时
        /// </summary>
        public StartStopwatch()
        {
            Sw = new Stopwatch();
            Sw.Start();
        }

        /// <summary>
        /// 开始计时 (日志输出)
        /// </summary>
        /// <param name="log">写入日志</param>
        public StartStopwatch(Action<object, Exception> log)
        {
            Log = log;
            Sw = new Stopwatch();
            Sw.Start();
        }

        /// <summary>
        /// 获取时间并停止
        /// </summary>
        public long GetTime()
        {
            Sw.Stop();
            return Sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// 获取时间并重置继续
        /// </summary>
        public long GetTimeRestart()
        {
            Sw.Stop();
            var ms = Sw.ElapsedMilliseconds;
            Sw.Restart();

            return ms;
        }

        /// <summary>
        /// 获取时间并停止
        /// </summary>
        public void ConsoleTime(string title = null)
        {
            Sw.Stop();
            var ms = Sw.ElapsedMilliseconds;
            Console.WriteLine($"{title}{(title != null ? ": " : "")}{ms}ms");
        }

        /// <summary>
        /// 获取时间并重置继续
        /// </summary>
        public void ConsoleTimeRestart(string title = null)
        {
            Sw.Stop();
            var ms = Sw.ElapsedMilliseconds;
            Console.WriteLine($"{title}{(title != null ? ": " : "")}{ms}ms");
            Sw.Restart();
        }

        /// <summary>
        /// 获取时间并停止
        /// </summary>
        public void LogTime(string title = null)
        {
            Sw.Stop();
            var ms = Sw.ElapsedMilliseconds;
            Log?.Invoke($"{title}{(title != null ? ": " : "")}{ms}ms", null);
        }

        /// <summary>
        /// 获取时间并重置继续
        /// </summary>
        public void LogTimeRestart(string title = null)
        {
            Sw.Stop();
            var ms = Sw.ElapsedMilliseconds;
            Log?.Invoke($"{title}{(title != null ? ": " : "")}{ms}ms", null);
            Sw.Restart();
        }

        /// <summary>
        /// 获取各线程任务运行时间（1、有误；2、生成环境必须移除）
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public string GetTaskTime(params Expression<Func<Task>>[] memberExpression)
        {
            var str = string.Empty;
            TaskHelper.GetTaskTime(Sw, s => str = s, memberExpression);
            return str;
        }

        /// <summary>
        /// 获取各线程任务运行时间（1、有误；2、生成环境必须移除）
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public void LogTaskTime(params Expression<Func<Task>>[] memberExpression)
        {
            TaskHelper.GetTaskTime(Sw, s => Log?.Invoke(s, null), memberExpression);
        }

        /// <summary>
        /// 获取各线程任务运行时间（1、有误；2、生成环境必须移除）
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public void ConsoleTaskTime(params Expression<Func<Task>>[] memberExpression)
        {
            TaskHelper.GetTaskTime(Sw, Console.WriteLine, memberExpression);
        }
    }
}
