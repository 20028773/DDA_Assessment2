using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Assessment2
{
    class Sql
    {
        const string CONNECTION_STRING = "server=localhost;user=nmt_demo_user;database=nmt_demo;port=3306;password=Password1";

        private static MySqlConnection connection;


        private static void GetConnection()
        {
            connection = new MySqlConnection(CONNECTION_STRING);
            connection.Open();
        }


        private void CloseConnection()
        {
            connection.Close();
        }

        public static List<T> Load<T>(T item)
        {
            string typeName = typeof(T).Name;

            List<T> list = new List<T>();

            string sql = "select * from " + typeName;

            GetConnection();

            //Type genericType = typeof(List<>);
            //Type[] listOfTypeArgs = new[] { typeof(T) };
            //var newObject = Activator.CreateInstance(genericType.MakeGenericType(listOfTypeArgs));

            using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
            {
                MySqlDataReader data = cmdSel.ExecuteReader();

                while (data.Read())
                {
                    //var item3 = Activator.CreateInstance
                    //item = default(T);
                    foreach (var item2 in item.GetType().GetProperties())
                    {
                        if (item2.GetCustomAttributesData().Count == 0)
                        {
                            switch (item2.PropertyType.Name)
                            {
                                case "String":
                                    item2.SetValue(item, data.GetString(item2.Name));
                                    break;
                                case "Double":
                                    item2.SetValue(item, data.GetDouble(item2.Name));
                                    break;
                                case "Int32":
                                    item2.SetValue(item, data.GetInt32(item2.Name));
                                    break;
                                case "DateTime":
                                    item2.SetValue(item, data.GetDateTime(item2.Name));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    list.Add(item);
                }
            }

            return list;
        }

        public static void Save<T>(List<T> saveList)
        {
            List<string> auxList = new List<string>();

            string sql = "Insert into " + typeof(T).Name + " (";

            foreach (var item in typeof(T).GetProperties())
            {
                if (item.GetCustomAttributesData().Count == 0)
                {
                    auxList.Add(item.Name);
                }
            }

            sql += string.Join<string>(",", auxList) + ") VALUES ";

            int count = 0;

            foreach (var item in saveList)
            {
                auxList.Clear();

                sql += count == 0 ? "( " : ", (";

                foreach (var item2 in item.GetType().GetProperties())
                {
                    if (item2.GetCustomAttributesData().Count == 0)
                    {
                        switch (item2.PropertyType.Name)
                        {
                            case "String":
                                auxList.Add(string.Format("'{0}'", item2.GetValue(item)));
                                break;
                            case "DateTime":
                                auxList.Add(string.Format("'{0:s}'", item2.GetValue(item)));
                                break;
                            default:
                                auxList.Add(item2.GetValue(item).ToString());
                                break;
                        }
                    }
                }

                sql += string.Join<string>(",", auxList) + ") ";
                count++;
            }



            //switch (typeName)
            //{
            //    case "Vehicle":
            //        //item.GetType().GetProperties()[0].SetValue(item, data.GetInt32("id"));
            //        //item.GetType().GetProperties()[1].SetValue(item, data.GetString("make"));
            //        //item.GetType().GetProperties()[2].SetValue(item, data.GetString("model"));
            //        //item.GetType().GetProperties()[3].SetValue(item, data.GetInt16("year"));
            //        //item.GetType().GetProperties()[4].SetValue(item, data.GetString("registration"));
            //        //item.GetType().GetProperties()[5].SetValue(item, data.GetDouble("odometer"));
            //        //item.GetType().GetProperties()[6].SetValue(item, data.GetDouble("tank"));
            //        //item.GetType().GetProperties()[7].SetValue(item, data.GetDateTime("created_at"));
            //        break;

            //    default:
            //        break;
            //}


            // sql += "location_name, two_letter_code) select '" + sName + "', '" + sCode + "' ";

            try
            {
                using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                {
                    cmdSel.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                //return false;
            }

        }
    }
}
