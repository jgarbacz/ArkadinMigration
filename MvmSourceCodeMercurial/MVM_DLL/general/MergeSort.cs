using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    class MergeSort
    {

        public class MergeSortElem : IComparable<MergeSortElem>
        {
            public int id;
            public StringArray sortKey;
            public string data;
            public MergeSortElem(int id, StringArray sortKey, string data)
            {
                this.id = id;
                this.sortKey = sortKey;
                this.data = data;
            }
            public int CompareTo(MergeSortElem other)
            {
                return sortKey.CompareTo(other.sortKey);
            }
        }

        public class SortElem : IComparable<SortElem>
        {
            public StringArray sortKey;
            public string data;
            public SortElem(StringArray sortKey, string data)
            {
                this.sortKey = sortKey;
                this.data = data;
            }
            public int CompareTo(SortElem other)
            {
                return sortKey.CompareTo(other.sortKey);
            }
        }

        // returns array of ranges [start,end] inclusive
        public static List<long[]> ChunkFile(int chunkBytes, string fileName, string rdel)
        {
            List<long[]> offsets = new List<long[]>();
            FileInfo info = new FileInfo(fileName);
            long len = info.Length;
            using (RecordReader rr = new RecordReader(fileName, rdel))
            {
                long start = 0;
                for (; ; )
                {
                    if (start >= len) return offsets;
                    long end = start + chunkBytes;
                    if (end >= (len - 1))
                    {
                        end = len - 1;
                        offsets.Add(new long[] { start, end });
                        return offsets;
                    }
                    else
                    {
                        rr.Seek(end);
                        string junk=rr.ReadLine();
                        end = rr.Position() - 1;
                        offsets.Add(new long[] { start, end });
                        start = rr.Position();
                    }
                }
            }
        }

        public static void MergeSortFile(int chunkBytes, string fileName, string sortFileName, string fdel, string rdel, int[] colNos)
        {
            //chunk up the file
            var offsets = ChunkFile(chunkBytes, fileName, rdel);
            // if just one partions, sort the file directly
            if (offsets.Count == 1)
            {
                SortFile(fileName, sortFileName, fdel, rdel, colNos, offsets[0][0], offsets[0][1]);
                return;
            }
            //sort chunks
            List<string> tempFiles = new List<string>();
            foreach (var offset in offsets)
            {
                long start = offset[0];
                long end = offset[1];
                string tempFileName = System.IO.Path.GetTempFileName();
                SortFile(fileName, tempFileName, fdel, rdel, colNos, start, end);
                tempFiles.Add(tempFileName);
            }
            //merge chunks
            MergeFiles(tempFiles, sortFileName, fdel, rdel, colNos);
        }

        // sorts the passed file
        public static void SortFile(string fileName, string sortFileName, string fdel, string rdel, int[] colNos,long start, long end)
        {
            Console.WriteLine("sort: file="+fileName+",sortFileName="+sortFileName+", "+start+"-"+end);
            List<SortElem> list = new List<SortElem>();
            RecordReader rr = new RecordReader(fileName, rdel);
            rr.Seek(start);
            string[] fdelArr = new String[] { fdel };
            string ln;
            while ((ln = rr.ReadLine()) != null)
            {
                string[] cols = ln.Split(fdelArr, StringSplitOptions.None);
                StringArray key = new StringArray(colNos.Length);
                for (int i = 0; i < colNos.Length; i++)
                {
                    key[i] = cols[colNos[i]];
                }
                SortElem sortElem = new SortElem(key, ln);
                list.Add(sortElem);
                if (rr.Position() > end) break;
            }
            rr.Close();
            list.Sort();
            StreamWriter sw = new StreamWriter(sortFileName);
            foreach (SortElem elem in list)
            {
                sw.Write(elem.data);
                sw.Write(rdel);
            }
            sw.Close();
        }

        // merges sorted files
        public static void MergeFiles(List<string> fileNames, string sortFileName, string fdel, string rdel, int[] colNos)
        {
            Console.WriteLine("mergeFiles:");
            Console.WriteLine(fileNames.Join(",\r\n"));
            Console.WriteLine("sortFileName=" + sortFileName);
            Console.WriteLine("colNos=" + colNos.Join(","));
           
            string[] fdelArr = new String[] { fdel }; 
            List<MergeSortElem> topSorted = new List<MergeSortElem>();
            List<RecordReader> rrs = new List<RecordReader>();
            for(int id=0;id<fileNames.Count;id++)
            {
                string fileName=fileNames[id];
                RecordReader rr=new RecordReader(fileName,rdel);
                rrs.Add(rr);
                MergeSortElem mse=GetMergeSortElem(id,rr,fdelArr,colNos);
                if (mse != null) topSorted.Add(mse);
            }
            StreamWriter sw = new StreamWriter(sortFileName);
            while (topSorted.Count > 0)
            {
                topSorted.Sort();
                MergeSortElem curr = topSorted[0];
                sw.Write(curr.data);
                sw.Write(rdel);
                RecordReader rr = rrs[curr.id];
                MergeSortElem mse = GetMergeSortElem(curr.id, rr, fdelArr, colNos);
                if (mse != null)
                {
                    topSorted[0] = mse;
                }
                else
                {
                    topSorted.RemoveAt(0);
                }
            }
            sw.Close();
        }

        // Takes a line form the reader and loads in into a new MergeSortElem
        public static MergeSortElem GetMergeSortElem(int id,RecordReader rr, string[] fdelArr, int[] colNos){
            string ln = rr.ReadLine();
            if (ln == null) return null;
            string[] cols = ln.Split(fdelArr, StringSplitOptions.None);
            StringArray key = new StringArray(colNos.Length);
            for (int j = 0; j < colNos.Length; j++)
            {
                key[j] = cols[colNos[j]];
            }
            MergeSortElem mse = new MergeSortElem(id,key, ln);
            return mse;
        }

        public static void Test()
        {
            string file = "C:\\_ROB\\mvm\\test_file_sort\\input.dat.txt";
            //MergeSort.SortFile(file,file+".sorted.txt","<|","<\r\n",new int[]{2,1});
            //MergeSort.SortFile(file, file + ".sorted.txt", "|", "\r\n", new int[] { 0 });
            //MergeSort.MergeSortFile(1,file, file + ".sorted.txt", "|", "\r\n", new int[] { 0 });
            MergeSort.MergeSortFile(25, file, file + ".sorted.txt", "<|", "+\r\n", new int[] { 2,1 });
        }


        //// merges sorted files
        //public static void MergeFiles(List<string> fileNames, string sortFileName, string fdel, string rdel, int[] colNos)
        //{
        //    Console.WriteLine("mergeFiles:");
        //    Console.WriteLine(fileNames.Join(",\r\n"));
        //    Console.WriteLine("sortFileName=" + sortFileName);
        //    Console.WriteLine("colNos=" + colNos.Join(","));

        //    string[] fdelArr = new String[] { fdel };

        //    StreamWriter sw = new StreamWriter(sortFileName);

        //    List<SortElem> list = new List<SortElem>();
        //    List<RecordReader> rrs = new List<RecordReader>();
        //    foreach (string fileName in fileNames)
        //    {
        //        rrs.Add(new RecordReader(fileName, rdel));
        //    }
        //    while (rrs.Count > 0)
        //    {
        //        for (int i = rrs.Count - 1; i >= 0; i--)
        //        {
        //            RecordReader rr = rrs[i];
        //            string ln = rr.ReadLine();
        //            if (ln == null)
        //            {
        //                rrs.PurgeCluster(rr);
        //            }
        //            else
        //            {
        //                string[] cols = ln.Split(fdelArr, StringSplitOptions.None);
        //                StringArray key = new StringArray(colNos.Length);
        //                for (int j = 0; j < colNos.Length; j++)
        //                {
        //                    key[j] = cols[colNos[j]];
        //                }
        //                SortElem sortElem = new SortElem(key, ln);
        //                list.Add(sortElem);
        //            }
        //        }
        //        list.Sort();
        //        foreach (SortElem childElem in list)
        //        {
        //            sw.Write(childElem.data);
        //            sw.Write(rdel);
        //        }
        //        list.Clear();
        //    }
        //    sw.FlushToFile();
        //}

    }
}
