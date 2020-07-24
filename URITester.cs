using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class URITester
    {
        public void TestURI()
        {
            string path = @"\\svmzedc\shr_dat$\REF\PDF_XChange_Pro\_Install\7.0";
            string ppath = Path.GetDirectoryName(path);
            Console.WriteLine(ppath);
        }
    }
}
