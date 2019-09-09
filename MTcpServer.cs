using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace TcpUdpServer
{
    public class MTcpServer
    {

        /// <summary>
        /// 心跳包存的时间
        /// </summary>
        static int heartbeatTime = 120;
        //static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        //public static Dictionary<string, SocketTime> socketMap = new Dictionary<string, SocketTime>();


        public delegate void onStart(object sender, EventArgs e);

        //public event onStart tcpStart;


        public void Start(string ipaddress, int tcpPort)
        {
            var config = new ServerConfig(tcpPort, 4530, ipaddress);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(config.listenAddress), config.tcpPort));
            server.Listen(20);
            server.BeginAccept(onTcpAccept, server);
        }

        /// <summary>
        /// 连接上时
        /// </summary>
        /// <param name="ar"></param>
        private void onTcpAccept(IAsyncResult ar)
        {
            var server = ar.AsyncState as Socket;

            var td = new TcpData();
            var pox = server.EndAccept(ar);
            //var _rm =(IPEndPoint) pox.RemoteEndPoint;
            //var _msg = "tcp连接上" + (_rm.Address) + " " + _rm.Port;
            //LogHelper.Info(_msg);


            td.socket = pox;
            try
            {
                pox.BeginReceive(td.bs, 0, td.bs.Length, SocketFlags.None, onTcpReceive, td);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            server.BeginAccept(onTcpAccept, server);

        }


       


        private void onTcpReceive(IAsyncResult ar)
        {
            var td = ar.AsyncState as TcpData;
            if (td != null)
            {
                var socket = td.socket;

                try
                {
                    ///客户端断开连接时，会引发一个异常
                    var len = socket.EndReceive(ar);
                    if (len == 0)
                    {
                        ///主动告知
                        td = null;
                        var _d = (IPEndPoint)socket.RemoteEndPoint;
                        Console.WriteLine("设备断开" + _d.Address + ":" + _d.Port);
                        Program.removeOnlineTcpRelation(_d.Address.ToString(),_d.Port);
                        ar = null;
                    }
                    else
                    {

                        var bys = td.bs.ToList().GetRange(0, len).ToArray();
                        var Info = MsgTypeHelper.GetHearBeatInfo(bys);
                        //Task.Factory.StartNew(() =>
                        //{
                           


                        //});
                        if (Info != null&&socket!=null)
                        {

                            try
                            {
                                var heartBeatType = Info.heartBeatType;
                                //忆林
                                if (heartBeatType == HeartBeatType.YILIN)
                                {
                                    YiLinMethod(socket, Info, bys);
                                }
                                ///汉枫
                                else if (heartBeatType == HeartBeatType.HANFEN)
                                {
                                    HanFengMethod(socket, Info, bys);
                                }
                                ///校验时间
                                else if (heartBeatType == HeartBeatType.CHECK_TIME)
                                {
                                    checkTimeMethod(socket, Info, bys);
                                }
                                ///报警
                                else if (heartBeatType == HeartBeatType.BAOJING)
                                {
                                    BaoJinMethod(socket, Info, bys);
                                }
                                ///命令返回
                                else if (heartBeatType == HeartBeatType.DEFAULT)
                                {
                                    defualtMethod(socket, Info, bys);

                                }
                            }
                            catch (Exception ex)
                            {
                                // throw ex;
                                LogHelper.Info("tcp:168" + ex.Message);
                            }

                        }



                        try
                        {
                            if (socket.Connected)
                            {
                                socket.BeginReceive(td.bs, 0, td.bs.Length, SocketFlags.None, onTcpReceive, td);
                            }
                            else
                            {
                                LogHelper.Info("TCP 179尝试设备已经断开");
                                var ipe = (IPEndPoint)socket.RemoteEndPoint;
                                var address = ipe.Address.ToString();
                                var port = ipe.Port;
                                Program.removeOnlineTcpRelation(address,port);
                                ar.AsyncWaitHandle.Close();
                                LogHelper.Info("设备已经断开");
                            }
                        }
                        catch (Exception ex)
                        {
                            var ipe = (IPEndPoint)socket.RemoteEndPoint;
                            var address = ipe.Address.ToString();
                            var port = ipe.Port;
                            Program.removeOnlineTcpRelation(address, port);
                            Console.WriteLine("tcp 130" + ex.Message);
                            LogHelper.Info("设备已经断开");
                            ar = null;
                        }
                    }
                }
                catch (SocketException ex)
                {

                    Console.WriteLine("tcp:138    errorCode:" + ex.ErrorCode);
                    //StackTrace stack = new StackTrace();
                    //var colNumber = stack.ToString();
                    //var ep = (IPEndPoint)socket.RemoteEndPoint;
                    //Console.WriteLine("_tcp:138" + ep.Address + "  " + ep.Port + colNumber);
                    var ipe = (IPEndPoint)socket.RemoteEndPoint;
                    var address = ipe.Address.ToString();
                    var port = ipe.Port;
                    Program.removeOnlineTcpRelation(address, port);
                    ar = null;
                }



            }

        }



        /// <summary>
        /// 命令返回
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="info"></param>
        /// <param name="bys"></param>
        private void defualtMethod(Socket socket, MacIPVersionInfo info, byte[] ds)
        {
            //var mac = info.mac;
            //if (socketMap.ContainsKey(mac))
            //{

            //}
            //else
            //{

            //}

            var ipe = (IPEndPoint)socket.RemoteEndPoint;
            var address = ipe.Address.ToString();
            var port = ipe.Port;
            var mu = Program.GetRelation(address,port);
            if (mu != null)
            {

                #region 长度小于5时等待接下来的数据
                if (ds.Length < 5)
                {
                    var orgds = ds;
                    var tmp = new byte[1024];
                    //Console.WriteLine("原先ds长度:" + ds.Length);
                    ds = new byte[1024];
                    var thread = new Thread(() =>
                    {

                        try
                        {
                            var len = socket.Receive(tmp, SocketFlags.None);
                            if (len > 0)
                            {
                                var qu = new Queue<byte>();

                                if (orgds.Length == 3 && ((orgds[0] == 85 && orgds[1] == 170 && orgds[2] == 255) || (orgds[0] == 85 && orgds[1] == 170 && orgds[2] == 0)))
                                {
                                    ///去掉前三个字节
                                }
                                else
                                {
                                    foreach (var q in orgds)
                                    {
                                        qu.Enqueue(q);
                                    }

                                }

                                for (var i = 0; i < len; i++)
                                {
                                    qu.Enqueue(tmp[i]);
                                }
                                var index = 0;
                                while (qu.Count > 0)
                                {
                                    var q = qu.Dequeue();
                                    if (index < 1023)
                                    {
                                        ds[index++] = q;
                                    }

                                }
                                ds = ds.ToList().GetRange(0, index).ToArray();
                                SendMsgToClient(ds, mu);

                            }
                            //var hex = StrHelper.GetHexStr(ds);
                            //Console.WriteLine("后来ds长度:" + ds.Length + "字符串：" + hex);
                            else if (len == 0)
                            {


                                Func<bool> func1 = () => true;
                                var msgx ="再次获取的时候居然断开了。。。。";
                                LogHelper.LogFilter(func1, msgx);
                             
                                Program.removeOnlineTcpRelation(address,port);


                            }
                        }
                        catch (SocketException sek)
                        {
                            Console.WriteLine(sek.Message);
                            Program.removeOnlineTcpRelation(address,port);
                        }


                    });
                    thread.Start();
                    thread.Join(150);



                }
                #endregion

                else
                {
                    SendMsgToClient(ds, mu);
                }



            }



        }

        private static void SendMsgToClient(byte[] ds, MacTcpUdp mu)
        {
            var rdata = StrHelper.GetHexStr(ds);
            var mac = mu.mac;
            if (!string.IsNullOrEmpty(mac))
            {

            }
            else
            {
                mac = string.Empty;
            }
            var bdata = Encoding.UTF8.GetBytes(JsonHelper<Status<string>>.GetJson(new Status<string>() { code = "1", data = rdata, mac = mac, msg = "", commandType = "1" }));
            var queue = mu.clients;
            ///能否正常发通UDP
            var flag = true;
            while (queue != null && queue.Count > 0)
            {
                var el = queue.Dequeue();
                if (el != null && !(el.time.AddSeconds(10) < DateTime.Now))
                {
                    try
                    {
                        if (mu.udpServer != null)
                        {

                            LogHelper.Info("设备mac：" + mac + " 数据" + rdata);
                            var iet = (IPEndPoint)el.point;
                            if (!iet.Address.Equals(IPAddress.Any))
                            {
                                try
                                {
                                    if (flag)
                                    {
                                        mu.udpServer.SendTo(bdata, el.point);
                                    }

                                }
                                catch (SocketException ex)
                                {
                                    flag = false;
                                    queue.Clear();
                                    LogHelper.Info("TCP:378:   " + ex.Message);
                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("TCP:395" + ex.Message);


                        Func<bool> func1 = () => true;
                        var msgx = "TCP:395" + ex.Message;
                        LogHelper.LogFilter(func1, msgx);
                    }

                }
            }
        }



        /// <summary>
        /// 报警方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="info"></param>
        /// <param name="bys"></param>
        private void BaoJinMethod(Socket socket, MacIPVersionInfo info, byte[] ds)
        {
            if (ds.Length == 11)
            {
                var id = ds.ToList().GetRange(4, 6).ToArray();
                var hexIds = StrHelper.byteToHexStr(id);
                var id10 = Convert.ToInt32(hexIds, 16);


                var sta = DALDefenseStatus.DefenseStatusGetByDeviceId(id10);
                if ((sta == null) || (sta != null && sta.status == 1))
                {
                    ///警报事件
                    if (ds[3] == 4)
                    {
                        if (id10 > 0)
                        {

                            var device = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_GetDataByRecordid(id10 + "");
                            if (device != null)
                            {
                                var device_mac = device.device_mac;

                                DALNotifyException.NotifyExceptionAddX(new NotifyException()
                                {
                                    bxkLabel = device.bxk_label,
                                    deviceId = device.deviceinfo_userinfo_id + "",
                                    deviceSetName = device.device_setname,
                                    recordTime = DateTime.Now,
                                    uid = device.uid,
                                    alarm_type = 0


                                });

                                var tokens = DALYZKPushToken.YZKPushToken_GetByUid(device.uid).FindAll(a => a.devicetype != 3);
                                foreach (var token in tokens)
                                {

                                    if (token != null)
                                    {
                                        if (token.devicetype == 0)
                                        {
                                            var devicetoken = token.devicetoken;
                                            if (!string.IsNullOrEmpty(devicetoken))
                                            {
                                                var alert = "";
                                                var category = "";
                                                var ticker = "";
                                                var title = "设备报警通知";
                                                var text = string.Format("报警通知,您房间{0}的{1}设备发生报警,请留意", device.bxk_label, device.device_setname);
                                                var dt = new Dictionary<string, string>
                                                {
                                                    { "type", "8" },
                                                    { "uid", device.uid }
                                                };
                                                var pushData = YouMengHelper.makePush(alert, category, DeviceType.Android_Center, devicetoken, ticker, title, text, dt);


                                            }

                                            //DALNotifyException.NotifyExceptionGet(d.deviceinfo_userinfo_id + "", (int)AlarmType.DEFAULT);

                                        }
                                        else
                                        {
                                            var devicetoken = token.devicetoken;
                                            if (!string.IsNullOrEmpty(devicetoken))
                                            {
                                                var alert = "";
                                                var category = "";
                                                var ticker = "";
                                                var title = "设备报警通知";
                                                var text = string.Format("报警通知,您房间{0}的{1}设备发生报警,请留意", device.bxk_label, device.device_setname);
                                                var dt = new Dictionary<string, string>();
                                                dt.Add("type", "8");
                                                dt.Add("uid", device.uid);
                                                var pushData = YouMengHelper.makePush(alert, category, DeviceType.YZK_IOS_APPStore, devicetoken, ticker, title, text, dt);


                                            }


                                        }


                                    }




                                }





                            }


                        }
                    }
                    ///表明是需要短信通知的报警事件
                    else if (ds[3] == 5)
                    {
                        #region 短信通知报警设置
                        ///4-9
                        ///id

                        if (id10 > 0)
                        {
                            var device = DALYZKDeviceInfo_UserInfocs.YZKDeviceInfo_UserInfo_GetDataByRecordid(id10 + "");
                            if (device != null)
                            {
                                var room = device.room_id;
                                var home_id = device.home_id;
                                var roomName = string.Empty;
                                var uid = device.uid;
                                if (!string.IsNullOrEmpty(uid))
                                {
                                    var users = DALYZKUser.YZKUser_GetByuuid(uid);
                                    if (users != null)
                                    {
                                        var device_setName = device.device_setname;
                                        var bxk_label = device.bxk_label.Replace("-", "");
                                        //bxk_label就是xxx房间， 去掉bxk_label中的“_”, xxx设备是setname字段
                                        //var msg = string.Format("报警通知,您{0}房间的{1}设备发生报警事件,请留意",bxk_label,device_setName);
                                        var mobile = users.mobile;
                                        var dfname = DALDefenseStatus.DefenseStatus_GetByHomeId(home_id);
                                        if (dfname != null && dfname.status == 1)
                                        {
                                            YZKMsgHelper.NotifyDanger(mobile, bxk_label, device_setName);
                                            ///记录异常信息
                                            DALNotifyException.NotifyExceptionAdd(new NotifyException()
                                            {
                                                bxkLabel = bxk_label,
                                                deviceSetName = device_setName,
                                                uid = uid,
                                                deviceId = device.deviceinfo_userinfo_id + ""

                                            });
                                        }


                                    }


                                }
                            }
                        }

                        #endregion


                    }
                }





            }
        }


        /// <summary>
        /// 校验时间的方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="info"></param>
        /// <param name="bys"></param>

        private void checkTimeMethod(Socket socket, MacIPVersionInfo info, byte[] bys)
        {

            //Task.Factory.StartNew(() =>
            //{



            //});


            var time = TianMao.GetCRC("55ff0e" + StrHelper.GetCheckTimeStr());
            var check_time_msg = StrHelper.strToHexByte(time);
            try
            {
                socket.Send(check_time_msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("tcp 462" + ex.Message);
                var ipe = (IPEndPoint)socket.RemoteEndPoint;
                var address = ipe.Address.ToString();
                var port = ipe.Port;
                Program.removeOnlineTcpRelation(address, port);


                Func<bool> func1 = () => true;
                var msgx = "tcp 462" + ex.Message;
                LogHelper.LogFilter(func1, msgx);
            }

        }


        /// <summary>
        /// 汉枫
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="info"></param>
        /// <param name="bys"></param>
        private void HanFengMethod(Socket socket, MacIPVersionInfo info, byte[] bys)
        {
            var ipEnd = (IPEndPoint)socket.RemoteEndPoint;
            var mac = info.mac;
            if (!string.IsNullOrEmpty(mac))
            {
                Program.upgradeDevice(mac, socket);
            }

            var heartBeatKey = "HeartBeat:" + ipEnd.Address.ToString() + "" + ipEnd.Port;
            if (RedisHelper<string>.IsKeyExist(heartBeatKey) == false)
            {

                var task = new TaskFactory();
                task.StartNew(() =>
                {
                    var ip = info.IP;
                    var orgText = StrHelper.GetHexStr(bys);
                    DALMacIPInfo.MacIPInfo_Add(new MacIPInfo()
                    {
                        mac = info.mac,
                        IP = ipEnd.Address.ToString(),
                        localip = ip,
                        ssid = "",
                        OrgText = orgText
                    });

                });
                RedisHelper<string>.StoreOneKey(heartBeatKey, "1", heartbeatTime);

            }


        }



        /// <summary>
        /// 忆林的
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="info"></param>
        /// <param name="bys"></param>
        private void YiLinMethod(Socket pox, MacIPVersionInfo info, byte[] bys)
        {
            var mac = info.mac;

            if (!string.IsNullOrEmpty(mac))
            {
                Program.upgradeDevice(mac, pox);
            }
            //LogWriteLock.EnterWriteLock();
            //var st = new SocketTime();
            //st.socket = pox;
            //st.time = DateTime.Now;
            //socketMap[mac] = st;          
            //LogWriteLock.ExitWriteLock();
            var ipEnd = (IPEndPoint)pox.RemoteEndPoint;
            var heartBeatKey = "HeartBeat:" + ipEnd.Address.ToString() + "" + ipEnd.Port;


            var key = "Socket.Tcp.SetTime." + mac;
            #region 设置时间
            if (!RedisHelper<string>.IsKeyExist(key))
            {
             
                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        ///60*60*24
                        RedisHelper<string>.StoreOneKey(key, "1", 86400);
                        var set_time_str = "55ff00010091010110a1000a0461" + StrHelper.BackTime();
                        var set_time_bytes = CRC16.CRC_XModem(set_time_str);
                        pox.Send(set_time_bytes);
                        Thread.Sleep(50);

                    }
                    catch(SocketException se)
                    {
                       


                        Func<bool> func1 = () => true;
                        var msgx = "TCP:670:" + se.ErrorCode + " " + se.Message;
                        LogHelper.LogFilter(func1, msgx);
                    }


                });


               

            }
            #endregion

            #region 设置心跳包
            if (RedisHelper<string>.IsKeyExist(heartBeatKey) == false)
            {
                var task = new TaskFactory();
                task.StartNew(() =>
                {
                    var orgText = StrHelper.GetHexStr(bys);
                    DALMacIPInfo.MacIPInfo_Add(new MacIPInfo()
                    {
                        mac = mac,
                        IP = ipEnd.Address.ToString(),
                        localip = "",
                        ssid = "",
                        OrgText = orgText
                    });

                });
                ///缓存2分钟
                RedisHelper<string>.StoreOneKey(heartBeatKey, "1", heartbeatTime);

            }
            else
            {

            }
            #endregion


        }










    }
}
