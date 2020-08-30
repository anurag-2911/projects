using System;
using System.Collections;
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
            TestHashTable();
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

        public static void TestHashTable()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add(1, 1);
            hashtable.Remove(2);
        }
    }
}
