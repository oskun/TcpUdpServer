using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TcpUdpServer
{


    /// <summary>
    /// 心跳包时间
    /// </summary>
   public class SocketTime
    {

        public Socket socket;


        /// <summary>
        /// 心跳包时间、命令时间
        /// </summary>
        public DateTime time { get; set; }
    }
}
