using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class YouMengHelper
    {

        /*
        为了确保用户发送的请求不被更改，我们设计了签名算法。该算法基本可以保证请求是合法者发送且参数没有被修改，但无法保证不被偷窥。 签名生成规则：
A）提取请求方法method（POST，全大写）；
B）提取请求url信息，包括Host字段的域名(或ip:端口)和URI的path部分。注意不包括path的querystring。
比如http://msg.umeng.com/api/send 或者 http://msg.umeng.com/api/status;
C）提取请求的post-body；
D）拼接请求方法、url、post-body及应用的app_master_secret；
E）将D形成字符串计算MD5值，形成一个32位的十六进制（字母小写）字符串，即为本次请求sign（签名）的值；
Sign=MD5($http_method$url$post-body$app_master_secret);
        */
        public static string GetSign(DeviceType type, string url, IPush postbody)
        {
            var app_master_secret = string.Empty;
            if (type == DeviceType.Andriod)
            {
                app_master_secret = "c02mxn27sntad8nr9am2dwd4ruddl4jf";
            }
            else if (type == DeviceType.IOS)
            {
                //57d0f6f8e0f55a1619000914
                //f1r4glcnnimzpfnqikdi5glntps8k1f7
                app_master_secret = "f1r4glcnnimzpfnqikdi5glntps8k1f7";
            }
            else if (type == DeviceType.IOS_APPStore)
            {
                app_master_secret = "6khvrxf6idmfwrythbjdo5rmwlcty0qq";
            }
            else if (type == DeviceType.Android_Center)
            {
                app_master_secret = "qnmiba5fthsf3eciqx6khxdxkomc136c";
            }
            ///易指控
            else if (type == DeviceType.YZK_IOS_APPStore)
            {
                app_master_secret = "xfldioemvvsewnkzcr45ge8s86knuihz";
            }

            if (!string.IsNullOrEmpty(app_master_secret))
            {
                var postbodyjson = JsonHelper<IPush>.GetJson(postbody);
                var sign = EncryptHelper.MD5Encoding("POST" + url + postbodyjson + app_master_secret);
                return sign;
            }
            else
            {
                return string.Empty;
            }

        }




        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <param name="device_tokens"></param>
        /// <param name="ticker"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static string makePush(string alert, string category, DeviceType type, string device_tokens, string ticker, string title, string text, Dictionary<string, string> extra)
        {
            var url = "http://msg.umeng.com/api/send";
            if (type == DeviceType.IOS)
            {
                var p = new IOSPayLoad(title, text, ticker, category);

                var ios = new IOS()
                {
                    device_tokens = device_tokens,
                    payload = p,
                    description = "苹果推送",
                    production_mode = "false",
                    policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
                };
                SetDataHelper<IOS>.RemoveStrNull(ios);
                var sign = YouMengHelper.GetSign(DeviceType.IOS, url, ios);
                var postbody = JsonHelper<IPush>.GetJson(ios);
                var dt = new Dictionary<string, string>();

                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                dt.Add("sign", sign);
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;
            }
            else if (type == DeviceType.YZK_IOS_APPStore)
            {
                var p = new IOSPayLoad(title, text, ticker, category, extra);

                var ios = new IOS(type)
                {
                    device_tokens = device_tokens,
                    payload = p,
                    description = "苹果推送",
                    production_mode = "false",
                    policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
                };
                SetDataHelper<IOS>.RemoveStrNull(ios);
                var sign = YouMengHelper.GetSign(type, url, ios);
                var postbody = JsonHelper<IPush>.GetJson(ios);
                var dt = new Dictionary<string, string>();

                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                dt.Add("sign", sign);
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;
            }
            else if (type == DeviceType.Andriod)
            {
                var p = new Payload("notification", ticker, title, text, extra);

                var android = new Android() { type = "listcast", device_tokens = device_tokens, payload = p, description = "android推送", production_mode = "false", policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") } };
                SetDataHelper<Android>.RemoveStrNull(android);
                var sign = YouMengHelper.GetSign(type, url, android);
                var postbody = JsonHelper<IPush>.GetJson(android);
                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;
            }
            else if (type == DeviceType.Android_Center)
            {
                var p = new Payload("notification", ticker, title, text, extra);

                var android = new Android(type) { type = "listcast", device_tokens = device_tokens, payload = p, description = "android推送", production_mode = "false", policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") } };
                SetDataHelper<Android>.RemoveStrNull(android);
                var sign = YouMengHelper.GetSign(type, url, android);
                var postbody = JsonHelper<IPush>.GetJson(android);
                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;
            }

            return string.Empty;




        }




        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <param name="device_tokens"></param>
        /// <param name="ticker"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        /// //                        (alert, categrory, typex, dtoken.devicetoken, ticker, title, text, extra_typex, mac, localip, action);
        public static string makePush(string alert, string category, DeviceType type, string device_tokens, string ticker, string title, string text, ExtraType extraType, params string[] extradatas)
        {
            var extra = new Dictionary<string, string>();
            if (extraType == ExtraType.CommonPush)
            {
                extra.Add("type", "0");
                extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
            }
            else if (extraType == ExtraType.ASKHelp)
            {
                if (extradatas.Length == 3)
                {
                    //增加conver_img 铭牌照片 component_img 零件照片， 点击消息可选择查看铭牌照片  零件照片
                    extra.Add("conver_img", extradatas[0]);
                    extra.Add("component_img", extradatas[1]);
                    extra.Add("skuid", extradatas[2]);
                    extra.Add("type", "3");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }

                else if (extradatas.Length == 7)
                {
                    //增加conver_img 铭牌照片 component_img 零件照片， 点击消息可选择查看铭牌照片  零件照片
                    extra.Add("conver_img", extradatas[0]);
                    extra.Add("component_img", extradatas[1]);
                    extra.Add("skuid", extradatas[2]);
                    extra.Add("type", "3");
                    extra.Add("price", extradatas[3]);
                    extra.Add("brand", extradatas[4]);
                    extra.Add("bigclass", extradatas[5]);
                    extra.Add("name", extradatas[6]);
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }


            }
            else if (extraType == ExtraType.ADPush)
            {
                if (extradatas.Length == 1)
                {
                    extra.Add("url", extradatas[0]);
                    extra.Add("type", "2");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }
            }
            else if (extraType == ExtraType.OrderPush)
            {
                if (extradatas.Length == 1)
                {
                    extra.Add("orderno", extradatas[0]);
                    extra.Add("type", "1");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }
            }
            else if (extraType == ExtraType.CenterNotify_3)
            {
                ///3个参数
                if (extradatas.Length == 3)
                {
                    extra.Add("sync_notify_type", extradatas[0]);
                    extra.Add("record_id", extradatas[1]);
                    extra.Add("action", extradatas[2]);
                    extra.Add("type", "1");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }

            }
            else if (extraType == ExtraType.CenterNotify_online_4)
            {
                if (extradatas.Length >= 4)
                {
                    extra.Add("sync_notify_type", extradatas[0]);
                    extra.Add("mac", extradatas[1]);
                    extra.Add("localip", extradatas[2]);
                    extra.Add("action", extradatas[3]);
                    extra.Add("type", "1");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }
            }


            var url = "http://msg.umeng.com/api/send";
            if (type == DeviceType.IOS)
            {
                var p = new IOSPayLoad(title, text, ticker, category, extra);

                var ios = new IOS()
                {
                    device_tokens = device_tokens,
                    payload = p,
                    description = "苹果推送",
                    production_mode = "true",
                    policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
                };
                SetDataHelper<IOS>.RemoveStrNull(ios);
                var sign = YouMengHelper.GetSign(DeviceType.IOS, url, ios);
                var postbody = JsonHelper<IPush>.GetJson(ios);
                var dt = new Dictionary<string, string>();

                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                dt.Add("sign", sign);
                try
                {
                    var data = WebHelper.DoPost(fullurl, postbody);
                    return data;
                }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
                catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
                {
                    return string.Empty;
                }

            }
            else if (type == DeviceType.Andriod)
            {
                var p = new Payload("notification", ticker, title, text, extra);
                var android = new Android() { type = "listcast", device_tokens = device_tokens, payload = p, description = "android推送", production_mode = "true", policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") } };
                SetDataHelper<Android>.RemoveStrNull(android);
                var sign = YouMengHelper.GetSign(type, url, android);
                var postbody = JsonHelper<IPush>.GetJson(android);
                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;


            }

            ///中控设备
            else if (type == DeviceType.Android_Center)
            {
                var cust_json = JsonHelper<Dictionary<string, string>>.GetJson(extra);
                var p = new Payload("message", ticker, title, text, extra, cust_json);
                ///中控设备的推送
                var appkey = "595312d182b63512f90019bc";
                var android = new Android(appkey)
                {
                    type = "listcast",
                    device_tokens = device_tokens,
                    payload = p,
                    description = "android推送",
                    production_mode = "true",
                    policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
                };
                SetDataHelper<Android>.RemoveStrNull(android);
                var sign = YouMengHelper.GetSign(type, url, android);
                var postbody = JsonHelper<IPush>.GetJson(android);
                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;

            }
            else if (type == DeviceType.IOS_APPStore)
            {
                var p = new IOSPayLoad(title, text, ticker, category, extra);

                var ios = new IOS_AppStore()
                {
                    device_tokens = device_tokens,
                    payload = p,
                    description = "苹果推送",
                    production_mode = "true",
                    policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
                };
                SetDataHelper<IOS_AppStore>.RemoveStrNull(ios);
                var sign = YouMengHelper.GetSign(DeviceType.IOS_APPStore, url, ios);
                var postbody = JsonHelper<IPush>.GetJson(ios);
                var dt = new Dictionary<string, string>();

                var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
                dt.Add("sign", sign);
                try
                {
                    var data = WebHelper.DoPost(fullurl, postbody);
                    return data;
                }
                catch
                {
                    return string.Empty;
                }


            }
            return string.Empty;




        }





        private static string AndroidPushAll(string ticker, string title, string text, Dictionary<string, string> extra)
        {

            var p = new Payload("notification", ticker, title, text, extra);
            var type = DeviceType.Andriod;
            var url = "http://msg.umeng.com/api/send";
            var android = new Android() { type = "broadcast", device_tokens = "", payload = p, description = "android推送", production_mode = "true", policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") } };
            SetDataHelper<Android>.RemoveStrNull(android);
            var sign = YouMengHelper.GetSign(type, url, android);
            var postbody = JsonHelper<IPush>.GetJson(android);
            var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
            var data = WebHelper.DoPost(fullurl, postbody);
            return data;




        }



        /// <summary>
        /// 苹果推送
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="ticker"></param>
        /// <param name="category"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        private static string ApplePushAll(string title, string text, string ticker, string category, Dictionary<string, string> extra)
        {

            var url = "http://msg.umeng.com/api/send";
            var p = new IOSPayLoad(title, text, ticker, category, extra);

            var ios = new IOS("broadcast")
            {
                payload = p,
                description = "苹果推送",
                production_mode = "true",
                policy = new Policy() { expire_time = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss") }
            };
            SetDataHelper<IOS>.RemoveStrNull(ios);
            var sign = YouMengHelper.GetSign(DeviceType.IOS_APPStore, url, ios);
            var postbody = JsonHelper<IPush>.GetJson(ios);
            var dt = new Dictionary<string, string>();

            var fullurl = "http://msg.umeng.com/api/send?sign=" + sign;
            dt.Add("sign", sign);
            try
            {
                var data = WebHelper.DoPost(fullurl, postbody);
                return data;

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return string.Empty;
            }
        }



        public static string makePushAll(string alert, string category, string ticker, string title, string text, ExtraType extraType, params string[] extradatas)
        {

            var extra = new Dictionary<string, string>();
            if (extraType == ExtraType.CommonPush)
            {
                extra.Add("type", "0");
                extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
            }
            else if (extraType == ExtraType.ASKHelp)
            {
                if (extradatas.Length == 3)
                {
                    //增加conver_img 铭牌照片 component_img 零件照片， 点击消息可选择查看铭牌照片  零件照片
                    extra.Add("conver_img", extradatas[0]);
                    extra.Add("component_img", extradatas[1]);
                    extra.Add("skuid", extradatas[2]);
                    extra.Add("type", "3");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }

                else if (extradatas.Length == 7)
                {
                    //增加conver_img 铭牌照片 component_img 零件照片， 点击消息可选择查看铭牌照片  零件照片
                    extra.Add("conver_img", extradatas[0]);
                    extra.Add("component_img", extradatas[1]);
                    extra.Add("skuid", extradatas[2]);
                    extra.Add("type", "3");
                    extra.Add("price", extradatas[3]);
                    extra.Add("brand", extradatas[4]);
                    extra.Add("bigclass", extradatas[5]);
                    extra.Add("name", extradatas[6]);
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }


            }
            else if (extraType == ExtraType.ADPush)
            {
                if (extradatas.Length == 1)
                {
                    extra.Add("url", extradatas[0]);
                    extra.Add("type", "2");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }
            }
            else if (extraType == ExtraType.OrderPush)
            {
                if (extradatas.Length == 1)
                {
                    extra.Add("orderno", extradatas[0]);
                    extra.Add("type", "1");
                    extra.Add("servertime", TimeHelper.JavaTime(DateTime.Now) + "");
                }
            }
#pragma warning disable CS0219 // 变量“url”已被赋值，但从未使用过它的值
            var url = "http://msg.umeng.com/api/send";
#pragma warning restore CS0219 // 变量“url”已被赋值，但从未使用过它的值
            ApplePushAll(title, text, ticker, category, extra);
            AndroidPushAll(ticker, title, text, extra);
            return string.Empty;

        }

    }
}
