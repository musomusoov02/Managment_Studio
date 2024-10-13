using Npgsql;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public void ChangeDataBase(string DataBaseName)
        {
            string[] name = Program.ConnectionString.Split(';');
            string result = "";
            for (int i = 0; i < name.Length; i++)
            {
                if (i == 1)
                {
                    result += $"Database ={DataBaseName};";
                    continue;
                }
                result += (name[i] += ";");
            }
            Program.ConnectionString = result;
        }

        public async Task<string> OpenConnectionStringAsync()
        {
            string ResultOpenConnectionString = "";
            bool OpenConnectionString = true;
            while (OpenConnectionString)
            {
                try
                {
                    string DatabaseCheck = "";
                    Console.Write("Server [localhost]:");
                    string LocalHost = Console.ReadLine();
                    Console.Write("Database [postgres]:");
                    string DataBase = Console.ReadLine();
                    DatabaseCheck = DataBase;
                    Console.Write("Port [5432]:");
                    string Port = Console.ReadLine();
                    if (string.IsNullOrEmpty(Port)) Port = "5432";
                    else
                    {
                        int portInt;
                        while (!int.TryParse(Port, out portInt))
                        {
                            Console.WriteLine("enter a number!!!!!!");
                            Port = Console.ReadLine();
                        }
                        Port = Convert.ToString(portInt);
                    }
                    Console.Write("Username [postgres]:");
                    string User = Console.ReadLine();
                    Console.Write("Password:");
                    string Password = Program.ReadPassword();
                    if (string.IsNullOrEmpty(LocalHost)) LocalHost = "localhost";
                    if (string.IsNullOrEmpty(DataBase)) DatabaseCheck = "postgres";
                    if (string.IsNullOrEmpty(User)) User = "postgres";
                    string Usernamedefault = "postgres";
                    string NewConnectionString = $"Host={LocalHost};Database={Usernamedefault};Port={Port};Username={User};Password={Password};";

                    using (var connection = new NpgsqlConnection(NewConnectionString))
                    {
                        await connection.OpenAsync();

                        string QueryDatabaseCheck = $"SELECT 1 FROM pg_database WHERE datname = '{DatabaseCheck}';";
                        using (var res = new NpgsqlCommand(QueryDatabaseCheck, connection))
                        {
                            var result = await res.ExecuteScalarAsync();
                            if (result != null)
                            {
                                OpenConnectionString = false;
                                ResultOpenConnectionString = $"Host={LocalHost};Database={DatabaseCheck};Port={Port};Username={User};Password={Password};";
                                continue;
                            }
                        }
                        string Query = $" CREATE DATABASE {DatabaseCheck};";
                        using (NpgsqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = Query;
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("\n** Add DataBase **");
                            Console.ReadKey();
                            Console.ReadKey();
                            await connection.CloseAsync();
                            OpenConnectionString = false;
                            ResultOpenConnectionString = $"Host={LocalHost};Database={DatabaseCheck};Port={Port};Username={User};Password={Password};";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("ConnectionString Error!!!!!!!!!!");
                    Console.ReadKey(true);
                    Console.ResetColor();
                    continue;
                }
            }
            return ResultOpenConnectionString;
        }
    }
}

