using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;

namespace Cantilever
{

    public class DataReader
    {
        string path = Path.GetFullPath(@"C:\Users\Ahmad\Desktop\Adj Racking Pallets\Cantilever\Cantilever\DATA\db.accdb");
        public DataReader()
        {

        }

        public Dictionary<int,Material> ReadMaterials()
        {
            Dictionary<int, Material> materials = new Dictionary<int, Material>();
            string connectionString =String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Persist Security Info=False;");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Materials";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            int id = int.Parse(reader["ID"].ToString());
                            double ym = double.Parse(reader["YoungsModulas"].ToString());
                            materials.Add(id,new Material(id, name, ym));
                        }
                    }
                }

                connection.Close();
            }
            return materials;
        }
    }




}
