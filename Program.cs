using Mavzu.Ado_net.Ado_net_Servis;

namespace Mavzu
{
    internal class Program
    {
        public static string ConnectionString { get; set; }

        static async Task Main(string[] args)
        {
            DataBaseService serves = new DataBaseService();
            ConnectionString = await serves.OpenConnectionStringAsync();

            //har doim typelarni nomini yozish orqali tez ishlatish yaxshiroq;
            //Procedure,Function

            var Menyu = new List<string>()
            {
                "             Databases                  ",
                "              Tables                    ",
                "              Columns                   ",
                "  List DataBase > Table > Column > Row  ",
                "              Search                    ",
                "            Value input                 ",
                "             Users List                 ",
                "            Write a query               "
            };
            
            var Databases = new List<string>()
            {
                "             Add Databases              ",
                "   List Databases AND DataBase Change   ",
                "            Update Databases            ",
                "            Delete Databases            "
            };

            var Tables = new List<string>()
            {
                "             Add Table                  ",
                "             List Table                 ",
                "            Update Table                ",
                "            Delete Table                "
            };

            var Columns = new List<string>()
            {
                "             Add Column                 ",
                "             List Column                ",
                "            Update Column               ",
                "            Delete Column               "
            };

            var WriteQuery = new List<string>()
            {
                "            Change query                ",
                "            Select query                "
            };

            bool continueProgram = true;
            while (continueProgram)
            {
                try
                {
                    var (index, name, maxList) = ArrowIndex(Menyu);

                    switch (index)
                    {
                        case 0:
                            bool DatabasesMenyu = true;

                            while (DatabasesMenyu)
                            {
                                (index, _, _) = ArrowIndex(Databases);

                                switch (index)
                                {
                                    case 0:
                                        await serves.CreateDatabaseAsync();
                                        Console.ReadKey();
                                        continue;

                                    case 1:
                                        (index, name, maxList) = ArrowIndex(await serves.ListDataBaseAsync());
                                        if (index == maxList)
                                            continue;
                                        serves.ChangeDataBase(name);

                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.ViewListColumnAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 2:
                                        (index, name, maxList) = ArrowIndex(await serves.ListDataBaseAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.UpdateDataBaseAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 3:
                                        (index, name, maxList) = ArrowIndex(await serves.ListDataBaseAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.DeleteDataBaseAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    default:
                                        DatabasesMenyu = false;
                                        continue;
                                }
                            }
                            continue;

                        case 1:
                            bool TableMenyu = true;

                            while (TableMenyu)
                            {
                                (index, name, maxList) = ArrowIndex(Tables);

                                switch (index)
                                {
                                    case 0:
                                        string TableName = await serves.CreateTableAsync();
                                        await serves.CreateColumnAsync(TableName);
                                        Console.ReadKey();
                                        continue;

                                    case 1:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.ViewListColumnAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 2:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.UpdateTableAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 3:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList) 
                                            continue;

                                        await serves.DeleteTableAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    default:
                                        TableMenyu = false;
                                        continue;
                                }
                            }
                            continue;

                        case 2:
                            bool ColumnMenyu = true;

                            while (ColumnMenyu)
                            {
                                (index, _, _) = ArrowIndex(Columns);

                                switch (index)
                                {
                                    case 0:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.CreateColumnAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 1:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        await serves.ViewListColumnAsync(name);
                                        Console.ReadKey();
                                        continue;

                                    case 2:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        var (indexColumn, nameColumn, maxListColumn) = ArrowIndex(
                                            (await serves.ListColumnAsync(name))
                                                         .Select(item => item.Item1)
                                                         .ToList()
                                        );
                                        if (indexColumn == maxListColumn)
                                            continue;

                                        await serves.UpdateColumnAsync(name, nameColumn);
                                        Console.ReadKey();
                                        continue;

                                    case 3:
                                        (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                        if (index == maxList)
                                            continue;

                                        (indexColumn, nameColumn, maxListColumn) = ArrowIndex(
                                            (await serves.ListColumnAsync(name))
                                                         .Select(item => item.Item1)
                                                         .ToList()
                                        );
                                        if (indexColumn == maxListColumn)
                                            continue;

                                        await serves.DeleteColumnAsync(name, nameColumn);
                                        Console.ReadKey();
                                        continue;

                                    default:
                                        ColumnMenyu = false;
                                        continue;
                                }
                            }
                            continue;

                        case 3:
                            bool GetRowsMenyu = true;

                            while (GetRowsMenyu)
                            {
                                (index, name, maxList) = ArrowIndex(await serves.ListDataBaseAsync());
                                if (index == maxList)
                                {
                                    GetRowsMenyu = false;
                                    continue;
                                }

                                serves.ChangeDataBase(name);
                                (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                                if (index == maxList)
                                    continue;

                                await serves.GetRowsAsync(name);
                                Console.ReadKey();
                                GetRowsMenyu = false;
                                continue;
                            }
                            continue;

                        case 4:
                            (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                            if (index == maxList)
                                continue;

                            var (indexColumnNew, nameColumnNew, maxListColumnNew) = ArrowIndex(
                                (await serves.ListColumnAsync(name))
                                             .Select(item => item.Item1)
                                             .ToList()
                            );
                            if (indexColumnNew == maxListColumnNew)
                                continue;

                            await serves.SearchAsyn(name, nameColumnNew);
                            continue;

                        case 5:
                            (index, name, maxList) = ArrowIndex(await serves.ListTableAsync());
                            if (index == maxList)
                                continue;

                            await serves.ValueInputAsync(name);
                            Console.ReadKey();
                            continue;

                        case 6:
                            await serves.ListUsersAsync();
                            Console.ReadKey();
                            continue;

                        case 7:
                            bool QueryMenyu = true;

                            while (QueryMenyu)
                            {
                                (index, name, maxList) = ArrowIndex(WriteQuery);

                                switch (index)
                                {
                                    case 0:
                                        await serves.WriteChangeQueryAsync();
                                        Console.ReadKey();
                                        continue;

                                    case 1:
                                        await serves.WriteSelectQueryAsync();
                                        Console.ReadKey();
                                        continue;

                                    default:
                                        QueryMenyu = false;
                                        continue;
                                }
                            }
                            continue;

                        case 8:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + "   <--  Program.cs error!!!!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey();
                    continue;
                }
            }
            #region AddJsonFile

            //ConfigurationBuilder builder = new ConfigurationBuilder();
            //builder.SetBasePath(Directory.GetCurrentDirectory())
            //       .AddJsonFile("appsettings.json");

            //IConfiguration configs = builder.Build();
            ////localhost -> ::1
            //ConnectionString = configs.GetConnectionString("MYDB1");
            #endregion
        }


        public static (int index, string name, int maxList) ArrowIndex(List<string> buyruq)
        {
            int selectIndex = 0;
            buyruq.Add("             <--Back                   ");

            while (true)
            {
                Console.Clear();
                for (int i = 0; i < buyruq.Count; i++)
                {
                    if (i == selectIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if (i == buyruq.Count - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.WriteLine(buyruq[i]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                    selectIndex = (selectIndex + 1) % buyruq.Count;
                else if (key.Key == ConsoleKey.UpArrow)
                    selectIndex = (selectIndex - 1 + buyruq.Count) % buyruq.Count;
                else if (key.Key == ConsoleKey.Enter)
                {
                    buyruq.RemoveAt(buyruq.Count - 1);
                    if (selectIndex == buyruq.Count)
                        return (selectIndex, null, buyruq.Count);

                    return (selectIndex, buyruq[selectIndex], buyruq.Count);
                }
            }
        }

        public static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            return password;
        }
    }
}
