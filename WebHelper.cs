using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class WebHelper
    {

        public static string PostData(string Url, Dictionary<string, string> param, System.Text.Encoding encode)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var key in param.Keys)
            {
                var value = string.Empty;
                var has = param.TryGetValue(key, out value);
                if (has)
                {
                    if (i == 0)
                    {
                        sb.Append(string.Format("{0}={1}", key, value));
                        i = 1;
                    }
                    else
                    {
                        sb.Append(string.Format("&{0}={1}", key, value));
                    }
                }
            }
            byte[] bs = encode.GetBytes(sb.ToString());
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.Method = "POST";
            req.KeepAlive = false;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            /*
            protected final String USER_AGENT = "Mozilla/5.0";
            */
            req.UserAgent = "Mozilla/5.0";
            using (Stream reqStream = req.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);
            }
            using (WebResponse wr = (HttpWebResponse)req.GetResponse())
            {
                var reader = new StreamReader(wr.GetResponseStream(), encode);
                string content = reader.ReadToEnd();
                return content;
            }
        }



        public static string PostDataHeader(string Url, Dictionary<string, string> param, string appcode)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var key in param.Keys)
            {
                var value = string.Empty;
                var has = param.TryGetValue(key, out value);
                if (has)
                {
                    if (i == 0)
                    {
                        sb.Append(string.Format("{0}={1}", key, value));
                        i = 1;
                    }
                    else
                    {
                        sb.Append(string.Format("&{0}={1}", key, value));
                    }
                }
            }
            byte[] bs = Encoding.UTF8.GetBytes(sb.ToString());
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.Method = "get";
            req.KeepAlive = false;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            req.Headers.Add("Authorization", "APPCODE " + appcode);
            /*
            protected final String USER_AGENT = "Mozilla/5.0";
            */
            req.UserAgent = "Mozilla/5.0";
            using (Stream reqStream = req.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);
            }
            using (WebResponse wr = (HttpWebResponse)req.GetResponse())
            {
                var reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                return content;
            }
        }


        public static string DoPost(string posturl, string param)
        {
            // string param = "hl=zh-CN&http://219.133.newwindow=1";
            byte[] bs = Encoding.UTF8.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(posturl);
            req.Method = "POST";
            req.KeepAlive = false;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            try
            {
                using (WebResponse wr = (HttpWebResponse)req.GetResponse())
                {
                    var reader = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    return content;
                }
            }
            catch
            {
                return string.Empty;
            }


        }


        public static string GetData(string Url, Dictionary<string, string> param)
        {
            try
            {
                var i = 0;
                var sb = new StringBuilder();
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        var value = string.Empty;
                        var has = param.TryGetValue(key, out value);
                        if (has)
                        {
                            if (i == 0)
                            {
                                sb.Append(string.Format("?{0}={1}", key, value));
                                i = 1;
                            }
                            else
                            {
                                sb.Append(string.Format("&{0}={1}", key, value));
                            }
                        }
                    }
                }


                Url = Url + sb.ToString();
                var request = (System.Net.HttpWebRequest)WebRequest.Create(Url);


                var response = (System.Net.HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = myreader.ReadToEnd();
                    return responseText;
                }
            }
            catch
            {
                return string.Empty;
            }

        }



        public static string GetData(string Url, Dictionary<string, string> param, string appcode)
        {
            try
            {
                var i = 0;
                var sb = new StringBuilder();
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        var value = string.Empty;
                        var has = param.TryGetValue(key, out value);
                        if (has)
                        {
                            if (i == 0)
                            {
                                sb.Append(string.Format("?{0}={1}", key, value));
                                i = 1;
                            }
                            else
                            {
                                sb.Append(string.Format("&{0}={1}", key, value));
                            }
                        }
                    }
                }


                Url = Url + sb.ToString();
                var request = (System.Net.HttpWebRequest)WebRequest.Create(Url);
                request.Headers.Add("Authorization", "APPCODE " + appcode);

                var response = (System.Net.HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = myreader.ReadToEnd();
                    return responseText;
                }
            }
            catch
            {
                return string.Empty;
            }

        }


        public static string GetData(string Url, Dictionary<string, string> param, System.Text.Encoding encode)
        {
            try
            {
                var i = 0;
                var sb = new StringBuilder();
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        var value = string.Empty;
                        var has = param.TryGetValue(key, out value);
                        if (has)
                        {
                            if (i == 0)
                            {
                                sb.Append(string.Format("?{0}={1}", key, value));
                                i = 1;
                            }
                            else
                            {
                                sb.Append(string.Format("&{0}={1}", key, value));
                            }
                        }
                    }

                }


                Url = Url + sb.ToString();
                var request = (System.Net.HttpWebRequest)WebRequest.Create(Url);

                var response = (System.Net.HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), encode))
                {
                    string responseText = myreader.ReadToEnd();
                    return responseText;
                }
            }
            catch
            {
                return string.Empty;
            }

        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="param"></param>
        /// <param name="headervalue"></param>
        /// <returns></returns>

        public static string GetData(string Url, Dictionary<string, string> param, Dictionary<string, string> headervalue)
        {
            /*
              string strURL = url + '?' + param;
    System.Net.HttpWebRequest request;
    request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
    request.Method = "GET";
    // 添加header
    request.Headers.Add("apikey", "您自己的apikey");
    System.Net.HttpWebResponse response;
    response = (System.Net.HttpWebResponse)request.GetResponse();
    System.IO.Stream s;
    s = response.GetResponseStream();
    string StrDate = "";
    string strValue = "";
    StreamReader Reader = new StreamReader(s, Encoding.UTF8);
    while ((StrDate = Reader.ReadLine()) != null)
    {
        strValue += StrDate + "\r\n";
    }
    return strValue;
            */

            try
            {
                var i = 0;
                var sb = new StringBuilder();
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        var value = string.Empty;
                        var has = param.TryGetValue(key, out value);
                        if (has)
                        {
                            if (i == 0)
                            {
                                sb.Append(string.Format("?{0}={1}", key, value));
                                i = 1;
                            }
                            else
                            {
                                sb.Append(string.Format("&{0}={1}", key, value));
                            }
                        }
                    }

                }


                Url = Url + sb.ToString();
                var request = (System.Net.HttpWebRequest)WebRequest.Create(Url);
                foreach (var key in headervalue.Keys)
                {
                    var value = headervalue[key];
                    request.Headers.Add(key, value);
                }
                request.Method = "GET";


                var response = (System.Net.HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    string responseText = myreader.ReadToEnd();
                    return responseText;
                }
            }
            catch
            {
                return string.Empty;
            }


        }







    }
}
