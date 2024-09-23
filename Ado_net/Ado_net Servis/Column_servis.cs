using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        public void Create_Column(string tableName)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string ColumnName = "";
                Console.Write("Column Name: ");
                ColumnName = Console.ReadLine();
                while (string.IsNullOrEmpty(ColumnName))
                {
                    Console.WriteLine("Kiritilmadi!!! ");
                    ColumnName = Console.ReadLine();
                }

                string ColumnType = "";
                Console.Write("Column Type: --> Misol uchun[integer] kiriting: ");
                ColumnType = Console.ReadLine();
                while (string.IsNullOrEmpty(ColumnType))
                {
                    Console.WriteLine("Kiritilmadi!!! ");
                    ColumnType = Console.ReadLine();
                }

                string Query = $"ALTER TABLE {tableName} ADD COLUMN {ColumnName} {ColumnType};";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = Query;
                    int num = res.ExecuteNonQuery();
                    Console.WriteLine("\n** Add Column **");
                }
                connection.Close();
            }
        }

        public List<string> List_Column(string tabl)
        {
            using (var connection = new NpgsqlConnection(Program. ConnectionString))
            {
                connection.Open();
                var lis = new List<string>();

                DataTable schema = connection.GetSchema("Columns", new string[] { null, null, tabl });

                Console.WriteLine("\nCOLUMN_NAME              |    DATA_TYPE");
                Console.WriteLine(new string('_', 35) + "\n");
                foreach (DataRow row in schema.Rows)
                {
                    Console.WriteLine($"{row["COLUMN_NAME"],-25}|    {row["DATA_TYPE"]}");
                    lis.Add($"{row["COLUMN_NAME"]}");
                }
                connection.Close();
                return lis;
            }
        }

        public void Update_Column(string MavjudDataBase,string ColumnName)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();
                string Type_New_Name= "";
                try
                {
                    Console.WriteLine("Type_New_Name: ");
                    Type_New_Name = Console.ReadLine();
                    while (string.IsNullOrEmpty(Type_New_Name))
                    {
                        Console.WriteLine("Kiritilmadi!!! ");
                        Type_New_Name = Console.ReadLine();
                    }

                    string query = $"ALTER TABLE \"{MavjudDataBase}\" ALTER COLUMN \"{ColumnName}\" TYPE \"{Type_New_Name}\";";
                
                    using (NpgsqlCommand res = connection.CreateCommand())
                    {
                        res.CommandText = query;
                        res.ExecuteNonQuery();
                        Console.WriteLine("\n** Update_Type **");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nColumn TYPE noto'g'ri!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'\n");
                }
            }
        }

        public void Delete_Column(string MavjudDataBase,string ColumnN)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                connection.Open();

                string query = $"ALTER TABLE \"{MavjudDataBase}\" DROP COLUMN \"{ColumnN}\";";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = query;
                    res.ExecuteNonQuery();
                    Console.WriteLine("\n** Delete_Column **");
                }
            }
        }
    }
}
