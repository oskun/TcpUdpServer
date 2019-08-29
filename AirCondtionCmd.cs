using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class AirCondtionCmd
    {

        /// <summary>
        /// 设备id
        /// </summary>
        public string deviceId { get; set; }



        /// <summary>
        /// 空调命令
        /// </summary>
        public string cmd { get; set; }



        /// <summary>
        /// 记录id
        /// </summary>
        public int recordId { get; set; }



        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime recordTime { get; set; }


    }
}
