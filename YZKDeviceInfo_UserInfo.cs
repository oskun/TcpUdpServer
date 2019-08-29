using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class YZKDeviceInfo_UserInfo
    {


        public string protocol_type { get; set; }
        /// <summary>
        /// 外键
        /// 设备id
        /// </summary>
        public string deviceid { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string device_type { get; set; }

        /// <summary>
        /// 设备搜索码, 比如HF-A11ASSISTHREAD
        /// </summary>

        public string device_scan_code { get; set; }

        /// <summary>
        /// 设备搜索ssid, 比如HF-LPB100
        /// </summary>
        public string device_scan_ssid { get; set; }

        /// <summary>
        /// modbus地址
        /// </summary>
        public string modbus_address { get; set; }

        /// <summary>
        /// modbus协议里的机号，如A0
        /// </summary>
        public string modbus_machineno { get; set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public string device_buy_price { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// 保修卡编号
        /// </summary>
        public string bxk_no { get; set; }

        /// <summary>
        /// 保修卡标签
        /// </summary>
        public string bxk_label { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string room_id { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contact_name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string contact_phone { get; set; }

        /// <summary>
        /// 是否是默认设备
        /// </summary>
        public bool is_default { get; set; }

        /// <summary>
        /// 记录id
        /// </summary>
        public int deviceinfo_userinfo_id { get; set; }

        /// <summary>
        /// 从设备里读取
        /// </summary>
        /*从设备里读取*/
        public int device_protocol_category { get; set; }

        /// <summary>
        /// 从设备里读取
        /// </summary>
        //public YZKDeviceBaseData device { get; set; }

        /*从设备里读取*/

        /// <summary>
        /// 从可用设备里读取
        /// </summary>
        public string device_config_guild { get; set; }

        /// <summary>
        /// 局域网ip
        /// </summary>
        public string localip { get; set; }

        /// <summary>
        /// ssid
        /// </summary>
        public string ssid { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string user_address { get; set; }

        /// <summary>
        /// 用户发票
        /// </summary>
        public string user_fapiao { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        public string buy_date { get; set; }

        /// <summary>
        /// 卖家手机号
        /// </summary>
        public string seller_mobile { get; set; }

        public int device_protocol { get; set; }

        /// <summary>
        /// 设备名
        /// </summary>
        public string device_name { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string dealer { get; set; }

        /// <summary>
        /// 销售区域
        /// </summary>
        public string sales_area { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        public string producing_area { get; set; }

        /// <summary>
        /// 公司网址
        /// </summary>
        public string company_url { get; set; }

        public string device_sn { get; set; }

        public string device_producing_date { get; set; }

        /// <summary>
        /// 设备开箱安装
        /// </summary>
        public string device_install_guild { get; set; }

        /// <summary>
        /// 设备使用说明
        /// </summary>
        public string device_usage_guild { get; set; }

        public string device_vender_kefu { get; set; }

        /// <summary>
        /// 设置名称
        /// </summary>
        public string device_setname { get; set; }

        /// <summary>
        /// 设备mac
        /// </summary>
        public string device_mac { get; set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public string buy_price { get; set; }

        /// <summary>
        /// 来自分享人的
        /// </summary>
        public string ShareFrom { get; set; }

        /// <summary>
        /// 来自分享的用户设备id
        /// </summary>
        public string shareFromRecordid { get; set; }

        /// <summary>
        /// 家id
        /// </summary>
        public string home_id { get; set; }

        public long time_start { get; set; }

        public long time_end { get; set; }

        /// <summary>
        /// 产品机编码
        /// </summary>
        public string product_machine_coding { get; set; }

        /// <summary>
        /// 关联主机id
        /// </summary>
        public string device_host_id { get; set; }



        /// <summary>
        /// 主机是否联动
        /// 默认是1
        /// </summary>
        public bool device_host_linkage { get; set; }

        /// <summary>
        /// 关联的额外数据
        /// </summary>

        public string device_host_linkage_extra_data { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string device_image { get; set; }

        /// <summary>
        /// 主机 温度 权限
        /// </summary>
        public string host_temp_curve { get; set; }

        /// <summary>
        ///设备地址对于RF433设备
        /// </summary>
        public string device_RFAddress { get; set; }

        /// <summary>
        /// 压缩机数量
        /// </summary>
        public string compressor_number { get; set; }

        /// <summary>
        /// 回路数量
        /// </summary>
        public string loop_number { get; set; }

        /// <summary>
        /// 主机类型
        /// </summary>
        public string mainframe_type { get; set; }

        /// <summary>
        /// 主机系列
        /// </summary>
        public string mainframe_series { get; set; }

        /// <summary>
        /// 按键映射
        /// </summary>
        public string keymap { get; set; }

        /// <summary>
        /// 变比
        /// </summary>
        public string bianbi { get; set; }





        /// <summary>
        /// 重要图片
        /// </summary>
        public string device_importPicture { get; set; }



        /// <summary>
        /// 遥控字符串
        /// </summary>
        public string ykmessageStr { get; set; }




        /// <summary>
        /// 是否是学习码
        /// </summary>
        public bool isStudy { get; set; }



        /// <summary>
        /// 分类名称
        /// </summary>
        public string category_name { get; set; }



        /// <summary>
        /// 遥控器蓝牙地址
        /// </summary>
        public string iclick_bt_mac { get; set; }





        public string device_pinpai { get; set; }






        /// <summary>
        /// 设备图标
        /// </summary>
        public string device_icon { get; set; }




        /// <summary>
        /// 协议索引
        /// </summary>
        public string protocol_index { get; set; }




        /// <summary>
        /// 可添加设备表
        /// </summary>
        public string AviableDeviceId { get; set; }



        ///// <summary>
        ///// 读取串口
        ///// </summary>
        //public int uart_port { get; set; }
    }
}
