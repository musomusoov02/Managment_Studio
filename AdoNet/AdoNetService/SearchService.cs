using Npgsql;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task SearchAsyn(string TableName, string ColumnName)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    int limit = 10;
                    int offset = 0;
                    bool exit = true;
                    string search = string.Empty;
                    int TotalRows = 0;

                    ConsoleKeyInfo key;
                    Console.Write($"\nsearch: {search}");

                    while (exit)
                    {
                        using (NpgsqlCommand res = connection.CreateCommand())
                        {
                            key = Console.ReadKey(true);
                            Console.Clear();
                            if (key.Key == ConsoleKey.RightArrow)
                            {
                                offset += limit;
                                if (offset > TotalRows) offset = 0;
                            }
                            else if (key.Key == ConsoleKey.LeftArrow)
                                offset = Math.Max(0, offset - limit);
                            else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.UpArrow)
                            {
                                exit = false;
                                continue;
                            }
                            else if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                            {
                                search += key.KeyChar;
                                //Console.Write("*");
                            }
                            else if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                            {
                                search = search.Substring(0, search.Length - 1);
                                //Console.Write("\b \b");
                            }
                            using (NpgsqlCommand command = connection.CreateCommand())
                            {
                                string QueryTotalRows = $"SELECT COUNT(*) FROM \"{TableName}\" WHERE CAST(\"{ColumnName}\" AS text) ILIKE '{search}%';";
                                command.CommandText = QueryTotalRows;
                                var TotalRowsNew = await command.ExecuteScalarAsync();
                                TotalRows = Convert.ToInt32(TotalRowsNew);
                            }
                            Console.WriteLine("\nDownArrow >> exit");
                            Console.Write($"\nsearch: {search}\n");
                            if (search.Length == 0)
                                continue;

                            string Query = $"SELECT * FROM \"{TableName}\" WHERE CAST(\"{ColumnName}\" AS text) ILIKE '{search}%' LIMIT {limit} OFFSET {offset};";
                            res.CommandText = Query;
                            var result = await res.ExecuteReaderAsync();

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
                                if(result.GetName(i)==ColumnName)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write($" {result.GetName(i).PadRight(Longest[i])} ");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("|");
                                    continue;
                                }
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write($" {result.GetName(i).PadRight(Longest[i])} |");
                            }

                            Console.WriteLine();
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                Console.Write($" {"-".PadRight(Longest[i], '-')} +");
                            }

                            await result.CloseAsync();
                            result = await res.ExecuteReaderAsync();

                            Console.WriteLine();
                            while (await result.ReadAsync())
                            {
                                for (int i = 0; i < ColumnCount; i++)
                                {
                                    Console.Write($" {result[i].ToString().PadRight(Longest[i])} |");
                                }
                                Console.WriteLine();
                            }
                            await result.CloseAsync();
                        }
                        Console.WriteLine("\n\n");
                        Console.Write("Total: " + TotalRows + "\t\t\t\t\t\t\t\t" +
                                      ((int)Math.Ceiling((double)offset / limit)) + "/" +
                                      (TotalRows / limit));

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n\n\n\t\t\t\t\t\t\t\tback page <<-- LeftArrow\t||\tRightArrow-- >> next page");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "   <--  Program error!!!!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();
                return;
            }
        }
    }
}
