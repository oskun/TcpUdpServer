using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.ServiceInterface.ServiceModel;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace TcpUdpServer
{
    public class YZKMsgHelper
    {
        const string appkey = "23763857";
        const string secret = "84f046c6a6b86a8d0ad38f878fb6514a";

        const string url = "http://gw.api.taobao.com/router/rest";
        public static string AliSendMsg(string mobile, string randcode, string modelid, bool israndcode = true)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);

            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "测试";


            if (!israndcode)
            {
                req.SmsParam = "{name:'" + randcode + "'}";
            }
            else
            {
                req.SmsParam = "{number:'" + randcode + "'}";
            }
            req.RecNum = mobile;
            req.SmsTemplateCode = modelid;

            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            var body = rsp.Body;
            return body;
        }


        /// <summary>
        /// 通知预警
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="room"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static string NotifyDanger(string mobile, string room, string deviceName)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);

            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "易指控";

            /*
             * ${name}向您发起了一笔交易请求，您的初始密码是${password}
             */

            //req.SmsParam = "{'room:'"+room+ ",'device:'"+deviceName+"}";
            req.SmsParam = "{room:'" + room + "',device:'" + deviceName + "'}";

            req.RecNum = mobile;
            req.SmsTemplateCode = "SMS_151995664";

            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            var body = rsp.Body;
            return body;
        }





        public static string AliSendMsg_password(string mobile, string name, string password, string modelid)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);

            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "测试";

            /*
             * ${name}向您发起了一笔交易请求，您的初始密码是${password}
             */

            req.SmsParam = "{name:'" + name + "',password:'" + password + "'}";

            req.RecNum = mobile;
            req.SmsTemplateCode = modelid;

            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            var body = rsp.Body;
            return body;
        }




        /// <summary>
        /// 补全账户信息
        /// </summary>
        public static string PaddingUserInfo(string SmsFreeSignName, string number, string mobile)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            //req.SmsFreeSignName = "测试";
            req.SmsFreeSignName = SmsFreeSignName;
            req.SmsParam = "{number:'" + number + "'}";
            req.RecNum = mobile;
            req.SmsTemplateCode = "SMS_60845304";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            var body = rsp.Body;
            return body;
        }


        /// <summary>
        /// 检查用户信息
        /// </summary>
        /// <param name="SmsFreeSignName"></param>
        /// <param name="code"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string CheckUserInfo(string SmsFreeSignName, string code, string mobile, string product)
        {
            ITopClient client = new DefaultTopClient(url, appkey, secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            //req.SmsFreeSignName = "测试";
            // req.SmsFreeSignName = SmsFreeSignName;
            req.SmsFreeSignName = SmsFreeSignName;
            req.SmsParam = "{code:'" + code + "',product:'" + product + "'}";
            req.RecNum = mobile;
            req.SmsTemplateCode = "SMS_62650172";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            var body = rsp.Body;
            return body;
        }


        public class DYRes
        {
            public ErrorResponse error_response { get; set; }
        }




    }
}
