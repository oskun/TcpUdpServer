using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class MsgTypeHelper
    {

        public static string GetMacMsg(byte[] msg, HeartBeatType htype)
        {
            if (msg != null && msg.Length > 0)
            {
                if (htype == HeartBeatType.HANFEN)
                {
                    var msg_str = Encoding.UTF8.GetString(msg);
                    //66ff00ff+ACCF23B87DAB,172.20.10.4
                    var hanfeng = msg_str.Split(new string[] { "+", "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (hanfeng.Length >= 3)
                    {
                        //66ff00ff+ACCF2349A932,192.168.1.150
                        var mac = hanfeng[1].ToLower();
                        return mac;
                    }
                }
                else if (htype == HeartBeatType.YILIN)
                {
                    var trans_data = StrHelper.GetHexStr(msg);
                    var split_data = trans_data.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    var mac_flag = "0079";///开始是mac 地址
                                          ///长度6个字节
                    var mac_len = 12;
                    ///心跳包开始标识
                    var heartBeatStart = "55ff";
                    /////表示心跳包
                    //var index_18_len_2 = "ff";
                    trans_data = trans_data.Replace(" ", "");
                    if (trans_data.IndexOf(mac_flag) != -1)
                    {
                        if (trans_data.Length > 20)
                        {
                            var flag_char = trans_data.Substring(18, 2);
                            if (trans_data.StartsWith(heartBeatStart) && flag_char.Equals("ff"))
                            {
                                var index = trans_data.IndexOf(mac_flag);
                                ///mac地址
                                if (trans_data.IndexOf(mac_flag) != -1)
                                {
                                    var start = trans_data.IndexOf(mac_flag) + mac_flag.Length;
                                    if (trans_data.Length >= start && trans_data.Length >= (start + mac_len))
                                    {
                                        var mac = trans_data.Substring(start, mac_len).ToLower();
                                        return mac;
                                    }
                                }
                            }
                        }
                    }
                }
               
            }

            return string.Empty;
        }

        /// <summary>
        /// 得到心跳包类型
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static HeartBeatType GetMsgType(byte[] msg)
        {


            if (msg.Length >= 4)
            {
                if (msg[0] == 54 && msg[1] == 54 && msg[2] == 102 && msg[3] == 102)
                {
                    return HeartBeatType.HANFEN;
                }
                else if (msg[0] == 85 && msg[1] == 255 && msg[2] == 0)
                {
                    return HeartBeatType.YILIN;
                }

                else if (msg[0] == 85 && msg[1] == 255 && msg[2] == 14 && msg[3] == 98)
                {
                    return HeartBeatType.CHECK_TIME;
                }
                ///报警事件
                else if ((msg[0] == 85 && msg[1] == 255 && msg[2] == 15) && (msg[3] == 4 || msg[4] == 5))
                {
                    return HeartBeatType.BAOJING;
                }
                //0x55 + 0xFF + 0x13 + mac(6B) + CRC
                else if (msg[0] == 85 && msg[1] == 255 && msg[2] == 19)
                {
                    return HeartBeatType.DEVICE_RESTART;
                }
                ///暖通设备

                else if (msg[0] == 85 && msg[1] == 255 && msg[2] == 18 && msg.Length > 20)
                {
                    /*
                     * 0x55 + 0xFF + 0x11 + mac(6B) +tCount(1B) + [cCount(1B) + command]+ CRC

tCount：表示有几个指令
[]：表示的是指令集，个数由tCount决定， 每个指令为指令长度+指令内容组成；
云端收到操作后按照指令集依次发送到对应mac设备， 如mac设备离线忽略操作；
                     * 
                     * 
                     */

                    return HeartBeatType.NUANTONG;

                }

                else if (msg[0] == 123 && msg[msg.Length - 1] == 125)
                {
                    return HeartBeatType.CMD;
                }

                //55 ff 08 00 01 5d
                //55 ff 08 00 00 5c
                //55 ff 08 01 01 5e
                //55 ff 08 01 00 5d
                if(msg.Length>=5)
                {
                    if ((msg[0] == 85 && msg[1] == 255 && msg[2] == 8 && msg[3] == 0 && msg[4] == 1 && msg[5] == 93) ||
                    (msg[0] == 85 && msg[1] == 255 && msg[2] == 8 && msg[3] == 0 && msg[4] == 0 && msg[5] == 92) ||
                     (msg[0] == 85 && msg[1] == 255 && msg[2] == 8 && msg[3] == 1 && msg[4] == 1 && msg[5] == 94) ||
                     (msg[0] == 85 && msg[1] == 255 && msg[2] == 8 && msg[3] == 1 && msg[4] == 0 && msg[5] == 93))
                    {
                        return HeartBeatType.BUFANGCHEFANG;
                    }
                }
             
                //var str = Encoding.UTF8.GetString(msg);
                //if (str.Equals("55ff0800015d") || str.Equals("55ff0800005c") || str.Equals("55ff0801015e") || str.Equals("55ff0801005d"))
                //{
                //    ///布防撤防的返回
                   
                //}


            }
            return HeartBeatType.DEFAULT;
        }

        public static string GetHANFENIP(byte[] msg)
        {
            var msg_str = Encoding.UTF8.GetString(msg);
            var hanfeng = msg_str.Split(new string[] { "+", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (hanfeng.Length >= 3)
            {
                //66ff00ff+ACCF2349A932,192.168.1.150
                var IP = clearIP(hanfeng[2].ToLower().Replace("66ff00ff", ""));
                return IP;
            }
            return string.Empty;
        }


        public static string GetHANFENIP(byte[] msg, out string mac)
        {
            var msg_str = Encoding.UTF8.GetString(msg);
            mac = string.Empty;
            var hanfeng = msg_str.Split(new string[] { "+", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (hanfeng.Length >= 3)
            {
                //66ff00ff+ACCF2349A932,192.168.1.150
                var IP = clearIP(hanfeng[2].ToLower().Replace("66ff00ff", ""));
                mac = hanfeng[1].ToLower();
                return IP;
            }
            return string.Empty;
        }


        public static MacIPVersionInfo GetHearBeatInfo(byte[] msg)
        {
            HeartBeatType ht = GetMsgType(msg);
            var  mo = new MacIPVersionInfo();
            mo.heartBeatType = ht;
            if (ht == HeartBeatType.HANFEN)
            {
                var mac = string.Empty;
                mo.IP = GetHANFENIP(msg, out mac);
                mo.mac = mac;
                mo.version = "";
            }
            else if (ht == HeartBeatType.YILIN)
            {
                var mac = string.Empty;
                mo.IP = "";
                mo.mac = GetMacMsg(msg, HeartBeatType.YILIN);
                mo.version = "";
            }
            else if (ht == HeartBeatType.DEFAULT)
            {
                var mac = string.Empty;

                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";

            }
            else if (ht == HeartBeatType.CMD)
            {
                var mac = string.Empty;

                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";
            }
            else if (ht == HeartBeatType.BUFANGCHEFANG)
            {
                var mac = string.Empty;

                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";
            }
            else if (ht == HeartBeatType.CHECK_TIME)
            {
                var mac = string.Empty;
                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";
            }
            else if (ht == HeartBeatType.BAOJING)
            {
                var mac = string.Empty;

                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";
            }
            else if (ht == HeartBeatType.DEVICE_RESTART)
            {
                string mac = string.Empty;
                mo.heartBeatType = ht;
                mo.IP = "";
                mo.mac = mac;
                mo.version = "";
                
            }
            else if (ht == HeartBeatType.NUANTONG)
            {
                /*
                 16) 暖通设备控制支持
目前暖通控制通过云端执行操作，格式如
0x55 + 0xFF + 0x11 + mac(6B) +tCount(1B) + [cCount(1B) + command]+ CRC

tCount：表示有几个指令
[]：表示的是指令集，个数由tCount决定， 每个指令为指令长度+指令内容组成；
云端收到操作后按照指令集依次发送到对应mac设备， 如mac设备离线忽略操作；
                */

                mo.heartBeatType = ht;
                mo.IP = "";
                mo.version = "";


                var str = StrHelper.GetHexStr(msg).Replace(" ", "");

                if (str.Length >= 20)
                {
                    ///mac
                    var mac = str.Substring(6, 12);

                    LogHelper.LogFilter(true, "暖通==>" + str);
                    var tcount = str.Substring(18, 2);
                    var count = Convert.ToInt32(tcount, 16);
                    //index=14
                    //55ff11 262040589573 01 07 0110660130fc9b
                    //55ff11 262040589573 02 07 0110660130fc9b 78070110660130fc9b78
                    //55ff11 262040589573 02 07 0110660130fc9b 07 0110660130fc9b78
                    var start = 20;
                    for (var i = 0; i < count; i++)
                    {
                        if (start >= str.Length)
                        {
                            break;
                        }
                        var cmdCount = Convert.ToInt32(str.Substring(start, 2).ToLower(), 16);
                        var cmd = str.Substring(start + 2, cmdCount * 2);
                        mo.listcmd.Add(cmd);
                        start = start + cmdCount * 2 + 2;

                    }
                }
            }

            return mo;
        }






        public static string clearIP(string ip)
        {
            var sb = new StringBuilder();
            foreach (var d in ip)
            {
                if (char.IsDigit(d) || d == '.')
                {
                    sb.Append(d);
                }
                else
                {
                    break;
                }
            }
            return sb.ToString();
        }

    }
}
