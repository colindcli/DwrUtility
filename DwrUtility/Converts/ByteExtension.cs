namespace DwrUtility.Converts
{
    public static class ByteExtension
    {
        /// <summary>
        /// 从字节流判断编码（返回null是不能判断出编码）
        /// </summary>
        /// <param name="bt">输入字节流</param>
        /// <returns></returns>
        public static string GetEncoding(this byte[] bt)
        {
            return ConvertHelper.GetEncoding(bt);
        }
    }
}
