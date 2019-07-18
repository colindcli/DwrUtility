namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 返回执行结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimeoutResult<T>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsTimeout { get; set; }
    }
}
