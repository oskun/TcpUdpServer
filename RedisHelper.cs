using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.Redis;

namespace TcpUdpServer
{
    public class RedisHelper<T>
    {
        /// <summary>
        /// 得到连接客户端
        /// </summary>
        /// <returns></returns>
        public static RedisClient GetRedisClient()
        {
            RedisClient rc = new RedisClient();
            return rc;
        }

        /// <summary>
        /// 存储一个值
        /// </summary>
        /// <typeparam name="T">存储值得类型</typeparam>
        /// <param name="key">存储的值的名称</param>
        /// <param name="storevalue">存储的值</param>
        /// <param name="storesecond">存储的时间(默认为一分钟)</param>
        public static bool StoreOneKey(string key, T storevalue, int storesecond = 60)
        {
            //RedisClient client = new RedisClient();
            //if (client.Exists(key) > 0)
            //{
            //    client.Del(key);
            //}
            //var b = client.Add<T>(key, storevalue, DateTime.Now.AddSeconds(storesecond));
            //client.Dispose();
            //return b;
            using (var client = new RedisClient())
            {
                var b = client.Add<T>(key, storevalue, DateTime.Now.AddSeconds(storesecond));
                return b;
            }
        }


        public static bool StoreOneKeyMilliseconds(string key, T storevalue, int Milliseconds = 500)
        {
            //RedisClient client = new RedisClient();
            //if (client.Exists(key) > 0)
            //{
            //    client.Del(key);
            //}
            //var b = client.Add<T>(key, storevalue, DateTime.Now.AddSeconds(storesecond));
            //client.Dispose();
            //return b;
            using (var client = new RedisClient())
            {
                var b = client.Add<T>(key, storevalue, DateTime.Now.AddMilliseconds(Milliseconds));
                return b;
            }
        }



        private static object locker = new object();

        public static bool StoreOneKeyMillseconds(string key, T storevalue, int millseconds = 60)
        {
            //RedisClient client = new RedisClient();
            //if (client.Exists(key) > 0)
            //{
            //    client.Del(key);
            //}
            //var b = client.Add<T>(key, storevalue, DateTime.Now.AddSeconds(storesecond));
            //client.Dispose();
            //return b;
            using (var client = new RedisClient())
            {
                lock (locker)
                {
                    var b = client.Add<T>(key, storevalue, DateTime.Now.AddMilliseconds(millseconds));
                    return b;
                }

            }
        }


        public static bool StoreOneKey(string key, T storevalue)
        {
            //RedisClient client = new RedisClient();
            //var b = client.Add<T>(key, storevalue);
            //client.Dispose();
            //return b;


            using (var client = new RedisClient())
            {
                var b = client.Add<T>(key, storevalue);
                return b;
            }
        }

        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyExist(string key)
        {
            //RedisClient rc = new RedisClient();
            //var l = rc.Exists(key);
            //rc.Dispose();
            //if (l > 0)
            //{
            //    return true;
            //}

            //return false;
            using (var rc = new RedisClient())
            {
                try
                {
                    var l = rc.Exists(key);
                    return l > 0 ? true : false;
                }
                catch
                {
                    Console.WriteLine("Key:" + key);
                    return false;

                }

            }
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static bool DelKey(string key)
        {
            //RedisClient rc = new RedisClient();
            //var ls = rc.Del(key);
            //rc.Dispose();
            //if (ls > 0)
            //{
            //    return true;
            //}

            //return false;
            using (var rc = new RedisClient())
            {
                var ls = rc.Del(key);
                return ls > 0 ? true : false;

            }
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Obsolete("删除太多不建议使用")]
        public static bool DelgroupKey(string key)
        {
            RedisClient rc = new RedisClient();
            var keys = rc.Keys(key + "*");
            foreach (var k in keys)
            {
                var str = System.Text.Encoding.UTF8.GetString(k);
                rc.Del(str);
            }
            rc.Dispose();
            return true;
        }

        /// <summary>
        /// 得到存储的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetStoreValue(string key)
        {
            //RedisClient client = new RedisClient();
            //var t = client.Get<T>(key);
            //client.Dispose();
            //return t;

            using (var client = new RedisClient())
            {
                var t = client.Get<T>(key);
                return t;
            }
        }
    }
}
