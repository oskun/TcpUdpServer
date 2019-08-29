using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class ClientCommand
    {

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// mac地址
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string command { get; set; }
        /// <summary>
        /// 对应uuid
        /// </summary>
        public string uid { get; set; }


        /// <summary>
        ///和云端通信的时候，增加一个字段app
        //云端收到指令，先检查是否有app字段，如果没有，不验证设备是否存在，
        //否则，如果app=YZK，
        //查询易指控的设备表，如果是HDL查询海底捞的设备表
        /// </summary>
        public string app { get; set; }
        /// <summary>
        /// 消息类型
        /// 3-天猫精灵发过来的消息
        /// 4-报警
        /// 5-布放撤防
        /// </summary>
        public int commandType { get; set; }


        /*
         * {"mac":"f0fe6bccd712","command":"TurnOn","commandType":3,"zone":"一楼客厅"}
         * 
         */
        /// <summary>
        /// 位置
        /// </summary>
        public string zone { get; set; }



        /// <summary>
        /// 类型
        /// </summary>
        public string model { get; set; }


        public string Id { get; set; }



        public int repeatCount { get; set; }



        /// <summary>
        /// 协议类别
        /// </summary>
        public int protocol_type { get; set; }


        public int sid { get; set; }


        public string value { get; set; }
    }
}
