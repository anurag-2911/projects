using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CodeLibrary
{
   public class ZacRef
    {
        public static void TriggerMultipleRefresh()
        {
            for (int i = 0; i < 100; i++)
            {
                TriggerRefresh();
            }
        }
        private static void TriggerRefresh()
        {
            string deviceRefreshTime = string.Empty;
            string userRefreshTime = string.Empty;
            string totalRefreshTime = string.Empty;

            CheckDbHealth();

            ZacRefresh(ref deviceRefreshTime, ref userRefreshTime, ref totalRefreshTime);

            Trace.WriteLine(string.Format("device refresh time : {0}, user refresh time: {1}, total refresh time: {2}",deviceRefreshTime,userRefreshTime,totalRefreshTime));
            Trace.WriteLine("");

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
            if(File.Exists(resultFile))
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

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
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
           // ZacCC();
            Thread.Sleep(1000);

            string command = "zac ref";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
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

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
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
