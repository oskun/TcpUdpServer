using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALNotifyException
    {


        public static void NotifyExceptionAddX(NotifyException notify)
        {
            /*
             *ALTER proc [dbo].[NotifyExceptionAdd]
@uid varchar(50),
@bxkLabel varchar(50),
@deviceSetName varchar(50),
@deviceId varchar(50),
@alarm_type int=0
as
insert into  NotifyException(uid,bxkLabel,deviceSetName,deviceId,alarm_type)
values(@uid,@bxkLabel,@deviceSetName,@deviceId,@alarm_type)
             * 
             */
            var paras = "uid,bxkLabel,deviceSetName,deviceId,alarm_type".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var c = new CommonAction<NotifyException>("NotifyExceptionAdd", Connection.YZK);
            c.Action(notify, paras);

        }



        /// <summary>
        /// 添加异常通知
        /// </summary>
        /// <param name="notify"></param>
        public static void NotifyExceptionAdd(NotifyException notify)
        {
            /*
             * alter proc NotifyExceptionAdd
@uid varchar(50),
@bxkLabel varchar(50),
@deviceSetName varchar(50),
@deviceId varchar(50)
as
insert into  NotifyException(uid,bxkLabel,deviceSetName,deviceId)
values(@uid,@bxkLabel,@deviceSetName,@deviceId)
             * 
             */
            var c = new CommonAction<NotifyException>("NotifyExceptionAdd", Connection.YZK);
            c.Action(notify, "uid", "bxkLabel", "deviceSetName", "deviceId");

        }
    }
}
