namespace DwrUtility.Images
{
    /// <summary>
    /// 缩略图类型
    /// </summary>
    public enum ThumbnailType
    {
        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        Zoom = 1,
        /// <summary>
        /// 不按比例缩小到指定宽高
        /// </summary>
        NotProportional = 2,
        /// <summary>
        /// 从原图中间切割一张指定宽高的图片
        /// </summary>
        Cut = 3,
    }
}
