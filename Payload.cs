using System.Collections.Generic;

namespace TcpUdpServer
{
     public class Payload
    {
        /// <summary>
        /// 必填 消息类型，值可以为:
        ///   notification-通知，message-消息
        /// </summary>
        public string display_type { get; set; }

        /// <summary>
        /// 必填 消息体。
        ///  display_type=message时,body的内容只需填写custom字段。
        ///   display_type=notification时, body包含如下参数:
        /// </summary>
        public Body body { get; set; }


        /// <summary>
        /// 必填 消息体。
        ///  display_type=message时,body的内容只需填写custom字段。
        ///   display_type=notification时, body包含如下参数:
        ///    必填 通知栏提示文字
        ///    必填 通知标题
        ///    必填 通知文字描述
        /// </summary>
        public Payload(string display_type, string ticker, string title, string text, Dictionary<string, string> extra, string customer_josn = "")
        {
            this.display_type = display_type;
            this.body = new Body(ticker, title, text);
            if (!string.IsNullOrEmpty(customer_josn))
            {
                body.custom = customer_josn;
            }
            this.extra = extra;

        }




        public Dictionary<string, string> extra = new Dictionary<string, string>();


    }
}