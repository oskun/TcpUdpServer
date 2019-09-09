using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TcpUdpServer
{
   public class MacTcpUdp
    {

        /// <summary>
        /// 设备mac
        /// </summary>
        public string mac { get; set; }




        /// <summary>
        /// 设备
        /// </summary>
        public Socket device { get; set; }




        /// <summary>
        /// 发送命令的客户端
        /// </summary>
        public Queue<EndPointTime> clients = new Queue<EndPointTime>();




        /// <summary>
        /// udpServer
        /// </summary>
        public Socket udpServer { get; set; }




        /// <summary>
        /// IP地址
        /// </summary>
        public string Address { get; set; }



        /// <summary>
        /// 端口号
        /// </summary>
        public int port { get; set; }


    }
}
