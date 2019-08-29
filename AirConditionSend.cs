using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class AirConditionSend
    {

        const int minValue = 16;
        const int maxValue = 30;
        /*
         * 2.3 空调红外码  

空调的发码规则不同于其它红外设备的发码,说明如下:共七个键, 搜索对码时, 发的是开机键的数据
所发数据内容：0x30+0x01+(2B)+(7B)+(1B)+(?B：arc_table)+0xFF+CHECKSUM(1B)         B:表示字节, 
含义:
1.	0x30 0x01 为固定值
2.	(2B)是该遥控器对的红外码是码库中的第几组码,如:第120组 则为 0x00 0x78
3.	(7B)共七个字节
第0个字节: 数据为对应空调的温度：19－30度(0x13-0x1E),默认：25度;十六进制,与显示对应,通过温度加减键调节
第1个字节: 风量数据：自动：01,低：02,中：03,高：04,与显示对应：默认：01,
第2个字节: 手动风向：向下：03,中：02,向上：01,默认02,与显示对应
第3个字节: 自动风向：01,打开,00关,默认开:01,与显示对应
第4个字节: 开关数据：开机时：0x01,关机时：0x00,通过按开关机（电源）键实现,开机后,其它键才有效,相关符号才显示)
第5个字节: 点击的按键对应数据,电源：0x01,模式：0x02,风量：0x03,手动风向：		
                  0x04, 自动风向：0x05,温度加：0x06,  温度减：0x07, /* 表示当前按下的是哪个键
第6个字节: 模式对应数据和显示：自动（默认）：0x01,制冷：0X02,抽湿：0X03,送风：0x04;制热：0x05,这些值按模式键实现
4.	(1B)对应码库中每组数据的第一个字节值+1, 即括号内的第一个字节加1:如第0组时为0x04+1=0x05,即0x05,第12组时为0x06+1=0x07,即0x07 
5.	(?B：arc_table) 对应码库中每组数据除第一个字节值之外的所有数据
6.	0xFF为固定值
7.	CHECKSUM(1B) 校验和,最后一个字节为前面所有数据之和的低8位,（第0x30到0xFF数据之和取低8位）
示例: 0x30 0x01 0x00 0x00 0x1B 0x01 0x02 0x01 0x01 0x01 0x02 0x05 0x00 0x0f 0x04 0x3D 0xFF 0xA9
         * 
         * 
         */

        public static string AirCode(string code, CommandMode mode, string value)
        {
            /*
             * 30 01 0215 19020100010102 030000 FF
             */

            var spdata = SplitData(code);
            if (spdata.Count == 6)
            {
                ///7b
                var _7b = spdata[2];
                //温度
                var wendu = _7b.Substring(0, 2);
                //风量
                var fengliang = _7b.Substring(2, 2);//4
                ///手工风向
                var manuFengXiang = _7b.Substring(4, 2);//6
                //自动风向
                var autoFengXiang = _7b.Substring(6, 2);//8
                //开关数据
                var data_kaiguan = _7b.Substring(8, 2);//10
                //点击的按键对应数据
                var key_rel = _7b.Substring(10, 2);//12
                //模式对应数据和显示
                var mode_data = _7b.Substring(12, 2);//模式对应
                ///调低温度
                if (mode == CommandMode.AdjustDownTemperature)
                {
                    //0x13-0x1E
                    //19－30
                    //Convert.ToString()
                    var ivalue = 0;
                    wendu = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", wendu);
                    var bint = int.TryParse(wendu, out ivalue);
                    if (bint)
                    {
                        if (ivalue >= minValue && ivalue <= maxValue)
                        {

                            int intValue = 0;
                            var beiint = int.TryParse(value, out intValue);
                            if (beiint)
                            {


                                var finalvalue = minValue;
                                if (ivalue - intValue > minValue)
                                {
                                    finalvalue = ivalue - intValue;
                                }
                                var tmp = (finalvalue).ToString("x2");
                                wendu = tmp;
                                /*
                                 * 点击的按键对应数据,电源：0x01,模式：0x02,风量：0x03,手动风向：		
                      0x04, 自动风向：0x05,温度加：0x06,  温度减：0x07,
                                 */

                                key_rel = "07";
                            }
                            else
                            {
                                var tmp = (minValue).ToString("x2");
                                wendu = tmp;
                                /*
                                 * 点击的按键对应数据,电源：0x01,模式：0x02,风量：0x03,手动风向：		
                      0x04, 自动风向：0x05,温度加：0x06,  温度减：0x07,
                                 */

                                key_rel = "07";
                            }
                        }
                    }


                }
                ///调高温度
                else if (mode == CommandMode.AdjustUpTemperature)
                {
                    var ivalue = 0;
                    wendu = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", wendu);
                    var bint = int.TryParse(wendu, out ivalue);
                    if (bint)
                    {
                        if (ivalue >= minValue && ivalue <= maxValue)
                        {

                            int intValue = 0;
                            var beiint = int.TryParse(value, out intValue);
                            if (beiint)
                            {
                                var finalvalue = maxValue;
                                if (ivalue + intValue < maxValue)
                                {
                                    finalvalue = ivalue + intValue;
                                }
                                var tmp = (finalvalue).ToString("x2");
                                wendu = tmp;
                                key_rel = "06";
                            }


                        }
                        else
                        {
                            var tmp = (maxValue).ToString("x2");
                            wendu = tmp;
                            key_rel = "06";
                        }
                    }
                }
                ///设置模式
                else if (mode == CommandMode.SetMode)
                {
                    //模式对应数据和显示：自动（默认）：0x01,制冷：0X02,抽湿：0X03,送风：0x04; 制热：0x05,这些值按模式键实现

                    ///制热
                    if (value.Equals("heat"))
                    {
                        mode_data = "05";
                    }
                    ///制冷
                    else if (value.Equals("cold") || value.Equals("cool"))
                    {
                        mode_data = "02";
                    }
                    ///送风
                    else if (value.Equals("airsupply") || value.Equals("fan"))
                    {
                        mode_data = "04";
                    }
                    ///抽湿
                    else if (value.Equals("dehumidification"))
                    {
                        mode_data = "03";
                    }
                    else if (value.Equals("auto"))
                    {
                        mode_data = "01";
                    }
                    key_rel = "02";

                }
                ///设置温度
                else if (mode == CommandMode.SetTemperature)
                {
                    var ivalue = 0;
                    wendu = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", wendu);
                    var bint = int.TryParse(wendu, out ivalue);
                    if (bint)
                    {

                        int intValue = 0;
                        var beiint = int.TryParse(value, out intValue);
                        if (beiint)
                        {

                            if (intValue >= minValue && intValue <= maxValue)
                            {


                                var tmp = intValue.ToString("x2");
                                if (intValue >= ivalue)
                                {
                                    key_rel = "06";
                                }
                                else
                                {
                                    key_rel = "07";
                                }
                                wendu = tmp;
                                /*
                                 * 点击的按键对应数据,电源：0x01,模式：0x02,风量：0x03,手动风向：		
                      0x04, 自动风向：0x05,温度加：0x06,  温度减：0x07,
                                 */


                            }

                        }



                    }





                }
                ///设置风速
                else if (mode == CommandMode.SetWindSpeed)
                {
                    /*
                     * 风量数据：自动：01,低：02,中：03,高：04,与显示对应：默认：01,
                     */
                    int ivalue = 1;
                    /*
                     * auto 自动风 
low 低风 
medium 中风 
high 高风
                     */
                    if (value.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ivalue = 1;
                        fengliang = (ivalue + "").PadLeft(2, '0');
                        key_rel = "03";
                    }
                    else if (value.Equals("low", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ivalue = 2;
                        fengliang = (ivalue + "").PadLeft(2, '0');
                        key_rel = "03";
                    }
                    else if (value.Equals("medium", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ivalue = 3;
                        fengliang = (ivalue + "").PadLeft(2, '0');
                        key_rel = "03";
                    }
                    else if (value.Equals("high", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ivalue = 4;
                        fengliang = (ivalue + "").PadLeft(2, '0');
                        key_rel = "03";
                    }
                    else
                    {
                        var bint = int.TryParse(value, out ivalue);
                        if (bint)
                        {
                            if (ivalue >= 1 && ivalue <= 4)
                            {
                                fengliang = (ivalue + "").PadLeft(2, '0');
                                key_rel = "03";
                            }
                        }
                    }
                }
                else if (mode == CommandMode.TurnOn)
                {
                    data_kaiguan = "01";
                    key_rel = "01";
                }
                else if (mode == CommandMode.TurnOff)
                {
                    data_kaiguan = "00";
                    key_rel = "01";
                }

                if (mode != CommandMode.TurnOff)
                {
                    data_kaiguan = "01";
                    key_rel = "01";
                }

                var pdata = wendu + fengliang + manuFengXiang + autoFengXiang + data_kaiguan + key_rel + mode_data;
                spdata[2] = pdata;
                var data = string.Join("", spdata);

                //var all = data + GetSum(data);
                var all = data;
                return all;
            }
            return string.Empty;
        }


        /// <summary>
        /// 中央空调校验
        /// </summary>
        /// <param name="arr_buff"></param>
        /// <returns></returns>


        public static int Compute_CRC_16_Modbus(byte[] bytes)
        {

            int len = bytes.Length;
            //预置 1 个 16 位的寄存器为十六进制FFFF, 称此寄存器为 CRC寄存器。
            int crc = 0xFFFF;
            int i, j;
            for (i = 0; i < len; i++)
            {
                //把第一个 8 位二进制数据 与 16 位的 CRC寄存器的低 8 位相异或, 把结果放于 CRC寄存器
                crc = ((crc & 0xFF00) | (crc & 0x00FF) ^ (bytes[i] & 0xFF));
                for (j = 0; j < 8; j++)
                {
                    //把 CRC 寄存器的内容右移一位( 朝低位)用 0 填补最高位, 并检查右移后的移出位
                    if ((crc & 0x0001) > 0)
                    {
                        //如果移出位为 1, CRC寄存器与多项式A001进行异或
                        crc = crc >> 1;
                        crc = crc ^ 0xA001;
                    }
                    else
                        //如果移出位为 0,再次右移一位
                        crc = crc >> 1;
                }
            }
            return crc;
        }


        private static int CenterAirConditionCRC(string[] arr_buff)
        {
            var crc = 0xffff;
            for (var j = 0; j < 4; j++)
            {
                crc = crc ^ Convert.ToInt32(JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", arr_buff[j]));
                for (var i = 0; i < 8; i++)
                {
                    var data = crc & 0x0001;
                    if (data > 0)
                    {
                        crc = crc >> 1;
                        crc = crc ^ 0xa001;
                    }
                    else
                    {
                        crc = crc >> 1;
                    }
                }

            }
            return crc;
        }


        public static byte[] convertByteHexStringToByte(String byteHexString)
        {
            byteHexString = byteHexString.Replace(" ", "");
            int len = byteHexString.Length;
            byte[] result = new byte[len / 2];
            len = result.Length;

            for (int index = 0; index < len; index++)
            {
                result[index] = (byte)Convert.ToInt32(byteHexString.Substring(index * 2, 2), 16);
            }
            return result;
        }


        /// <summary>
        /// 中央空调发送码(485)
        /// </summary>
        /// <param name="modbus">modus地址</param>
        /// <param name="mode">模式</param>
        /// <param name="value">值</param>
        /// <param name="preValue">前值</param>
        /// <returns></returns>
        public static string CenterAirCode(string modbus, CommandMode mode, string value, string preValue)
        {
            //03 06 0003 00c8
            var listStr = new List<string>();
            preValue = preValue.Replace(" ", "");
            if (string.IsNullOrEmpty(preValue))
            {
                preValue = "0306000300c8";
            }
            if (preValue.Length == 12 || preValue.Length == 16)
            {
                var address = preValue.Substring(4, 4);
                var avalue = preValue.Substring(8, 4);
                var x_tmp = new string[] { modbus, "06", address, avalue, "" };
                x_tmp[0] = modbus.PadLeft(2, '0');
                var workMode = "";
                var workModeValue = "";
                //调低温度
                if (mode == CommandMode.AdjustDownTemperature)
                {

                    workMode = "0003";
                    var d = 0.0;
                    //avalue=0000
                    var result = JinZhiConvert.JinZhiResult("0123456789abcdef", avalue) * 1.0 / 10;
                    if (result == 0 || result < 10 || result > 30)
                    {
                        d = 18;
                    }
                    else
                    {
                        d = Convert.ToDouble(result);
                    }

                    d = d - 1;
                    if (d >= 10 && d <= 30)
                    {
                        var dvalue = (int)Math.Floor(d * 10);
                        var hexValue = Convert.ToString(dvalue, 16);
                        workModeValue = hexValue.PadLeft(4, '0');

                    }
                    // wendu = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", wendu);
                }
                else if (mode == CommandMode.AdjustUpTemperature)
                {
                    workMode = "0003";
                    var d = 0.0;
                    //avalue=0000
                    var result = JinZhiConvert.JinZhiResult("0123456789abcdef", avalue) * 1.0 / 10;
                    if (result == 0 || result < 10 || result > 30)
                    {
                        d = 18;
                    }
                    else
                    {
                        d = Convert.ToDouble(result);
                    }

                    d = d + 1;
                    if (d >= 10 && d <= 30)
                    {
                        var dvalue = (int)Math.Floor(d * 10);
                        var hexValue = Convert.ToString(dvalue, 16);
                        workModeValue = hexValue.PadLeft(4, '0');

                    }
                }
                else if (mode == CommandMode.TurnOn)
                {
                    workMode = "0000";
                    workModeValue = "0001";
                }
                else if (mode == CommandMode.TurnOff)
                {
                    workMode = "0000";
                    workModeValue = "0000";
                }
                ///设置工作模式
                else if (mode == CommandMode.SetMode)
                {
                    workMode = "0001";
                    if (value.Equals("heat", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workModeValue = "0003";
                    }
                    ///制冷
                    else if (value.Equals("cold", StringComparison.CurrentCultureIgnoreCase) || value.Equals("cool", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workModeValue = "0000";
                    }
                    ///送风
                    else if (value.Equals("airsupply", StringComparison.CurrentCultureIgnoreCase) || value.Equals("fan", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workModeValue = "0002";
                    }
                    ///抽湿
                    else if (value.Equals("dehumidification", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workModeValue = "0001";
                    }
                    else if (value.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workModeValue = "0001";
                    }

                }
                else if (mode == CommandMode.SetWindSpeed)
                {
                    ///设置风速
                    if (value.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workMode = "0002";
                        workModeValue = "0000";
                    }
                    else if (value.Equals("low", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workMode = "0002";
                        workModeValue = "0003";
                    }
                    else if (value.Equals("medium", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workMode = "0002";
                        workModeValue = "0002";
                    }
                    else if (value.Equals("high", StringComparison.CurrentCultureIgnoreCase))
                    {
                        workMode = "0002";
                        workModeValue = "0001";
                    }
                    else
                    {
                        workMode = "0002";
                        workModeValue = "0000";
                    }
                }
                else if (mode == CommandMode.SetTemperature)
                {
                    var d = 0.0;
                    workMode = "0003";
                    var isD = double.TryParse(value, out d);
                    workModeValue = "0100";
                    if (isD)
                    {
                        if (d >= 10 && d <= 30)
                        {
                            var dvalue = (int)Math.Floor(d * 10);
                            var hexValue = Convert.ToString(dvalue, 16);
                            workModeValue = hexValue.PadLeft(4, '0');

                        }
                    }

                }
                if (!string.IsNullOrEmpty(workMode) && !string.IsNullOrEmpty(workModeValue))
                {
                    x_tmp[2] = workMode;
                    x_tmp[3] = workModeValue;
                }
                var t1 = convertByteHexStringToByte(string.Join("", x_tmp));
                var t2 = Compute_CRC_16_Modbus(t1);
                var c1 = Convert.ToString(t2, 16).PadLeft(4, '0');
                var change = c1.Substring(2, 2) + c1.Substring(0, 2);
                // var crc = Convert.ToString(centerAirConditionCRC(x_tmp), 16).PadLeft(4, '0');
                x_tmp[4] = change;
                return string.Join("", x_tmp);
            }
            return string.Empty;
        }



        public static int CRC_XModem(byte[] bytes)
        {
            int crc = 0x00;          // initial value
            int polynomial = 0x1021;
            for (int index = 0; index < bytes.Length; index++)
            {
                byte b = bytes[index];
                for (int i = 0; i < 8; i++)
                {
                    bool bit = ((b >> (7 - i) & 1) == 1);
                    bool c15 = ((crc >> 15 & 1) == 1);
                    crc <<= 1;
                    if (c15 ^ bit) crc ^= polynomial;
                }
            }
            crc &= 0xffff;
            return crc;
        }
        /// <summary>
        /// 忆林中央空调
        /// </summary>
        /// <param name="mode">模式</param>
        /// <param name="value">值</param>
        /// <param name="preValue">前值</param>
        /// <returns></returns>
        public static string CenterAirCodeYiLin(CommandMode mode, string value, string preValue)
        {
            //5ff00(固定值) c0(序列) 0091010110(固定值) a1(设置指令) 0003(长度) 1013(寄存器地址) 00(设置值) 34c3(校验码)
            var ser_code = Convert.ToString(DateTime.Now.Millisecond % 256, 16).PadLeft(2, '0');
            //6c
            // var ser_code = "6c";
            var len = "0003";
            var reg_address = "1013";//寄存器地址
            var reg_value = "00";//寄存器的值
            var crc = "0000";
            var arr = new string[] { "55ff00", ser_code, "0091010110", "a1", len, reg_address, reg_value };
            var izheng = 18;
            var ilin = 0;
            if (!string.IsNullOrEmpty(preValue) && preValue.Length >= 30)
            {
                //20+4+4+4(2)
                var setValue = preValue.Substring(24, preValue.Length - 20 - 8);
                ///上一个指令是温度
                if (setValue.Length == 8)
                {
                    var tvalue = setValue.Substring(4, 4);
                    var start = tvalue.Substring(0, 2);
                    var end = tvalue.Substring(2, 2);
                    izheng = Convert.ToInt32(start, 16);
                    _ = Convert.ToInt32(end, 16);
                }
            }
            ///设置温度
            if (mode == CommandMode.AdjustDownTemperature)
            {
                reg_address = "00f1";
                izheng = izheng - 1;
                if (izheng >= 10 && izheng <= 30)
                {
                    reg_value = Convert.ToString(izheng, 16).PadLeft(2, '0') + "00";
                }
            }
            else if (mode == CommandMode.AdjustUpTemperature)
            {
                reg_address = "00f1";
                izheng += 1;
                if (izheng >= 10 && izheng <= 30)
                {
                    reg_value = Convert.ToString(izheng, 16).PadLeft(2, '0') + "00";
                }
            }
            else if (mode == CommandMode.TurnOn)
            {
                reg_address = "1000";
                reg_value = "01";
            }
            else if (mode == CommandMode.TurnOff)
            {
                reg_address = "1000";
                reg_value = "00";
            }
            else if (mode == CommandMode.SetMode)
            {
                reg_address = "1013";
                if (value.Equals("heat", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "03";
                }
                ///制冷
                else if (value.Equals("cold", StringComparison.CurrentCultureIgnoreCase) || value.Equals("cool", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "00";
                }
                ///送风
                else if (value.Equals("airsupply", StringComparison.CurrentCultureIgnoreCase) || value.Equals("fan", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "02";
                }
                ///抽湿
                else if (value.Equals("dehumidification", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "01";
                }
                else if (value.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "04";
                }
            }
            else if (mode == CommandMode.SetWindSpeed)
            {
                reg_address = "100a";

                ///设置风速
                if (value.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "03";
                }
                else if (value.Equals("low", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "02";
                }
                else if (value.Equals("medium", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "01";
                }
                else if (value.Equals("high", StringComparison.CurrentCultureIgnoreCase))
                {
                    reg_value = "00";
                }
                else
                {
                    reg_value = "00";
                }

            }
            else if (mode == CommandMode.SetTemperature)
            {
                reg_address = "00f1";
                int tvalue = 0;
                var bvalue = int.TryParse(value, out tvalue);
                if (bvalue)
                {
                    if (tvalue >= 10 && tvalue <= 30)
                    {
                        reg_value = Convert.ToString(tvalue, 16).PadLeft(2, '0') + "00";
                    }
                    else if (tvalue < 10)
                    {
                        tvalue = 10;
                        reg_value = Convert.ToString(tvalue, 16).PadLeft(2, '0') + "00";
                    }
                    else if (tvalue > 30)
                    {
                        tvalue = 30;
                        reg_value = Convert.ToString(tvalue, 16).PadLeft(2, '0') + "00";
                    }

                }

            }
            len = ((reg_address.Length + reg_value.Length) / 2 + "").PadLeft(4, '0');
            arr[1] = ser_code;
            arr[4] = len;
            arr[5] = reg_address;
            arr[6] = reg_value;
            var t1 = convertByteHexStringToByte(string.Join("", arr));
            var t2 = CRC_XModem(t1);
            var c1 = Convert.ToString(t2, 16).PadLeft(4, '0');
            // var arr = new string[] { "55ff00", ser_code, "0091010110", "a1", len, reg_address, reg_value };

            var data = string.Join("", arr) + c1;
            return data;

        }




        /// <summary>
        /// 包括校验码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> SplitData(string code, bool includeCheckSum = false)
        {
            var list = new List<string>();
            var allCode = code.Replace(" ", "");
            if (includeCheckSum)
            {
                if (allCode.Length > 28)
                {
                    //0102
                    var _header = allCode.Substring(0, 4);//4
                    var _group = allCode.Substring(4, 4);//8
                    var _7b = allCode.Substring(8, 14);//22
                    var _fsum = allCode.Substring(22, 2);//24
                    var otherdata = allCode.Substring(24, allCode.Length - 4 - 24);
                    var ff = allCode.Substring(allCode.Length - 4, 2);
                    var _checkSum = allCode.Substring(allCode.Length - 2);

                    list.Add(_header);
                    list.Add(_group);
                    list.Add(_7b);
                    list.Add(_fsum);
                    list.Add(otherdata);
                    list.Add(ff);
                    list.Add(_checkSum);

                }

            }
            else
            {

                if (allCode.Length > 26)
                {
                    //0102
                    var _header = allCode.Substring(0, 4);//4
                    var _group = allCode.Substring(4, 4);//8
                    var _7b = allCode.Substring(8, 14);//22
                    var _fsum = allCode.Substring(22, 2);//24

                    var otherdata = allCode.Substring(24, allCode.Length - 2 - 24);
                    var ff = allCode.Substring(allCode.Length - 2, 2);


                    list.Add(_header);
                    list.Add(_group);
                    list.Add(_7b);
                    list.Add(_fsum);
                    list.Add(otherdata);
                    list.Add(ff);


                }
            }


            return list;


        }




        public static string GetSum(string hexString)
        {

            var carray = hexString.ToLower().Replace(" ", "").ToArray();
            if (carray.Length % 2 == 0)
            {
                var list = new List<string>();
                for (var i = 0; i < carray.Length / 2; i++)
                {
                    var j = i * 2;
                    var each = carray[j] + "" + carray[j + 1];
                    list.Add(each);

                }
                var sum = 0;
                foreach (var d in list)
                {
                    var data = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", d);
                    sum += Convert.ToInt32(data);
                }
                var result = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", sum + "");
                if (result.Length > 1)
                {
                    return result.Substring(result.Length - 2);
                }
                return result;
            }
            return string.Empty;


        }


        public static string GetSum(string hexString, out List<string> list)
        {
            list = new List<string>();
            var carray = hexString.Replace(" ", "").ToArray();
            if (carray.Length % 2 == 0)
            {

                for (var i = 0; i < carray.Length / 2; i++)
                {
                    var each = carray[i] + "" + carray[i + 1];
                    list.Add(each);

                }
                var sum = 0;
                foreach (var d in list)
                {
                    var data = JinZhiConvert.JinZhiResult("0123456789abcdef", "0123456789", d);
                    sum += Convert.ToInt32(data);
                }
                var result = JinZhiConvert.JinZhiResult("0123456789", "0123456789abcdef", sum + "");
                if (result.Length > 1)
                {
                    return result.Substring(result.Length - 2);
                }
                return result;
            }
            return string.Empty;


        }




    }
}
