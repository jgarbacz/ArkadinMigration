using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;

namespace MVM
{
    class FileUtils
    {
        public static void fileconvert(string fromFile, string fromSet, string toFile, string toSet)
        {
            Console.WriteLine("converting file " + fromFile + " [" + fromSet + "] to file " + toFile + " [" + toSet + "].");
            Encoding fromEncoding;
            int fromCodePage;
            if (int.TryParse(fromSet, out fromCodePage))
            {
                fromEncoding = Encoding.GetEncoding(fromCodePage);
            }
            else
            {
                fromEncoding = Encoding.GetEncoding(fromSet);
            }
            Encoding toEncoding;
            int toCodePage;
            if (int.TryParse(toSet, out toCodePage))
            {
                toEncoding = Encoding.GetEncoding(toCodePage);
            }
            else
            {
                toEncoding = Encoding.GetEncoding(toSet);
            }
            StreamReader r = new StreamReader(fromFile, fromEncoding);
            StreamWriter w = new StreamWriter(toFile, false, toEncoding);
            char[] buf = new char[1024 * 100];
            for (; ; )
            {
                int count=r.ReadBlock(buf, 0, buf.Length);
                if (count == 0) break;
                w.Write(buf, 0, count);
            }
            r.Close();
            w.Close();
        }

        const int BYTES_TO_READ = sizeof(Int64);

        public static bool FilesAreEqual(string first, string second)
        {
            return FilesAreEqual(new FileInfo(first), new FileInfo(second));
        }

        public static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }
    }
}
