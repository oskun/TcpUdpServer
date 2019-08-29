using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class TimeHelper
    {
        /// <summary>
        /// 格林威治时间
        ///是一种时间表示方法，定义为从格林威治时间1970年01月01日00时00分00秒起至现在的总秒数。
        /// </summary>
        /// <returns></returns>
        public static long JavaTime()
        {
            var time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return time;
        }

        /// <summary>
        /// 精确到秒
        /// </summary>
        /// <param name="timex"></param>
        /// <returns></returns>
        public static long JavaTime(DateTime timex)
        {
            var time = (timex.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return time;
        }

        public static string ConvertDateTimeInt()
        {
            var time = DateTime.Now.ToUniversalTime();
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalMilliseconds + 28800000;
            var str = intResult.ToString();
            var indexpoint = str.IndexOf(".");
            var index = string.Empty;
            if (indexpoint > 0)
            {
                index = str.Substring(0, str.IndexOf("."));
                return index;
            }
            else
            {
                time = DateTime.Now.ToUniversalTime();
                startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                intResult = (time - startTime).TotalMilliseconds + 28800000;
                str = intResult.ToString();
                indexpoint = str.IndexOf(".");
                index = string.Empty;
                if (indexpoint > 0)
                {
                    index = str.Substring(0, str.IndexOf("."));
                    return index;
                }
                else
                {
                    return str;
                }
            }
        }

        /// <summary>
        /// 格林威治时间转到年月日的时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime RJavaTime(string timeStamp)
        {
            ///精确到毫秒
            if (timeStamp.Length == 13)
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            ///精确到
            else
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
        }
    }
}
