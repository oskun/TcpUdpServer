using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class MacIPInfo
    {
        public string mac { get; set; }

        public string IP { get; set; }

        public string localip { get; set; }

        public int recordid { get; set; }

        public string ssid { get; set; }

        /// <summary>
        /// 原始字符串
        /// </summary>
        public string OrgText { get; set; }
    }
}
