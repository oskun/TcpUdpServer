using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class MYKMessage
    {
        public string key_name { get; set; }


        public int key_index { get; set; }


        public int key_index_source { get; set; }


        public List<YKValue> key_value { get; set; }

    }
}
