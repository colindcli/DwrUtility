#if NETFULL
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace DwrUtility.Registers
{
    /// <summary>
    /// 注册表辅助类
    /// </summary>
    public class RegisterHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private RegistryKey SoftWare { get; }

        /// <summary>
        /// 返回RegistryKey对象
        /// </summary>
        /// <param name="keyType">注册表基项枚举</param>
        /// <returns></returns>
        private static RegistryKey GetRegistryKey(RegisterKeyType keyType)
        {
            RegistryKey rk = null;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (keyType)
            {
                case RegisterKeyType.HKEY_CLASS_ROOT:
                    rk = Registry.ClassesRoot;
                    break;
                case RegisterKeyType.HKEY_CURRENT_USER:
                    rk = Registry.CurrentUser;
                    break;
                case RegisterKeyType.HKEY_LOCAL_MACHINE:
                    rk = Registry.LocalMachine;
                    break;
                case RegisterKeyType.HKEY_USERS:
                    rk = Registry.Users;
                    break;
                case RegisterKeyType.HKEY_CURRENT_CONFIG:
                    rk = Registry.CurrentConfig;
                    break;
            }

            return rk;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="keyType">注册表基项枚举</param>
        /// <param name="baseKey">基项的名称</param>
        public RegisterHelper(RegisterKeyType keyType, string baseKey = "SOFTWARE")
        {
            var rk = GetRegistryKey(keyType);
            SoftWare = rk.OpenSubKey(baseKey, true);
        }

        /// <summary>
        /// 删除注册表项
        /// </summary>
        /// <param name="item">注册表项，如Tencent或Tencent\\QQBrowser</param>
        /// <returns></returns>
        public void DeleteItem(string item)
        {
            SoftWare.DeleteSubKeyTree(item);
        }

        /// <summary>
        /// 删除注册表项中的键
        /// </summary>
        /// <param name="item">注册表项，如Tencent或Tencent\\QQBrowser</param>
        /// <param name="key">值名称</param>
        public void DeleteName(string item, string key)
        {
            var rkt = SoftWare.OpenSubKey(item, true);
            var value = rkt?.GetValue(key);
            if (value != null)
            {
                rkt.DeleteValue(key, true);
            }
        }

        /// <summary>
        /// 判断项是否存在
        /// </summary>
        /// <param name="item">注册表项，如Tencent或Tencent\\QQBrowser</param>
        /// <returns></returns>
        public bool IsExistItem(string item)
        {
            return SoftWare.OpenSubKey(item) != null;
        }

        /// <summary>
        /// 获取项的键值
        /// </summary>
        /// <param name="item">注册表项，如Tencent或Tencent\\QQBrowser</param>
        /// <param name="name">值名称</param>
        /// <returns>返回字符串</returns>
        public object GetValue(string item, string name)
        {
            var rkt = SoftWare.OpenSubKey(item);
            return rkt?.GetValue(name);
        }

        /// <summary>
        /// 获取项的键值
        /// </summary>
        /// <param name="item">注册表项，如Tencent或Tencent\\QQBrowser</param>
        /// <returns></returns>
        public List<KeyValueResult> GetValues(string item)
        {
            var rkt = SoftWare?.OpenSubKey(item);
            if (rkt == null)
            {
                return new List<KeyValueResult>();
            }

            var names = rkt.GetValueNames();
            if (names.Length == 0)
            {
                return new List<KeyValueResult>();
            }

            return names.Select(name => new KeyValueResult()
            {
                Key = name,
                Value = rkt.GetValue(name)
            }).ToList();
        }

        /// <summary>
        /// 写入注册表,如果指定项已经存在,则修改指定项的值
        /// </summary>
        /// <param name="item">注册表项</param>
        /// <param name="name">值名称</param>
        /// <param name="value">值</param>
        public bool SetValue(string item, string name, string value)
        {
            var rkt = SoftWare?.CreateSubKey(item);
            if (rkt == null)
            {
                return false;
            }

            rkt.SetValue(name, value);
            return true;
        }
    }
}

#endif
