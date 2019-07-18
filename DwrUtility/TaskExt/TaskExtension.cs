using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    public static class TaskExtension
    {
        /// <summary>
        /// 线程超时，超时后返回默认值
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
        /// 线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <param name="isTimeout">超时</param>
        /// <returns></returns>
        public static Task<T> SetTimeout<T>(this Task<T> task, int millisecondsDelay, out bool isTimeout)
        {
            return TaskHelper.SetTimeout(task, millisecondsDelay, out isTimeout);
        }
    }
}
