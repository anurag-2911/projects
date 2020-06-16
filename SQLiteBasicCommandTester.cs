using System;
using System.Data;
using System.Data.SQLite;

namespace TestApp
{
    public class SQLiteBasicCommandTester
    {
        public static void SQLiteTester()
        {
            string connectionString = @"Data Source = E:\Learn\codebase\C#\bin\mdstatus.db;PRAGMA journal_mode=WAL;";
            ReadFromSQliteDataBase(connectionString);
        }

        /// <summary>
        /// THis method checks if connection can be closed by SQLiteDataReader close method 
        /// </summary>
        /// <param name="connectionString"></param>
        private static void ReadFromSQliteDataBase(string connectionString)
        {
            string queryString =
                "SELECT * from zstatus";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SQLiteCommand command =
                        new SQLiteCommand(queryString, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            // Call Read before accessing data.
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("{0}, {1}",
                                    reader[0], reader[1]));
                            }

                            // Call Close when done reading.
                            reader.Close();
                        }
                    }
                }
                catch (Exception exception)
                {

                    Console.WriteLine(exception.ToString());
                }
            }
        }
    }
}
