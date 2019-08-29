using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class SetDataHelper<T> where T : class
    {

        public static T RemoveStrNull(T t)
        {
            var type = t.GetType();
            var ps = type.GetProperties();
            foreach (var p in ps)
            {
                var val = p.GetValue(t, null);
                var ptype = p.PropertyType;
                if (ptype == typeof(string))
                {
                    if (val == null)
                    {
                        p.SetValue(t, "", null);
                    }
                }
            }
            return t;
        }


    }
}
