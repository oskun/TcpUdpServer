using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALYZKDeviceInfo_UserInfocs
    {


        public static YZKDeviceInfo_UserInfo YZKDeviceInfo_UserInfo_GetDataByRecordid(string recordid)
        {
            /*
            * create proc  YZKDeviceInfo_UserInfo_GetByRecordid
    @deviceinfo_userinfo_id varchar(50)
    as
    select  * from  YZKDeviceInfo_UserInfo where  deviceinfo_userinfo_id=@deviceinfo_userinfo_id
            */
            var dt = new Dictionary<string, string>();
            dt.Add("@deviceinfo_userinfo_id", recordid);
            var method = new CReader<YZKDeviceInfo_UserInfo>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "YZKDeviceInfo_UserInfo_GetByRecordid");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;


        }



        /// <summary>
        /// 某个家下是否存在某个mac的设备
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="device_mac">设备mac</param>
        /// <param name="home_id">家id</param>
        /// <returns></returns>
        public static bool YZKDeviceInfo_UserInfo_HomeContainMac(string device_mac, string home_id)
        {
            /*
             * alter proc  YZKDeviceInfo_UserInfo_HomeContainMac
@home_id varchar(50),
@device_mac varchar(50),
@uid varchar(50)
as 
if exists(select  * from  YZKDeviceInfo_UserInfo where home_id=@home_id and  device_mac=@device_mac)
begin
select  '1'
end
else
begin
select  '0'
end

             */
            var com = new CommonAction<YZKDeviceInfo_UserInfo>("YZKDeviceInfo_UserInfo_HomeContainMac", Connection.YZK);
            var data = com.Scalar(new YZKDeviceInfo_UserInfo() { device_mac = device_mac, home_id = home_id }, "home_id", "device_mac");
            if (data.Equals("1"))
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// 获取小飞碟（中间转发器）
        ///     public static final int device_protocol_64 = 64;
        /// </summary>
        /// <param name="home_id"></param>
        /// <returns></returns>
        public static List<YZKDeviceInfo_UserInfo> YZKDeviceInfo_UserInfo_GetXiaoFeiDie(string home_id, int device_protocol, string device_type, string bxk_label)
        {
            /*
             * create proc  YZKDeviceInfo_UserInfo_GetXiaoFeiDie
  @device_type varchar(50),
  @device_protocol int,
  @home_id varchar(50)
  as
  select  * from  YZKDeviceInfo_UserInfo where device_type=@device_type and device_protocol=@device_protocol and  home_id=@home_id
             */
            var dt = new Dictionary<string, string>();
            dt.Add("@device_type", device_type);
            dt.Add("@device_protocol", device_protocol + "");
            dt.Add("@home_id", home_id);
            dt.Add("@bxk_label", bxk_label);


            var method = new CReader<YZKDeviceInfo_UserInfo>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "YZKDeviceInfo_UserInfo_GetXiaoFeiDie");
            return data;

        }


        /// <summary>
        /// 根据home_id,device_protocol,device_type
        /// </summary>
        /// <param name="home_id"></param>
        /// <param name="device_protocol"></param>
        /// <param name="device_type"></param>
        /// <returns></returns>
        public static List<YZKDeviceInfo_UserInfo> YZKDeviceInfo_UserInfo_GetXiaoFeiDieByHomeId(string home_id, int device_protocol, string device_type)
        {
            /*
             * use YZK
go
create proc  YZKDeviceInfo_UserInfoGetXiaoFeiDieByHomeId
@device_type varchar(50),
@device_protocol int,
@home_id varchar(50) 
as
select  *from  YZKDeviceInfo_UserInfo where device_type=@device_type and  device_protocol=@device_protocol and home_id=@home_id

             */
            var dt = new Dictionary<string, string>();
            dt.Add("@device_type", device_type);
            dt.Add("@device_protocol", device_protocol + "");
            dt.Add("@home_id", home_id + "");


            var method = new CReader<YZKDeviceInfo_UserInfo>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "YZKDeviceInfo_UserInfoGetXiaoFeiDieByHomeId");
            return data;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceinfo_userinfo_id"></param>
        /// <returns></returns>
        public static bool YZKDeviceInfo_UserInfo_IsDeviceExists(string deviceinfo_userinfo_id)
        {
            /*
             *create proc YZKDeviceInfo_UserInfo_IsDataExists
@deviceinfo_userinfo_id  varchar(50)
as
if exists(select  * from  YZKDeviceInfo_UserInfo where  deviceinfo_userinfo_id=@deviceinfo_userinfo_id)
begin
select '1'
end
else 
begin
select '0'
end 
             */

            var key = "key:deviceinfo_userinfo_id:" + deviceinfo_userinfo_id;
            if (RedisHelper<bool>.IsKeyExist(key))
            {
                return true;
            }
            else
            {
                var sp = new SqlParameter[]
              {
                    new SqlParameter("@deviceinfo_userinfo_id",deviceinfo_userinfo_id)
              };
                var db = new CommonDBHelper(Connection.YZK);
                var data = db.getSalar("YZKDeviceInfo_UserInfo_IsDataExists", sp) + "";
                if (data.Equals("1"))
                {
                    RedisHelper<string>.StoreOneKeyMilliseconds(key, "1", 1000 * 120);
                    return true;
                }
               
                return false;
            }

        }


        /// <summary>
        /// 判断海底捞设备是否存在
        /// </summary>
        /// <param name="deviceinfo_userinfo_id"></param>
        /// <returns></returns>
        public static bool YZKDeviceInfo_UserInfo_IsHDLDeviceExists(string deviceinfo_userinfo_id)
        {
            /*
             * 判断海底捞设备是否存在
             * create proc YZKDeviceInfo_UserInfo_IsDataExists
@deviceinfo_userinfo_id  varchar(50)
as
if exists(select  * from  YZKDeviceInfo_UserInfo where  deviceinfo_userinfo_id=@deviceinfo_userinfo_id)
begin
select '1'
end
else 
begin
select '0'
end 
             */

            var key = "key:deviceinfo_userinfo_id:" + deviceinfo_userinfo_id;
            if (RedisHelper<string>.IsKeyExist(key))
            {
                return true;
            }
            else
            {
                var sp = new SqlParameter[]
             {
                    new SqlParameter("@deviceinfo_userinfo_id",deviceinfo_userinfo_id)
             };
                var db = new CommonDBHelper(Connection.hdl);
                var data = db.getSalar("YZKDeviceInfo_UserInfo_IsDataExists", sp) + "";
                if (data.Equals("1"))
                {
                    RedisHelper<string>.StoreOneKeyMilliseconds(key, "1", 1000 * 120);
                    return true;
                }
               
                return false;
            }
           

        }




    }
}
