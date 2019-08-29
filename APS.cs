using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class APS
    {

        //{"aps":{"sound":"default","badge":1,"alert":"dfadfadf","category":"dsfadf"}}


        public string sound = "default";

        public int badge = 1;


        public Alert alert { get; set; }





        public string category { get; set; }



        public APS(string category, string title, string body, string subtitle)
        {
            this.alert = new Alert() { body = body, title = title, subtitle = subtitle };
            this.category = category;
        }



    }
}
