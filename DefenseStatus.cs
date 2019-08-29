using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DefenseStatus
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public int recordId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }


        /// <summary>
        /// 记录
        /// </summary>
        public DateTime recordTime { get; set; }


        /// <summary>
        /// 家id
        /// </summary>
        public string homeId { get; set; }


    }
}
