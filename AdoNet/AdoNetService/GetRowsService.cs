using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task GetRowsAsync(string TableName)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    int TotalRows;
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        string QueryTotalRows = $"Select Count(*) FROM {TableName};";
                        command.CommandText = QueryTotalRows;
                        var TotalRowsNew = await command.ExecuteScalarAsync();
                        TotalRows = Convert.ToInt32(TotalRowsNew);
                    }

                    int limit = 10;
                    int offset = 0;
                    bool exit = true;
                    while (exit)
                    {
                        Console.Clear();
                        using (NpgsqlCommand res = connection.CreateCommand())
                        {
                            string Query = $"Select * FROM {TableName} Limit {limit} OFFSET {offset};";
                            res.CommandText = Query;
                            var result = await res.ExecuteReaderAsync();

                            int ColumnCount = result.FieldCount;//      <<<<< ---------
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
                            result = await res.ExecuteReaderAsync();

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
                            await result.CloseAsync();
                        }

                        Console.WriteLine("\n\n");
                        Console.Write("Total: "+TotalRows+ "\t\t\t\t\t\t\t\t" + (int)Math.Ceiling((double)offset / limit) + "/" + TotalRows / limit);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"\n\n\n\t\t\t\t\t\t\t\tback page <<-- LeftArrow\t||\tRightArrow-- >> next page");
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("\nDownArrow >> exit");
                        Console.ForegroundColor= ConsoleColor.Gray;

                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.RightArrow)
                        {
                            offset += limit;
                            if (offset > TotalRows) offset = 0;
                        }
                        else if (key.Key == ConsoleKey.LeftArrow) offset = Math.Max(0, offset - limit);
                        else if (key.Key == ConsoleKey.DownArrow) exit = false;
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
