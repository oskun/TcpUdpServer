using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class IOS_AppStore : IPush
    {


        public Policy policy { get; set; }
        /// <summary>
        /// 格林威治时间
        ///是一种时间表示方法，定义为从格林威治时间1970年01月01日00时00分00秒起至现在的总秒数。
        /// </summary>
        /// <returns></returns>
        public static long JavaTime()
        {
            var time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return time;
        }


        /// <summary>
        /// 必填 应用唯一标识
        /// </summary>
        public string appkey { get; set; }




        public IOS_AppStore()
        {
            this.appkey = "582955d5aed17911850027ff";
            timestamp = JavaTime() + "";
            type = "listcast";
        }


        /// <summary>
        ///  必填 时间戳，10位或者13位均可，时间戳有效期为10分钟
        /// </summary>
        public string timestamp { get; set; }


        /// <summary>
        ///  必填 消息发送类型,其值可以为:
        ///   unicast-单播
        ///listcast-列播(要求不超过500个device_token)
        /// filecast-文件播
        ///(多个device_token可通过文件形式批量发送）
        ///broadcast-广播
        ///groupcast-组播
        ///(按照filter条件筛选特定用户群, 具体请参照filter参数)
        ///customizedcast(通过开发者自有的alias进行推送),
        ///包括以下两种case:
        ///- alias: 对单个或者多个alias进行推送
        ///- file_id: 将alias存放到文件后，根据file_id来推送
        /// </summary>
        public string type { get; set; }



        /// <summary>
        /// 可选 设备唯一表示
        /// 当type=unicast时,必填, 表示指定的单个设备
        ///  当type=listcast时,必填,要求不超过500个,
        ///   多个device_token以英文逗号间隔
        /// </summary>

        public string device_tokens { get; set; }


        /// <summary>
        /// 可选 当type=customizedcast时，必填，alias的类型,
        ///  alias_type可由开发者自定义,开发者在SDK中
        ///  调用setAlias(alias, alias_type)时所设置的alias_type
        /// </summary>
       // public string alias_type { get; set; }



        /// <summary>
        /// 可选 当type=customizedcast时, 开发者填写自己的alias。
        /// 要求不超过50个alias,多个alias以英文逗号间隔。
        ///   在SDK中调用setAlias(alias, alias_type)时所设置的alias
        /// </summary>
      //  public string alias { get; set; }


        /// <summary>
        /// 可选 当type=filecast时，file内容为多条device_token,
        ///  device_token以回车符分隔
        ///   当type=customizedcast时，file内容为多条alias，
        ///    alias以回车符分隔，注意同一个文件内的alias所对应
        ///     的alias_type必须和接口参数alias_type一致。
        ///      注意，使用文件播前需要先调用文件上传接口获取file_id,
        ///       具体请参照"2.4文件上传接口"
        /// </summary>
       // public string file_id { get; set; }


        /// <summary>
        /// 可选 终端用户筛选条件,如用户标签、地域、应用版本以及渠道等,
        ///   具体请参考附录G。
        /// </summary>
       // public Filter filter { get; set; }




        public IOSPayLoad payload { get; set; }







        /// <summary>
        /// 可选 正式/测试模式。测试模式下，广播/组播只会将消息发给测试设
        /// </summary>
        public string production_mode { get; set; }


        /// <summary>
        /// 可选 发送消息描述，建议填写。
        /// </summary>
        public string description { get; set; }


        /// <summary>
        /// 可选 开发者自定义消息标识ID, 开发者可以为同一批发送的消息提供
        /// </summary>
      //  public string thirdparty_id { get; set; }



    }
}
