using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JHSYS.Code;

namespace JHSYS.DAL
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
                    string sql = Jcode.SelectSql(Table,Files,Where,OrderBy);
                    dt =new SQLHelp().GetTable(sql, sp);
                }
            }
            catch 
            {
                dt = null;
            }
            return dt;
        }

    }
}
