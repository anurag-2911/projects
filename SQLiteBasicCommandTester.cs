using System;
using System.Data;
using System.Data.SQLite;
using ExtensionMethods;

namespace TestApp
{
    public class SQLiteBasicCommandTester
    {
        public static void SQLiteTester()
        {            
            string connectionString = @"Data Source = E:\Learn\codebase\C#\bin\mdstatus.db;pragma journal_mode=wal;";
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = @"E:\Learn\codebase\C#\bin\mdstatus.db";
            connectionStringBuilder.JournalMode = SQLiteJournalModeEnum.Wal;
            connectionString = connectionStringBuilder.ToString();
            try
            {
                ReadFromSQliteDataBase(connectionString);
                string path = Environment.CurrentDirectory;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                
            }
        }

        /// <summary>
        /// THis method checks if connection can be closed by SQLiteDataReader close method 
        /// </summary>
        /// <param name="connectionString"></param>
        private static void ReadFromSQliteDataBase(string connectionString)
        {
            string queryString = "pragma journal_mode";
                //"SELECT * from zstatus";

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
                                
                                object value1 = reader.GetValue(0,1);
                                Console.WriteLine(String.Format("{0}",value1));
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

namespace ExtensionMethods
{
    public static class SQLiteDataReaderExtension
    {
        public static object GetValue(this SQLiteDataReader reader, int value,int value2)
        {
            object obj;

            try
            {
                obj=reader.GetValue(value);
            }
            catch (Exception exception)
            {
                return null;
                
            }

            return obj;
        }
    }
}
