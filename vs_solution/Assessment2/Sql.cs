using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Assessment2
{
    class Sql
    {
        const string CONNECTION_STRING = "server=localhost;user=nmt_fleet_manager_user;database=nmt_fleet_manager;port=3306;password=Password1";

        private static MySqlConnection connection;

        public static bool TestConnection()
        {
            if (GetConnection())
            {
                CloseConnection();
                return true;
            }

            return false;
        }

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

        private static void CloseConnection()
        {
            connection.Close();
        }

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

        public static T sqlSelect<T>(ulong id, string field = "id")
        {
            string sql = "Select * from " + typeof(T).Name;
            sql += " Where " + field + " = " + id.ToString();

            return GetItem<T>(selectTable(sql).Rows[0]);
        }

        public static List<T> sqlSelectAll<T>(string filter = "")
        {
            string sql = "Select * from " + typeof(T).Name;

            return ConvertDataTable<T>(selectTable(sql));
        }

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
