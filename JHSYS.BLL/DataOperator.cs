
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;
using JHSYS.DAL;
using System.Collections;

namespace JHSYS.BLL
{
    public class DataOperator
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static string GetTableName(Type t)
        {
            //Type DataType = t;
            //string ret = "";
            //if (!string.IsNullOrEmpty(DataType.Name))
            //{
            //    ret = DataType.Name;
            //}
            //return ret;
            Type DataType = t;
            var TableNamesAttributes = DataType.GetCustomAttributes(typeof(TableNamesAttribute), false);
            string TableName = null;
            if (TableNamesAttributes.Count() > 0)
            {
                TableName = (TableNamesAttributes.First() as TableNamesAttribute).TableName;
            }
            return TableName;
        }

        /// <summary>
        /// 更新数据:List型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <param name="data"></param>
        /// <param name="sWhere"></param>
        /// <param name="pams"></param>
        /// <returns></returns>
        public static string UpdateData<T>(string TableName, T data, string sWhere = null, SqlParameter[] pams = null)
            where T : class, new()
        {
            var msg = "";
            try
            {
                if (data != null)
                {
                    List<string> filedName = new List<string>();
                    List<string> values = new List<string>();
                    var columnProperties = data.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DataBaseColumnAttribute), false) != null);
                    foreach (PropertyInfo item in columnProperties)
                    {

                        object value = item.GetValue(data);
                        if (value != null)
                        {
                            filedName.Add(item.Name);
                            values.Add(value.ToString());
                        }
                    }
                    msg = ExecuteNoQueryList(TableName, filedName, values, sWhere, pams);
                    if (msg.StartsWith("Err"))
                    {
                        throw new Exception(msg);
                    }
                }
            }
            catch(Exception ex)
            {
                msg = string.Format("Err:异常,{0}",ex.Message);
                return msg;
            }
            return msg;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static List<T> GetDataList<T>(string sFields, string sWhere = "", string sOrderBy = "", SqlParameter[] sParameter = null)
            where T : class, new()
        {

            string TableName = GetTableName(typeof(T));
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("无法确定表名！");
            }
            DataTable dt = JSQL.GetDataTable(TableName, sFields, sWhere, sParameter, sOrderBy);
            if (dt == null && dt.Rows.Count == 0)
            {
                return null;
            }
            //实例化一个List<>泛型集合  
            List<T> modelList = Jcode.DataTableToList<T>(dt);
            return modelList;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        //public static string InsertMileageData(object data, int dbIndex = 0)
        //{
        //    string TableName = GetTableName(data.GetType());
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (data != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = data.GetType().GetProperties();
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.Insert(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //        return msg;
        //    }
        //    return null;

        //}
        /// <summary>
        /// 插入数据:事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        //public static void InsertMileageDataT<T>(T data, int dbIndex = 0)
        //{
        //    string TableName = GetTableName(data.GetType());
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (data != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = data.GetType().GetProperties();
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null && !string.IsNullOrEmpty(value.ToString()))
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.InsertT(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new InvalidOperationException(msg);

        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("data不能为null");
        //    }


        //}

        /// <summary>
        /// 插入数据:返回主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        //public static string InsertMileageDataT2<T>(T data, int dbIndex = 0)
        //{
        //    string ret = "";
        //    string TableName = GetTableName(data.GetType());
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (data != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = data.GetType().GetProperties();
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null && !string.IsNullOrEmpty(value.ToString()))
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.InsertT(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new InvalidOperationException(msg);
        //        }
        //        ret = msg;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("data不能为null");
        //    }

        //    return ret;
        //}

        /// <summary>
        /// 根据columnname特性插入数据库，使用事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dbIndex"></param>
        //public static void InsertIntoTableByColumnNameT<T>(T model, int dbIndex = 0)
        //{
        //    string TableName = GetTableName(model.GetType());
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (model != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = model.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(ColumnNameAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(model);
        //            if (value != null)
        //            {
        //                var attribute = item.GetCustomAttribute(typeof(ColumnNameAttribute), false) as ColumnNameAttribute;
        //                filedName.Add(attribute.ColumnName);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.InsertT(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }

        //}
        /// <summary>
        /// 根据columnname特性插入数据库，不使用事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dbIndex"></param>
        //public static void InsertIntoTableByColumnName<T>(T model, int dbIndex = 0)
        //{
        //    string TableName = GetTableName(model.GetType());
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (model != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = model.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(ColumnNameAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(model);
        //            if (value != null)
        //            {
        //                var attribute = item.GetCustomAttribute(typeof(ColumnNameAttribute), false) as ColumnNameAttribute;
        //                filedName.Add(attribute.ColumnName);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.Insert(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }
        //}
        ///// <summary>


        #region 插入数据:List型
        /// <summary>
        /// 插入数据:List型
        /// </summary>
        /// <param name="T">实体类</param>
        /// <param name="data">数据集</param>
        /// <returns></returns>
        public static string InsertListData<T>(T data, int dbIndex = 0)
            where T : class, new()
        {
            string TableName = GetTableName(typeof(T));
            if (string.IsNullOrEmpty(TableName))
            {
                throw new Exception("无法确定表名！");
            }
            if (data != null)
            {
                List<string> filedName = new List<string>();
                List<string> values = new List<string>();
                var columnProperties = data.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DataBaseColumnAttribute), false) != null);
                foreach (PropertyInfo item in columnProperties)
                {

                    object value = item.GetValue(data);
                    if (value != null)
                    {
                        filedName.Add(item.Name);
                        values.Add(value.ToString());
                    }
                }
                var msg = InsertList(TableName, filedName, values);
                return msg;
            }
            return null;

        }
        #endregion

        #region 插入数据:List型
        /// <summary>
        /// 插入数据:List型
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="filed">字段集</param>
        /// <param name="values">数据集</param>
        /// <returns></returns>
        private static string InsertList(string table, List<string> filed, List<string> values)
        {
            var msg="";
            try
            {
                string SQL = Jcode.getInsertListString(table, filed, values);
                if (SQL.StartsWith("Err"))
                {
                    return SQL;
                }
                SQLHelp sh = new SQLHelp();
                msg = sh.Insert(SQL, Jcode.getListToSqlParameter(filed, values));
                if (msg.StartsWith("Err"))
                {
                    throw new Exception(msg);
                }
            }
            catch(Exception ex)
            {
                msg = string.Format("Err:异常,{0}",ex.Message);
                return msg;
            }
            return msg;
        }
        #endregion

        #region 更新数据:List型
        private static string ExecuteNoQueryList(string table, List<string> filed, List<string> values,string sWhere, SqlParameter[] sp)
        {
            var msg = "";
            try
            {
                string SQL = Jcode.getUpdateListString(table, filed, values, sWhere);
                if (SQL.StartsWith("Err"))
                {
                    return SQL;
                }
                SQLHelp sh = new SQLHelp();
                //SetListToSqlParameter
                List<SqlParameter> listsp = new List<SqlParameter>();
                
                //where条件参数
                foreach(SqlParameter itme in sp)
                {
                    listsp.Add(itme);
                }
                //更新字段参数
                foreach (SqlParameter item2 in Jcode.SetListToSqlParameter(filed, values))
                {
                    listsp.Add(item2);
                }
                if (listsp.Count == 0)
                {
                    return msg = string.Format("Err:参数不能为空！");
                }
                SqlParameter[] spAll = new SqlParameter[listsp.Count-1];

                foreach(SqlParameter item3 in listsp)
                {
                    int indexer = 0;
                    spAll[indexer] = item3;
                    indexer++;
                }
                bool ret = sh.ExecuteNoQuery(SQL, spAll);
                if (!ret)
                {
                    msg = string.Format("Err:更新失败");
                    throw new Exception(msg);
                }
                msg = "OK:更新成功！";
            }
            catch (Exception ex)
            {
                msg = string.Format("Err:异常,{0}", ex.Message);
                return msg;
            }
            return msg;
        }
        #endregion

    

        //public static string InsertMileageData1<T>(T data, int dbIndex = 0)
        //    where T : class, new()
        //{
        //    string TableName = GetTableName(typeof(T));
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (data != null)
        //    {
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = data.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DataBaseColumnAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.InsertT(TableName, filedName.ToArray(), values.ToArray(), dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //        return msg;
        //    }
        //    return null;

        //}
        //public static bool CheckAppear(string tableName, string sWhere, SqlParameter[] pams, int dbIndex = 0)
        //{
        //    var data = ZYUClasses.ZYUSQL.getDataReader(tableName, "*", condString: sWhere, sParameter: pams, dbIndex: dbIndex);
        //    if (data.IsError)
        //    {
        //        throw new Exception(data.ErrorMessage);
        //    }
        //    return data.RowsCount > 0;
        //}

        //public static void UpdateMileageData<T>(T data, int dbIndex = 0)
        //    where T : class, new()
        //{
        //    string TableName = GetTableName(typeof(T));
        //    if (string.IsNullOrEmpty(TableName))
        //    {
        //        throw new Exception("无法确定表名！");
        //    }
        //    if (data != null)
        //    {
        //        Type t = data.GetType();
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = t.GetProperties().Where(x => x.GetCustomAttribute(typeof(DataBaseColumnAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var id = t.GetProperties().Where(x => x.GetCustomAttribute(typeof(KeyAttribute), false) != null);// t.GetCustomAttribute(typeof(KeyAttribute), false);
        //        if (id.Count() <= 0)
        //        {
        //            throw new Exception("对象没有主键属性！");
        //        }
        //        string idName = id.First().Name;
        //        var msg = ZYUClasses.ZYUSQL.Update(TableName, filedName.ToArray(), values.ToArray(), string.Format("{0}=@pam", idName), new SqlParameter[] { new SqlParameter("pam", id.First().GetValue(data)) }, dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }

        //}
        //public static void UpdateData<T>(string tableName, T data, string sWhere = null, SqlParameter[] pams = null, int dbIndex = 0)
        //    where T : class, new()
        //{
        //    if (data != null)
        //    {
        //        Type t = data.GetType();
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = t.GetProperties().Where(x => x.GetCustomAttribute(typeof(ColumnNameAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                var attribute = item.GetCustomAttribute(typeof(ColumnNameAttribute), false) as ColumnNameAttribute;
        //                filedName.Add(attribute.ColumnName);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.Update(tableName, filedName.ToArray(), values.ToArray(), sWhere, pams, dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }

        //}
        //public static void UpdateDataT<T>(string tableName, T data, string sWhere = null, SqlParameter[] pams = null, int dbIndex = 0)
        //   where T : class, new()
        //{
        //    if (data != null)
        //    {
        //        Type t = data.GetType();
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = t.GetProperties().Where(x => x.GetCustomAttribute(typeof(ColumnNameAttribute), false) != null);
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                var attribute = item.GetCustomAttribute(typeof(ColumnNameAttribute), false) as ColumnNameAttribute;
        //                filedName.Add(attribute.ColumnName);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.UpdateT(tableName, filedName.ToArray(), values.ToArray(), sWhere, pams, dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }

        //}
        //public static void UpdateDataTNotByName<T>(string tableName, T data, string sWhere = null, SqlParameter[] pams = null, int dbIndex = 0)
        //  where T : class, new()
        //{
        //    if (data != null)
        //    {
        //        Type t = data.GetType();
        //        List<string> filedName = new List<string>();
        //        List<string> values = new List<string>();
        //        var columnProperties = t.GetProperties();
        //        foreach (PropertyInfo item in columnProperties)
        //        {

        //            object value = item.GetValue(data);
        //            if (value != null)
        //            {
        //                filedName.Add(item.Name);
        //                values.Add(value.ToString());
        //            }
        //        }
        //        var msg = ZYUClasses.ZYUSQL.UpdateT(tableName, filedName.ToArray(), values.ToArray(), sWhere, pams, dbIndex);
        //        if (msg.StartsWith("Err"))
        //        {
        //            throw new Exception(msg);

        //        }
        //    }

        //}
        //public static bool IsAppear<T>(string idValue, int dbIndex = 0)
        //    where T : class, new()
        //{

        //    string TableName = GetTableName(typeof(T));
        //    var id = typeof(T).GetProperties().Where(x => x.GetCustomAttribute(typeof(KeyAttribute), false) != null);// t.GetCustomAttribute(typeof(KeyAttribute), false);
        //    if (id.Count() <= 0)
        //    {
        //        throw new Exception("对象没有主键属性！");
        //    }
        //    int ret = ZYUClasses.ZYUSQL.cExist(TableName, id.First().Name, idValue, dbIndex);

        //    return ret > 0;
        //}

        //public static List<T> ExecuteProduce<T>(string procName, int dbIndex = 0, params SqlParameter[] pams)
        //      where T : class, new()
        //{
        //    var data = ZYUClasses.ZYUSQL.ExecuteProduce1(procName, pams, dbIndex);
        //    if (data.IsError)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return ConvertToModel<T>(data);
        //    }
        //}
        //public static List<T> ConvertToModel<T>(DataTable dt)
        //    where T : class, new()
        //{
        //    // 定义集合    
        //    List<T> ts = new List<T>();
        //    // 获得此模型的类型   
        //    Type type = typeof(T);
        //    string tempName = "";

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        T t = new T();
        //        // 获得此模型的公共属性      
        //        PropertyInfo[] propertys = t.GetType().GetProperties();
        //        foreach (PropertyInfo pi in propertys)
        //        {
        //            tempName = pi.Name;  // 检查DataTable是否包含此列    

        //            if (dt.Columns.Contains(tempName))
        //            {
        //                // 判断此属性是否有Setter      
        //                if (!pi.CanWrite) continue;

        //                object value = dr[tempName];
        //                if (value != DBNull.Value)
        //                    pi.SetValue(t, value, null);
        //            }
        //        }
        //        ts.Add(t);
        //    }
        //    return ts.Count > 0 ? ts : null;
        //}

    }
}
