using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;

namespace Cantilever
{

    public class DataReader
    {
        //string path = Path.GetFullPath(@"C:\Users\Ahmad\Desktop\Adj Racking Pallets\Cantilever\Cantilever\DATA\db.accdb");
        string path=Path.GetFullPath(@"C:\Users\Ahmad\Desktop\Adj Racking Pallets\Cantilever\API\bin\Release\net6.0\DATA\db.accdb");
        string webpath ="data/db.accdb";
        public DataReader()
        {

        }
        public Dictionary<int,Material> ReadMaterials()
        {
            string physicalPath = MapVirtualPathToPhysical(webpath);
            Dictionary<int, Material> materials = new Dictionary<int, Material>();
            //string connectionString = String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Persist Security Info=False;");

            string connectionString =String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={physicalPath};Persist Security Info=False;");
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
        public Dictionary<int,RectSection> ReadSections()
        {
            string physicalPath = MapVirtualPathToPhysical(webpath);
            Dictionary<int, RectSection> sections = new Dictionary<int, RectSection>();
            //string connectionString = String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Persist Security Info=False;");
            string connectionString =String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={physicalPath};Persist Security Info=False;");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Sections";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            int id = int.Parse(reader["ID"].ToString());
                            double b = double.Parse(reader["B"].ToString());
                            double d = double.Parse(reader["D"].ToString());
                            sections.Add(id,new RectSection(id,name, b,d));
                        }
                    }
                }

                connection.Close();
            }
            return sections;
        }
        private string MapVirtualPathToPhysical(string virtualPath)
        {
            // Replace this with the root directory of your application
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Remove the tilde (~) if it exists in the virtual path
            virtualPath = virtualPath.Replace("~", "");

            // Combine the root directory with the virtual path
            string physicalPath = Path.Combine(rootDirectory, virtualPath.TrimStart('/').Replace('/', '\\'));

            // Normalize the path to handle any ".." or extra slashes
            physicalPath = Path.GetFullPath(physicalPath);

            return physicalPath;
        }
    }




}
