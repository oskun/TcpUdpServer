using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{

    public class MacIPVersionInfo
    {
        /// <summary>
        /// mac 地址
        /// </summary>
        public string mac { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 心跳包类别
        /// </summary>
        public HeartBeatType heartBeatType { get; set; }


    }
}
