using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class YZKUser
    {
        /// <summary>
        /// 用户手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string pwd { get; set; }

        public int userid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime recordtime { get; set; }

        /// <summary>
        /// 最新登录时间
        /// </summary>
        public DateTime last_logintime { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avator { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }



        /// <summary>
        /// 乐橙用户是否绑定
        /// </summary>
        public bool lechange_has_bind { get; set; }



        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool is_Init { get; set; }


    }
}
