using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JHSYS.BLL
{
    public class Jcode
    {
        /// <summary>
        /// 生成通用查询 Select-SqlOrderBy 语句
        /// </summary>
        /// <param name="sTable">要查询的表名称</param>
        /// <param name="sFields">要查询的字段，多个用逗号(,)隔开，需解密字段以#开头</param>
        /// <param name="sWhere">查询条件</param>
        /// <param name="sOrderBy">排序条件</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SelectSqlOrderBy(string sTable, string sFields, string sWhere = "", string sOrderBy = "")
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by " + sOrderBy;
            }
            sb.Append(string.Format("select {0} from {1} where {2} {3}", sFields, sTable, sWhere, sOrderBy));
            return sb.ToString();
        }

        /// <summary>
        /// 生成通用查询 Select-Sql 语句
        /// </summary>
        /// <param name="sTable">要查询的表名称</param>
        /// <param name="sFields">要查询的字段，多个用逗号(,)隔开，需解密字段以#开头</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns></returns>
        public static string SelectSql(string sTable, string sFields, string sWhere = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select {0} from {1} where {2}", sFields, sTable, sWhere));
            return sb.ToString();
        }

        /// <summary>
        /// 拼接更新数据语句
        /// </summary>
        /// <param name="sTable">表名</param>
        /// <param name="sFields">字段名</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns></returns>
        public static string UpdateSql(string sTable, string[] sFields, string[] values, string sWhere = "")
        {
            if (sFields.Length <= 0)
            {
                return "Err:字段不能为空";
            }
            if (!string.IsNullOrEmpty(sWhere))
            {
                sWhere = " where " + sWhere;
            }
            string s=getValueString(sFields);
            if (s.StartsWith("Err"))
            {
                return "Err:字段转换出错";
            }
            string sql = string.Format("update {0} set {1} {2}", sTable, s, sWhere);
            return sql;
        }

        /// <summary>
        /// 字段集转换成 "item=@valueitem"
        /// </summary>
        /// <param name="sFields"></param>
        /// <returns></returns>
        public static string getValueString(string[] sFields)
        {
            if (sFields.Length <= 0)
            {
                return "Err:字段不能为空";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sFields.Length; i++)
            {
                    if (sb.ToString() == "")
                    {
                        sb.Append(string.Format("{0}=@value{0}", sFields[i]));
                    }
                    else
                    {
                        sb.Append(string.Format(",{0}=@value{0}", sFields[i]));
                    }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字段集转换成 "item=@valueitem"
        /// </summary>
        /// <param name="sFields"></param>
        /// <returns></returns>
        public static string getValueStringP(string[] sFields, string[] values)
        {
            if (sFields.Length <= 0)
            {
                return "Err:字段不能为空";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sFields.Length; i++)
            {
                if (sb.ToString() == "")
                {
                    sb.Append(string.Format("{0}={1}", sFields[i], values[i]));
                }
                else
                {
                    sb.Append(string.Format(",{0}={1}", sFields[i], values[i]));
                }
            }
            return sb.ToString();
        }

        /// <summary>  
        /// 利用反射和泛型:DataTable转为List  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> DataTableToList<T>(DataTable dt)
             where T : class, new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i], null);
                }
                modelList.Add(model);
            }

            return modelList.Count > 0 ? modelList : null;

        }

        //--------------------------------------------------------------
        #region 拼接数据库插入语句:List型
        /// <summary>
        /// 拼接数据库插入语句:List型
        /// </summary>
        /// <param name="table">数据库表名</param>
        /// <param name="filed">字段集</param>
        /// <param name="values">数据集</param>
        /// <returns></returns>
        public static string getInsertListString(string table, List<string> filed, List<string> values)
        {
            string ret = "";
            if (string.IsNullOrEmpty(table))
            {
                return ret = "Err:表名不能为空!";
            }
            if (filed.Count <= 0)
            {
                return ret = "Err:字段名不能为空!";
            }
            if (values.Count <= 0)
            {
                return ret = "Err:数据不能为空!";
            }
            string f = getListToString(filed);
            string v = getListToStringP(filed);
            ret = string.Format("insert into {0} ({1}) values({2})", table, f, v);
            return ret;
        }
        #endregion

        #region 拼接数据库更新语句:List
        public static string getUpdateListString(string table, List<string> filed, List<string> values, string sWhere)
        {
            string ret = "";
            if (string.IsNullOrEmpty(table))
            {
                return ret = "Err:表名不能为空!";
            }
            if (filed.Count <= 0)
            {
                return ret = "Err:字段名不能为空!";
            }
            if (values.Count <= 0)
            {
                return ret = "Err:数据不能为空!";
            }
            string f = getSetListToUpdate(filed);
            //string v = getListToStringP(values);
            //update table set item1=@item1,item2=@item2 where id=@id
            ret = string.Format("update {0} set {1} where {2}", table, f, sWhere);
            return ret;
        }
        #endregion

        #region List转String
        /// <summary>
        /// List转String
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static string getListToString(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i] + ",");
            }
            string ret = sb.ToString();
            return ret.Trim(',');
        }
        #endregion

        #region List转@集合
        /// <summary>
        /// List转@集合
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static string getListToStringP(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append("@" + list[i] + ",");
            }
            string ret = sb.ToString();
            return ret.Trim(',');
        }
        #endregion

        #region item1=@item1
        /// <summary>
        /// item1=@item1
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string getSetListToUpdate(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (sb.ToString() == "")
                {
                    sb.Append(string.Format("{0}=@value{0}", list[i]));
                }
                else
                {
                    sb.Append(string.Format(",{0}=@value{0}", list[i]));
                }
            }
            string ret = sb.ToString();
            return ret;
        }
        #endregion

        #region 生成SqlParameter[]
        /// <summary>
        /// 生成SqlParameter[]
        /// </summary>
        /// <param name="filed">字段集</param>
        /// <param name="values">数据集</param>
        /// <returns></returns>
        public static SqlParameter[] getListToSqlParameter(List<string> filed, List<string> values)
        {
            int num = filed.Count;
            if (num != values.Count)
            {
                return null;
            }
            SqlParameter[] array = new SqlParameter[num];
            try
            {
                for (int i = 0; i < filed.Count; i++)
                {
                    array[i] = new SqlParameter("@" + filed[i], values[i]);
                }
            }
            catch
            {
                return null;
            }
            return array;

        }
        #endregion

        #region 生成SqlParameter[]：List型
        /// <summary>
        /// 生成SqlParameter[] @value+filed:List型
        /// </summary>
        /// <param name="filed">字段集</param>
        /// <param name="values">数据集</param>
        /// <returns></returns>
        public static SqlParameter[] SetListToSqlParameter(List<string> filed, List<string> values)
        {
            int num = filed.Count;
            if (num != values.Count)
            {
                return null;
            }
            SqlParameter[] array = new SqlParameter[num];
            try
            {
                for (int i = 0; i < filed.Count; i++)
                {
                    array[i] = new SqlParameter("@value" + filed[i], values[i]);
                }
            }
            catch
            {
                return null;
            }
            return array;

        }
        #endregion

        #region 生成SqlParameter[] : Array
        /// <summary>
        /// 生成SqlParameter[] @value+filed : Array型
        /// </summary>
        /// <param name="filed">字段集</param>
        /// <param name="values">数据集</param>
        /// <returns></returns>
        public static SqlParameter[] SetArrayToSqlParameter(string[] filed, string[] values, SqlParameter[] sp)
        {
            int spnum = 0;
            int num = filed.Length;
            if (num != values.Length)
            {
                return null;
            }
            if (sp != null && sp.Length > 0)
            {
                spnum = sp.Length;
            }

            SqlParameter[] array = new SqlParameter[num+spnum];
            try
            {
                for (int i = 0; i < num; i++)
                {
                    array[i] = new SqlParameter("@value" + filed[i], values[i]);
                }
            }
            catch
            {
                return null;
            }

            if (spnum > 0)
            {
                int num4 = num;
                int num5 = num + spnum - 1;
                for (int j = num4; j <= num5; j++)
                {
                    array[j] = new SqlParameter(sp[j - num].ParameterName, RuntimeHelpers.GetObjectValue(sp[j - num].Value));
                }
            }
            return array;

        }
        #endregion

        #region 两个SqlParameter[] 拼接成一个SqlParameter[]
        public static SqlParameter[] SetToSqlParameter(SqlParameter[] valuesp, SqlParameter[] sp)
        {
            int num = valuesp.Length + sp.Length;
            SqlParameter[] allsp = new SqlParameter[num];
            List<SqlParameter> listsp = new List<SqlParameter>();
            if (valuesp != null && valuesp.Length > 0)
            {
                foreach (SqlParameter item1 in valuesp)
                {
                    int i = 0;
                    allsp[i]=item1;
                    if (i == valuesp.Length - 1)
                        break;
                    i++;
                }
            }
            if (sp != null && sp.Length > 0)
            {
                foreach (SqlParameter item2 in sp)
                {
                    int i = valuesp.Length;
                    allsp[i] = item2;
                    if (i == num - 1)
                        break;
                    i++;
                }
                
            }

            return allsp.Length > 0 ? allsp : null;
        }
        #endregion
    }
}
