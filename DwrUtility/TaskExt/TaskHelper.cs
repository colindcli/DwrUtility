using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskHelper
    {
        /// <summary>
        /// 设置有返回值的线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        public static Task<T> SetTimeout<T>(Task<T> task, int millisecondsDelay)
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
        public static Task<TimeoutResult<T>> SetTimeoutResult<T>(Task<T> task, int millisecondsDelay)
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
