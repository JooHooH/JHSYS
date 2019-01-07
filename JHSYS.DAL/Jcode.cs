using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHSYS.DAL
{
    public class Jcode
    {
        /// <summary>
        /// 生成通用查询 Select-SqlString 语句
        /// </summary>
        /// <param name="sTable">要查询的表名称</param>
        /// <param name="sFields">要查询的字段，多个用逗号(,)隔开，需解密字段以#开头</param>
        /// <param name="sWhere">查询条件</param>
        /// <param name="sOrderBy">排序条件</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SelectSql(string sTable, string sFields, string sWhere = "", string sOrderBy = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select {0} from {1} where {2} {3}", sFields, sTable, sWhere, sOrderBy));
            return sb.ToString();
        }
    }
}
