using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    /// <summary>
    /// 服务器端配置
    /// </summary>
   public class ServerConfig
    {
        /// <summary>
        /// 服务器tcp监听端口
        /// </summary>
        public int tcpPort { get;private set; }



        /// <summary>
        /// 服务器udp监听端口
        /// </summary>
        public int udpPort { get;private set; }




        /// <summary>
        /// 监听地址
        /// </summary>
        public string listenAddress { get;private set; }


        public ServerConfig(int tcpPort,int udpPort,string listenAddress)
        {

            this.tcpPort = tcpPort;
            this.udpPort = udpPort;
            this.listenAddress = listenAddress;
        }


        

    }
}
