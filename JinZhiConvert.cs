using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class JinZhiConvert
    {
        public static int JinZhiResult(string orgjinzhi, string totrans)
        {
            ///原来是多少进制数
            var orgj = orgjinzhi.Length;
            ///目标是多少进制数
            var reverse = totrans.Reverse().ToArray();
            var orgvalue = 0;
            for (var i = 0; i < reverse.Length; i++)
            {
                orgvalue += Convert.ToInt32(orgjinzhi.IndexOf(reverse[i]) * (Math.Pow(orgj, i)));
            }
            return orgvalue;
        }

        /// <summary>
        /// 转换进制是否忽略大小写
        /// </summary>
        /// <param name="orgjinzhi"></param>
        /// <param name="totrans"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static int JinZhiResult(string orgjinzhi, string totrans, bool ignoreCase)
        {
            if (ignoreCase)
            {
                orgjinzhi = orgjinzhi.ToLower();
                totrans = totrans.ToLower();
            }
            ///原来是多少进制数
            var orgj = orgjinzhi.Length;
            ///目标是多少进制数
            var reverse = totrans.Reverse().ToArray();
            var orgvalue = 0;
            for (var i = 0; i < reverse.Length; i++)
            {
                orgvalue += Convert.ToInt32(orgjinzhi.IndexOf(reverse[i]) * (Math.Pow(orgj, i)));
            }
            return orgvalue;
        }

        /// <summary>
        /// 将原来进制数 转为目标精制数
        /// </summary>
        /// <param name="orgjinzhi">原来进制数包含哪些字符（字符从小到大）</param>
        /// <param name="descjinzhi">目标进制数（字符从小到大）</param>
        /// <param name="totrans">要转化的进制数</param>
        /// <param name="pianyi">偏移量</param>
        /// <returns></returns>
        public static string JinZhiResult(string orgjinzhi, string descjinzhi, string totrans, int pianyi = 0)
        {
            ///原来是多少进制数
            var orgj = orgjinzhi.Length;
            ///目标是多少进制数
            var descj = descjinzhi.Length;
            var reverse = totrans.Reverse().ToArray();
            var orgvalue = pianyi * 1.0;
            var result = new StringBuilder();
            for (var i = 0; i < reverse.Length; i++)
            {
                orgvalue += Convert.ToInt64(orgjinzhi.IndexOf(reverse[i]) * (Math.Pow(orgj, i)));
            }
            if (orgvalue == 0)
            {
                return "0";
            }

            while (orgvalue / descj > 0)
            {
                var shang = Convert.ToInt64(Math.Floor(orgvalue * 1.0 / descj));
                var yu = orgvalue % descj;
                var value = descjinzhi[(int)yu];
                result.Append(value);
                orgvalue = shang * 1.0;
            }
            var data = result.ToString().ToArray().Reverse();
            result.Clear();
            foreach (var d in data)
            {
                result.Append(d);
            }
            return result.ToString();
        }
    }
}
