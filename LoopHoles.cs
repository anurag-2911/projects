using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class LoopHoles
    { 
        public static void TestLoop()
        {
            int count = 4;
            int i = 0;
            while(i<=count)
            {
                if(i==3)
                {
                    break;
                }
                i++;
            }
            
        }
    }
}
