using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace sqlite_Global_tool
{
    internal static class Sqlite
    {
        private static string _dbPath = "";

        private static void Main(string[] args)
        {
            _dbPath = ReadVariable();
            switch (args.Length)
            {
                case 2:
                    switch (args[0])
                    {
                        case "-db":
                            SaveValue(args[1]);
                            _dbPath = args[1];
                            break;
                        case "-q" when _dbPath == "empty":
                            Console.WriteLine(@" Please Specify the sqlite database path first by typing -db ""path""");
                            Console.WriteLine();
                            break;
                        case "-q":
                            ExecuteQuery(args[1]);
                            break;
                        case "-r" when _dbPath == "empty":
                            Console.WriteLine(@" Please Specify the sqlite database path first by typing -db ""path""");
                            Console.WriteLine();
                            break;
                        case "-r":
                            ExecuteQueryWithResult(args[1], false);
                            break;
                        case "-s":
                        {
                            if (args[1] != null)
                            {
                                ExecuteQueryWithResult(args[1], true);
                            }
                            break;
                        }
                        default:
                            Console.WriteLine("\n  Command not found check the option --h for more information");
                            Console.WriteLine();
                            break;
                    }

                    break;
                case 1 when args[0] == "--h":
                    Console.WriteLine("\n => Welcome to sqlite .net core global tool version 1.0");
                    Console.WriteLine("\nOptions:");
                    Console.WriteLine(@"   -db ""Sqlite Database Path""");
                    Console.WriteLine(@"   -q  ""Query to execute""");
                    Console.WriteLine(@"   -r  ""Query to execute with result""");
                    Console.WriteLine(@"   -s  ""the table that you want to show, its data""");
                    Console.WriteLine();
                    break;
                case 1:
                    Console.WriteLine("\n   Need to insert a value for the option\n");
                    break;
                default:
                    Console.WriteLine("\n   Check the help section by typing -h\n");
                    break;
            }
        }

        private static void ExecuteQueryWithResult(string queryOrTable, bool isTable)
        {
            try
            {
                if (_dbPath == "empty")
                {
                    Console.WriteLine("\n => Welcome to sqlite .net core global tool version 1.0 <=");
                    Console.WriteLine("       Check the help section by typing sqlite --h \n");
                }
                else
                {
                    var connectionStringBuilder = new SqliteConnectionStringBuilder {DataSource = _dbPath};
                    using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                    {
                        connection.Open();
                        var selectCmd = connection.CreateCommand();
                        if (!isTable)
                        {
                            selectCmd.CommandText = queryOrTable;
                        }
                        else
                        {
                            selectCmd.CommandText = "Select * from " + queryOrTable;
                        }

                        using (var reader = selectCmd.ExecuteReader())
                        {
                            var count = reader.FieldCount;
                            var numberRecord = 0;
                            while (reader.Read())
                            {
                                numberRecord++;
                            }
                            var arrValues = new string[numberRecord + 1, count];
                            for (var i = 0; i < count; i++)
                            {
                                arrValues[0, i] = " " + reader.GetName(i) + " ";
                                var j = 1;
                                while (reader.Read())
                                {
                                    arrValues[j, i] = " " + reader[reader.GetName(i)] + " ";
                                    j++;
                                }
                            }
                            Console.WriteLine();
                            ArrayPrinter.PrintToConsole(arrValues);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n There is an error in your sql syntax");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }
        }

        private static void ExecuteQuery(string query)
        {
            try
            {
                if (_dbPath == "empty")
                {
                    Console.WriteLine("\n => Welcome to sqlite .net core global tool version 1.0 <=");
                    Console.WriteLine("       Check the help section by typing sqlite --h \n");
                }
                else
                {
                    var connectionStringBuilder = new SqliteConnectionStringBuilder {DataSource = _dbPath};
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n There is an error in your sql syntax:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   " + ex.Message + "\n");
                Console.ResetColor();
            }
        }

        private static string ReadVariable()
        {
            const string path = "Sample.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.Write("empty");
                }
            }
            var sr = new StreamReader(path);
            var line = sr.ReadLine();
            sr.Close();
            return line;
        }

        private static void SaveValue(string value)
        {
            var sw = new StreamWriter("Sample.txt");
            sw.Write(value);
            sw.Close();
            Console.WriteLine("\n Database saved successfully \n");
        }
    }

}