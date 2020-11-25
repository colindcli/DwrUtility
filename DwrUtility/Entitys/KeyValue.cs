namespace DwrUtility
{
    /// <summary>
    /// 泛型键值
    /// </summary>
    public class KeyValue<TKey, TValue>
    {
        /// <summary>
        /// 键
        /// </summary>
        public TKey Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 字符串键值
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
