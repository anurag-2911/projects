using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary
{
	
	public class MergeSort: SortingAlgo
	{
        public override void Sort(int[] array)
        {
			Sort(array, 0, array.Length - 1);
        }

        void Merge(int[] arr, int l, int m, int r)
		{
			int n1 = m - l + 1;
			int n2 = r - m;

			int[] L = new int[n1];
			int[] R = new int[n2];

			for (int i1 = 0; i1 < n1; ++i1)
				L[i1] = arr[l + i1];
			for (int j1 = 0; j1 < n2; ++j1)
				R[j1] = arr[m + 1 + j1];

			int i = 0, j = 0;

			int k = l;
			while (i < n1 && j < n2)
			{
				if (L[i] <= R[j])
				{
					arr[k] = L[i];
					i++;
				}
				else
				{
					arr[k] = R[j];
					j++;
				}
				k++;
			}

			while (i < n1)
			{
				arr[k] = L[i];
				i++;
				k++;
			}

			while (j < n2)
			{
				arr[k] = R[j];
				j++;
				k++;
			}
		}

		
		void Sort(int[] arr, int l, int r)
		{
			if (l < r)
			{
				// Find the middle point 
				int m = (l + r) / 2;

				// Sort first and second halves 
				Sort(arr, l, m);
				Sort(arr, m + 1, r);

				// Merge the sorted halves 
				Merge(arr, l, m, r);
			}
		}

		
	}
	

}
