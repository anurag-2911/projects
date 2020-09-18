using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class SortParentChild
    {
        public static void TestSort()
        {
            Workset workset1 = new Workset { masterItemID = "6b9c0ebf7180f7184884577a4d299937", currentItemID = "c5082c2ea95e51be0449ab5a5c869557", immediateMasterItemID = "028993c3c6206661398141ec2128c786" };
            Workset workset2 = new Workset { masterItemID = "6b9c0ebf7180f7184884577a4d299937", currentItemID = "6b9c0ebf7180f7184884577a4d299937", immediateMasterItemID = null };
            Workset workset3 = new Workset { masterItemID = "6b9c0ebf7180f7184884577a4d299937", currentItemID = "028993c3c6206661398141ec2128c786", immediateMasterItemID = "6b9c0ebf7180f7184884577a4d299937" };
            //Workset workset4 = new Workset { masterItemID = "xyz", currentItemID = "ghi", immediateMasterItemID = "def" };
            //Workset workset7 = new Workset { masterItemID = "xy", currentItemID = "ghi", immediateMasterItemID = "abc" };
            //Workset workset5 = new Workset { masterItemID = "xy", currentItemID = "xy", immediateMasterItemID = null };
            //Workset workset6 = new Workset { masterItemID = "xy", currentItemID = "abc", immediateMasterItemID = "xy" };
           



            Workset[] worksets = new List<Workset> { workset1, workset2, workset3 }.ToArray();

            Workset[] sortedWorkset = new Workset[worksets.Length];

            int count = worksets.Length;
            InsertionSort(ref worksets);

            foreach (var item in worksets)
            {
                Console.WriteLine(item);
            }
            
        }
        static void InsertionSort(ref Workset[] worksets)
        {
            int i, j;
            
            for (i = 1; i < worksets.Length; i++)
            {
                Workset key = worksets[i];
                j = i - 1;

                /* Move elements of arr[0..i-1], that are  
                greater than key, to one position ahead  
                of their current position */
                while (j >= 0 && worksets[j].currentItemID == key.immediateMasterItemID)
                {
                    worksets[j + 1] = worksets[j];
                    j = j - 1;
                }
                worksets[j + 1] = key;
            }
        } // end insertionSort()

        class Workset
        {
            public string masterItemID { get; set; }
            public string immediateMasterItemID { get; set; }

            public string currentItemID { get; set; }

            public override string ToString()
            {
                return "masteritemid: " + masterItemID + " " + "immediateMasterItemID " + immediateMasterItemID + " " + "currentItemID " + currentItemID;
            }
        }
    }
}
