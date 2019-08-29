using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public enum COMMANDTYPE
    {

        /// <summary>
        /// 一般消息
        /// </summary>
        COM = 1,
        /// <summary>
        /// 天猫精灵
        /// </summary>
        TMJL = 3,


        /// <summary>
        /// 报警
        /// </summary>
        BAOJIN = 4,

        /// <summary>
        /// 部防撤防
        /// </summary>

        BUFANGCHEFANG = 5

    }
}
