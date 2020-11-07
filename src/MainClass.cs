using Microsoft.Win32;

using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
            Zipper();
            AddingTraceListener();

            Console.ReadKey();

        }

        private static void Zipper()
        {
            string fileName = @"E:\Learn\codebase\angular\logapp\logs\app1.log" + ".zip";
            
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            
            string file = @"E:\Learn\codebase\angular\logapp\logs\app1.log";
            
            zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
           
            zip.Dispose();
        }

        private static void AddingTraceListener()
        {
            // Create a file for output named TestFile.txt.
            Stream myFile = File.Create("TestFile.txt");

            /* Create a new text writer using the output stream, and add it to
             * the trace listeners. */
            TextWriterTraceListener myTextListener = new TextWriterTraceListener(myFile);
            Trace.Listeners.Add(myTextListener);

            // Write output to the file.
            Trace.Write("Test output ");

            // Flush the output.
            Trace.Flush();
        }

        private static void TestMethods()
        {
            Refresh();
        }

        private static void ReadZenLoginLogs()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string directoryPath = @"D:\Work\Task\Bugs\Internal Bugs\pst-zencache\parsed-logs\DUT";
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            FileInfo[] files = dir.GetFiles("*.log");
            Console.WriteLine("total files "+files.Length);
            long timeTakenInzaccommand=0;
            long timetakenInGettingAuthToken=0;
            long timeTakenInGetCacheEntryOperation=0;

            foreach (FileInfo file in files)
            {
                string filepath = file.FullName;
                try
                {
                    string[] allLines = GetAllLines(filepath);
                    ParseFile(allLines,ref timeTakenInzaccommand,ref timetakenInGettingAuthToken,ref timeTakenInGetCacheEntryOperation,filepath);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("exception in reading file "+ex.ToString());
                }
                

            }
            int percentauthtoken = (int)Math.Round((double)(100 * timetakenInGettingAuthToken) / timeTakenInzaccommand);
            int percenagecacheop= (int)Math.Round((double)(100 * timeTakenInGetCacheEntryOperation) / timeTakenInzaccommand);
            Console.WriteLine(string.Format("getauthtoken is {0} percent of total time and get cache has taken {1} percentage of total time",percentauthtoken,percenagecacheop));
            Console.WriteLine("total files " + files.Length);
            stopwatch.Stop();
            Console.WriteLine("time taken by algo " +stopwatch.ElapsedMilliseconds);

            //string result=string.Join("", lines);
            //WriteInOutPutFile(stringBuilder.ToString(), true);

        }

        private static void ParseFile(string[] lines,ref long timeTakenInzaccommand,ref long timetakenInGettingAuthToken,ref long timeTakenInGetCacheEntryOperation,
            string filepath)
        {
            string startText = @"[Processing Command for handler zen-login]";
            string endText = @"[Finished Processing Command for handler zen-login.  Result = SUCCESS]";
            
            string authTokenStartSearchText = @"ObtainAuthToken entered";
            string authTokenEndSearchText = @"ObtainAuthToken returned with code 0";
            string cacheStartText = @"GetObject(userResponse:Registration";
            string cacheEndText = @"getcacheEntry exited for key userResponse:Registration";
            
            DateTime startTimeCallingAuthToken = DateTime.Now;
            DateTime endTimeCallingAuthToken = DateTime.Now;
            DateTime cacheStartTime = DateTime.Now;
            DateTime cacheEndTime = DateTime.Now;
            
            string authTokenStart = string.Empty;

            Dictionary<string, string> timeTaken = new Dictionary<string, string>();
            string dateTimeFormat = @"MM/dd/yyyy hh:mm:ss.fff";
            int processid = 0;
            int threadid = 0;
            int allSearchTextFound = 0;
            DateTime zenlgnStart = DateTime.Now;
            DateTime zenlgnEnd = DateTime.Now;
            foreach (var item in lines)
            {
                
                if (item.Contains(startText))
                {
                    processid = Convert.ToInt32(item.Split('[')[3].Split(']')[0]);
                    threadid = Convert.ToInt32(item.Split('[')[5].Split(']')[0]);
                    string temp = item.Split('[')[2];
                    allSearchTextFound++;
                    zenlgnStart = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                }
                if (item.Contains(authTokenStartSearchText) && string.IsNullOrEmpty(authTokenStart))
                {
                    if (Convert.ToInt32(item.Split('[')[3].Split(']')[0]) == processid && Convert.ToInt32(item.Split('[')[5].Split(']')[0]) == threadid)
                    {
                        startTimeCallingAuthToken = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                        allSearchTextFound++;
                    }
                }
                if (item.Contains(authTokenEndSearchText))
                {
                    if (Convert.ToInt32(item.Split('[')[3].Split(']')[0]) == processid && Convert.ToInt32(item.Split('[')[5].Split(']')[0]) == threadid)
                    {
                        endTimeCallingAuthToken = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                        allSearchTextFound++;
                    }

                }
                if (item.Contains(cacheStartText))
                {
                    if (Convert.ToInt32(item.Split('[')[3].Split(']')[0]) == processid && Convert.ToInt32(item.Split('[')[5].Split(']')[0]) == threadid)
                    {
                        cacheStartTime = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                        allSearchTextFound++;
                    }
                }
                if (item.Contains(cacheEndText))
                {
                    if (Convert.ToInt32(item.Split('[')[3].Split(']')[0]) == processid && Convert.ToInt32(item.Split('[')[5].Split(']')[0]) == threadid)
                    {
                        allSearchTextFound++;
                        cacheEndTime = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                    }
                }
                if (item.Contains(endText))
                {
                    authTokenStart = string.Empty;
                    if (Convert.ToInt32(item.Split('[')[3].Split(']')[0]) == processid && Convert.ToInt32(item.Split('[')[5].Split(']')[0]) == threadid)
                    {
                        zenlgnEnd = DateTime.ParseExact(item.Split('[')[2].Split(']')[0], dateTimeFormat, CultureInfo.InvariantCulture);
                        allSearchTextFound++;
                    }
                    processid = 0;
                    threadid = 0;
                    if(allSearchTextFound==6)
                    {
                        TimeSpan timeTakenByAuthToken = endTimeCallingAuthToken - startTimeCallingAuthToken;
                        Console.WriteLine("time taken by auth token in milliseconds: " + timeTakenByAuthToken.TotalMilliseconds);
                        TimeSpan timeTakenByCache = cacheEndTime - cacheStartTime;
                        TimeSpan timeTakenByZaccommand = zenlgnEnd - zenlgnStart;
                        timeTakenInzaccommand = timeTakenInzaccommand + (long)(timeTakenByZaccommand.TotalMilliseconds);
                        timetakenInGettingAuthToken = timetakenInGettingAuthToken + (long)timeTakenByAuthToken.TotalMilliseconds;
                        timeTakenInGetCacheEntryOperation = timeTakenInGetCacheEntryOperation + (long)timeTakenByCache.TotalMilliseconds;
                        string message = string.Format("time taken by zac lgn :{0}, authtoken call : {1} , getcacheobject call :{2},zac lgn started at :{3},file name :{4}  ",
                            (zenlgnEnd - zenlgnStart).TotalMilliseconds, timeTakenByAuthToken.TotalMilliseconds, timeTakenByCache.TotalMilliseconds, zenlgnStart,filepath);
                        Console.WriteLine(message);
                        WriteInOutPutFile(message, true);
                        Console.WriteLine("time taken by cache operation in milliseconds: " + timeTakenByCache.TotalMilliseconds);
                        allSearchTextFound = 0;
                    }
                    
                   

                }
                
            }

        }

        private static string[] GetAllLines(string logFilePath)
        {
            string[] lines;
           
            lines = File.ReadAllLines(logFilePath);
            
            return lines;
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
