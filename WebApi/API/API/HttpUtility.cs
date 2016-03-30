using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;

namespace WebApi
{   
    /// <summary>
    /// Http请求
    /// </summary>
    public class HttpUtility
    {
        /// <summary>
        ///通用HttpWebRequest
        /// </summary>
        /// <param name="method">请求方式（POST/GET）</param>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="onComplete">完成后执行的操作(可选参数，通过此方法可以获取到HTTP状态码)</param>
        /// <returns>请求返回的结果</returns>
        public static string Request(string method, string url, string path, string param, Action<HttpStatusCode,string> onComplete = null)
        {
            if(string.IsNullOrEmpty(url))
             throw new ArgumentNullException("URL");
    
            switch (method.ToUpper())
            { 
                case "POST":
                    return Post(url, path, param, onComplete);
                case "GET":
                    return Get(url, path, param, onComplete);
                default:
                    throw new EntryPointNotFoundException("method:" + method);
            }
        }
    
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数</param>
        /// <param name="onComplete">完成后执行的操作(可选参数，通过此方法可以获取到HTTP状态码)</param>
        /// <returns>请求返回的结果</returns>
        public static string Post(string url, string path, string param, Action<HttpStatusCode, string> onComplete = null)
        {
            UrlCheck(ref url);
    
            byte[] bufferBytes = Encoding.UTF8.GetBytes(param);
    
            var request = WebRequest.Create(url) as HttpWebRequest;//HttpWebRequest方法继承自WebRequest, Create方法在WebRequest中定义，因此此处要显示的转换
            request.Method = "POST";
            request.ContentLength = bufferBytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            string fileName = url.Substring(url.LastIndexOf("/") + 1, url.Length - (url.LastIndexOf("/") + 1));    
            try
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bufferBytes, 0, bufferBytes.Length);
                }
            }
            catch (WebException ex)
            {
    
                return ex.Message;
            }

            return HttpRequest(request, path, fileName, onComplete);
        }
    
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数</param>
        /// <param name="onComplete">完成后执行的操作(可选参数，通过此方法可以获取到HTTP状态码)</param>
        /// <returns>请求返回结果</returns>
        public static string Get(string url, string path, string param, Action<HttpStatusCode, string> onComplete = null)
        {
            UrlCheck(ref url);    
            if (!string.IsNullOrEmpty(param))
                if (!param.StartsWith("?"))
                    param += "?" + param;
                else
                    param += param;
    
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            string fileName = url.Substring(url.LastIndexOf("/") + 1, url.Length - (url.LastIndexOf("/") + 1));
            return HttpRequest(request, path, fileName, onComplete);
        }
    
        /// <summary>
        /// 请求的主体部分（由此完成请求并返回请求结果）
        /// </summary>
        /// <param name="request">请求的对象</param>
        /// <param name="onComplete">完成后执行的操作(可选参数，通过此方法可以获取到HTTP状态码)</param>
        /// <returns>请求返回结果</returns>
        private static string HttpRequest(HttpWebRequest request, string path, string fileName, Action<HttpStatusCode, string> onComplete = null)
        {
            string result = string.Empty;
            HttpWebResponse response = null;    
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
                result = ex.Message;
            }    
            if (response == null)
            {
                if (onComplete != null)
                    onComplete(HttpStatusCode.NotFound, "请求远程返回为空");
                return null;
            }
            try {
                using (Stream stream = response.GetResponseStream())
                {
                    string sPath = System.Web.HttpContext.Current.Request.MapPath(path);
                    byte[] bytes = StreamToBytes(stream);
                    using (FileStream fs = File.Create(sPath + "\\" + fileName, bytes.Length, FileOptions.None))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                        result = fileName;
                    }
                }  
            }
            catch (Exception ex) { result = ex.Message; }  
            if (onComplete != null)
                onComplete(response.StatusCode, result);    
            return result;    
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }
    
        /// <summary>
        /// URL拼写完整性检查
        /// </summary>
        /// <param name="url">待检查的URL</param>
        private static void UrlCheck(ref string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url="http://"+url;
        }
    }
}