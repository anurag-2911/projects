using Microsoft.Win32;
using Novell.Zenworks.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestApp
{
    class Program
    {
        static string outputFile = Environment.CurrentDirectory + "\\" + "output.txt";
        static void Main(string[] args)
        {
            ReadZenLoginLogs();
           // TestMethods();

            Console.ReadKey();

        }

        private static void TestMethods()
        {
            SQLiteLog.Enabled = true;
            SQLiteConnection.Changed += SQLiteConnection_Changed;
            SQLiteLog.Log += SQLiteLog_Log;

            GetObjectEntry();
            WalFileSize();
            RunQuery();
            Refresh();
        }

        private static void ReadZenLoginLogs()
        {
            string startText = @"[Processing Command for handler zen-login]";
            string endText = @"[Finished Processing Command for handler zen-login.  Result = SUCCESS]";
            string logFile = Environment.CurrentDirectory + "\\" + "zmd-messages.log";
            IEnumerable<string> lines = GetLinesBetweenStartAndEndTexts(startText, endText, logFile);
            StringBuilder stringBuilder = new StringBuilder();
            string authTokenStartSearchText = @"ObtainAuthTokenFromServer entered";
            string authTokenEndSearchText = @"ObtainAuthTokenFromServer returned with code";
            string cacheStartText = @"GetObject(userResponse:Registration";
            string cacheEndText = @"getcacheEntry exited for key userResponse:Registration";
            DateTime startTimeCallingAuthToken =DateTime.Now;
            DateTime endTimeCallingAuthToken=DateTime.Now;
            DateTime cacheStartTime=DateTime.Now;
            DateTime cacheEndTime=DateTime.Now;
            string authTokenStart = string.Empty;
            foreach (var item in lines)
            {
                if(item.Contains(authTokenStartSearchText) && string.IsNullOrEmpty(authTokenStart))
                {
                    authTokenStart = item;
                    authTokenStart= authTokenStart.Split('[')[2];
                    authTokenStart = authTokenStart.Split(']')[0];
                    startTimeCallingAuthToken = DateTime.ParseExact(authTokenStart, "MM/dd/yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture);    
                }
                if(item.Contains(authTokenEndSearchText))
                {
                    string authTokenEnd = item.Split('[')[2].Split(']')[0];
                    endTimeCallingAuthToken = DateTime.ParseExact(authTokenEnd, "MM/dd/yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture);


                }
                if(item.Contains(cacheStartText))
                {
                    string cacheStart = item.Split('[')[2].Split(']')[0];
                    cacheStartTime = DateTime.ParseExact(cacheStart, "MM/dd/yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                if(item.Contains(cacheEndText))
                {
                    string cacheEnd = item.Split('[')[2].Split(']')[0];
                    cacheEndTime = DateTime.ParseExact(cacheEnd, "MM/dd/yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture);
                }
                stringBuilder.AppendLine(item);
            }
            TimeSpan timeTakenByAuthToken = endTimeCallingAuthToken - startTimeCallingAuthToken;
            Console.WriteLine("time taken by auth token in milliseconds: "+timeTakenByAuthToken.TotalMilliseconds);
            TimeSpan timeTakenByCache = cacheEndTime - cacheStartTime;
            Console.WriteLine("time taken by cache operation in milliseconds: "+timeTakenByCache.TotalMilliseconds);
            //string result=string.Join("", lines);
            WriteInOutPutFile(stringBuilder.ToString(), true);

        }
        private static IEnumerable<string> GetLinesBetweenStartAndEndTexts(string startString,string endString,string logFilePath)
        {
            IEnumerable<string> lines;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            lines=File.ReadLines(logFilePath).SkipWhile(line => !line.Contains(startString)).TakeWhile(line => !line.Contains(endString));
            stopwatch.Stop();
            long timeTakentoReadLogFile = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("time taken to read zmd-message.log file "+timeTakentoReadLogFile);
            
            return lines;
        }
        private static void GetObjectEntry()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["sqliteconnection"].ConnectionString;
                using (SQLiteConnection sQLiteConnection = new SQLiteConnection(connectionString))
                {
                    sQLiteConnection.Open();
                    SqliteEntryInfoProvider sqliteEntryInfoProvider = SqliteEntryInfoProvider.Instance;
                    sqliteEntryInfoProvider.GetCacheEntry(sQLiteConnection, "userResponse:Registration", UserContext.PublicUser,typeof(ObjectCacheEntry));
                    //sQLiteConnection.Open();
                    //string queryJournalMode = "pragma journal_mode";
                    //string querysyncMode = "pragma Synchronous";
                    //using (SQLiteCommand sQLiteCommand = new SQLiteCommand(queryJournalMode, sQLiteConnection))
                    //{
                    //    object journalmode = sQLiteCommand.ExecuteScalar();
                    //    sQLiteCommand.CommandText = querysyncMode;
                    //    object syncmode = sQLiteCommand.ExecuteScalar();
                        
                    //}
                }

                
            }
            catch (SQLiteException sqe)
            {
                Console.WriteLine(sqe.ToString());                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SQLiteConnection_Changed(object sender, ConnectionEventArgs eventArgs)
        {
            try
            {
                string info = string.Empty;
                object[] eventArgsData = eventArgs.Data as object[];
                string data = string.Empty;
                if (eventArgsData != null)
                {
                    data = string.Join("-", eventArgsData);
                }
                info = info + eventArgs.EventType.ToString() + "-" + eventArgs.Text + "-" + data;
                WriteInOutPutFile(info);
            }
            catch (Exception)
            {

            }
            
        }

        private static void SQLiteLog_Log(object sender, LogEventArgs e)
        {
            try
            {
                WriteInOutPutFile(string.Format("{0} - {1} - {2}", e.Data, e.ErrorCode, e.Message));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static string GetConnectionString()
        {
            string additionalParameters = string.Empty;

            string cachedbparametersKey = @"SOFTWARE\Novell\ZCM\AgentCacheDBParams";
            string cachedbparametersValue = "objInfo";
            //string connect=ConfigurationManager.ConnectionStrings["sqliteconnection"].ConnectionString;           
            
            try
            {
               
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(cachedbparametersKey))
                {
                    if (key != null)
                    {
                        additionalParameters = key.GetValue(cachedbparametersValue).ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            

            // connectionString.Append(additionalParameters);
            string dbpath = @"C:\Program Files (x86)\Novell\ZENworks\cache\zmd\ZenCache\metaData\objInfo.db";

            string dataSource = string.Format("Data Source={0}", dbpath);
            string connectionString = dataSource + ";"+ additionalParameters;


            //string db = string.Format("Data Source={0}", dbPath);


            //if (!string.IsNullOrEmpty(additionalParameters))
            //{
            //    connectionString.Append("; ");
            //    connectionString.Append(additionalParameters);
            //    connectionString.Append("; ");
            //}
            //string conn = @connectionString.ToString();
            return  connectionString;
        }

        private static void WalFileSize()
        {
            string walfilePath = @"C:\Program Files (x86)\Novell\ZENworks\cache\zmd\ZenCache\metaData\objInfo.db-wal";
            FileInfo fileInfo = new FileInfo(walfilePath);
            long len= fileInfo.Length;
        }
        private static void Refresh()
        {
            string path = Environment.CurrentDirectory + "\\refreshResult.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            decimal dvcRefTime = 0;
            decimal usrRefTime = 0;
            decimal ttlRefTime = 0;
            int count = 60;
            Console.WriteLine("starting refresh");
            for (int i = 0; i < count; i++)
            {

                string deviceRefreshTime = string.Empty; string userRefreshTime = string.Empty; string totalRefreshTime = string.Empty;
                Console.WriteLine("counter " + i.ToString());
                TriggerRefresh(ref deviceRefreshTime, ref userRefreshTime, ref totalRefreshTime);
                dvcRefTime = dvcRefTime + Convert.ToDecimal(deviceRefreshTime, new CultureInfo("en-US"));
                usrRefTime = usrRefTime + Convert.ToDecimal(userRefreshTime, new CultureInfo("en-US"));
                ttlRefTime = ttlRefTime + Convert.ToDecimal(totalRefreshTime, new CultureInfo("en-US"));

                Console.WriteLine(string.Format("device refresh time : {0}, user refresh time: {1}, total refresh time: {2}",deviceRefreshTime,userRefreshTime,totalRefreshTime));
            }
            Console.WriteLine("completed");
            WriteInOutPutFile(string.Format("average device refresh time : {0},user refresh time :{1}, total refresh time : {2}", dvcRefTime / count, usrRefTime / count, ttlRefTime / count));
            Console.WriteLine(string.Format("average device refresh time : {0},user refresh time :{1}, total refresh time : {2}", dvcRefTime / count, usrRefTime / count, ttlRefTime / count));
        }

        static void RunQuery()
        {
            string connectionString = GetConnectionString();
                    
            using (SQLiteConnection sQLiteConnection = new SQLiteConnection(connectionString))
            {
                try
                {
                    sQLiteConnection.Open();
                   
                    SQLiteCommand sQLiteCommand = new SQLiteCommand("pragma journal_mode", sQLiteConnection); 
                    object journal_mode = sQLiteCommand.ExecuteScalar();

                    SQLiteCommand syncMode = new SQLiteCommand("pragma Synchronous", sQLiteConnection);
                    object sync_mode = syncMode.ExecuteScalar();
                }

                catch(SQLiteException sqliteException)
                {
                    Console.WriteLine(sqliteException.ToString());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
        
        }

        private static void TriggerRefresh(ref string deviceRefreshTime, ref string userRefreshTime, ref string totalRefreshTime)
        {
            try
            {
                
                ZacRefresh(ref deviceRefreshTime, ref userRefreshTime, ref totalRefreshTime);

                WriteInOutPutFile(string.Format("device refresh time : {0}, user refresh time: {1}, total refresh time: {2}", deviceRefreshTime, userRefreshTime, totalRefreshTime));
                WriteInOutPutFile("");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
               
            }

        }

        private static void WriteInOutPutFile(string message,bool clearLogs =false)
        {
            if(clearLogs)
            {
                if(File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }
            }

            if (!File.Exists(outputFile))
            {
               var file= File.Create(outputFile);
               file.Close();
               Thread.Sleep(1000);
            }

            
            using (StreamWriter sw = File.AppendText(outputFile))
            {
                sw.WriteLine(message);
                
            }
        }
       

        private static void CheckDbHealth()
        {
            string dbpath = Environment.CurrentDirectory + "\\objInfo.db";
            string srcFilePath = @"C:\Program Files (x86)\Novell\ZENworks\cache\zmd\ZenCache\metaData\objInfo.db";
            string resultFile = Environment.CurrentDirectory + "\\output.txt";
            if (File.Exists(dbpath))
            {
                File.Delete(dbpath);

            }
            if (File.Exists(resultFile))
            {
                File.Delete(resultFile);
            }
            Thread.Sleep(100);

            File.Copy(srcFilePath, dbpath, true);

            string command = "sqlite3_analyzer.exe objInfo.db >> output.txt";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                process.WaitForExit();

                string[] lines = File.ReadAllLines(resultFile);
                Trace.WriteLine("");
                Trace.WriteLine(lines[2]);
                Trace.WriteLine(lines[3]);
                Trace.WriteLine(lines[4]);
                Trace.WriteLine(lines[5]);
                Trace.WriteLine("-------------------------------------------------");

            }
        }

        private static void ZacRefresh(ref string deviceRefreshTime, ref string userRefreshTime, ref string totalRefreshTime)
        {            
            string command = "zac ref";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                process.WaitForExit();

                string result = process.StandardOutput.ReadToEnd();

                deviceRefreshTime = GetRefreshTime(result, "Device refresh completed after ", " seconds");
                userRefreshTime = GetRefreshTime(result, "User refresh completed after ", " seconds.");
                totalRefreshTime = GetRefreshTime(result, "Total refresh time: ", " seconds");


            }
        }

        private static void ZacCC()
        {
            string command = "zac cc";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                process.WaitForExit();

                string result = process.StandardOutput.ReadToEnd();



            }
        }

        public static string GetRefreshTime(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int start, end;
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.IndexOf(strEnd, start);
                return strSource.Substring(start, end - start);
            }

            return string.Empty;
        }


    }
}
