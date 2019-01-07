using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace JHSYS.DAL
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class SQLHelp
    {
        private SqlConnection conn;
        public SqlConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    conn = new SqlConnection(connStr);
                }
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;
            }
        }

        //查询:DataReader
        public SqlDataReader GetReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public SqlDataReader GetReader(string sql, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(paras);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        //查询:DataTable
        public DataTable GetTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, Conn);
            DataTable dt = new DataTable();
            dap.Fill(dt);
            conn.Close();
            return dt;
        }
        public  DataTable GetTable(string sql, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(paras);
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dap.Fill(dt);
            conn.Close();
            return dt;

        }

        //增改删
        public bool ExecuteNoQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            int result = cmd.ExecuteNonQuery();
            this.conn.Close();
            return result > 0;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="sql">数据库语句</param>
        /// <param name="paras">参数</param>
        /// <returns>主键</returns>
        public string Insert(string sql, SqlParameter[] paras)
        {
            sql += " SELECT @@IDENTITY AS returnName ";//返回主键
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(paras);
            int result = cmd.ExecuteNonQuery();
            this.conn.Close();
            return result.ToString();
        }

        public bool ExecuteNoQuery(string sql, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(paras);
            int result = cmd.ExecuteNonQuery();
            this.conn.Close();
            return result > 0;
        }

        //执行聚合函数
        public object ExecuteScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            object result = cmd.ExecuteScalar();
            this.conn.Close();
            return result;
        }
        public object ExecuteScalar(string sql, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(paras);
            object result = cmd.ExecuteScalar();
            this.conn.Close();
            return result;
        }

        //执行存储过程获取数据集(包括sql语句和带参数的SQL语句)
        //<img alt = "" src="http://images.cnblogs.com/OutliningIndicators/ContractedBlock.gif" class="code_img_closed" style="border: 0px; font-family: Helvetica, Tahoma, Arial, sans-serif; font-size: 14px; line-height: 25.1875px;" /><span class="cnblogs_code_collapse" style="font-family: Helvetica, Tahoma, Arial, sans-serif; font-size: 14px; line-height: 25.1875px;">执行存储过程获取数据集(查询)</span>
        //执行存储过程获取数据集(查询)
        public DataTable ExecuteProcSelect(string ProcName, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(ProcName, Conn);
            cmd.Parameters.AddRange(paras);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dap.Fill(dt);
            this.conn.Close();
            return dt;
        }


        //执行存储过程做增删改
        public bool ExecuteProcUpdate(string ProcName, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(ProcName, Conn);
            cmd.Parameters.AddRange(paras);
            cmd.CommandType = CommandType.StoredProcedure;
            int result = cmd.ExecuteNonQuery();
            this.conn.Close();
            return result > 0;
        }

        //执行事物(ADO.NET)
        public bool ExecuteTrasaction(string sqlStr, string TranName)
        {
            bool result = true;
            SqlTransaction tran = null;
            try
            {
                tran = Conn.BeginTransaction(TranName);
                SqlCommand cmd = new SqlCommand(sqlStr, Conn, tran);
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    tran.Commit();
                    result = true;
                }
                else
                {
                    tran.Rollback();
                    result = false;
                }
            }
            catch
            {
                tran.Rollback();
                result = false;
            }
            return result;
        }

       


    }
}
