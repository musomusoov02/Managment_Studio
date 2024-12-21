using Npgsql;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task WriteSelectQueryAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.Write("Query input: ");
                    string query = Console.ReadLine();

                    while (string.IsNullOrEmpty(query))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        query = Console.ReadLine();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        var result = await command.ExecuteReaderAsync();

                        int ColumnCount = result.FieldCount;//      <<<<< ---------
                        var LongestRow = new int[ColumnCount];

                        while (await result.ReadAsync())
                        {
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                LongestRow[i] = Math.Max(LongestRow[i], result[i].ToString().Length);
                            }
                        }
                        var Longest = new int[ColumnCount];

                        Console.WriteLine();
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            Longest[i] = Math.Max(LongestRow[i], result.GetName(i).Length);
                            Console.Write($" {result.GetName(i).PadRight(Longest[i])} |");
                        }

                        Console.WriteLine();
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            Console.Write($" {"-".PadRight(Longest[i], '-')} +");
                        }

                        await result.CloseAsync();
                        result = await command.ExecuteReaderAsync();

                        Console.WriteLine();
                        while (await result.ReadAsync())
                        {
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                Console.Write($" {result[i].ToString().PadRight(Longest[i])} |");
                            }
                            Console.WriteLine();
                        }
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

        public async Task WriteChangeQueryAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    Console.Write("Query input: ");
                    string query = Console.ReadLine();
                    while (string.IsNullOrEmpty(query))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        query = Console.ReadLine();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        var result = await command.ExecuteNonQueryAsync();
                        Console.WriteLine( "\n** Successful **");
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
