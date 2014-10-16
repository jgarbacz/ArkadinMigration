using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public static class MyArray
    {
        public static void TestArrayCopyPlans()
        {
            {
                int[] fromArray = new int[] { 3, 4, 6 };
                int[] toArray = new int[] { 1,2,3, 4, 5 };
                Console.WriteLine("from:"+fromArray.JoinStrings(","));
                Console.WriteLine("to:"+toArray.JoinStrings(","));
                int[] plan = fromArray.BuildArrayCopyPlan(toArray);
                Console.WriteLine("plan:"+plan.JoinStrings(","));
                int[] result = fromArray.ArrayCopyWithPlan(plan, -1);
                Console.WriteLine("result:"+result.JoinStrings(","));
            }
        }


        public static T[] ArrayCopyWithPlan<T>(this T[] fromArray, int[] copyPlan, T fillValue)
        {
            int toArrayLen = copyPlan[0];
            T[] toArray = new T[toArrayLen];
            for (int i = 1; i < copyPlan.Length; i += 3)
            {
                long fromIdx = copyPlan[i];
                long toIdx = copyPlan[i + 1];
                long count = copyPlan[i + 2];
                if (fromIdx >= 0)
                {
                    Array.Copy(fromArray,fromIdx, toArray,toIdx, count);
                }
                else
                {
                    // tbd: test native memset http://stackoverflow.com/questions/2303601/what-is-the-fastest-way-to-initialize-all-elements-in-an-array-to-nan
                    for (int j = 0; j < count;j++)
                    {
                        toArray[toIdx+j] = fillValue;
                    }
                }
            }
            return toArray;
        }

        
       
        /// <summary>
        /// Figures out a plan to transform an array of fromFormat to toFormat. Returns the plan which
        /// is packed into an int[]. The format of the plan is as follows:
        /// [toArraySize, fromIdx1, toIdx1, copyCount1, fromIdxN, toIdxN, copyCountN...]
        /// fromIdx LT 0 means new values not in from array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromFormat"></param>
        /// <param name="toFormat"></param>
        /// <returns></returns>
        public static int[] BuildArrayCopyPlan<T>(this T[] fromFormat, T[] toFormat)
        {
            int[] lastOp = null;
            List<int[]> planOps = new List<int[]>();
            for (int toIdx = 0; toIdx < toFormat.Length; toIdx++)
            {
                T toValue=toFormat[toIdx];
                int fromIdx = Array.IndexOf(fromFormat, toValue); // return -1 if not found
                if (lastOp != null)
                {
                    // if this is another missing value, just bump the count
                    if (lastOp[0] == -1)
                    {
                        if (fromIdx == -1)
                        {
                            lastOp[2] += 1;
                            continue;
                        }
                    }
                    // if this is 1 after the last op, just bump the count
                    else if (lastOp[0] == fromIdx - 1)
                    {
                        lastOp[2] += 1;
                        continue;
                    }
                }
                // otherwise add a new entry
                lastOp = new int[] { fromIdx, toIdx, 1 };
                planOps.Add(lastOp);
            }
           
            // flatten planOps into the plan array
            int[] plan = new int[1 + (planOps.Count * 3)];
            int planIdx = 1;
            plan[0]=toFormat.Length;
            foreach (int[] op in planOps)
            {
                plan[planIdx++] = op[0];
                plan[planIdx++] = op[1];
                plan[planIdx++] = op[2];
            }
            return plan;
        }

        public static bool NotNullOrEmpty<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }

        public static T[] ShallowCopy<T>(this T[] array)
        {
            T[] copy = new T[array.Length];
            array.CopyTo(copy, 0);
            return copy;
        }

        // Joins elements together with a delimiter
        public static string Join(this string[] list, string delim)
        {
            if (list.Length <= 0) return "";
            StringBuilder output = new StringBuilder();
            foreach (string elem in list)
            {
                output.Append(elem);
                output.Append(delim);
            }
            return output.ToString().Substring(0, output.Length - delim.Length);
        }

        // Surrounds all elements with the prefix and the suffix returning a new string list
        public static string[] SurroundAll(this string[] list, string prefix, string suffix)
        {
            string[] output = new string[list.Length];
            for(int i=0;i<list.Length;i++)
            {
                string elem=list[i];
                output[i]=prefix + elem + suffix;
            }
            return output;
        }

    }
}
