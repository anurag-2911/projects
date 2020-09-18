using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Reflector
    {
        public static void Reflect()
        {
            try
            {
                Assembly zmdAssembly = Assembly.LoadFrom(@"C:\Program Files (x86)\Novell\ZENworks\bin\zmd.dll");
                Type Zenfile = zmdAssembly.GetType("Novell.Zenworks.ZenFile");
                Type Session = zmdAssembly.GetType("Novell.Zenworks.Zmd.Session");
                Type Progress = zmdAssembly.GetType("Novell.Zenworks.Zmd.Progress");
                MethodInfo UploadFile = Zenfile.GetMethod("UploadFile", new[] { Session, typeof(string), typeof(string), Progress });
                object zenFile = Activator.CreateInstance(Zenfile);

                string src = @"c:\temp\one.txt";
                string destination = @"http://10.71.68.92/zenworks-fileupload/upload?type=messages&overwrite=true&filename=test999.txt";

                UploadFile.Invoke(zenFile, new object[] { null, src, destination, null });

            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());

            }
        }
    }
}
