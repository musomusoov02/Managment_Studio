using Npgsql;
using System.Data;
using System.Xml.Linq;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        #region Create_Database
        public void Create_Database()
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string DataBaseName = "";
                bool exit = true;
                while (exit)
                {
                    Console.Write("DataBase Name: ");
                    DataBaseName = Console.ReadLine();
                    while (string.IsNullOrEmpty(DataBaseName))
                    {
                        Console.WriteLine("Kiritilmadi!!! ");
                        DataBaseName = Console.ReadLine();
                    }
                    string DB_mavjudmi = $"SELECT 1 FROM pg_database WHERE datname = '{DataBaseName}';";

                    using (var res = new NpgsqlCommand(DB_mavjudmi, connection))
                    {
                        var result = res.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("Baza allaqachon mavjud.");
                            continue;
                        }
                        else exit = false;
                    }
                }
                string Query = $" CREATE DATABASE {DataBaseName};";

                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = Query;
                    int num = res.ExecuteNonQuery();
                    Console.WriteLine("\n** Add DataBase **");
                }
                connection.Close();
            }
        }
        #endregion

        public List<string> List_DataBase()
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                List<string> lis = new List<string>();

                DataTable catalogs = connection.GetSchema("Databases");

                foreach (DataRow row in catalogs.Rows)
                {
                    lis.Add($"{row["Database_Name"]}");
                }
                connection.Close();
                return lis;
            }
        }

        public void Update_DataBase(string MavjudDataBase)
        {
            using(var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string DataBase_New_Name = "";

                Console.Write("DataBase New Name: ");
                DataBase_New_Name = Console.ReadLine();
                while (string.IsNullOrEmpty(DataBase_New_Name))
                {
                    Console.WriteLine("Kiritilmadi!!! ");
                    DataBase_New_Name = Console.ReadLine();
                }
                
                string query = $"ALTER DATABASE \"{MavjudDataBase}\" RENAME TO \"{DataBase_New_Name}\";";
                using(NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = query;
                    res.ExecuteNonQuery();
                    Console.WriteLine("\n** Update_DataBase **");
                }
            }
        }

        public void Delete_DataBase(string MavjudDataBase)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();

                string query = $"DROP DATABASE \"{MavjudDataBase}\";";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = query;
                    res.ExecuteNonQuery();
                    Console.WriteLine("\n** Delete_DataBase **");
                }
            }
        }
    }
}
