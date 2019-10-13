using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class TUdpServer
    {
        //public static Dictionary<string, Socket> mapUdp = new Dictionary<string, Socket>();

        //static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        // 192.168.1.205


        static string IPADDRESS;


        static int UDPPORT;

        public void UdpStart(string ipaddress, int udpPort)
        {
            IPADDRESS = ipaddress;
            UDPPORT = udpPort;
            for (var index = 0; index <= 10; index++)
            {

                var config = new ServerConfig(4198, udpPort + index, ipaddress);
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    server.Bind(new IPEndPoint(IPAddress.Parse(config.listenAddress), config.udpPort));
                    EndPoint point = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                    byte[] bs = new byte[1024];
                    UdpData ud = new UdpData();
                    ud.socket = server;
                    ud.data = bs;
                    server.BeginReceiveFrom(bs, 0, bs.Length, SocketFlags.None, ref point, OnUdpRecieve, ud);

                }
                catch (SocketException se)
                {
                    var _msg = "异常：UDP No:38   socket 异常" + se.ErrorCode + "    " + se.Message;

                    Func<bool> func1 = () => true;
                    //var msgx = "UDP:" + "64-----" + se.ErrorCode + "----" + se.Message;
                    LogHelper.LogFilter(func1, _msg);
                    Environment.Exit(0);
                }

            }

        }


        private static void NotInline(Socket userver, EndPoint point, string mac, int cmdtype, string msg = "设备未上线")
        {
            var code = "-2";
            if (msg.Equals("设备已删除"))
            {
                code = "-3";
            }
            var bdata = Encoding.UTF8.GetBytes(JsonHelper<Status<string>>.GetJson(new Status<string>()
            { code = code, data = "", mac = mac, msg = msg, commandType = cmdtype + "" }));
            var iet = (IPEndPoint)point;
            try
            {

                if (!iet.Address.Equals(IPAddress.Any))
                {
                    userver.SendTo(bdata, point);
                }


            }
            catch (SocketException se)
            {

                Console.WriteLine("64-----" + se.ErrorCode + "----" + se.Message);
                //ReConnectUDP(userver);
                //StackTrace stack = new StackTrace();
                //var colNumber = stack.ToString();
                //Console.WriteLine("1512" + colNumber);


                Func<bool> func1 = () => true;
                var msgx = "UDP:" + "64-----" + se.ErrorCode + "----" + se.Message;
                LogHelper.LogFilter(func1, msgx);

            }
        }

        /// <summary>
        /// 给设备发送信息
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="cmdbytes"></param>
        private static void SendDataToDevice(Socket st, byte[] cmdbytes)
        {
            try
            {
                if (st != null && st.Connected)
                {
                    st.Send(cmdbytes);
                }
                else
                {
                    var ipe = (IPEndPoint)st.RemoteEndPoint;
                    var address = ipe.Address.ToString();
                    var port = ipe.Port;
                    Program.removeOnlineTcpRelation(address, port);
                   
                }

            }
            catch (SocketException se)
            {
                var _msg = "101异常:" + se.ErrorCode + "     " + se.Message;
                var ipe = (IPEndPoint)st.RemoteEndPoint;
                var address = ipe.Address.ToString();
                var port = ipe.Port;
                Program.removeOnlineTcpRelation(address, port);
                //LogHelper.Info(_msg);

                LogHelper.LogFilter(true, se.StackTrace);

            }


        }

        private static int checkIsCenterAirCondition(YZKDeviceInfo_UserInfo device_info)
        {
            ///485
            if (device_info.device_protocol == 2 && string.IsNullOrEmpty(device_info.ykmessageStr) && !string.IsNullOrEmpty(device_info.device_mac) && !string.IsNullOrEmpty(device_info.modbus_address))
            {
                return 1;
            }
            ///忆林
            else if (device_info.device_protocol_category == 1 && (device_info.device_protocol == 1 || device_info.device_protocol == 8))
            {
                return 2;
            }
            return -1;
        }


        private static void RepeatCmd(List<MYKMessage> ykMessages, int repeatCount, Socket device, params string[] findValue)
        {

            foreach (var d in findValue)
            {
                var findKeyValue = ykMessages.Find(a => a.key_name.IndexOf(d) != -1);
                if (findKeyValue != null && findKeyValue.key_value != null && findKeyValue.key_value.Count > 0)
                {

                    var cmdValueData = findKeyValue.key_value[0];
                    var str = TianMao.GetOrgMsg(cmdValueData.a, cmdValueData.b, cmdValueData.value);
                    var cmdbyte = StrHelper.strToHexByte(str);
                    //var repeatCount = cmd.repeatCount==0?1:cmd.repeatCount;
                    // 0表示红外遥控  1表示射频遥控， 2表示空调红外
                    for (var i = 0; i < repeatCount; i++)
                    {
                        Console.WriteLine("即将发送命令：" + str);

                        //Func<bool> func =()=> true;

                        //LogHelper.LogFilter(func, "发送命令:" + str);
                        //var state = new List<IPEndPointState>();
                        //dicMacCmd[feidiemac] = point;
                        SendDataToDevice(device, cmdbyte);
                        ///射频
                        if (cmdValueData.b.Equals(1))
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    break;

                }
            }

        }






        private void TurnOnMethod(int protocol_type, EndPoint point, Socket device, string feidiemac, List<MYKMessage> ykMessages, string[] findValue)
        {
            if (protocol_type == 0 || protocol_type == (int)Protocol_Type.电视机)
            {
                //var findValue = "开,电源".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //UdpServerPoint up = new UdpServerPoint();
                //up.point = (IPEndPoint)point;
                //up.uServer = udpServer;
                RepeatCmd(ykMessages, 1, device, findValue);
            }
            else if (protocol_type == 0 || protocol_type == (int)Protocol_Type.有线电视盒)
            {
                //var findValue = "开,电源".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //UdpServerPoint up = new UdpServerPoint();
                //up.point = (IPEndPoint)point;
                //up.uServer = udpServer;
                RepeatCmd(ykMessages, 1, device, findValue);
            }
            else if (protocol_type == (int)Protocol_Type.灯)
            {
                //var findValue = "开,电源".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //UdpServerPoint up = new UdpServerPoint();
                //up.point = (IPEndPoint)point;
                //up.uServer = udpServer;
                RepeatCmd(ykMessages, 1, device, findValue);
            }
            else if (protocol_type == (int)Protocol_Type.窗帘)
            {
                //var findValue = "开,电源".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //UdpServerPoint up = new UdpServerPoint();
                //up.point = (IPEndPoint)point;
                //up.uServer = udpServer;
                RepeatCmd(ykMessages, 1, device, findValue);
            }




        }


        private static void SendAirConditionCode(string code, Socket pox)
        {
            try
            {
                var cmdbyte = StrHelper.strToHexByte(code);
                pox.Send(cmdbyte);

            }
            catch(SocketException se)
            {

                Func<bool> func1 = () => true;
                LogHelper.LogFilter(true, se.StackTrace);
            }
        }


        private static bool KongTiaoCmd(string deviceId, string feidiemac, int protocol_type, List<MYKMessage> ykMessages, CommandMode mode, string value, Socket pox)
        {
            var a = 0;
            var b = 2;
            if (protocol_type == (int)Protocol_Type.空调)
            {
                var cmdCode = string.Empty;
                if (ykMessages != null && ykMessages.Count > 0)
                {
                    var kv = ykMessages[0].key_value;
                    if (kv.Count > 0)
                    {
                        cmdCode = kv[0].value;
                        a = kv[0].a;
                        b = kv[0].b;
                    }
                }
                var isCmdExists = DALAirCondtionCmd.IsCmdExists(deviceId);

                if (isCmdExists)
                {
                    var cmddata = DALAirCondtionCmd.GetByDeviceId(deviceId);
                    if (cmddata != null && !string.IsNullOrEmpty(cmddata.cmd))
                    {
                        cmdCode = cmddata.cmd;
                    }
                }

                if (!string.IsNullOrEmpty(cmdCode))
                {


                    var sendCode = AirConditionSend.AirCode(cmdCode, mode, value);
                    ///给空调发命令
                    ///
                   // var str = "55ff0100020f3001033e1b020100010602030000FF01";
                    var str = TianMao.GetOrgMsg(a, b, sendCode);
                    SendAirConditionCode(str, pox);
                    DALAirCondtionCmd.addOrUpdateCmd(new AirCondtionCmd()
                    {
                        cmd = sendCode,
                        deviceId = deviceId

                    });

                    var data = string.Format("给小飞碟{0}发送信息{1}", feidiemac, str);
                    Console.WriteLine(data);

                }


            }
            return false;
        }






        private void OnUdpRecieve(IAsyncResult ar)
        {
            var udp = ar.AsyncState as UdpData;
            if (udp != null)
            {
                var server = udp.socket;



                EndPoint point = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    var len = server.EndReceiveFrom(ar, ref point);
                   
                    if (len > 0)
                    {
                        var bs = udp.data.ToList().GetRange(0, len).ToArray();
                        string msg = Encoding.UTF8.GetString(bs, 0, len);
                      
                     
                        if (!string.IsNullOrEmpty(msg))
                        {
                            ///喂狗
                            if (msg.Equals("echo_data"))
                            {
                                var echo_data = Encoding.UTF8.GetBytes(msg);
                                try
                                {
                                    server.SendTo(echo_data, point);
                                }
                                catch
                                {

                                }
                               
                            }
                            ///正常应答
                            else
                            {
                                var msgMd5 = EncryptHelper.MD5Encoding(msg);
                                //LogHelper.Info("收到UDP消息:"+msg);
                                if (!RedisHelper<string>.IsKeyExist(msgMd5))
                                {
                                    var cmd = JsonHelper<ClientCommand>.GetObject(msg);
                                    if (cmd != null)
                                    {
                                        var cmdtype = cmd.commandType;
                                        var command = cmd.command;
                                        var value = cmd.value;
                                        var username = cmd.username;
                                        if (cmd != null)
                                        {
                                            var mac = cmd.mac;
                                            mac = !string.IsNullOrEmpty(mac) ? mac.ToLower() : "";

                                            ///筛选日志
                                            var ipe = (IPEndPoint)point;
                                            //Func<bool> func = () => (!string.IsNullOrEmpty(username) && username.Equals("贝贝") && ipe.Address.ToString().Equals("180.155.69.150") == false && (!string.IsNullOrEmpty(command) && command.StartsWith("01") == false && command.StartsWith("02") == false)) || (!string.IsNullOrEmpty(cmd.Id) && string.IsNullOrEmpty(username));
                                            //LogHelper.LogFilter(func, msg);


                                            #region 正常命令
                                            if (!string.IsNullOrEmpty(mac))
                                            {
                                                var mu = Program.GetRelation(mac);
                                                //Program.upgradeClients(mac, point, server);
                                                if (mu != null && mu.device != null)
                                                {

                                                    if (cmd.sid > 0)
                                                    {
                                                        var app = cmd.app;

                                                        if (!string.IsNullOrEmpty(app))
                                                        {
                                                            app = cmd.app.ToLower();
                                                        }
                                                        else
                                                        {
                                                            app = string.Empty;
                                                        }
                                                        var isExists = true;
                                                        if (app.Equals("yzk"))
                                                        {
                                                            isExists = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_IsDeviceExists(cmd.sid + "");
                                                        }
                                                        else if (app.Equals("hdl"))
                                                        {
                                                            isExists = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_IsHDLDeviceExists(cmd.sid + "");
                                                           
                                                        }
                                                        if (isExists)
                                                        {
                                                            var cmdbytes = StrHelper.strToHexByte(cmd.command);
                                                            ///给设备发送命令
                                                            LogHelper.Info("给设备发送命令：" + cmd.command, mu.device);
                                                            if (!string.IsNullOrEmpty(app) && app.Equals("hdl"))
                                                            {
                                                                LogHelper.LogFilter(true, "cmd=>" + cmd.command);
                                                            }
                                                            Program.upgradeClients(mac, point, server);
                                                            SendDataToDevice(mu.device, cmdbytes);
                                                        }
                                                        else
                                                        {
                                                            ///通知设备已删除
                                                            NotInline(server, point, mac, cmdtype, "设备已删除");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        var cmdbytes = StrHelper.strToHexByte(cmd.command);
                                                        ///给设备发送命令
                                                        LogHelper.Info("给设备发送命令：" + cmd.command, mu.device);
                                                        Program.upgradeClients(mac, point, server);
                                                        SendDataToDevice(mu.device, cmdbytes);
                                                    }


                                                }
                                                else
                                                {
                                                    ///通知设备未上线
                                                    NotInline(server, point, mac, cmdtype);
                                                }
                                            }
                                            else if (!string.IsNullOrEmpty(cmd.Id))
                                            {
                                                var device_info = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_GetDataByRecordid(cmd.Id);
                                                if (device_info != null)
                                                {
                                                    mac = device_info.device_mac;

                                                    var mu = Program.GetRelation(mac);

                                                    ///判断是否是中央空调
                                                    var isAir = checkIsCenterAirCondition(device_info);
                                                    #region 485设备
                                                    if (isAir == 1)
                                                    {
                                                        var modbus_address = device_info.modbus_address;

                                                        if (!string.IsNullOrEmpty(modbus_address))
                                                        {
                                                            var isCmdExists = DALAirCondtionCmd.IsCmdExists(cmd.Id);
                                                            var preCode = string.Empty;
                                                            if (isCmdExists)
                                                            {
                                                                var cmddata = DALAirCondtionCmd.GetByDeviceId(cmd.Id);
                                                                if (cmddata != null && !string.IsNullOrEmpty(cmddata.cmd))
                                                                {
                                                                    preCode = cmddata.cmd;
                                                                }
                                                            }
                                                            var strToSend = string.Empty;
                                                            if (command.Equals("TurnOn"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.TurnOn, cmd.value, preCode);
                                                            }
                                                            else if (command.Equals("TurnOff"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.TurnOff, cmd.value, preCode);
                                                            }
                                                            else if (command.Equals("AdjustUpTemperature"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.AdjustUpTemperature, cmd.value, preCode);
                                                            }
                                                            else if (command.Equals("AdjustDownTemperature"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.AdjustDownTemperature, cmd.value, preCode);
                                                            }
                                                            else if (command.Equals("SetTemperature"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.SetTemperature, cmd.value, preCode);
                                                            }
                                                            else if (command.Equals("SetMode"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.SetMode, cmd.value, preCode);
                                                            }
                                                            ///设置风速
                                                            else if (command.Equals("SetWindSpeed"))
                                                            {
                                                                strToSend = AirConditionSend.CenterAirCode(modbus_address, CommandMode.SetWindSpeed, cmd.value, preCode);
                                                            }
                                                            LogHelper.Info(" 中央空调:" + strToSend);
                                                            if (!string.IsNullOrEmpty(strToSend))
                                                            {

                                                                Program.upgradeClients(mac, point, server);

                                                                if (!command.Equals("TurnOn") && !command.Equals("TurnOff"))
                                                                {
                                                                    DALAirCondtionCmd.addOrUpdateCmd(new AirCondtionCmd()
                                                                    {
                                                                        cmd = strToSend,
                                                                        deviceId = cmd.Id
                                                                    });
                                                                }
                                                                SendAirConditionCode(strToSend, mu.device);
                                                            }


                                                        }
                                                    }


                                                    #endregion

                                                    #region  忆林设备
                                                    ///忆林温控器
                                                    else if (isAir == 2)
                                                    {



                                                        var isCmdExists = DALAirCondtionCmd.IsCmdExists(cmd.Id);
                                                        var preCode = string.Empty;
                                                        if (isCmdExists)
                                                        {
                                                            var cmddata = DALAirCondtionCmd.GetByDeviceId(cmd.Id);
                                                            if (cmddata != null && !string.IsNullOrEmpty(cmddata.cmd))
                                                            {
                                                                preCode = cmddata.cmd;
                                                            }
                                                        }
                                                        Console.WriteLine("msg:" + msg);
                                                        var strToSend = string.Empty;
                                                        if (command.Equals("TurnOn"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.TurnOn, cmd.value, preCode);
                                                        }
                                                        else if (command.Equals("TurnOff"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.TurnOff, cmd.value, preCode);
                                                        }
                                                        else if (command.Equals("AdjustUpTemperature"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.AdjustUpTemperature, cmd.value, preCode);
                                                        }
                                                        else if (command.Equals("AdjustDownTemperature"))
                                                        {
                                                            Console.WriteLine("降低空调温度");
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.AdjustDownTemperature, cmd.value, preCode);
                                                        }
                                                        else if (command.Equals("SetTemperature"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.SetTemperature, cmd.value, preCode);
                                                        }
                                                        else if (command.Equals("SetMode"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.SetMode, cmd.value, preCode);
                                                        }
                                                        ///设置风速
                                                        else if (command.Equals("SetWindSpeed"))
                                                        {
                                                            strToSend = AirConditionSend.CenterAirCodeYiLin(CommandMode.SetWindSpeed, cmd.value, preCode);
                                                        }
                                                        LogHelper.Info("中央空调忆林:" + strToSend);
                                                        if (!string.IsNullOrEmpty(strToSend))
                                                        {

                                                            mac = device_info.device_mac;

                                                            Program.upgradeClients(mac, point, server);

                                                            if (!command.Equals("TurnOn") && !command.Equals("TurnOff"))
                                                            {
                                                                DALAirCondtionCmd.addOrUpdateCmd(new AirCondtionCmd()
                                                                {
                                                                    cmd = strToSend,
                                                                    deviceId = cmd.Id
                                                                });
                                                            }

                                                            LogHelper.Info("忆林发送设备:" + strToSend, mu.device);



                                                            SendAirConditionCode(strToSend, mu.device);
                                                        }


                                                    }




                                                    #endregion


                                                    #region  天猫精灵

                                                    else
                                                    {
                                                        //var device_info = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_GetDataByRecordid(cmd.Id);
                                                        if (device_info != null)
                                                        {
                                                            var bxk_label = device_info.bxk_label;
                                                            var home_id = device_info.home_id;
                                                            var device_mac = device_info.device_mac;


                                                            ///判断是否是中央空调
                                                            var isCenterAirCondition = checkIsCenterAirCondition(device_info);
                                                            var lx = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_GetXiaoFeiDie(home_id, 64, "3", bxk_label);
                                                            if (lx != null && lx.Count > 0)
                                                            {
                                                                ///小飞碟mac
                                                                var feidiemac = lx[0].device_mac.ToLower();
                                                                Console.WriteLine("查到小飞碟mac:" + feidiemac);
                                                                Program.upgradeClients(feidiemac, point, server);


                                                                if (!string.IsNullOrEmpty(feidiemac))
                                                                {

                                                                    mu = Program.GetRelation(feidiemac);


                                                                    if (mu != null && mu.device != null)
                                                                        if (cmdtype == (int)COMMANDTYPE.TMJL)
                                                                        {
                                                                            var protocol_type = cmd.protocol_type;
                                                                            var ykMessages = JsonHelper<List<MYKMessage>>.GetObject(device_info.ykmessageStr);
                                                                            if (ykMessages != null)
                                                                            {

                                                                                LogHelper.Info(string.Format("天猫精灵-----  Id:{0}  mac:{1}  内容:{2} ", cmd.Id, feidiemac, device_info.ykmessageStr), mu.device);
                                                                                ///开机
                                                                                if (command.Equals("TurnOn"))
                                                                                {
                                                                                    if (protocol_type == (int)Protocol_Type.空调)
                                                                                    {
                                                                                        KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.TurnOn, value, mu.device);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        var findValue = "开,电源".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                                                        TurnOnMethod(protocol_type, point, mu.device, mac, ykMessages, findValue);
                                                                                    }

                                                                                }
                                                                                ////关机
                                                                                else if (command.Equals("TurnOff"))
                                                                                {

                                                                                    if (protocol_type == (int)Protocol_Type.空调)
                                                                                    {
                                                                                        KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.TurnOff, value, mu.device);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        var findValue = "电源,关".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                                                        TurnOnMethod(protocol_type, point, mu.device, mac, ykMessages, findValue);
                                                                                    }



                                                                                }
                                                                                ///调高温度
                                                                                else if (command.Equals("AdjustUpTemperature"))
                                                                                {
                                                                                    var hasSend = KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.AdjustUpTemperature, value, mu.device);


                                                                                }
                                                                                else if (command.Equals("AdjustDownTemperature"))
                                                                                {
                                                                                    var hasSend = KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.AdjustDownTemperature, value, mu.device);


                                                                                }
                                                                                else if (command.Equals("SetTemperature"))
                                                                                {
                                                                                    if (protocol_type == (int)Protocol_Type.空调)
                                                                                    {
                                                                                        var hasSend = KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.SetTemperature, value, mu.device);

                                                                                    }

                                                                                }
                                                                                else if (command.Equals("SetMode"))
                                                                                {
                                                                                    if (protocol_type == (int)Protocol_Type.空调)
                                                                                    {
                                                                                        var hasSend = KongTiaoCmd(cmd.Id, feidiemac, protocol_type, ykMessages, CommandMode.SetMode, value, mu.device);

                                                                                    }

                                                                                }

                                                                                ///选台
                                                                                else if (command.Equals("SelectChannel"))
                                                                                {

                                                                                }
                                                                                ///上一台
                                                                                else if (command.Equals("AdjustUpChannel"))
                                                                                {

                                                                                }
                                                                                else if (command.Equals("AdjustDownChannel"))
                                                                                {

                                                                                }
                                                                                else if (command.Equals("AdjustUpVolume"))
                                                                                {
                                                                                    if (protocol_type == 0 || protocol_type == (int)Protocol_Type.电视机)
                                                                                    {
                                                                                        var repeatCount = 5;
                                                                                        var findValue = "音量+";
                                                                                        RepeatCmd(ykMessages, repeatCount, mu.device, findValue);

                                                                                    }


                                                                                }
                                                                                else if (command.Equals("AdjustDownVolume"))
                                                                                {

                                                                                    if (protocol_type == 0 || protocol_type == (int)Protocol_Type.电视机)
                                                                                    {
                                                                                        var repeatCount = 5;
                                                                                        var findValue = "音量-";
                                                                                        RepeatCmd(ykMessages, repeatCount, mu.device, findValue);
                                                                                    }



                                                                                }
                                                                                else if (command.Equals("SetWindSpeed"))
                                                                                {
                                                                                    if (protocol_type == (int)Protocol_Type.空调)
                                                                                    {

                                                                                    }
                                                                                }
                                                                                else if (command.Equals("AdjustDownVolume"))
                                                                                {

                                                                                }
                                                                                ///静音
                                                                                else if (command.Equals("SetMute"))
                                                                                {
                                                                                }
                                                                                ///暂停
                                                                                else if (command.Equals("Pause"))
                                                                                {
                                                                                    if (protocol_type == 0 || protocol_type == (int)Protocol_Type.电视机)
                                                                                    {
                                                                                        var findValue = "暂停";

                                                                                        RepeatCmd(ykMessages, 1, mu.device, findValue);
                                                                                    }
                                                                                    else if (protocol_type == (int)Protocol_Type.灯)
                                                                                    {
                                                                                        var findValue = "暂停";
                                                                                        UdpServerPoint up = new UdpServerPoint();
                                                                                        RepeatCmd(ykMessages, 1, mu.device, findValue);
                                                                                    }
                                                                                    else if (protocol_type == (int)Protocol_Type.窗帘)
                                                                                    {
                                                                                        var findValue = "暂停";
                                                                                        RepeatCmd(ykMessages, 1, mu.device, findValue);
                                                                                    }

                                                                                }
                                                                                ///继续
                                                                                else if (command.Equals("Continue"))
                                                                                {

                                                                                }
                                                                                ///调高亮度
                                                                                else if (command.Equals("AdjustUpBrightness"))
                                                                                {
                                                                                }
                                                                                ///调低亮度
                                                                                else if (command.Equals("AdjustDownBrightness"))
                                                                                {
                                                                                }

                                                                            }
                                                                        }
                                                                        else
                                                                        {

                                                                            if (mu != null && mu.device != null)
                                                                            {
                                                                                try
                                                                                {
                                                                                    var cmdbytes = StrHelper.strToHexByte(cmd.command);
                                                                                    mu.device.Send(cmdbytes);
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    LogHelper.Info("UDP:717发送给设备异常:" + ex.Message);
                                                                                    Func<bool> func1 = () => true;
                                                                                    var msgx = "UDP:717发送给设备异常:" + ex.Message;
                                                                                    LogHelper.LogFilter(func1, msgx);

                                                                                }

                                                                            }



                                                                        }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("小飞碟mac有误");
                                                                }

                                                            }
                                                            else
                                                            {
                                                                #region  未找到小飞碟
                                                                var bdata = Encoding.UTF8.GetBytes(JsonHelper<Status<string>>.GetJson(new Status<string>() { code = "0", data = "", mac = mac, msg = "未找到小飞碟", commandType = cmdtype + "" }));
                                                                try
                                                                {
                                                                    server.SendTo(bdata, point);
                                                                }
                                                                catch (SocketException ex)
                                                                {
                                                                    LogHelper.Info("未找转发器异常:" + ex.Message);


                                                                    //Func<bool> func1 = () => true;
                                                                    var msgx = "UDP:800未找转发器异常:" + ex.Message;
                                                                    LogHelper.LogFilter(true, ex.Message);
                                                                }
                                                                #endregion
                                                            }

                                                        }
                                                    }


                                                    #endregion

                                                }


                                            }


                                            #endregion
                                        }

                                    }

                                }
                                RedisHelper<string>.StoreOneKeyMilliseconds(msgMd5, "1", 500);

                            }



                           
                        }

                    }
                }
                catch (SocketException se)
                {
                    var _msg = "异常：UDP:752 " + se.ErrorCode + "     " + se.Message+"   "+se.StackTrace;

                    point = ReConnctUDP(server, point);
                  

                    //LogHelper.Info(_msg);
                    LogHelper.LogFilter(true, se.StackTrace);
                    return;
                }
                try
                {
                    point = ReConnctUDP(server, point);
                }
                catch (SocketException se)
                {
                    var _msg = "异常:UDP:769 " + se.ErrorCode + "     " + se.Message;
                    LogHelper.Info(_msg+"异常后是否绑定:"+server.IsBound);
                    point = ReConnctUDP(server, point);


                    LogHelper.LogFilter(true, se.StackTrace);

                }
            }
            try
            {
                ar.AsyncWaitHandle.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine("901"+ex.Message);
                LogHelper.LogFilter(true, ex.StackTrace);
            }
           


        }

        private EndPoint ReConnctUDP(Socket server, EndPoint point)
        {
            var next = new byte[1024];
            UdpData ud = new UdpData
            {
                socket = server,
                data = next
            };
            server.BeginReceiveFrom(next, 0, next.Length, SocketFlags.None, ref point, OnUdpRecieve, ud);
            return point;
        }
    }
}
