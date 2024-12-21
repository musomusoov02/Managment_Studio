using Npgsql;
using System.Data;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        public async Task ListUsersAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    DataTable users = await connection.GetSchemaAsync("Users");
                    foreach (DataRow row in users.Rows)
                    {
                        Console.WriteLine($"Users: {row["User_Name"]}");
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
