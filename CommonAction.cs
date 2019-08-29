using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TcpUdpServer
{
    public class CommonAction<T> where T : class, new()
    {
        /// <summary>
        /// 存储过程的名称
        /// </summary>
        private string procname;


        private string constr;


        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="procname">存储过程的名称</param>
        public CommonAction(string procname, string constr)
        {
            this.procname = procname;
            this.constr = constr;
        }



        /// <summary>
        /// 通用的插入
        /// </summary>
        /// <param name="t">插入(更新,删除)的单个类型的值</param>
        /// <param name="ignorecol">不忽略的属性列</param>
        public void Action(T t, params string[] needcol)
        {
            SetDataHelper<T>.RemoveStrNull(t);
            var listsp = new List<SqlParameter>();
            var type = t.GetType();
            //  var rank= type.GetArrayRank();
            var ignorelist = needcol.ToList();
            var Properties = type.GetProperties();
            foreach (var p in Properties)
            {
                var name = p.Name;
                if (ignorelist.FindIndex(a => a.Equals(name)) != -1)
                {
                    var value = p.GetValue(t, null);
                    var sp = new SqlParameter("@" + p.Name, value);
                    listsp.Add(sp);
                }
            }
            var db = new DBHelper(constr);
            db.execute(procname, listsp.ToArray());

        }



        public string Scalar(T t, params string[] needcol)
        {
            SetDataHelper<T>.RemoveStrNull(t);
            var listsp = new List<SqlParameter>();
            var type = t.GetType();
            //  var rank= type.GetArrayRank();
            var ignorelist = needcol.ToList();
            var Properties = type.GetProperties();
            foreach (var p in Properties)
            {
                var name = p.Name;
                if (ignorelist.FindIndex(a => a.Equals(name)) != -1)
                {
                    var value = p.GetValue(t, null);
                    var sp = new SqlParameter("@" + p.Name, value);
                    listsp.Add(sp);
                }
            }
            var db = new DBHelper(constr);
            var data = db.getSalar(procname, listsp.ToArray()) + "";
            return data;
        }




        /// <summary>
        ///生成通用的sql 语句
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="primarykey">主键</param>
        /// <param name="ignorecol">忽略的列</param>
        /// <returns></returns>
        public string ActionSqlCommit(T t, string primarykey, params string[] ignorecol)
        {
            ///表明
            var tablename = t.GetType().Name;
            var ps = t.GetType().GetProperties();
            var primaryvalue = string.Empty;
            var stringarr = new List<string>();
            foreach (var p in ps)
            {
                var colname = p.Name;
                var flag = true;
                foreach (var ig in ignorecol)
                {
                    if (colname.Equals(ig))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    var colvalue = p.GetValue(t, null);
                    if (colname.Equals(primarykey))
                    {
                        primaryvalue = colvalue.ToString();
                    }
                    else
                    {
                        stringarr.Add("" + colname + "='" + colvalue + "'");
                    }
                }

            }
            var append = string.Join(",", stringarr.ToArray());
            var sql = " update " + tablename + " set " + append + "  where " + primarykey + "='" + primaryvalue + "'";
            var db = new DBHelper(constr);
            db.execute(sql);
            return sql;
        }




        /// <summary>
        ///
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ignorecol"></param>
        /// <returns></returns>
        public string ActionSqlAdd(T t, params string[] needcol)
        {
            ///表明
            var tablename = t.GetType().Name;
            var ps = t.GetType().GetProperties();

            var listname = new List<string>();
            var listvalue = new List<string>();
            foreach (var p in ps)
            {
                var colname = p.Name;
                var flag = false;
                foreach (var ig in needcol)
                {
                    if (colname.Equals(ig))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {

                    var colvalue = p.GetValue(t, null).ToString().Replace("'", "''");
                    listname.Add(colname);
                    listvalue.Add(colvalue.ToString());

                }

            }


            var col_str = string.Join(",", listname);
            var col_value = string.Join("','", listvalue);

            var sql = " insert into " + tablename + "(" + col_str + ") values('" + col_value + "'); select SCOPE_IDENTITY();  ";

            var db = new DBHelper(constr);
            var data = db.getSalar(sql) + "";

            return data;
        }




        /// <summary>
        ///
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ignorecol"></param>
        /// <returns></returns>
        public string ActionSqlAdd_Ignore(T t, params string[] ignore)
        {
            ///表明
            var tablename = t.GetType().Name;
            var ps = t.GetType().GetProperties();

            var listname = new List<string>();
            var listvalue = new List<string>();
            foreach (var p in ps)
            {
                var colname = p.Name;
                var flag = true;
                foreach (var ig in ignore)
                {
                    if (colname.Equals(ig))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {

                    var colvalue = p.GetValue(t, null).ToString().Replace("'", "''");
                    listname.Add(colname);
                    listvalue.Add(colvalue.ToString());

                }

            }


            var col_str = string.Join(",", listname);
            var col_value = string.Join("','", listvalue);

            var sql = " insert into " + tablename + "(" + col_str + ") values('" + col_value + "'); select SCOPE_IDENTITY();  ";


            var db = new DBHelper(constr);
            var data = db.getSalar(sql) + "";

            return data;
        }





        /// <summary>
        /// 批量的插入
        /// </summary>
        /// <param name="list">插入(更新,删除)的类型的列表值</param>
        /// <param name="ignorecol">忽略的属性列</param>
        public void Action(List<T> list, params string[] needcol)
        {
            foreach (var di in list)
            {
                Action(di, needcol);
            }
        }
    }
}
