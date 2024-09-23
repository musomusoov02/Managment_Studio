using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mavzu.Ado_net.Ado_net_Servis
{
    public partial class DataBase_sevis
    {
        public void SelectC(string Table_Name)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Program. ConnectionString))
            {
                connection.Open();
                string Query = $"Select * FROM {Table_Name};";
                using (NpgsqlCommand res = connection.CreateCommand())
                {
                    res.CommandText = Query;
                    var result = res.ExecuteReader();

                    int Column_FieldCount = result.FieldCount;//      <<<<< ---------

                    var maxLengths = new int[Column_FieldCount];
                    while (result.Read())
                    {
                        for (int i = 0; i < Column_FieldCount; i++)
                        {
                            int currentLength = result[i].ToString().Length;
                            if (currentLength > maxLengths[i])
                            {
                                maxLengths[i] = currentLength;
                            }
                        }
                    }

                    var ColumnName = new string[Column_FieldCount];
                    for (int i = 0; i < Column_FieldCount; i++)
                    {
                        ColumnName[i] = result.GetName(i);
                    }

                    var ResMax = new int[Column_FieldCount];
                    for (int i = 0; i < Column_FieldCount; i++)
                    {
                        ResMax[i] = maxLengths[i] > ColumnName[i].Length ? maxLengths[i] : ColumnName[i].Length;
                    }

                    result.Close();
                    result = res.ExecuteReader();

                    Console.WriteLine();

                    for (int i = 0; i < Column_FieldCount; i++)
                    {
                        string pp = ColumnName[i].ToString();
                        Console.Write($" {pp.PadRight(ResMax[i])} |");
                    }

                    var max___Length = new int[Column_FieldCount];
                    string l__l = "-";
                    Console.WriteLine();
                    for (int i = 0; i < Column_FieldCount; i++)
                    {
                        max___Length[i] += ResMax[i];
                        Console.Write($" {l__l.PadRight(max___Length[i], '-')} +");
                    }
                    Console.WriteLine();

                    while (result.Read())
                    {
                        for (int i = 0; i < Column_FieldCount; i++)
                        {
                            string lll = result[i].ToString();
                            Console.Write($" {lll.PadRight(ResMax[i])} |");
                        }
                        Console.WriteLine();
                    }
                }
                connection.Close();
            }
        }

    }
}
