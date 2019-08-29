using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALMacIPInfo
    {
        public static void MacIPInfo_Add(MacIPInfo macinfo)
        {
            /*
             use YZK
 go
  alter proc  MacIPInfo_Add_heartbeat
 @mac varchar(50),
 @IP varchar(50),
 @localip varchar(50),
 @ssid varchar(50)
 as
 if not exists(select * from  MacIPInfo where mac=@mac)
 begin
 insert into  MacIPInfo(mac,IP)
 values(@mac,@IP)
 end
 else
 begin
 update MacIPInfo set IP=@IP,record_time=GETDATE() where mac=@mac
 end

             */
            var com = new CommonAction<MacIPInfo>("MacIPInfo_Add_heartbeat", Connection.YZK);
            var para = "OrgText,mac,IP,localip,ssid".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            com.Action(macinfo, para);
        }

        /// <summary>
        /// 根据mac地址获取 macIPInfo
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static MacIPInfo MacIPInfo_GetInfoByMac(string mac)
        {
            /*
            use YZK
go
create  proc MacIPInfo_GetDatabymac
@mac varchar(50)
as
select * from  MacIPInfo where mac=@mac

            */
            var dt = new Dictionary<string, string>();
            dt.Add("@mac", mac);

            var method = new CReader<MacIPInfo>(Connection.YZK);
            var data = method.GetOrgRecord(dt, "MacIPInfo_GetDatabymac");
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;
        }
    }
}
