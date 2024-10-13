using Npgsql;
using System.Data;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        #region CreateDatabase
        public async Task CreateDatabaseAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    string DataBaseName = "";
                    bool exit = true;
                    while (exit)
                    {
                        Console.Write("DataBase Name: ");
                        DataBaseName = Console.ReadLine();
                        while (string.IsNullOrEmpty(DataBaseName))
                        {
                            Console.WriteLine("Not Entered!!! ");
                            DataBaseName = Console.ReadLine();
                        }
                        string DatabaseCheck = $"SELECT 1 FROM pg_database WHERE datname = '{DataBaseName}';";

                        using (var command = new NpgsqlCommand(DatabaseCheck, connection))
                        {
                            var result = await command.ExecuteScalarAsync();
                            if (result != null)
                            {
                                Console.WriteLine("DataBase Already Exists.");
                                continue;
                            }
                            else exit = false;
                        }
                    }
                    string Query = $" CREATE DATABASE {DataBaseName};";

                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = Query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Add DataBase **");
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
        #endregion

        public async Task<List<string>> ListDataBaseAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    List<string> list = new List<string>();
                    DataTable catalogs = await connection.GetSchemaAsync("Databases");
                    foreach (DataRow row in catalogs.Rows)
                    {
                        list.Add($"{row["Database_Name"]}");
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

        public async Task UpdateDataBaseAsync(string DataBaseName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    string NewDataBaseName = "";
                    Console.Write("DataBase New Name: ");
                    NewDataBaseName = Console.ReadLine();
                    while (string.IsNullOrEmpty(NewDataBaseName))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        NewDataBaseName = Console.ReadLine();
                    }
                    string query = $"ALTER DATABASE \"{DataBaseName}\" RENAME TO \"{NewDataBaseName}\";";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Update DataBase **");
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

        public async Task DeleteDataBaseAsync(string DataBaseName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    string query = $"DROP DATABASE \"{DataBaseName}\";";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Delete DataBase **");
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
