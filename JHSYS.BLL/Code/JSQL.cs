using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHSYS.Code;
using JHSYS.DAL;

namespace JHSYS.BLL
{
    public class JSQL
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Files">查询字段</param>
        /// <param name="Where">查询条件</param>
        /// <param name="sp">查询参数</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string Table, string Files, string Where, SqlParameter [] sp, string OrderBy)
        {
            DataTable dt = new DataTable();
            try
            {
                if(!(string.IsNullOrEmpty(Table)|| string.IsNullOrEmpty(Files) || string.IsNullOrEmpty(Where)))
                {
                    string sql = Jcode.SelectSqlOrderBy(Table,Files,Where,OrderBy);
                    dt =new SQLHelp().GetTable(sql, sp);
                }else
                {
                    dt = null;
                }
            }
            catch(Exception ex) 
            {
                dt = null;
            }
            return dt;
        }

        /// <summary>
        /// 检验是否存在
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <param name="Files">字段名</param>
        /// <param name="Where">条件</param>
        /// <param name="sp">参数</param>
        /// <returns></returns>
        public static bool CheckFile(string Table, string Files, string Where, SqlParameter[] sp)
        {
                string sql = Jcode.SelectSql(Table, Files, Where);
                var dt = new SQLHelp().GetTable(sql, sp);
                return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Files">字段集合</param>
        /// <param name="Where">条件</param>
        /// <param name="sp">@参数</param>
        /// <returns></returns>
        public static string Update(string Table, string[] Files, string[] value, string Where, SqlParameter[] sp)
        {
            string sql = Jcode.UpdateSql(Table,Files, value, Where);//生成Sql语句
            SqlParameter[] valuesp = Jcode.SetArrayToSqlParameter(Files,value, sp);//生成更新参数语句
            //SqlParameter[] spAll = Jcode.SetToSqlParameter(valuesp, sp);//更新参数语句+查询参数语句
            var dt = new SQLHelp().ExecuteNoQuery(sql, valuesp);
            if (!dt)
            {
                return "Err:更新失败";
            }
            return "OK:更新成功";
        }

    }
}
