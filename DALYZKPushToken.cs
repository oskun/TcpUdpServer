using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALYZKPushToken
    {
        public static void YZKPushToken_Add(YZKPushToken pushtoken)
        {
            /*
             *
             *alter proc YZKPushToken_Add
@devicetoken varchar(50),
@devicetype int,
@uid varchar(50)
as
if not exists(select * from YZKPushToken where uid=@uid)
begin
insert into YZKPushToken(uid,devicetoken,devicetype)
values(@uid,@devicetoken,@devicetype)
end
else
begin
update YZKPushToken set devicetype=@devicetype,
devicetoken=@devicetoken
where uid=@uid
end
             *
             */
            var paras = "device_mac,devicetoken,devicetype,uid".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var com = new CommonAction<YZKPushToken>("YZKPushToken_Add", Connection.YZK);
            com.Action(pushtoken, paras);
        }

        public static YZKPushToken YZKPushToken_GetByUidDeviceType(string uid, DeviceType devicetype)
        {
            /*
           use YZK
go
create proc  YZKPushToken_GetByuiddevicetype
@uid varchar(50),
@devicetype int
as
select * from  YZKPushToken where uid=@uid and devicetype=@devicetype

            */
            var type = ((int)devicetype) + "";
            var dt = new Dictionary<string, string>();
            dt.Add("@uid", uid);
            dt.Add("@devicetype", type);

            var method = new CReader<YZKPushToken>(Connection.YZK);

            var data = method.GetOrgRecord(dt, "YZKPushToken_GetByuiddevicetype");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;
        }


        /// <summary>
        /// 根据uid获取设备token
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public static List<YZKPushToken> YZKPushToken_GetByUid(string uid)
        {
            /*
             * 
             * use  YZK
go
create proc YZKPushToken_GetDataByuid
@uid varchar(50)
as
select  * from  YZKPushToken where uid=@uid
             * 
             */
            var dt = new Dictionary<string, string>();
            dt.Add("@uid", uid);
            var method = new CReader<YZKPushToken>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "YZKPushToken_GetDataByuid");
            return data;


        }



        public static YZKPushToken YZKPushToken_GetByUidDeviceTypeMac(string uid, DeviceType devicetype, string mac)
        {
            /*
           use YZK
go
create proc  YZKPushToken_GetByuiddevicetypeMac
@uid varchar(50),
@devicetype int,
@device_mac varchar(50)
as
select * from  YZKPushToken where uid=@uid and devicetype=@devicetype and device_mac=@device_mac

            */
            var type = ((int)devicetype) + "";
            var dt = new Dictionary<string, string>();
            dt.Add("@uid", uid);
            dt.Add("@devicetype", type);
            dt.Add("@device_mac", mac);

            var method = new CReader<YZKPushToken>(Connection.YZK);

            var data = method.GetOrgRecord(dt, "YZKPushToken_GetByuiddevicetypeMac");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;
        }


        /// <summary>
        /// 根据设备的mac和设备的协议类型获取设备的token
        /// </summary>
        /// <param name="device_mac"></param>
        /// <param name="protocol_type"></param>
        /// <returns></returns>
        public static List<YZKPushToken> YZKPushToken_GetByDeviceMacProcoltype(string device_mac, string protocol_type)
        {
            /*
             * 
             * create  proc YZKPushToken_GetByDeviceMacProcoltype
@device_mac varchar(50),
@device_protocol varchar(50)
as
select  uid,devicetoken,devicetype,device_mac from  YZKPushToken where uid in(select  uid from YZKDeviceInfo_UserInfo where device_mac=@device_mac and  device_protocol=@device_protocol)
group by uid,devicetoken,devicetype,device_mac
             */
            var dt = new Dictionary<string, string>();
            dt.Add("@device_mac", device_mac);
            dt.Add("@device_protocol", protocol_type);
            var method = new CReader<YZKPushToken>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "YZKPushToken_GetByDeviceMacProcoltype");
            return data;
        }

    }
}
