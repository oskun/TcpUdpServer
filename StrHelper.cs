using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class StrHelper
    {
        /// <summary>
        /// 得到16进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetHexStr(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (var ix = 0; ix < bytes.Length; ix++)
            {
                var str = (Convert.ToString(bytes[ix], 16) + "").PadLeft(2, '0');
                sb.Append(str + " ");
            }
            return sb.ToString();
        }


        public static string BackTime()
        {
            DateTime now = DateTime.Now;
            var year = now.Year;
            var month = now.Month;
            var day = now.Day;

            var weekday = (int)now.DayOfWeek;
            if (weekday == 0)
            {
                weekday = 7;
            }
            var hour = now.Hour;
            var minite = now.Minute;
            var seconds = now.Second;
            //a.ToString("x8")

            var yy = Convert.ToInt32((year + "").Substring(0, 2)); ;
            var yy1 = Convert.ToInt32((year + "").Substring(2));
            var str = Convert.ToString(yy, 16) + Convert.ToString(yy1, 16).PadLeft(2, '0') + Convert.ToString(month, 16).PadLeft(2, '0') + Convert.ToString(day, 16).PadLeft(2, '0') + Convert.ToString(weekday, 16).PadLeft(2, '0') + Convert.ToString(hour, 16).PadLeft(2, '0') +
               Convert.ToString(minite, 16).PadLeft(2, '0') + Convert.ToString(seconds, 16).PadLeft(2, '0');
            return str;
        }


        public static byte[] strToHexByte(string hexString)
        {

            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString = hexString + "0";
            }

            //声明字符串变量
            byte[] buffer = new byte[hexString.Length / 2];
            if (Regex.IsMatch(hexString, "[0-9a-f]{" + hexString.Length + "}", RegexOptions.IgnoreCase))
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    //将指定基的数字的字符串表示形式转换为等效的8位无符号整数
                    //即将字符串转换为16进制形式的字节
                    var c = hexString.Substring(i * 2, 2);
                    buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 0x10);

                }
            }



            return buffer;
        }



        public static string GetCheckTimeStr()
        {
            var now = DateTime.Now.ToString("yyMMdd HH:mm:ss");
            var parts = string.Join("", now.Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries)).ToCharArray();
            var sb = new StringBuilder();
            for (var i = 0; i < parts.Length / 2; i++)
            {
                var start = i * 2;
                var end = i * 2 + 1;
                var each = Convert.ToInt32(parts[start].ToString() + parts[end].ToString()).ToString("x2");
                sb.Append(each);
            }
            //return "190802152329";

            return sb.ToString();

        }


        /// <summary>
        /// 2进制数组转16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        /// <summary>
        /// 单个16进制数转为10进制数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToInt(string hex)
        {
            var hexArray = "0123456789abcdef";
            return hexArray.IndexOf(hex);
        }
    }
}
