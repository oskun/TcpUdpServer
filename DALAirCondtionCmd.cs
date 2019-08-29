using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALAirCondtionCmd
    {

        /// <summary>
        /// 添加或更新空调命令
        /// 这里存放通过天猫精灵发出的命令
        /// </summary>
        /// <param name="cmd"></param>
        public static void addOrUpdateCmd(AirCondtionCmd cmd)
        {
            /*
             * create proc  AirCondtionCmdAddOrUpdate
@deviceId varchar(50),
@cmd varchar(200)
as
if not exists(select  * from AirCondtionCmd with(index=IX_AirCondtionCmd_deviceId) where deviceId=@deviceId)
begin
 insert into  AirCondtionCmd(deviceId,cmd)
 values(@deviceId,@cmd)
end
else
begin
update AirCondtionCmd set cmd=@cmd where deviceId=@deviceId 
end

             */
            var com = new CommonAction<AirCondtionCmd>("AirCondtionCmdAddOrUpdate", Connection.YZK);
            com.Action(cmd, "deviceId", "cmd");


        }



        /// <summary>
        /// 命令是否存在
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static bool IsCmdExists(string deviceId)
        {
            /*
             * create proc  AirCondtionCmdIsExists
@deviceId varchar(50)
as
if not exists(select  * from AirCondtionCmd with(index=IX_AirCondtionCmd_deviceId) where deviceId=@deviceId)
begin
 select '1'
end
else
begin
 select '0'
end
             */

            var com = new CommonDBHelper(Connection.YZK);
            var sp = new SqlParameter[]
                {
                    new SqlParameter("@deviceId",deviceId)
                  };
            var data = com.getSalar("AirCondtionCmdIsExists", sp);
            if (data.Equals("0"))
            {
                return true;
            }
            return false;



        }





        /// <summary>
        /// 根据记录id获取空调命令
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static AirCondtionCmd GetByDeviceId(string deviceId)
        {
            /*
             * create proc  AirCondtionCmdGetByDeviceId
@deviceId varchar(50)
as
select  * from AirCondtionCmd where  deviceId=@deviceId

             */
            var dt = new Dictionary<string, string>();
            dt.Add("@deviceId", deviceId);

            var method = new CReader<AirCondtionCmd>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "AirCondtionCmdGetByDeviceId");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;
        }

    }
}
