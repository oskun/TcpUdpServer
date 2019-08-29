using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpUdpServer
{
    public class UdpServerPoint
    {

        /// <summary>
        /// 客户端节点
        /// </summary>
        public IPEndPoint point;


        /// <summary>
        /// udp服务端
        /// </summary>
        public Socket uServer;


    }
}
