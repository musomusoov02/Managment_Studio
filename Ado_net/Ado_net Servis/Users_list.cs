using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        public void List_Users()
        {
            using (var connection = new NpgsqlConnection(Program. ConnectionString))
            {
                connection.Open();

                DataTable catalogs = connection.GetSchema("Users");

                foreach (DataRow row in catalogs.Rows)
                {
                    Console.WriteLine($"Users: {row["User_Name"]}");
                }
                connection.Close();
            }
        }
    }
}
