using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALDefenseStatus
    {




        /// <summary>
        /// 根据家id获取撤防状态
        /// </summary>
        /// <param name="homeId"></param>
        /// <returns></returns>
        public static DefenseStatus DefenseStatus_GetByHomeId(string homeId)
        {
            /*
             * 
 create  proc  DefenseStatus_GetByHomeId
 @homeId varchar(50)
 as
 select  * from  DefenseStatus where homeId=@homeId
             */
            var dt = new Dictionary<string, string>();
            dt.Add("@homeId", homeId);
            var method = new CReader<DefenseStatus>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "DefenseStatus_GetByHomeId");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;

            //var method = 
            //var data = method.GetOrgRecord(dt, "DefenseStatus_GetByHomeId");
            //if (data.Count > 0)
            //{
            //    return data[0];
            //}
            //return null;

        }




        /// <summary>
        /// 安防状态
        /// </summary>
        /// <param name="dss"></param>
        public static void DefenseStatus_Add(DefenseStatus dss)
        {
            /*
             * create proc DefenseStatus_Add
 @status int,
 @homeId varchar(50)
 as
 if not exists(select * from  DefenseStatus where  homeId=@homeId)
 begin
 insert into  DefenseStatus([status],homeId)
 values(@status,@homeId)
 end
 else 
 begin
 update DefenseStatus set [status]=@status where homeId=@homeId 
 end

             * 
             */
            var com = new CommonAction<DefenseStatus>("DefenseStatus_Add", Connection.YZK);
            com.Action(dss, "status", "homeId");

        }



        public static DefenseStatus DefenseStatusGetByDeviceId(int deviceinfo_userinfo_id)
        {
            /*
               create  proc DefenseStatus_GetStatusByDeviceID
  @deviceinfo_userinfo_id int
  as
  select  * from  DefenseStatus where homeId in(select  homeId from  YZKDeviceInfo_UserInfo
  where deviceinfo_userinfo_id=@deviceinfo_userinfo_id)
             
             */
            var dt = new Dictionary<string, string>();
            dt.Add("@deviceinfo_userinfo_id", deviceinfo_userinfo_id + "");

            var method = new CReader<DefenseStatus>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "DefenseStatus_GetStatusByDeviceID");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;

        }



    }
}
