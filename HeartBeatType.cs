using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    /// <summary>
    /// 心跳包类型
    /// </summary>
    public enum HeartBeatType
    {
        DEFAULT = -1,
        /// <summary>
        /// 忆林
        /// </summary>
        YILIN = 1,
        /// <summary>
        /// 汉枫
        /// </summary>
        HANFEN = 2,


        /// <summary>
        /// 单相电表消息返回
        /// </summary>
        ElectricityMeters = 3,

        /// <summary>
        /// 命令
        /// </summary>
        CMD = 4,


        /// <summary>
        /// 布防撤防
        /// </summary>
        BUFANGCHEFANG = 5,



        /// <summary>
        /// 校对时间
        /// </summary>
        CHECK_TIME = 6,



        /// <summary>
        /// 报警
        /// </summary>
        BAOJING = 7,


       
        /// <summary>
        /// 暖通设备控制
        /// </summary>
        NUANTONG = 8,



        /// <summary>
        /// 设备重启
        /// </summary>
        DEVICE_RESTART=9


    }
}
