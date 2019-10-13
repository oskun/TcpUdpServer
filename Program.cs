using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

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
                var ipEndPort = (IPEndPoint)device.RemoteEndPoint;
                if (!macRelation.ContainsKey(mac))
                {
                    macRelation.Add(mac, new MacTcpUdp()
                    {
                        device = device,
                        mac = mac,
                        Address = ipEndPort.Address.ToString(),
                        port = ipEndPort.Port
                    });
                }
                else
                {
                    var mu = macRelation[mac];
                    mu.device = device;
                    mu.mac = mac;
                    mu.Address = ipEndPort.Address.ToString();
                    mu.port = ipEndPort.Port;
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
                        time = DateTime.Now,
                        

                    });
                    macRelation.Add(mac, new MacTcpUdp()
                    {
                        clients = queue,
                        mac = mac,
                        udpServer=udpServer
                    });
                }
                else
                {
                    var mu = macRelation[mac];
                    var count = 1;
                    if (mu.clients.Count < count)
                    {
                        var db = new EndPointTime();
                        db.point = client;
                        db.time = DateTime.Now;
                        mu.clients.Enqueue(db);
                        mu.udpServer = udpServer;
                    }
                    else
                    {
                        while(mu.clients.Count>=count)
                        {
                            mu.clients.Dequeue();
                        }                    
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
            MacTcpUdp value=null;
            if (!string.IsNullOrEmpty(mac))
            {
                var bvalue = macRelation.TryGetValue(mac, out value);

            }
            return value;
        }






        public static MacTcpUdp GetRelation(string address, int port)
        {
            ///throw new Exception("haha");
          
            MacTcpUdp value = null;
            ///锁住macRelation
            lock (macRelation)
            {
                
                foreach (var key in macRelation.Keys)
                {
                    try
                    {
                        var item = macRelation[key];
                        if(item!=null&&!string.IsNullOrEmpty(item.Address)&&item.port>0)
                        {
                            if (item.Address.Equals(address) && item.port.Equals(port))
                            {
                                value = macRelation[key];
                                value.mac = key;
                                break;
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {

                        Func<bool> func = () => true;
                        LogHelper.LogFilter(func, ex.StackTrace+"    "+ex.Message);
                        return value;

                       
                    }
                   
                }
                return value;
            }
           
        }








        public static void removeOnlineTcpRelation(string address, int port)
        {
            var mu = GetRelation(address, port);
            if (mu != null && !string.IsNullOrEmpty(mu.mac))
            {
                try
                {
                    macRelation.Remove(mu.mac);
                    //var _rm = (IPEndPoint)device.RemoteEndPoint;
                    var _msg = "移除" + address + " " + port;
                    LogHelper.Info(_msg);

                }
                catch(Exception ex)
                {
                    LogHelper.LogFilter(true, ex.StackTrace);
                }
               

                try
                {
                    mu.device.Shutdown(SocketShutdown.Both);
                    mu.device.Disconnect(true);
                    mu.device.Close();
                }
                catch (SocketException se)
                {
                    LogHelper.LogFilter(true, se.StackTrace);
                }

            }
        }
        internal static Boolean IsPortOccupedFun2(params Int32[] ports)
        {
            Boolean result = false;
            try
            {
                System.Net.NetworkInformation.IPGlobalProperties iproperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
                System.Net.IPEndPoint[] ipEndPoints = iproperties.GetActiveTcpListeners();
                foreach (var item in ipEndPoints)
                {
                    foreach (var port in ports)
                    {
                        if (item.Port == port)
                        {
                            result = true;
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogFilter(true, ex.StackTrace);
            }
            return result;
        }




        ///const string ipaddress = "192.168.1.207";

        const int tcpPort = 4198;
        const int udpPort = 4530;
        //const string ipaddress = "118.25.68.161";

        //const string ipaddress = ConfigurationManager.AppSettings["ipaddress"].ToString();

        ///const string ipaddress = "115.29.231.29";



        ///const string ipaddress = "121.40.53.77";
        static void Main(string[] args)
        {

            ///端口正在被占用
            if (IsPortOccupedFun2(4198, 4530, 4531, 4532, 4533))
            {
                Environment.Exit(0);
            }
            else
            {
                string ipaddress = ConfigurationManager.AppSettings["ipaddress"].ToString();
                try
                {
                    TUdpServer udpServer = new TUdpServer();
                    udpServer.UdpStart(ipaddress, udpPort);

                    MTcpServer tcpServer = new MTcpServer();
                    tcpServer.Start(ipaddress, tcpPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //LogHelper.Info("program运行错误：" + ex.Message);
                    Environment.Exit(0);
                }
            }
            Console.Read();
        }
    }
}
