using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class CommonDBHelper
    {
        private string conName;

        /// <summary>
        /// 配置连接
        /// </summary>
        private string config_constr;

        public CommonDBHelper(string conName)
        {
            this.conName = conName;
            this.config_constr = ConfigurationManager.AppSettings[this.conName].ToString();
        }

        /// <summary>
        /// 得到sqldatareader
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns> 得到sqldatareader</returns>
        public SqlDataReader getReader(string procname, SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection(config_constr);

            con.Open();
            SqlCommand com = new SqlCommand(procname, con);
            com.CommandTimeout = 180;
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            com.CommandType = CommandType.StoredProcedure;
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <returns> 得到sqldatareader</returns>
        public SqlDataReader getReader(string procname, SqlParameter[] sp, CommandType ct)
        {
            SqlConnection con = new SqlConnection(config_constr);
            con.Open();
            SqlCommand com = new SqlCommand(procname, con);
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            com.CommandType = ct;
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <returns> 得到sqldatareader</returns>
        public SqlDataReader getReaders(string procname, SqlParameter[] sp, string constr)
        {
            SqlConnection con = new SqlConnection(config_constr);
            con.Open();
            SqlCommand com = new SqlCommand(procname, con);
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            com.CommandType = CommandType.StoredProcedure;
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 得到sqldatareader
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns> 得到sqldatareader</returns>
        public SqlDataReader getReader(string sql)
        {
            SqlConnection con = new SqlConnection(config_constr);
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            //com.CommandType = CommandType.StoredProcedure;
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行存储过程并返回受影响的行数
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns>执行存储过程并返回受影响的行数</returns>
        public int execute(string procname, SqlParameter[] sp)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(procname, con))
                {
                    if (sp != null)
                    {
                        com.Parameters.AddRange(sp);
                    }

                    com.CommandType = CommandType.StoredProcedure;
                    int count = com.ExecuteNonQuery();
                    con.Close();
                    return count;
                }
            }
        }

        /// <summary>
        /// 执行存储过程并返回受影响的行数
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns>执行存储过程并返回受影响的行数</returns>
        public int execute(string sql)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    int count = com.ExecuteNonQuery();
                    con.Close();
                    return count;
                }
            }
        }

        /// <summary>
        /// 执行存储过程并返回受影响的行数
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns>执行存储过程并返回受影响的行数</returns>
        public int execute(string procname, SqlParameter[] sp, CommandType ct)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(procname, con))
                {
                    if (sp != null)
                    {
                        com.Parameters.AddRange(sp);
                    }
                    com.CommandType = ct;
                    int count = com.ExecuteNonQuery();
                    return count;
                }
            }
        }

        /// <summary>
        /// 执行存储过程并返回首行首列数据
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns>执行存储过程并返回首行首列数据</returns>
        public object getSalar(string procname, SqlParameter[] sp)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(procname, con))
                {
                    if (sp != null)
                    {
                        com.Parameters.AddRange(sp);
                    }
                    com.CommandType = CommandType.StoredProcedure;
                    object count = com.ExecuteScalar();
                    con.Close();
                    return count;
                }
            }
        }

        public object getSalar(string sql)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.CommandType = CommandType.Text;
                    object count = com.ExecuteScalar();
                    con.Close();
                    return count;
                }
            }
        }

        /// <summary>
        /// 执行存储过程并返回首行首列数据
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">参数列表</param>
        /// <returns>执行存储过程并返回首行首列数据</returns>
        public object getSalar(string procname, SqlParameter[] sp, CommandType ct)
        {
            using (SqlConnection con = new SqlConnection(config_constr))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(procname, con))
                {
                    if (sp != null)
                    {
                        com.Parameters.AddRange(sp);
                    }
                    com.CommandType = ct;
                    object count = com.ExecuteScalar();

                    return count;
                }
            }
        }
    }
}
