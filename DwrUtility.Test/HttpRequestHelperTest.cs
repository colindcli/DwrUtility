using DwrUtility.Https;
using DwrUtility.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DwrUtility.Test
{
    [TestClass]
    public class HttpRequestHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var url = "https://www.baidu.com/";
            var result = HttpRequestHelper.GetData(url);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var b2 = result.Content.IsContains("百度");
            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var url = "https://api.mch.weixin.qq.com/sandboxnew/pay/getsignkey";
            var xml = ObjectToXml(new
            {
                mch_id = "123456",
                nonce_str = Guid.NewGuid().ToString("N"),
            }, "123456");

            var result = HttpRequestHelper.PostPayload(url, xml);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var content = result.Content;
            var b2 = content.IsContains("请确认请求参数是否正确");
            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var url = $"{TestConfig.WebUrl}/Product/PostJson";
            var m = new RequestModel()
            {
                Guid = Guid.NewGuid()
            };
            var json = JsonConvert.SerializeObject(m);

            var result = HttpRequestHelper.PostJson(url, json);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var content = result.Content;
            var b2 = content.IsContains(m.Guid.ToString());
            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var url = $"{TestConfig.WebUrl}/Product/PostPayload";
            var m = new RequestModel()
            {
                Guid = Guid.NewGuid()
            };
            var payload = JsonConvert.SerializeObject(m);

            var result = HttpRequestHelper.PostPayload(url, payload);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var content = result.Content;
            var b2 = content.IsContains(m.Guid.ToString());
            Assert.IsTrue(b1 && b2);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var url = $"{TestConfig.WebUrl}/Product/PostForm";
            var id = "8852773b-d515-41a5-9d2a-dbccfe16326d";
            var name = "测试&名称=哈#哈";
            var dict = new Dictionary<string, string>()
            {
                {"Guid", id },
                {"Name", name },
            };

            var result = HttpRequestHelper.PostForm(url, dict);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var content = result.Content;
            var b2 = content.IsContains(id) && content.IsContains(name);
            Assert.IsTrue(b1 && b2);
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">提交对象</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string ObjectToXml<T>(T obj, string key)
        {
            var props = typeof(T).GetProperties();
            var kvs = (from prop in props
                       where !prop.Name.IsEquals("sign")
                       select new KeyValue()
                       {
                           Key = prop.Name,
                           Value = prop.GetValue(obj)?.ToString(),
                       }).ToList();

            var list = kvs.Where(p => !p.Value.IsWhiteSpace()).OrderBy(p => p.Key).ToList();
            var signStr = $"{string.Join("&", list.Select(p => $"{p.Key}={p.Value}"))}&key={key}";
            kvs.Add(new KeyValue()
            {
                Key = "sign",
                Value = signStr.ToMd5().ToUpper(),
            });

            var sb = new StringBuilder();
            sb.AppendLine("<xml>");
            foreach (var item in kvs)
            {
                sb.AppendLine($"<{item.Key}><![CDATA[{item.Value}]]></{item.Key}>");
            }
            sb.AppendLine("</xml>");

            return sb.ToString();
        }

        public class RequestModel
        {
            public Guid Guid { get; set; }

            public string Name { get; set; }
        }

        /// <summary>
        /// 连接超时测试
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            var time = 1500;
            var url = $"{TestConfig.WebUrl}/Request/TimeOut?time={time}";
            var result = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                Timeout = 1000,
            });

            var b = !result.IsSuccessful && result.StatusCode == HttpStatusCode.InternalServerError;
            Assert.IsTrue(b);
        }

        /// <summary>
        /// 连接超时测试
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            var time = 1000;
            var url = $"{TestConfig.WebUrl}/Request/TimeOut?time={time}";
            var result = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                Timeout = 2000,
            });

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var b2 = result.Content.IsContains("OK");
            Assert.IsTrue(b1 && b2);
        }

        /// <summary>
        /// Get Chunked不限下载时间
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            var id = Guid.NewGuid();
            var url = $"{TestConfig.WebUrl}/Request/GetChunked?id={id}";
            var result = HttpRequestHelper.GetData(url);

            var path = $"{TestConfig.WebDir}Logs/{id}.txt";
            var content = File.ReadAllText(path);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var b2 = result.Content.IsContains(content);
            Assert.IsTrue(b1 && b2);
        }

        /// <summary>
        /// Get Chunked超过最长下载时间，下载中断
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            var sw = new Stopwatch();
            sw.Start();

            var id = Guid.NewGuid();
            var url = $"{TestConfig.WebUrl}/Request/GetChunked?id={id}";
            var result = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                DownloadTimeout = 1500
            });

            sw.Stop();

            var b1 = sw.ElapsedMilliseconds < 2500;
            
            var b2 = !result.IsSuccessful && result.StatusCode == HttpStatusCode.RequestTimeout;
            Assert.IsTrue(b1 && b2);
        }

        /// <summary>
        /// Get Chunked不限下载时间
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            var id = Guid.NewGuid();
            var url = $"{TestConfig.WebUrl}/Request/PostChunked?id={id}";
            var result = HttpRequestHelper.PostPayload(url, "Id=1");
            //
            var path = $"{TestConfig.WebDir}Logs/{id}.txt";
            var content = File.ReadAllText(path);
            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var b2 = content.IsEquals(result.Content);
            Assert.IsTrue(b1 && b2);
        }

        /// <summary>
        /// Get Chunked超过最长下载时间，下载中断
        /// </summary>
        [TestMethod]
        public void TestMethod11()
        {
            var sw = new Stopwatch();
            sw.Start();

            var id = Guid.NewGuid();
            var url = $"{TestConfig.WebUrl}/Request/PostChunked?id={id}";
            var result = HttpRequestHelper.PostPayload(url, "Id=1", new HttpRequestParam()
            {
                DownloadTimeout = 1500
            });

            sw.Stop();

            var b1 = sw.ElapsedMilliseconds < 2500;

            //
            var b2 = !result.IsSuccessful && result.StatusCode == HttpStatusCode.RequestTimeout;

            var b = b1 && b2;
            Assert.IsTrue(b, $"b1:{b1}；b2:{b2}");
        }

        /// <summary>
        /// Timeout必须小于DownloadTimeout
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            //
            var m1 = new HttpRequestParam()
            {
                Timeout = 30000,
                DownloadTimeout = 20000
            };
            var b1 = m1.Timeout == 20000 && m1.Timeout <= m1.DownloadTimeout;

            //
            var m2 = new HttpRequestParam()
            {
                Timeout = 30000,
                DownloadTimeout = 40000
            };
            var b2 = m2.Timeout == 30000 && m2.Timeout <= m2.DownloadTimeout;

            //
            var m3 = new HttpRequestParam()
            {
                Timeout = 30000,
                DownloadTimeout = null
            };
            var b3 = m3.Timeout == 30000;

            //
            var m4 = new HttpRequestParam()
            {
                //Timeout = 30000,
                DownloadTimeout = null
            };
            var b4 = m4.Timeout == 30000;

            //
            var m5 = new HttpRequestParam()
            {
                //Timeout = 30000,
                //DownloadTimeout = null
            };
            var b5 = m5.Timeout == 30000;

            //
            var m6 = new HttpRequestParam()
            {
                //Timeout = 30000,
                DownloadTimeout = 20000
            };
            var b6 = m6.Timeout == 20000 && m6.Timeout <= m6.DownloadTimeout;

            Assert.IsTrue(b1 &&
                b2 &&
                b3 &&
                b4 &&
                b5 &&
                b6
                );
        }
    }
}
