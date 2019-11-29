using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Assessment2
{
    class Sql
    {
        /// <summary>
        /// DATABASE CONNECTION STRING
        /// </summary>
        const string CONNECTION_STRING = "server=localhost;user=nmt_fleet_manager_user;database=nmt_fleet_manager;port=3306;password=Password1";

        private static MySqlConnection connection;
        /// <summary>
        /// METHOD TO TEST DATABASE CONNECTION
        /// </summary>
        /// <returns></returns>
        public static bool TestConnection()
        {
            if (GetConnection())
            {
                CloseConnection();
                return true;
            }

            return false;
        }
        /// <summary>
        /// METHOD TO CONNECT TO THE DATABASE
        /// </summary>
        /// <returns></returns>
        private static bool GetConnection()
        {
            try
            {
                connection = new MySqlConnection(CONNECTION_STRING);
                connection.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// METHOD TO CLOSE THE DATABASE CONNECTION
        /// </summary>
        private static void CloseConnection()
        {
            if (connection != null)
                connection.Close();
        }
        /// <summary>
        /// METHOD THAT RETURNS THE QUERY INTO A DATATABLE
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static DataTable selectTable(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                if (GetConnection())
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(sql, connection))
                    {
                        da.Fill(dt);
                    }

                    CloseConnection();
                }
            }
            catch (Exception)
            {

            }

            return dt;
        }
        /// <summary>
        /// METHOD THAT EXECUTE THE SQL PASSED AS PARAMETER
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static bool sqlExecute(string sql)
        {
            try
            {

                if (GetConnection())
                {

                    using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                    {
                        cmdSel.ExecuteNonQuery();
                    }

                    CloseConnection();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// METHOD CONVERTS DATATABLE INTO A LIST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }
        /// <summary>
        /// METHOD RETURN A OBJECT FROM A DATA ROW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                    {
                        if (column.ColumnName == "rentType")
                            pro.SetValue(obj, (dr[column.ColumnName].ToString() == "Day" ? 0 : 1), null);
                        else
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }

            return obj;
        }
        /// <summary>
        /// METHOD CREATE DINAMICALLY A SELECT QUERY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static T sqlSelect<T>(ulong id, string field = "id")
        {
            string sql = "Select * from " + typeof(T).Name;
            sql += " Where " + field + " = " + id.ToString();

            return GetItem<T>(selectTable(sql).Rows[0]);
        }
        /// <summary>
        /// METHOD CREATE DINAMICALLY A SELECT QUERY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<T> sqlSelectAll<T>(string filter = "")
        {
            string sql = "Select * from " + typeof(T).Name;

            return ConvertDataTable<T>(selectTable(sql));
        }
        /// <summary>
        ///  METHOD CREATE DINAMICALLY A INSERT QUERY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObj"></param>
        /// <returns></returns>
        public static bool sqlInsert<T>(T classObj)
        {
            List<string> auxList = new List<string>();
            List<string> auxList2 = new List<string>();

            string sql = "Insert into " + typeof(T).Name + " (";

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    if (item.GetValue(classObj) != null && item.GetValue(classObj).ToString() != "0")
                    {
                        auxList.Add(item.Name);

                        switch (item.PropertyType.Name)
                        {
                            case "type":
                            case "String":
                                auxList2.Add(string.Format("'{0}'", item.GetValue(classObj)));
                                break;
                            case "DateTime":
                                auxList2.Add(string.Format("'{0:s}'", item.GetValue(classObj)));
                                break;
                            default:
                                auxList2.Add(item.GetValue(classObj).ToString());
                                break;
                        }
                    }
                }
            }

            sql += string.Join<string>(" ,", auxList) + ") VALUES (";
            sql += string.Join<string>(" ,", auxList2) + ") ";

            return sqlExecute(sql);
        }
        /// <summary>
        ///  METHOD CREATE DINAMICALLY A UPDATE QUERY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObj"></param>
        /// <returns></returns>
        public static bool sqlUpdate<T>(T classObj)
        {
            List<string> auxList = new List<string>();

            string sql = "Update " + typeof(T).Name + " Set ";
            string sWHereClause = string.Empty;

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    if (item.Name == "Id")
                    {
                        sWHereClause = " Where id = " + item.GetValue(classObj).ToString();
                    }
                    else
                    {
                        if (item.GetValue(classObj) != null && item.GetValue(classObj).ToString() != "0")
                        {
                            switch (item.PropertyType.Name)
                            {
                                case "type":
                                case "String":
                                    auxList.Add(string.Format("{0} = '{1}'", item.Name, item.GetValue(classObj)));
                                    break;
                                case "DateTime":
                                case "Nullable`1":
                                    auxList.Add(string.Format("{0} = '{1:s}'", item.Name, item.GetValue(classObj)));
                                    break;
                                default:
                                    auxList.Add(string.Format("{0} = {1}", item.Name, item.GetValue(classObj)));
                                    break;
                            }
                        }
                    }
                }
            }

            sql += string.Join<string>(" ,", auxList) + sWHereClause;

            return sqlExecute(sql);
        }
        /// <summary>
        ///  METHOD CREATE DINAMICALLY A DELETE QUERY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObj"></param>
        /// <returns></returns>
        public static bool sqlDelete<T>(T classObj)
        {
            List<string> auxList = new List<string>();

            string sql = "Delete ";
            sql += "From " + typeof(T).Name;

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    if (item.Name == "Id")
                    {
                        sql += " Where id = " + item.GetValue(classObj).ToString();
                        break;
                    }
                }
            }

            return sqlExecute(sql);
        }
    }
}
