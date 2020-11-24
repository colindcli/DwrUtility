#if NETFULL
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace DwrUtility
{
    /// <summary>
    /// 获取或修改配置文件
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private Configuration Config { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">web.config或app.config文件路径</param>
        public ConfigHelper(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new Exception("path文件不存在");
            }

            var map = new ExeConfigurationFileMap
            {
                ExeConfigFilename = path
            };
            Config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 获取ConnectionString值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            return Config.ConnectionStrings.ConnectionStrings[name]?.ConnectionString;
        }

        /// <summary>
        /// 获取AppSetting值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAppSetting(string key)
        {
            return Config.AppSettings.Settings[key]?.Value;
        }

        /// <summary>
        /// 获取ConnectionString值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string path, string name)
        {
            return new ConfigHelper(path).GetConnectionString(name);
        }

        /// <summary>
        /// 获取AppSetting值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string path, string key)
        {
            return new ConfigHelper(path).GetAppSetting(key);
        }

        #region 公共方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tagName"></param>
        /// <param name="keyAtrrName"></param>
        /// <param name="keyAttrValue"></param>
        /// <param name="valueAttrName"></param>
        /// <param name="valueAttrValue"></param>
        private static bool SetValue(string path, string tagName, string keyAtrrName, string keyAttrValue, string valueAttrName, string valueAttrValue)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            var nodeList = xml.GetElementsByTagName(tagName);

            if (nodeList.Count == 0)
            {
                //在configuration下创建一个节点
                nodeList = xml.GetElementsByTagName("configuration");
                if (nodeList.Count == 0)
                {
                    return false;
                }

                var nd = nodeList[0];
                var e = xml.CreateElement(tagName);
                nd.AppendChild(e);

                //重新获取节点
                nodeList = xml.GetElementsByTagName(tagName);
            }

            var node = nodeList[0];
            var flag = false;
            foreach (XmlNode item in node)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                if (item.Attributes == null)
                {
                    continue;
                }

                var attr = item.Attributes[keyAtrrName];
                if (!string.Equals(attr.Value, keyAttrValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                item.Attributes[valueAttrName].Value = valueAttrValue;

                //保存
                xml.Save(path);
                flag = true;
                break;
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tagName"></param>
        /// <param name="keyAtrrName"></param>
        /// <param name="keyAttrValue"></param>
        /// <param name="valueAttrName"></param>
        /// <param name="valueAttrValue"></param>
        private static bool AddValue(string path, string tagName, string keyAtrrName, string keyAttrValue, string valueAttrName, string valueAttrValue)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            var nodeList = xml.GetElementsByTagName(tagName);

            if (nodeList.Count == 0)
            {
                //在configuration下创建一个节点
                nodeList = xml.GetElementsByTagName("configuration");
                if (nodeList.Count == 0)
                {
                    return false;
                }

                var nd = nodeList[0];
                var e = xml.CreateElement(tagName);
                nd.AppendChild(e);

                //重新获取节点
                nodeList = xml.GetElementsByTagName(tagName);
            }

            var node = nodeList[0];
            var exists = false;
            foreach (XmlNode item in node)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                var attr = item.Attributes?[keyAtrrName];
                if (attr == null)
                {
                    continue;
                }

                if (!string.Equals(attr.Value, keyAttrValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                item.Attributes[valueAttrName].Value = valueAttrValue;
                exists = true;
                break;
            }

            //已存在保存
            if (exists)
            {
                xml.Save(path);
                return true;
            }

            //不存在添加
            var el = xml.CreateElement("add");
            el.SetAttribute(keyAtrrName, keyAttrValue);
            el.SetAttribute(valueAttrName, valueAttrValue);
            node.AppendChild(el);
            xml.Save(path);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tagName"></param>
        /// <param name="keyAtrrName"></param>
        /// <param name="keyAttrValue"></param>
        private static void RemoveValue(string path, string tagName, string keyAtrrName, string keyAttrValue)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            var nodeList = xml.GetElementsByTagName(tagName);

            if (nodeList.Count == 0)
            {
                return;
            }

            var node = nodeList[0];
            foreach (XmlNode item in node)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                var attr = item.Attributes?[keyAtrrName];
                if (attr == null)
                {
                    continue;
                }

                if (!string.Equals(attr.Value, keyAttrValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                node.RemoveChild(item);

                //保存
                xml.Save(path);
                break;
            }
        }

        #endregion

        #region connectionStrings

        /// <summary>
        /// 设置ConnectionString值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool UpdateConnectionString(string path, string name, string connectionString)
        {
            return SetValue(path, "connectionStrings", "name", name, "connectionString", connectionString);
        }

        /// <summary>
        /// 添加ConnectionString或更新ConnectionString值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool UpsetConnectionString(string path, string name, string connectionString)
        {
            return AddValue(path, "connectionStrings", "name", name, "connectionString", connectionString);
        }

        /// <summary>
        /// 删除ConnectionString
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void RemoveConnectionString(string path, string name)
        {
            RemoveValue(path, "connectionStrings", "name", name);
        }


        #endregion

        #region appSettings

        /// <summary>
        /// 设置AppSetting值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool UpdateAppSetting(string path, string key, string value)
        {
            return SetValue(path, "appSettings", "key", key, "value", value);
        }

        /// <summary>
        /// 添加AppSetting或更新AppSetting值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool UpsetAppSetting(string path, string key, string value)
        {
            return AddValue(path, "appSettings", "key", key, "value", value);
        }

        /// <summary>
        /// 删除AppSetting
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        public static void RemoveAppSetting(string path, string key)
        {
            RemoveValue(path, "appSettings", "key", key);
        }

        #endregion
    }
}
#endif
