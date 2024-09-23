using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        public void Create_Table()
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string TableName = "";
                bool exit = true;
                while (exit)
                {
                    Console.Write("Table Name: ");
                    TableName = Console.ReadLine();
                    while (string.IsNullOrEmpty(TableName))
                    {
                        Console.WriteLine("Kiritilmadi!!! ");
                        TableName = Console.ReadLine();
                    }
                    string tableMavjudmi = $"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{TableName}');";

                    using (var res = new NpgsqlCommand(tableMavjudmi, connection))
                    {
                        var result = res.ExecuteScalar();
                        if (result != null && (bool)result)
                        {
                            Console.WriteLine("Jadval allaqachon mavjud.");
                        }
                        else exit = false;
                    }
                }
                string Query = $@"
                        CREATE TABLE {TableName} ();";

                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = Query;
                    int num = res.ExecuteNonQuery();
                    Console.WriteLine("\n** Add Table **");
                }
                connection.Close();
            }
        }

        public List<string> List_Table()
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                List<string> lis = new List<string>();

                DataTable catalogs = connection.GetSchema("Tables");

                foreach (DataRow row in catalogs.Rows)
                {
                    lis.Add($"{row["Table_Name"]}");
                }
                connection.Close();
                return lis;
            }
        }

        public void Update_Table(string MavjudDataBase)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string Table_New_Name = "";

                Console.Write("Table New Name: ");
                Table_New_Name = Console.ReadLine();
                while (string.IsNullOrEmpty(Table_New_Name))
                {
                    Console.WriteLine("Kiritilmadi!!! ");
                    Table_New_Name = Console.ReadLine();
                }

                string query = $"ALTER TABLE \"{MavjudDataBase}\" RENAME TO \"{Table_New_Name}\";";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = query;
                    res.ExecuteNonQuery();
                    Console.WriteLine("\n** Update_Table **");
                }
            }
        }

        public void Delete_Table(string MavjudDataBase)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();

                string query = $"DROP TABLE \"{MavjudDataBase}\";";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = query;
                    res.ExecuteNonQuery();
                    Console.WriteLine("\n** Delete_Table **");
                }
            }
        }
    }
}
