using System;
using System.IO;
using System.Net;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using UnityEngine;

namespace ServerLib.Utils.HTTP
{
    public static class HttpUtils
    {
        private static string _host;

        public class Create<T>
        {
            private readonly string _phpSession;

            public Create(string phpSession = null)
            {
                _phpSession = phpSession.Replace("pmc", "");
                _host = "https://127.0.0.1";
            }

            public T Get(string url)
            {
                var webRequest = GetWebRequest(_host + url);
                webRequest.Method = "GET";

                return ParseResponse(webRequest);
            }

            public T Post(string url, string data, bool compression)
            {
                var webRequest = GetWebRequest(_host + url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";

                if (data.Length <= 0) return ParseResponse(webRequest);

                var sendData = compression ? SimpleZlib.CompressToBytes(data, 9) : Encoding.UTF8.GetBytes(data);
                webRequest.ContentLength = sendData.Length;
                var dataStream = webRequest.GetRequestStream();
                dataStream.Write(sendData, 0, sendData.Length);
                dataStream.Close();

                return ParseResponse(webRequest);
            }

            private HttpWebRequest GetWebRequest(string url)
            {
                var webRequest = (HttpWebRequest) WebRequest.Create(url);
                webRequest.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                webRequest.Timeout = 10000;

                if (_phpSession == null) return webRequest;
                if (webRequest.CookieContainer == null)
                    webRequest.CookieContainer = new CookieContainer();

                var domain = _host.Replace("https://", "").Replace("/", "");
                webRequest.CookieContainer.Add(new Cookie("PHPSESSID", _phpSession) {Domain = domain});

                return webRequest;
            }

            private static T ParseResponse(WebRequest webRequest)
            {
                T result = default;
                var response = (HttpWebResponse) webRequest.GetResponse();

                try
                {
                    var buffer = response.GetResponseStream()?.ReadFully();
                    var res = SimpleZlib.Decompress(buffer).ParseJsonTo<ServerResponse<T>>();

                    if (res != null)
                        result = res.Data;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    response.Close();
                }

                return result;
            }
        }

        private static byte[] ReadFully(this Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, bytesRead);
            }

            return ms.ToArray();
        }

        public class ServerResponse<T>
        {
            public int err;
            public string Errmsg;
            public T Data;
            public uint? crc;
        }
    }
}
