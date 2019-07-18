using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DwrUtility
{
    /// <summary>
    /// 加密解密帮助类
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// 文件MD5值
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>文件不存在或报错返回NULL</returns>
        public static string FileMd5(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            try
            {
                var md5 = new MD5CryptoServiceProvider();
                var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var retVal = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                foreach (var t in retVal)
                {
                    sb.Append(t.ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">this</param>
        /// <returns>MD5值</returns>
        public static string Md5(string str)
        {
            if (str == null)
            {
                return null;
            }

            var md5 = MD5.Create();

            // 计算字符串的散列值
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sbd = new StringBuilder();
            foreach (var item in bytes)
            {
                //如果使用大写（X）则格式后的字符是大写字符
                sbd.Append(item.ToString("x2"));
            }
            return sbd.ToString();
        }

        #region DES加密/解密
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="key">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回null</returns>
        public static string EncryptDes(string str, string key)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                var rgbIv = rgbKey;
                var inputByteArray = Encoding.UTF8.GetBytes(str);
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                var ret = new StringBuilder();
                foreach (var b in mStream.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="str">待解密的字符串</param>
        /// <param name="key">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返回null</returns>
        public static string DecryptDes(string str, string key)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(key);
                var rgbIv = rgbKey;
                var inputByteArray = new byte[str.Length / 2];
                for (var x = 0; x < str.Length / 2; x++)
                {
                    var i = (Convert.ToInt32(str.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
