using Npgsql;
using System.Data;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task<string> CreateTableAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    string TableName = "";
                    bool exit = true;

                    while (exit)
                    {
                        Console.Write("Table Name: ");
                        TableName = Console.ReadLine();

                        while (string.IsNullOrEmpty(TableName))
                        {
                            Console.WriteLine("Not Entered!!! ");
                            TableName = Console.ReadLine();
                        }
                        string TableCheck = $"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{TableName}');";
                        using (var res = new NpgsqlCommand(TableCheck, connection))
                        {
                            var result = await res.ExecuteScalarAsync();
                            if (result != null && (bool)result)
                            {
                                Console.WriteLine("Table Already Exists.");
                            }
                            else exit = false;
                        }
                    }
                    string Query = $@" CREATE TABLE {TableName} ();";

                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = Query;
                        await command.ExecuteNonQueryAsync();
                    }
                    await connection.CloseAsync();
                    return TableName;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "   <--  Program error!!!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return null;
            }
        }

        public async Task<List<string>> ListTableAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    List<string> list = new List<string>();
                    DataTable tables = await connection.GetSchemaAsync("Tables");

                    foreach (DataRow row in tables.Rows)
                    {
                        list.Add($"{row["Table_Name"]}");
                    }
                    await connection.CloseAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "   <--  Program error!!!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return null;
            }
        }

        public async Task UpdateTableAsync(string TableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    string NewTableName = "";
                    Console.Write("Table New Name: ");
                    NewTableName = Console.ReadLine();

                    while (string.IsNullOrEmpty(NewTableName))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        NewTableName = Console.ReadLine();
                    }
                    string query = $"ALTER TABLE \"{TableName}\" RENAME TO \"{NewTableName}\";";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Update Table **");
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "   <--  Program error!!!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
        }

        public async Task DeleteTableAsync(string TableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    string query = $"DROP TABLE \"{TableName}\";";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Delete Table **");
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "   <--  Program error!!!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
        }
    }
}
