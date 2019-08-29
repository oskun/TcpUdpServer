using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class DALYZKUser
    {
        /// <summary>
        /// 根据uuid得到
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static YZKUser YZKUser_GetByuuid(string uuid)
        {
            /*
             * create proc YZKUser_GetByuuid
@uuid varchar(50)
as
select * from  YZKUser where uuid=@uuid

             */
            var key = "YZK.user.uid:" + uuid;
            if (RedisHelper<YZKUser>.IsKeyExist(key) == false)
            {
                var dt = new Dictionary<string, string>();
                dt.Add("@uuid", uuid);

                var conName = Connection.YZK;

                var ac = new CReader<YZKUser>(conName);
                var data = ac.GetOrgRecord(dt, "YZKUser_GetByuuid");
                if (data.Count > 0)
                {
                    RedisHelper<YZKUser>.StoreOneKey(key, data[0], 3000);
                    return data[0];
                }
                return null;
            }
            else
            {
                var signdata = RedisHelper<YZKUser>.GetStoreValue(key);
                return signdata;
            }
        }

        /// <summary>
        /// 根据uid 获取账户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static YZKUser YZKUser_GetByuid(string uid)
        {
            /*
               create proc  YZKUser_Getbyuserid
  @userid varchar(50)
  as
  select * from YZKUser where  userid=@userid

             */
            var readmethod = new CReader<YZKUser>(Connection.YZK);
            var dt = new Dictionary<string, string>();
            dt.Add("@userid", uid);

            var data = readmethod.GetOrgRecord(dt, "YZKUser_Getbyuserid");
            if (data != null && data.Count > 0)
            {
                return data[0];
            }
            return null;
        }









    }
}
