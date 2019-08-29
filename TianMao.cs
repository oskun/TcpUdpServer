using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class TianMao
    {
        /// <summary>
        /// 解析压缩码
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetOrgMsg(int a, int b, string value)
        {
            var str = "55ff01" + (a + "").PadLeft(2, '0') + (b + "").PadLeft(2, '0');
            if (value.IndexOf("_") != -1)
            {
                var values = value.Split(new string[] { "_", "." }, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                foreach (var v in values)
                {
                    if (v.Length < 2)
                    {
                        sb.Append(v.PadLeft(2, '0'));
                    }
                    else if (v.Length < 4)
                    {
                        sb.Append(v);
                    }
                    else if (v.Length >= 4)
                    {
                        if (v.StartsWith("ffff", StringComparison.CurrentCultureIgnoreCase) || v.StartsWith("0000", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //需要补充的字符
                            var toPaddingChar = v[3];

                            var toPaddingCount = v.Substring(4).ToLower();

                            var intPaddingCount = Convert.ToInt32(JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", toPaddingCount, 0)) * 2;
                            var data = /*v.Substring(0, 4) +*/ (toPaddingChar + "").PadRight(intPaddingCount, toPaddingChar);
                            sb.Append(data);
                        }
                        else
                        {
                            sb.Append(v);
                        }

                    }
                }
                var lenx = sb.ToString().Length / 2;
                var len = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", lenx + "").PadLeft(2, '0');
                var fstr = GetCRC(str + len + sb.ToString());
                return fstr;
            }
            else
            {
                var lenx = value.Length / 2;
                var len = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", lenx + "").PadLeft(2, '0');
                var fstr = GetCRC(str + len + value.ToString());
                return fstr;
            }


        }



        public static string GetOrgMsg_New(int a, int b, string value)
        {
            var str = "55ff01" + (a + "").PadLeft(2, '0') + (b + "").PadLeft(2, '0');
            if (value.IndexOf("_") != -1)
            {
                var values = value.Split(new string[] { "_", "." }, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                foreach (var v in values)
                {
                    if (v.StartsWith("0000") || v.StartsWith("ffff", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var toPaddingChar = v[0];
                        var toPadding = v.Substring(4).ToLower();
                        var intPaddingCount = Convert.ToInt32(JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", toPadding, 0)) * 2;
                        var data = /*v.Substring(0, 4) +*/ (v).PadRight(intPaddingCount, toPaddingChar);
                        sb.Append(data);
                    }
                    else
                    {
                        sb.Append(v);
                    }



                }

                var lenx = sb.ToString().Length / 2;
                var len = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", lenx + "").PadLeft(2, '0');
                var fstr = GetCRC(str + len + sb.ToString());
                return fstr;
            }
            else
            {
                var lenx = value.Length / 2;
                var len = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", lenx + "").PadLeft(2, '0');
                var fstr = GetCRC(str + len + value);
                return fstr;
            }

        }


        public static string GetCRC(string str)
        {

            // str = "55ff010100e6004921003422003c002100260055002500a202240000002000ffffffffffffffffffffffff01221212122301224f0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010f067600";
            var sum = 0;
            if (str.Length % 2 != 0)
            {
                return string.Empty;
            }
            else
            {
                var list_two = new List<string>();
                for (var i = 0; i < str.Length / 2; i++)
                {
                    var j = i * 2;
                    var c1 = str[j];
                    var c2 = str[j + 1];
                    var sr = c1 + "" + c2;
                    list_two.Add(sr);
                }
                foreach (var d in list_two)
                {
                    sum += Convert.ToInt32(JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", d.ToLower()));
                }
            }

            var crc = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", sum + "");
            if (crc.Length > 2)
            {
                crc = str + crc.Substring(crc.Length - 2);
                return crc;
            }
            return string.Empty;
        }
    }
}
