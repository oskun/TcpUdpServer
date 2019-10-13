using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using log4net;
using System.Net;

namespace TcpUdpServer
{
    public class LogHelper
    {

        private static readonly ILog logInfo = LogManager.GetLogger("Log");
        private static readonly ILog logErr = LogManager.GetLogger("Err");
        

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            Console.WriteLine(message);
            //logInfo.Info(message);
          
        }


        public static void Info(string message,Socket socket)
        {
            //if (socket != null && socket.Connected)
            //{
            //    var point = (IPEndPoint)socket.RemoteEndPoint;

            //    var msg = point.Address + "    " + point.Port + "    " + message;
            //    Console.WriteLine(msg);
            //    logInfo.Info(msg);

            //}
            //else
            //{
              
            //    Console.WriteLine(message);
            //    logInfo.Info(message);
            //}
          
        }





        /// <summary>
        /// 过滤日志
        /// </summary>
        /// <param name="func"></param>
        /// <param name="msg"></param>
        public static void LogFilter(Func<bool> func,string msg)
        {
            if (func())
            {
                logInfo.Info(msg);

            }
        }



        public static void LogFilter(bool  isRecord,string msg)
        {
            if (isRecord)
            {
                Console.WriteLine(msg);
                logInfo.Info(msg);
            }
        }

    }



}
