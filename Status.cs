using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class Status<T>
    {
        public string code { get; set; }


        public string msg { get; set; }


        public T data { get; set; }


        public string mac { get; set; }


        /// <summary>
        /// 命令类别
        /// </summary>
        public string commandType { get; set; }
    }
}
