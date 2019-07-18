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
        public static Task<T> SetTimeout<T>(Task<T> task, int millisecondsDelay)
        {
            var completedTask = Task.WhenAny(task, Task.Delay(millisecondsDelay));
            Task.WaitAll(completedTask);
            return completedTask.Result == task
                ? task :
                Task.Run(() => default(T));
        }
    }
}
