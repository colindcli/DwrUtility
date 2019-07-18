using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 任务扩展
    /// </summary>
    public static class TaskExtension
    {
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
    }
}
