using Mavzu.Ado_net.Ado_net_Servis;
using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace Mavzu
{
    internal class Program
    {
        public static string ConnectionString { get; set; }

        public static List<string> ArrowTableName { get; set; }

        public static string DataBaseMavjudmi____ { get; set; }

        static void Main(string[] args)
        {
            menyu:
            Console.Write("Server [localhost]:");
            string loca = Console.ReadLine();
            Console.Write("Database [postgres]:");
            string data = Console.ReadLine();
            DataBaseMavjudmi____ = data;
            Console.Write("Port [5432]:");
            string port = Console.ReadLine();
            Console.Write("Username [postgres]:");
            string user = Console.ReadLine();
            Console.Write("Password:");
            string password = Console.ReadLine();
            if (string.IsNullOrEmpty(loca)) loca = "localhost";
            if (string.IsNullOrEmpty(data)) DataBaseMavjudmi____ = "postgres";
            if (string.IsNullOrEmpty(port)) port = "5432";
            if (string.IsNullOrEmpty(user)) user = "postgres";

            DataBase_sevis serves = new DataBase_sevis();

            string ConnDEFAULT = "postgres";
            string NEwConnectionString = $"Host={loca};Database={ConnDEFAULT};Port={port};Username={user};Password={password};";

            string MAvjudmi = serves.OpenConnectionString(NEwConnectionString, DataBaseMavjudmi____);
            
            
            


            ConnectionString = $"Host={loca};Database={MAvjudmi};Port={port};Username={user};Password={password};";



            int intD;
            List<string> DataBaseName = serves.List_DataBase();

            int intT;
            List<string> TableName = serves.List_Table();

            int intC;
            List<string> ColumnName;// = serves.List_Column();





            //*******************************************************************



            //DataBase Add  Create  ->> Database bo'lmasa Create qilish kerak,Create bildirish kerak.......
            //************************************
            //Databases     //CRUD
            //Tables       //CRUD
            //Columns     //DataTypes    //CRUD  // insert into
            //Users list
            //List_TAble_ROW
            //Database change
            //------//Host change
            //back
            //ipconfig ---ip_adress

            //*******************************************************************



            try
            {
                var Menyu = new List<string>()
                {
                    "Databases",
                    "Tables",
                    "Columns",
                    "List_DataBase_Table_Column_Row",
                    "Users_List",
                    "exit"
                };
                var Databases = new List<string>()
                {
                    "Add Databases",
                    "List Databases AND DataBase Change",
                    "Update Databases",
                    "Delete Databases",
                    "Back"
                };
                var Tables = new List<string>()
                {
                    "Add Table",
                    "List Table",
                    "Update Table",
                    "Delete Table",
                    "Back"
                };
                var Columns = new List<string>()
                {
                    "Add Column",
                    "List Column",
                    "Update Column",
                    "Delete Column",
                    "Back"
                };



            back:
                int num = ArrowIndex(Menyu);
                switch (num)
                {
                    case 0:
                    cate:
                        num = ArrowIndex(Databases);
                        switch (num)
                        {
                            case 0:
                                serves.Create_Database();
                                DataBaseName=serves.List_DataBase();
                                Console.ReadKey();
                                goto cate;
                            case 1:
                                intD = ArrowIndex(DataBaseName);
                                ConnectionString = $"Host={loca};Database={DataBaseName[intD]};Port={port};Username={user};Password={password};";
                                TableName = serves.List_Table();

                                intT = ArrowIndex(TableName);
                                serves.SelectC(TableName[intT]);

                                Console.ReadKey();
                                goto cate;
                            case 2:

                                DataBaseName = serves.List_DataBase();
                                intD = ArrowIndex(serves.List_DataBase());
                                serves.Update_DataBase(DataBaseName[intD]);
                                DataBaseName= serves.List_DataBase();
                                Console.ReadKey();
                                goto cate;
                            case 3:
                                intD = ArrowIndex(serves.List_DataBase());
                                serves.Delete_DataBase(DataBaseName[intD]);
                                DataBaseName= serves.List_DataBase();
                                Console.ReadKey();
                                goto cate;
                            case 4:
                                goto back;
                        }
                        break;
                    case 1:
                    pro:
                        num = ArrowIndex(Tables);
                        switch (num)
                        {
                            case 0:
                                serves.Create_Table();
                                TableName= serves.List_Table();
                                Console.ReadKey();
                                goto pro;
                            case 1:
                                TableName= serves.List_Table();
                                intT= ArrowIndex(TableName);
                                serves.List_Column(TableName[intT]);
                                Console.ReadKey();
                                goto pro;
                            case 2:
                                TableName = serves.List_Table();
                                intT = ArrowIndex(serves.List_Table());
                                serves.Update_Table(TableName[intT]);
                                TableName= serves.List_Table();
                                Console.ReadKey();
                                goto pro;
                            case 3:
                                intT = ArrowIndex(serves.List_Table());
                                serves.Delete_Table(TableName[intT]);
                                TableName = serves.List_Table();
                                Console.ReadKey();
                                goto pro;
                            case 4:
                                goto back;
                        }
                        break;

                    case 2:
                    colu:
                        num = ArrowIndex(Columns);
                        switch (num)
                        {
                            case 0:
                                intT = ArrowIndex(serves.List_Table());
                                serves.Create_Column(TableName[intT]);
                                Console.ReadKey();
                                goto colu;
                            case 1:
                                intT = ArrowIndex(serves.List_Table());
                                serves.List_Column(TableName[intT]);
                                Console.ReadKey();
                                goto colu;
                            case 2:
                                intT = ArrowIndex(serves.List_Table());
                                ColumnName = serves.List_Column(TableName[intT]);
                                intC = ArrowIndex(serves.List_Column(TableName[intT]));
                                serves.Update_Column(TableName[intT], ColumnName[intC]);
                                Console.ReadKey();
                                goto colu;
                            case 3:
                                intT = ArrowIndex(serves.List_Table());
                                ColumnName = serves.List_Column(TableName[intT]);
                                intC = ArrowIndex(serves.List_Column(TableName[intT]));
                                serves.Delete_Column(TableName[intT], ColumnName[intC]);
                                Console.ReadKey();
                                goto colu;
                            case 4:
                                goto back;
                        }
                        break;

                    case 3:
                        //list table row
                        DataBaseName=serves.List_DataBase();
                         intD = ArrowIndex(DataBaseName);
                        ConnectionString = $"Host={loca};Database={DataBaseName[intD]};Port={port};Username={user};Password={password};";
                        TableName = serves.List_Table();
                        intT = ArrowIndex(serves.List_Table());
                        serves.SelectC(TableName[intT]);
                        Console.ReadKey();
                        goto back;
                    case 4:
                        //user_list
                        serves.List_Users();
                        Console.ReadKey();
                        goto back;
                    case 5:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine( "DataBase Mavjud emas yoki Password noto'g'ri!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.ReadKey();
                goto menyu;
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



        
        public static int ArrowIndex(List<string> buyruq)
        {
            int selectIndex = 0;
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < buyruq.Count; i++)
                {
                    if (i == selectIndex) Console.WriteLine($">>>>  {buyruq[i]}");
                    else Console.WriteLine($"      {buyruq[i]}");
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow) selectIndex = (selectIndex + 1) % buyruq.Count;
                else if (key.Key == ConsoleKey.UpArrow) selectIndex = (selectIndex - 1 + buyruq.Count) % buyruq.Count;
                else if (key.Key == ConsoleKey.Enter) return selectIndex;
            }
        }
    }
}

