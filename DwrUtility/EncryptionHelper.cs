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
        #region Md5
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
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
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
        #endregion

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
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
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
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return null;
            }
        }
        #endregion

        #region SHA
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string Sha1(string s)
        {
            return GetSha(s, new SHA1CryptoServiceProvider());
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string Sha256(string s)
        {
            return GetSha(s, new SHA256CryptoServiceProvider());
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string Sha384(string s)
        {
            return GetSha(s, new SHA384CryptoServiceProvider());
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string Sha512(string s)
        {
            return GetSha(s, new SHA512CryptoServiceProvider());
        }

        /// <summary>
        /// SHA加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="provider"></param>
        /// <returns>失败返回null</returns>
        private static string GetSha(string s, ICryptoTransform provider)
        {
            try
            {
                var bytesIn = Encoding.UTF8.GetBytes(s);
                var bytesOut = ((HashAlgorithm)provider).ComputeHash(bytesIn);
                provider.Dispose();
                var result = BitConverter.ToString(bytesOut);
                result = result.Replace("-", "");
                return result.ToLower();
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return null;
            }
        }
        #endregion

        #region Base64
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string EncryptBase64(string s)
        {
            try
            {
                var array = Encoding.UTF8.GetBytes(s);
                return Convert.ToBase64String(array);
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return null;
            }
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="s"></param>
        /// <returns>失败返回null</returns>
        public static string DecryptBase64(string s)
        {
            try
            {
                var bytes = Convert.FromBase64String(s);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return null;
            }
        }
        #endregion

        #region RSA

        /// <summary>
        /// RSA产生密钥
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="dwKeySize"></param>
        public static bool RsaGenerateKey(out string privateKey, out string publicKey, int dwKeySize = 1024)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider(dwKeySize);
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                privateKey = publicKey = null;
                return false;
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKey"></param>
        /// <param name="result">加密结果</param>
        /// <returns></returns>
        public static bool RsaEncrypt(string content, string publicKey, out string result)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKey);
                var bt = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                result = Convert.ToBase64String(bt);
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateKey"></param>
        /// <param name="result">解密结果</param>
        /// <returns></returns>
        public static bool RsaDecrypt(string content, string privateKey, out string result)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privateKey);
                var bt = rsa.Decrypt(Convert.FromBase64String(content), false);
                result = Encoding.UTF8.GetString(bt);
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="signData">签名</param>
        /// <returns>是否签名成功</returns>
        public static bool RsaSignData(string content, string privateKey, out byte[] signData)
        {
            try
            {
                var bt = Encoding.UTF8.GetBytes(content);
                var provider = new RSACryptoServiceProvider();
                provider.FromXmlString(privateKey);
                signData = provider.SignData(bt, "SHA512");
                return true;
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                signData = null;
                return false;
            }
        }

        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="signData">签名</param>
        /// <returns>签名是否正确</returns>
        public static bool RsaVerify(string content, string publicKey, byte[] signData)
        {
            try
            {
                var bt = Encoding.UTF8.GetBytes(content);
                var provider = new RSACryptoServiceProvider();
                provider.FromXmlString(publicKey);
                return provider.VerifyData(bt, "SHA512", signData);
            }
            catch (Exception ex)
            {
                DwrUtilitySetting.Log?.Invoke(ex);
                return false;
            }
        }

        #endregion
    }
}
