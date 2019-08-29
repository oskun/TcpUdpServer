using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TcpUdpServer
{
    public class JsonHelper<T> where T : class
    {
        public static T GetObject(string jsonstr)
        {
            try
            {

                if (!string.IsNullOrEmpty(jsonstr))
                {
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(jsonstr);
                        return t;
                    }
                    catch
                    {
                        Console.WriteLine("json:" + jsonstr);
                        return default(T);
                    }
                }
                return null;
            }
            catch
            {
                Console.WriteLine(jsonstr);
                return null;
            }
        }

        /// <summary>
        /// 得到json  字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetJson(T t)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            return str;
        }
    }
}
