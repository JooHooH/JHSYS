using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace JHSYS.BLL
{
    /// <summary>
    /// Home数据逻辑
    /// </summary>
    public class HomeDB
    {
        public static DataTable MenuParent(string ParentID)
        {
            int state = 1;
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@MenuState", state), new SqlParameter("@ParentID", ParentID) };
            var dt = JSQL.GetDataTable("Sys_Menu","*", "MenuState=@MenuState and ParentID=@ParentID",sp," MenuSort ");
            if (dt!=null&&dt.Rows.Count>0)
            {
                return dt;
            }
            return null;
        }

    }
}
