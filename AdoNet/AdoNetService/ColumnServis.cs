using Npgsql;
using System.Data;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBaseService
    {
        #region PostgreSQLDataTypes
        public List<string> PostgreSQLDataTypes { get; set; } = new List<string>
        {
            "smallint",
            "integer",
            "bigint",
            "decimal",
            "real",
            "double precision",
            "serial",
            "bigserial",
            "char",
            "varchar",
            "text",
            "timestamp",
            "timestamptz",
            "date",
            "time",
            "timetz",
            "interval",
            "boolean",
            "point",
            "line",
            "linesegment",
            "box",
            "path",
            "polygon",
            "circle",
            "cidr",
            "inet",
            "macaddr",
            "json",
            "jsonb",
            "xml",
            "array",
            "composite",
            "int4range",
            "int8range",
            "numrange",
            "tsrange",
            "tstzrange",
            "daterange",
            "enum",
            "uuid",
            "tsvector",
            "tsquery",
            "ltree",
            "hstore",
            "pg_lsn"
        };
        #endregion

        public async Task CreateColumnAsync(string TableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    string ColumnName = "";
                    Console.Write("Column Name: ");
                    ColumnName = Console.ReadLine();

                    while (string.IsNullOrEmpty(ColumnName))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        ColumnName = Console.ReadLine();
                    }
                    var (_, name, _) = Program.ArrowIndex(PostgreSQLDataTypes);

                    string Query = $"ALTER TABLE {TableName} ADD COLUMN {ColumnName} {name};";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = Query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Add Column **");
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


        public async Task ViewListColumnAsync(string TableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    DataTable schemaColumn = await connection.GetSchemaAsync("Columns", new string[] { null, null, TableName });

                    Console.WriteLine("\nCOLUMN NAME              |    DATA TYPE");
                    Console.WriteLine(new string('_', 35) + "\n");
                    foreach (DataRow row in schemaColumn.Rows)
                    {
                        Console.WriteLine($"{row["COLUMN_NAME"],-25}|    {row["DATA_TYPE"]}");
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

        public async Task<List<(string,string)>> ListColumnAsync(string TableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();
                    var list = new List<(string, string)>();

                    DataTable schemaColumn = await connection.GetSchemaAsync("Columns", new string[] { null, null, TableName });

                    foreach (DataRow row in schemaColumn.Rows)
                    {
                        list.Add((row["COLUMN_NAME"].ToString(),row["DATA_TYPE"].ToString())) ;
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

        public async Task UpdateColumnAsync(string TableName, string ColumnName)
        {
            using (var connection = new NpgsqlConnection(Program.ConnectionString))
            {
                await connection.OpenAsync();
                string NewTypeName = "";
                try
                {
                    Console.WriteLine("New Type Name: ");
                    NewTypeName = Console.ReadLine();
                    while (string.IsNullOrEmpty(NewTypeName))
                    {
                        Console.WriteLine("Not Entered!!! ");
                        NewTypeName = Console.ReadLine();
                    }
                    string query = $"ALTER TABLE \"{TableName}\" ALTER COLUMN \"{ColumnName}\" TYPE \"{NewTypeName}\";";

                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Update Type **");
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

        public async Task DeleteColumnAsync(string TableName, string ColumnName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Program.ConnectionString))
                {
                    await connection.OpenAsync();

                    string query = $"ALTER TABLE \"{TableName}\" DROP COLUMN \"{ColumnName}\";";
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("\n** Delete Column **");
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
