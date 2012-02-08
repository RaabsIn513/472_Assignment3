using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _472_Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] myList = { 23, 12, 13, 14, 22, 18, 33, 8, 3, 4 };
            int X = 2;
            int piv = 3;
            print(myList);
            select2(ref myList, 0, myList.Length, 3, ref X, ref piv);
            print(myList);

        }

        public static void select2(ref  int[] L, int low, int high, int t, ref int x, ref int pivot )
	    {
            int remainder = 0;
            int[] Meds;
            Console.WriteLine("low: " + low + " high: " + high + " t: " + t);
            Console.WriteLine();
            print(L);
		    if( high - low + 1 <= 5)
		    {
			    // ad hoc method here...
                int f = ad_hoc(L, t, low, high);
                x = f;
                Console.WriteLine("ad_hoc: the median value is: " + x);
		    }
            int q = Convert.ToInt32(Math.Floor(Convert.ToDouble((high - low + 1) / 5)));     // the number of sublists of size 5; 
            
            Meds = MediansSubFives(L, low, high);

            if ((high - low + 1) % 5 != 0) // If there are remainders then do an adhoc approach to get the median
            {
                List<int> LeftOver = new List<int>();
                remainder = (high - low + 1) % 5;
                LeftOver = L.ToList().GetRange(L.Length - remainder, (L.Length - 1) - (L.Length - remainder));
                LeftOver.Sort();
                if (remainder == 1)
                    Meds = LeftOver.ToArray();
                else 
                {
                    int len = LeftOver.Count;
                    if ( len % 2 == 0)      // average of the middle two elements of the sorted list
                        x = ((LeftOver[len / 2] + LeftOver[(len / 2) - 1]) / 2); 
                    else                    // the middle element of the sorted list...
                        x = (LeftOver[Convert.ToInt16(Math.Floor(Convert.ToDouble(len / 2)))]);
                }
                q += 1; // necessary when we append hte median from the overflow elements not in a group of 5.
            }
            //puts "median of medians array: #{median_array}"
            Console.WriteLine("median of medians array: ");
            print(Meds);
            int pos = 0;
            select2(ref Meds, (q - 1) / 2, 0, q - 1, ref pos, ref pivot);   // make a recursive call to select2 in order to get the pseudomedian value
            // puts "Pivot value from median of medians: #{pivot}"
            Console.WriteLine("Pivot value from median of medians: " + pivot);
            int index_pivot = L.ToList().IndexOf(pivot);        // get the index of the pivot element to be used
            int temp = L[low];                                  // swap the value at the low index with the value at the pivot index
            L[low] = L[index_pivot];
            L[index_pivot] = temp;
            
            partition2(ref L, low, high, ref pos);     // call partition using the first element in the array (pivot value)

            if (t == pos)
                x = L[pos];
            if (t < pos)
                select2(ref L, low, pos - 1, t, ref x, ref pivot);
            if (t > pos)
                select2(ref L, pos + 1, high, t, ref x, ref pivot);
            
        }

        public static void partition2(ref int[] L, int low, int high, ref int position)
        {
            int movR = low;
            int movL = high;
            int x = L[low];
            while (movR < movL)
            {
                while (L[movR] >= x)
                    movR += 1;
                while (L[movL] <= x)
                    movL -= 1;
                if (L[movR] == L[movL])       // The case of repeated elements continue to increment movR
                    movR += 1;
                if (movR < movL)
                {
                    int temp = L[movL];
                    L[movL] = L[movR];
                    L[movR] = temp;
                }
            }
            position = movL;
        }

        public static int[] MediansSubFives(int[] L, int low, int high)
        {
            int q = Convert.ToInt32(Math.Floor(Convert.ToDouble((high - low + 1) / 5)));   // the number of sublists of size 5. 
            int[] M = new int[q];
            List<int> tempSub = new List<int>();
            List<int> Li = new List<int>();
            tempSub = L.ToList();
            Li = tempSub.GetRange(low, high - low);
            tempSub.Clear();

            for( int i = 0; i < q; i++ )
                M[i] = fastmedian((Li.GetRange(i * 5, 5)).ToArray());

            return M;
        }

        public static int fastmedian(int[] a)
	    {
		    if( a.Length != 5 )
		    {
			    throw new Exception("fastmedian accepts only lists of size 5");
		    }
		    int t = 0;
		    int y = 0;
		    // swap a[1] with a[0];
		    if (a[0]<a[1])
		        t=a[1]; a[1]=a[0]; a[0]=t;
		    // swap a[3] with a[2];
		    if (a[2]<a[3])
		        t=a[2]; a[2]=a[3]; a[3]=t;

		    // swap a[0] with a[2] and swap a[1] with a[3]
		    if (a[0] < a[2])
		    {
			    t=a[0]; a[0]=a[2]; a[2]=t;
		        t=a[1]; a[1]=a[3]; a[3]=t;
		    }

		    // swap a[1] with a[4]
		    if (a[1] < a[4])
		          t=a[1]; a[1]=a[4]; a[4]=t;

		    if (a[1]>a[2])
		    {	    
			    if (a[2]>a[4])
		            y=a[2];
		        else
		            y=a[4];
		    }
		    else if (a[1]>a[3])
		    {        
			    y=a[1];
		    }
		    else
			    y=a[3];
		    return y;
	    }

        public static int ad_hoc(int[] L, int t, int low, int high)
        {
            // returns the element at the median index after sorting.. doesn't return the index. 
            List<int> Li = L.ToList().GetRange(low, high - low);
            Li.Sort();
            int len = (high - low + 1);
            // check if sub list if even
            if (len % 2 == 0)
                return ((Li[len / 2] + Li[(len / 2) - 1]) / 2);
            else
                return (Li[Convert.ToInt16(Math.Floor(Convert.ToDouble(len / 2)))]);
        }

        public static void print( int[] a )
	    {
		    for( int i = 0; i < a.Length; i++ )
		    {
			    if( i != a.Length -1 )
				    Console.Write( a[i] + ", ");
			    else
				    Console.Write( a[i] );
		    }
	    }
	
    }
}

// The program has been ran multiple times with different input sizes.
// If the array you built has <= 5 elements we just perform an adhoc method.
// If the number of elements > 5 then we find the median of medians by breaking the array into
// n-1/5 sublists where n is the length of the array and using the groups of 5 in the fast median
// of five algorithm. Some of the issues I ran into was the pass by reference which is needed for 
// recursion. The pass by reference construct is used to allow the value to be used in subsequent
// operations until the answer is found. 
// For the elements not in a group of five, an adhoc method is used and that value is pushed
// onto the median of medians list. After getting the median of medians list, recursion is used
// to pass the median of medians list back through select2 in order to find the pseudomedian value,
// which is the pivot variable that will be used for partition. Once we get the pivot is known swap the
// value that is located at low with the value at the pivot index in L. Then pass the
// modified array to partition2. Partition2 then organizes are array based on pivot. 
// Elements lower than the pivot value are placed to the left of the pivot while elements greater than
// the pivot are placed to the right of the pivot. Return the position of where the
// movL has stopped, or the index where pivot stopped at. Then knowing what index we want we determine
// what to do using a case statement. If the index inputted by the user equals the position returned we
// know we have the exact value, else if the position is greater than the inputted index we know
// that the value that should be in that index is located from low to position - 1 then we make a
// recursive call again to select2, and finally if the position is less than the index inputted we
// know that the value at the index inputted should be located somewhere from position + 1 to
// high and we make a recursive call again to select2. Then we continue the process with a smaller
// input array, different low and different high and we eventually obtain our value for the smallest
// element that should be located at the index provided by the user.