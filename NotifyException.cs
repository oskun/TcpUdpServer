using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class NotifyException
    {

        /// <summary>
        /// uid
        /// </summary>
        public string uid { get; set; }


        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime recordTime { get; set; }


        /// <summary>
        /// 保修卡标签
        /// </summary>
        public string bxkLabel { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        public string deviceSetName { get; set; }


        /// <summary>
        /// 设备id
        /// </summary>
        public string deviceId { get; set; }



        /// <summary>
        /// 记录时间
        /// </summary>
        public int recordId { get; set; }



        public int alarm_type { get; set; }
    }
}
