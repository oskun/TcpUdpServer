using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class IOSPayLoad
    {
        public APS aps { get; set; }

        public Dictionary<string, string> dicData { get; set; }

        public IOSPayLoad(string title, string body, string subtitle, string category)
        {
            aps = new APS(category, title, body, subtitle);
        }



        public IOSPayLoad(string title, string body, string subtitle, string category, Dictionary<string, string> extra)
        {
            aps = new APS(category, title, body, subtitle);
            this.extra = extra;
            dicData = extra;


        }


        public Dictionary<string, string> extra = new Dictionary<string, string>();
        /*
        {"aps":{"sound":"default","badge":1,"alert":"dfadfadf","category":"dsfadf"}}

            */







    }
}
