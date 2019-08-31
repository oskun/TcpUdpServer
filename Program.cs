using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace TcpUdpServer
{
    public class Program
    {

        public static Dictionary<string, MacTcpUdp> macRelation = new Dictionary<string, MacTcpUdp>();

        private static object locker = new object();


        /// <summary>
        /// 添加或更新设备
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="device"></param>
        public static void upgradeDevice(string mac, Socket device)
        {
            lock (locker)
            {
                if (!macRelation.ContainsKey(mac))
                {
                    macRelation.Add(mac, new MacTcpUdp()
                    {
                        device = device,
                        mac = mac
                    });
                }
                else
                {
                    var mu = macRelation[mac];
                    mu.device = device;
                }
            }
        }

        public static void upgradeClients(string mac, EndPoint client, Socket udpServer)
        {
            lock (locker)
            {
                if (!macRelation.ContainsKey(mac))
                {
                    var queue = new Queue<EndPointTime>();
                    queue.Enqueue(new EndPointTime()
                    {
                        point = client,
                        time = DateTime.Now

                    });
                    macRelation.Add(mac, new MacTcpUdp()
                    {
                        clients = queue,
                        mac = mac
                    });
                }
                else
                {
                    var mu = macRelation[mac];
                    if (mu.clients.Count < 10)
                    {
                        var db = new EndPointTime();
                        db.point = client;
                        db.time = DateTime.Now;
                        mu.clients.Enqueue(db);
                        mu.udpServer = udpServer;
                    }
                    else
                    {
                        mu.clients.Dequeue();
                        var db = new EndPointTime();
                        db.point = client;
                        db.time = DateTime.Now;
                        mu.clients.Enqueue(db);
                        mu.udpServer = udpServer;
                    }


                }
            }
        }


        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static MacTcpUdp GetRelation(string mac)
        {
            MacTcpUdp value;
            var bvalue = macRelation.TryGetValue(mac, out value);
            return value;
        }

        public static MacTcpUdp GetRelation(Socket device)
        {
            MacTcpUdp value = null;
            lock (locker)
            {
               
               
               
               

            }

            try
            {
                var target = (IPEndPoint)device.RemoteEndPoint;
                foreach (var key in macRelation.Keys)
                {
                    var item = macRelation[key];
                    if (item != null && item.device != null)
                    {
                        var d = item.device;
                        try
                        {
                            var ipEnd = (IPEndPoint)d.RemoteEndPoint;
                            if (ipEnd != null && target.Address.Equals(ipEnd.Address) && target.Port.Equals(ipEnd.Port))
                            {
                                value = macRelation[key];
                                value.mac = key;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            //180.155.69.150    486    异常：112无法访问已释放的对象。
                            if (d.Connected)
                            {
                                var _msg = "异常：112" + ex.Message + "是否链接" + d.Connected;
                                LogHelper.Info(_msg, device);
                                removeOnlineTcpRelation(d);
                            }
                            return null;

                        }


                    }

                }
                return value;
            }
            catch (Exception ex)
            {
                LogHelper.Info("PC:148" + ex.Message);
                return null;
            }








        }


        public static void removeOnlineTcpRelation(Socket device)
        {
            lock (locker)
            {
                try
                {

                    LogHelper.Info("移除设备的获取设备方法");
                    var mu = GetRelation(device);
                    if (mu != null && !string.IsNullOrEmpty(mu.mac))
                    {
                        macRelation.Remove(mu.mac);
                        var _rm = (IPEndPoint)device.RemoteEndPoint;
                        var _msg = "移除" + _rm.Address + " " + _rm.Port;
                        LogHelper.Info(_msg);
                        LogHelper.Info("未查询到mac");
                        if (device.Connected)
                        {


                        }
                        device.Shutdown(SocketShutdown.Both);
                        device.Disconnect(true);
                        device.Close();
                    }
                   

                }
                catch (Exception ex)
                {
                    //LogHelper.Loginfo.Info("移除设备时异常:"+ex.Message);
                    //Console.WriteLine("142" + ex.Message);



                    //                 at System.Net.Sockets.Socket.get_RemoteEndPoint()
                    //at TcpUdpServer.Program.GetRelation(Socket device) in C: \Users\json\Source\Repos\TcpUdpServer\Program.cs:line 111
                    //at TcpUdpServer.Program.removeOnlineTcpRelation(Socket device) in C: \Users\json\Source\Repos\TcpUdpServer\Program.cs:line 161

                    LogHelper.Info("异常:NO.156     " + ex.Message+"   "+ex.StackTrace, device);
                }


            }
        }





        const string ipaddress = "192.168.1.207";

        const int tcpPort = 4198;
        const int udpPort = 4530;
        /// const string ipaddress = "115.29.231.29";
        static void Main(string[] args)
        {
            try
            {
                TUdpServer udpServer = new TUdpServer();
                udpServer.UdpStart(ipaddress, udpPort);

                MTcpServer tcpServer = new MTcpServer();
                tcpServer.Start(ipaddress, tcpPort);
            }
            catch (Exception ex)
            {
                LogHelper.Info("program运行错误：" + ex.Message);
                Environment.Exit(0);
            }



            Console.Read();
        }
    }
}
