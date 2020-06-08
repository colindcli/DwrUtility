using System;

namespace DwrUtility.TaskExt
{
    /// <summary>
    /// 批量运行任务参数
    /// </summary>
    public class TaskParam<T>
    {
        /// <summary>
        /// 业务处理函数
        /// </summary>
        public Action<T> Method { get; set; }
        /// <summary>
        /// 同步处理最大线程数
        /// </summary>
        public int MaxThread { get; set; }
        /// <summary>
        /// 当前完成进度 (type, start, end, T, num)，type消息类型，start已完成进度 - end已完成+进行中，T处理项，num只有ReportTypeEnum.Wait时才有次数
        /// </summary>
        public Action<ReportTypeEnum, int, int, T, int> Report { get; set; }
        /// <summary>
        /// 暂停多少毫秒后继续
        /// </summary>
        public int Sleep { get; set; }
    }

    /// <summary>
    /// 批量运行任务参数
    /// </summary>
    public class TaskParam<T, TReturn>
    {
        /// <summary>
        /// 业务处理函数
        /// </summary>
        public Func<T, TReturn> Method { get; set; }
        /// <summary>
        /// 同步处理最大线程数
        /// </summary>
        public int MaxThread { get; set; }
        /// <summary>
        /// 当前完成进度 (type, start, end, T, num)，type消息类型，start已完成进度 - end已完成+进行中，T处理项，num只有ReportTypeEnum.Wait时才有次数
        /// </summary>
        public Action<ReportTypeEnum, int, int, T, int> Report { get; set; }
        /// <summary>
        /// 暂停多少毫秒后继续
        /// </summary>
        public int Sleep { get; set; }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public enum ReportTypeEnum
    {
        /// <summary>
        /// 开始加入列队
        /// </summary>
        Start = 1,
        /// <summary>
        /// 等待完成
        /// </summary>
        Wait = 2,
        /// <summary>
        /// 完成
        /// </summary>
        Finished = 3,
    }
}
