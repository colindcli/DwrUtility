using System.Threading.Tasks;

namespace DwrUtility.TaskExt
{
    public class TaskHelper
    {
        /// <summary>
        /// 线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <returns></returns>
        // ReSharper disable once FunctionRecursiveOnAllPaths
        public static Task<T> SetTimeout<T>(Task<T> task, int millisecondsDelay)
        {
            // ReSharper disable once TailRecursiveCall
            return SetTimeout(task, millisecondsDelay, out var _);
        }

        /// <summary>
        /// 线程超时，超时后返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="task">线程任务</param>
        /// <param name="millisecondsDelay">超时(毫秒)</param>
        /// <param name="isTimeout">超时</param>
        /// <returns></returns>
        public static Task<T> SetTimeout<T>(Task<T> task, int millisecondsDelay, out bool isTimeout)
        {
            var completedTask = Task.WhenAny(task, Task.Delay(millisecondsDelay));
            if (completedTask.Result == task)
            {
                isTimeout = false;
                return task;
            }

            isTimeout = true;
            return Task.Run(() => default(T));
        }
    }
}
