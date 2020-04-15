﻿using DwrUtility.TaskExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DwrUtility.Https
{
    /// <summary>
    /// Http请求类
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HttpResponseResult GetData(string url, HttpRequestParam param = null)
        {
            if (param == null)
            {
                param = new HttpRequestParam();
            }

            if (!param.DownloadTimeout.HasValue)
            {
                return GetDataNext(url, param, null);
            }

            var cts = new CancellationTokenSource();
            var task = Task.Run(() => GetDataNext(url, param, cts), cts.Token).SetTimeoutResult(param.DownloadTimeout.Value);
            Task.WaitAll(task);

            //没有超时
            if (!task.Result.IsTimeout)
            {
                return task.Result.Value;
            }

            //超时
            cts.Cancel();
            return ReturnTimeout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        private static HttpResponseResult GetDataNext(string url, HttpRequestParam param, CancellationTokenSource cts)
        {
            //HttpWebRequest
            var request = GetRequest(url, param, "GET");
            var result = GetResponse(request);
            request.Abort();
            return result;
        }

        /// <summary>
        /// PostPayload请求
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="payload"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HttpResponseResult PostPayload(string url, string payload, HttpRequestParam param = null)
        {
            return PostData(url, payload, param, "text/plain");
        }

        /// <summary>
        /// PostJson请求
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="jsonString"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HttpResponseResult PostJson(string url, string jsonString, HttpRequestParam param = null)
        {
            return PostData(url, jsonString, param, "application/json");
        }

        /// <summary>
        /// PostFrom请求
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="dict"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HttpResponseResult PostForm(string url, Dictionary<string, string> dict, HttpRequestParam param = null)
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}");
            }
            return PostData(url, sb.ToString(), param, "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="param"></param>
        /// <param name="contentType">类型：1Payload；2Json；3Form</param>
        /// <returns></returns>
        private static HttpResponseResult PostData(string url, string data, HttpRequestParam param, string contentType)
        {
            if (param == null)
            {
                param = new HttpRequestParam();
            }

            if (!param.DownloadTimeout.HasValue)
            {
                return PostDataNext(url, data, param, contentType, null);
            }

            var cts = new CancellationTokenSource();
            var task = Task.Run(() => PostDataNext(url, data, param, contentType, cts), cts.Token).SetTimeoutResult(param.DownloadTimeout.Value);
            Task.WaitAll(task);

            //没有超时
            if (!task.Result.IsTimeout)
            {
                return task.Result.Value;
            }

            //超时
            cts.Cancel();
            return ReturnTimeout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="param"></param>
        /// <param name="contentType"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        private static HttpResponseResult PostDataNext(string url, string data, HttpRequestParam param, string contentType, CancellationTokenSource cts)
        {
            //HttpWebRequest
            var request = GetRequest(url, param, "POST");
            request.ContentType = contentType;
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
            }
            var result = GetResponse(request);
            request.Abort();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="method">GET;POST</param>
        /// <returns></returns>
        private static HttpWebRequest GetRequest(string url, HttpRequestParam param, string method)
        {
            HttpWebRequest request;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.ProtocolVersion = HttpVersion.Version10;
                }
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (request == null)
            {
                throw new Exception("创建HttpWebRequest失败");
            }

            request.CookieContainer = param.CookieContainer;
            request.Timeout = param.Timeout;
            request.ContinueTimeout = -1;
            request.ReadWriteTimeout = -1;
            request.Method = method;
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.UserAgent = param.UserAgent;
            request.Accept = param.Accept;

            return request;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static HttpResponseResult GetResponse(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            var result = new HttpResponseResult();
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                //状态
                result.StatusCode = response.StatusCode;
                result.ContentType = response.Headers.Get("Content-Type");

                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    switch (response.ContentEncoding.ToLower())
                    {
                        case "gzip":
                            {
                                using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                                {
                                    //成功
                                    result.IsSuccessful = true;

                                    //字节
                                    using (var ms = new MemoryStream())
                                    {
                                        var buffer = new byte[16384];
                                        int count;
                                        while ((count = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            ms.Write(buffer, 0, count);
                                        }
                                        result.RawBytes = ms.ToArray();
                                    }
                                }
                                break;
                            }
                        case "deflate":
                            {
                                using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
                                {
                                    //成功
                                    result.IsSuccessful = true;

                                    //字节
                                    using (var ms = new MemoryStream())
                                    {
                                        var buffer = new byte[16384];
                                        int count;
                                        while ((count = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            ms.Write(buffer, 0, count);
                                        }
                                        result.RawBytes = ms.ToArray();
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                //成功
                                result.IsSuccessful = true;

                                //字节
                                using (var ms = new MemoryStream())
                                {
                                    var buffer = new byte[16384];
                                    int count;
                                    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        ms.Write(buffer, 0, count);
                                    }
                                    result.RawBytes = ms.ToArray();
                                }
                                break;
                            }
                    }
                }
                else
                {
                    result.IsSuccessful = false;
                    result.RawBytes = null;
                    result.Exception = new Exception("输出Stream为null");
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.RawBytes = null;
                result.Exception = ex;
            }
            response?.Close();
            response?.Dispose();
            return result;
        }

        /// <summary>
        /// 请求超时
        /// </summary>
        /// <returns></returns>
        private static HttpResponseResult ReturnTimeout()
        {
            return new HttpResponseResult()
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.RequestTimeout,
                RawBytes = null,
                Exception = new Exception("请求超时")
            };
        }
    }
}
