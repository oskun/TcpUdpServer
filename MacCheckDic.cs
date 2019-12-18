using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpUdpServer
{
   public class MacCheckDic
    {
        private static Dictionary<string, Func<string, string>> dic = new Dictionary<string, Func<string, string>>();


        static MacCheckDic()
        {
            dic.Add("98d8634a0344", CRC16.CRC16Data);
            dic.Add("98d8634a034e", CRC16.CRC16Data);
            dic.Add("98d86330a323", CRC16.CRC16Data);
            dic.Add("20180ee1d857", CRC16.CRC16Data);
        }



        /// <summary>
        /// 校验是否是正确的码
        /// </summary>
        /// <param name="mac">mac</param>
        /// <param name="result">待校验的字符串</param>
        /// <returns></returns>
        public static bool isRightCode(string mac, string result)
        {

            if (dic.ContainsKey(mac))
            {
                var func = dic[mac];
                var computer_result = func(result);
                if (computer_result.Equals(result, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false; 
            }
            return true;




        }
    }
}
