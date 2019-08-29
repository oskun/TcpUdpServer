using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public enum ExtraType
    {
        /// <summary>
        /// 通用的消息
        /// </summary>
        CommonPush = 0,


        /// <summary>
        /// 订单消息
        /// 添加orderno
        /// </summary>
        OrderPush = 1,


        /// <summary>
        /// 推广活动连接
        /// 增加url 字段，点击打开url 网页
        /// </summary>
        ADPush = 2,




        /// <summary>
        /// 求助通知
        /// 增加conver_img 铭牌照片 component_img 零件照片， 点击消息可选择查看铭牌照片  零件照片
        /// </summary>
        ASKHelp = 3,



        /// <summary>
        /// 记录id
        ///
        /// </summary>
        Recordid = 4,



        /// <summary>
        /// 中控的推送
        /// 3个参数
        /// sync_notify_type
        /// record_id
        /// action
        /// </summary>
        CenterNotify_3 = 5,


        /// <summary>
        /// 设备上线
        /// </summary>
        CenterNotify_online_4 = 6


    }
}
