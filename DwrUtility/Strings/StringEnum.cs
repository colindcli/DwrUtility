namespace DwrUtility.Strings
{
    /// <summary>
    /// 查找方向
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 从左往右搜索
        /// </summary>
        LeftRight = 1,
        /// <summary>
        /// 从右往左搜索
        /// </summary>
        RightLeft = 2,
    }

    /// <summary>
    /// 字符串不存在分割符时返回规则
    /// </summary>
    public enum DefaultValue
    {
        /// <summary>
        /// 从左往右搜索
        /// </summary>
        TrueAndValue = 1,
        /// <summary>
        /// 从右往左搜索
        /// </summary>
        FalseAndNull = 2,
    }
}
