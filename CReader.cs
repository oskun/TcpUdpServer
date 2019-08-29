using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class CReader<T> where T : class, new()
    {
        private string conName;

        public CReader(string conName)
        {
            this.conName = conName;
        }

        /// <summary>
        /// 得到原始的数据数据
        /// 时间将不会被转化
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="procname">存储过程的名称</param>
        /// <param name="changeTime">是否将时间转为格林威治时间</param>
        /// <returns></returns>
        public List<T> GetOrgRecord(Dictionary<string, string> para, string procname)
        {
            var sp = new SqlParameter[] { };
            var lsp = sp.ToList();
            if (para != null)
            {
                foreach (var s in para)
                {
                    var key = s.Key;
                    if (key.StartsWith("@") == false)
                    {
                        lsp.Add(new SqlParameter("@" + key, para[key]));
                    }
                    else
                    {
                        lsp.Add(new SqlParameter(key, para[key]));
                    }
                }
            }
            List<T> list = new List<T>();
            var db = new CommonDBHelper(conName);

            using (var reader = db.getReader(procname, lsp.ToArray()))
            {
                while (reader.Read())
                {
                    T t = new T();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);
                        var lowername = name.ToString();
                        var propertyInfo = t.GetType().GetProperty(name);
                        try
                        {
                            if (propertyInfo != null)
                            {
                                ///当前列的值
                                var value = reader.GetValue(i);
                                var ft = reader.GetFieldType(i);
                                if (value == DBNull.Value)
                                {
                                    ///int 类型
                                    if (ft == typeof(int) || ft == typeof(double) || ft == typeof(float))
                                    {
                                        propertyInfo.SetValue(t, 0, null);
                                    }
                                    ///string 类型
                                    else if (ft == typeof(string))
                                    {
                                        // propertyInfo.SetValue(t, value, null);
                                        propertyInfo.SetValue(t, "", null);
                                    }
                                    else if (ft == typeof(DateTime))
                                    {
                                        propertyInfo.SetValue(t, DateTime.MinValue, null);
                                    }
                                }
                                else if (value != DBNull.Value)
                                {
                                    var ptype = propertyInfo.PropertyType;
                                    if (name.IndexOf("lat") != -1 || name.IndexOf("lng") != -1)
                                    {
                                    }
                                    else
                                    {
                                        if (ft == typeof(double))
                                        {
                                            value = Math.Round((double)value, 2);
                                        }
                                    }

                                    propertyInfo.SetValue(t, value, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var msg = name + ex.Message;
                            throw new Exception(msg);
                        }
                    }
                    list.Add(t);
                }
            }

            return list;
        }
    }
}
