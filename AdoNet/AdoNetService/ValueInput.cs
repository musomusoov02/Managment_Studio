using Npgsql;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task ValueInputAsync(string TableName)
        {
            try
            {
                List<(string, string)> listColumn = await ListColumnAsync(TableName);
                int columnCount = listColumn.Count;
                string ColumnName = string.Empty;

                for (int i = 0; i < columnCount; i++)
                    ColumnName += $"\"{listColumn[i].Item1}\",";

                ColumnName = ColumnName.Remove(ColumnName.Length - 1);

                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        string value = string.Empty;
                        for (int i = 0; i < columnCount; i++)
                        {
                            Console.WriteLine($"\n({i+1}) Column name: {listColumn[i].Item1,-15}| Data type: {listColumn[i].Item2}");
                            Console.WriteLine(new string('-', 50) + "\n");
                            Console.Write("Enter a value: ");
                            value += $"'{Console.ReadLine()}',";
                        }
                        value = value.Remove(value.Length - 1);

                        string query = $"INSERT INTO \"{TableName}\" ({ColumnName}) VALUES ({value});";
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Add successfully **");
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