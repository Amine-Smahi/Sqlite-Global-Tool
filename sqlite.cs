using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace sqlite_Global_tool
{
    class sqlite
    {
        static string DbPath = "";
        static void Main(string[] args)
        {
            DbPath = ReadVariable();
                if (DbPath == "empty")
                {
                        Console.WriteLine("\n => Welcome to sqlite .net core global tool version 1.0 <=");
                        Console.WriteLine("       Check the help section by typing sqlite --h \n");
                }
                

            if (args.Length <= 2 && args.Length >= 1)
            {
                switch (args[0])
                {
                    case "-db":
                        SaveValue(args[1]);
                        DbPath = args[1];
                        break;
                    case "-q":
                        if (DbPath == "empty")
                        {
                            Console.WriteLine(@" Please Specify the sqlite database path first by typing -db ""path""");
                            Console.WriteLine();
                        }
                        else
                        {
                            ExecuteQuery(args[1]);
                        }
                        break;
                    case "-r":
                        if (DbPath == "empty")
                        {
                            Console.WriteLine(@" Please Specify the sqlite database path first by typing -db ""path""");
                            Console.WriteLine();
                        }
                        else
                        {
                            ExecuteQueryWithResult(args[1]);
                        }
                        break;
                    case "--h":
                        Console.WriteLine("\n => Welcome to sqlite .net core global tool version 1.0");
                        Console.WriteLine("\nOptions:");
                        Console.WriteLine(@"   -db ""Sqlite Database Path""");
                        Console.WriteLine(@"   -q  ""Query to execute""");
                        Console.WriteLine(@"   -r  ""Query to execute with result""");
                        Console.WriteLine();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Check the help section by typing -h");
            }

        }
        static void ExecuteQueryWithResult(string query)
        {
            try{
            int count = 0;
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = DbPath;
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = query;

                using (var reader = selectCmd.ExecuteReader())
                {
                    count = reader.FieldCount;
                    int numberRecord = 0;
                    while (reader.Read())
                    {
                        numberRecord++;
                    }
                    string[,] arrValues = new string[numberRecord + 1, count];
                    for (int i = 0; i < count; i++)
                    {
                        arrValues[0, i] = " " + reader.GetName(i) + " ";
                        int j = 1;
                        while (reader.Read())
                        {
                            arrValues[j, i] = " " + reader[reader.GetName(i)].ToString() + " ";
                            j++;
                        }
                    }
                    Console.WriteLine();
                    ArrayPrinter.PrintToConsole(arrValues);
                }
            }
            }
            catch(Exception){
                Console.WriteLine("\n There is an error in your sql syntax");
                Console.WriteLine("                OR");
                Console.WriteLine(" The query does not contain a select\n");
            }
        }

        static void ExecuteQuery(string query)
        {
            try{
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = DbPath;
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = query;
                    insertCmd.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            Console.WriteLine("\n Query executed successfully \n");
            }
            catch(Exception){
                Console.WriteLine("\n There is an error in your sql syntax \n");
            }
        }
        static string ReadVariable()
        {
            string path = "Sample.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.Write("empty");
                }
            }
            StreamReader sr = new StreamReader(path);
            string line = "";
            line = sr.ReadLine();
            sr.Close();
            return line;
        }
        static void SaveValue(string value)
        {
            StreamWriter sw = new StreamWriter("Sample.txt");
            sw.Write(value);
            sw.Close();
            Console.WriteLine("\n Database saved successfully \n");
        }
    }

}