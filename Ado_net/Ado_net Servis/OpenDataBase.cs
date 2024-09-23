using Npgsql;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        public string OpenConnectionString(string NEwConnectionString, string DataBaseMavjudmi)
        {
            using (var connection = new NpgsqlConnection(NEwConnectionString))
            {
                connection.Open();
                string DB_mavjudmi = $"SELECT 1 FROM pg_database WHERE datname = '{DataBaseMavjudmi}';";

                using (var res = new NpgsqlCommand(DB_mavjudmi, connection))
                {
                    var result = res.ExecuteScalar();
                    if (result != null)
                    {
                        return DataBaseMavjudmi;
                    }
                    else
                    {

                    }
                }

                string Query = $" CREATE DATABASE {DataBaseMavjudmi};";

                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = Query;
                    int num = res.ExecuteNonQuery();
                    Console.WriteLine("\n** Add DataBase **");
                    Console.ReadKey();
                    Console.ReadKey();
                    return DataBaseMavjudmi;
                }
                connection.Close();
            }
        }
    }
}

