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

        [TestMethod]
        public void TestMethod13()
        {
            var url = $"{TestConfig.WebUrl}/Product/PostFile";
            var name = "测试&名称=哈#哈";
            var dict = new Dictionary<string, string>()
            {
                {"Name", name },
                {"Content", "Content" },
            };

            var path = Path.GetFullPath($"{DwrUtilitySetting.Root}/../../../DataFiles/");
            var files = new List<HttpRequestFileModel>()
            {
                new HttpRequestFileModel($"{path}test_1.json"),
                new HttpRequestFileModel($"{path}test_1_result.json"),
            };

            var result = HttpRequestHelper.PostFormFile(url, files, dict);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            var content = result.Content;

            Assert.IsTrue(content.IsContains("文件数：2；内容：测试&名称=哈#哈；Content；"));
        }

        /// <summary>
        /// 请求被中止: 未能创建 SSL/TLS 安全通道。
        /// Could not create SSL/TLS secure channel
        /// </summary>
        [TestMethod]
        public void TestMethod14()
        {
            //Assert.Inconclusive("请求被中止: 未能创建 SSL/TLS 安全通道。");

            //Tls1.3
            var url = "https://www.ddun.com/";
            var result = HttpRequestHelper.GetData(url);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            Assert.IsTrue(b1, result.Exception?.Message);
        }

        /// <summary>
        /// 文档：https://support.microsoft.com/en-us/help/3140245/update-to-enable-tls-1-1-and-tls-1-2-as-default-secure-protocols-in-wi
        /// 安装更新：http://www.catalog.update.microsoft.com/search.aspx?q=kb3140245 （win7：https://www.microsoft.com/en-us/download/details.aspx?id=53335）
        /// 更新补丁：KB3140245
        /// 
        /// 请求被中止: 未能创建 SSL/TLS 安全通道。
        /// Could not create SSL/TLS secure channel
        /// 
        ///  TLS1.2 只有在 Win8.1 和 Win 10 上才默认支持
        /// 
        /// PowerShell:
        /// [Net.ServicePointManager]::SecurityProtocol
        /// [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Ssl3 -bor [Net.SecurityProtocolType]::Tls -bor [Net.SecurityProtocolType]::Tls11 -bor [Net.SecurityProtocolType]::Tls12
        /// </summary>
        [TestMethod]
        public void TestMethod14A()
        {
            //Assert.Inconclusive("请求被中止: 未能创建 SSL/TLS 安全通道。");

            //Tls1.2
            var url = "https://ieltsliz.com/";
            var result = HttpRequestHelper.GetData(url);

            var b1 = result.IsSuccessful && result.StatusCode == HttpStatusCode.OK;
            Assert.IsTrue(b1, result.Exception?.Message);
        }

        [TestMethod]
        public void TestMethod15()
        {
            var url = "https://www.baidu.com/link?url=Sy2569H4e4mGS5q7LxmwiFWJdxRJJBhGekKMEwo-Tx_&wd=&eqid=d255d347000484ac000000035f803f0f";
            var result = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                AllowAutoRedirect = false,
            });

            Assert.IsTrue(result.Content.IsContains("0;URL='https://www.baidu.com/'"));

            //var val = result.Headers["Location"];
            //Assert.IsTrue(val == "https://www.xftsoft.com/news/zixun/sftsoft6.0.html");
        }

        [TestMethod]
        public void TestMethod16()
        {
            var url = $"{TestConfig.WebUrl}/Cookie/RequestCookie";

            var res = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                CookieValue = "Hm_lvt_49541358a94eea717001819b500f76c8=1597235189,1597314394,1597320946,1597393762; Hm_lvt_cdf7318ce2f5dc58e89bb436a5989912=1597323174,1597323207,1597330588,1597393762; Hm_lvt_d165b0df9d8b576128f53e461359a530=1597323142,1597323175,1597323207,1597393762; active_2020814=1; Hm_lpvt_cdf7318ce2f5dc58e89bb436a5989912=1597393768; Hm_lpvt_49541358a94eea717001819b500f76c8=1597393768; Hm_lpvt_d165b0df9d8b576128f53e461359a530=1597393768",
            });

            Assert.IsTrue(res.Content == "1597323142,1597323175,1597323207,1597393762");
        }

        [TestMethod]
        public void TestMethod17()
        {
            var kvs = new List<KeyValue<long, long>>();
            var url = $"{TestConfig.WebUrl}/soft/XFTV12.0.zip";
            var http = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                Progress = (n, t) =>
                {
                    kvs.Add(new KeyValue<long, long>()
                    {
                        Key = n,
                        Value = t,
                    });
                }
            });

            var r1 = kvs.Select(p => p.Key).ToList();
            var total = r1.Sum(p => p);
            var r2 = kvs.Select(p => p.Value).ToList();

            Assert.IsTrue(r1.Count == 12 && total == 1340886);
            Assert.IsTrue(r2.Distinct().Count() == 1 && r2.First() == 1340886);
        }

        [TestMethod]
        public void TestMethod17A()
        {
            var kvs = new List<KeyValue<long, long>>();
            var url = $"{TestConfig.WebUrl}/Request/GetChunked/{Guid.NewGuid()}";
            var http = HttpRequestHelper.GetData(url, new HttpRequestParam()
            {
                Progress = (n, t) =>
                {
                    kvs.Add(new KeyValue<long, long>()
                    {
                        Key = n,
                        Value = t,
                    });
                }
            });

            var r1 = kvs.Select(p => p.Key).ToList();
            var total = r1.Sum(p => p);
            var r2 = kvs.Select(p => p.Value).ToList();

            Assert.IsTrue(r1.Count == 2 && total == 144028);
            Assert.IsTrue(r2.Distinct().Count() == 1 && r2.First() == -1);
        }
    }
}
