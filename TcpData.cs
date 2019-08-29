using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpUdpServer
{
  public  class TcpData
    {
        public byte[] bs = new byte[1024];



        public Socket socket { get; set; }


    }
}
