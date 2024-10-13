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

                        int ColumnCount = result.FieldCount;
                        var LongestRow = new int[ColumnCount];
                        while (await result.ReadAsync())
                        {
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                int currentLength = result[i].ToString().Length;
                                if (currentLength > LongestRow[i]) LongestRow[i] = currentLength;
                            }
                        }

                        var ColumnsName = new string[ColumnCount];
                        var Longest = new int[ColumnCount];
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            ColumnsName[i] = result.GetName(i);
                            Longest[i] = LongestRow[i] > ColumnsName[i].Length ? LongestRow[i] : ColumnsName[i].Length;
                        }
                        await result.CloseAsync();
                        result = await command.ExecuteReaderAsync();

                        Console.WriteLine();
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            string SeparatingTheColumnWithLine = ColumnsName[i].ToString();
                            Console.Write($" {SeparatingTheColumnWithLine.PadRight(Longest[i])} |");
                        }

                        var Num = new int[ColumnCount];
                        string Lines = "-";
                        Console.WriteLine();
                        for (int i = 0; i < ColumnCount; i++)
                        {
                            Num[i] += Longest[i];
                            Console.Write($" {Lines.PadRight(Num[i], '-')} +");
                        }
                        Console.WriteLine();
                        while (await result.ReadAsync())
                        {
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                string SeparatingTheRowWithLine = result[i].ToString();
                                Console.Write($" {SeparatingTheRowWithLine.PadRight(Longest[i])} |");
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
