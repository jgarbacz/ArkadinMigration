using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    public class FileUtils2
    {
        public static readonly string DefaultNewline = new StringBuilder().AppendLine().ToString();
        public static readonly char DirSep = Path.DirectorySeparatorChar;

        public static string ConvertGlobToRegex(string glob)
        {
            return '^' + glob.Replace("*", ".*") + '$';
        }

        public static string PathParent(string inputPath)
        {
            inputPath = StripRelativeDirInfo(inputPath);
            return Directory.GetParent(inputPath).ToString();
        }

        public static string PathBasename(string inputPath)
        {
            inputPath = StripRelativeDirInfo(inputPath);
            int i = inputPath.LastIndexOf(DirSep);
            if (i < 0) return "";
            string outputPath = inputPath.Substring(i + 1);
            return outputPath;
        }

        public static List<string> GlobToList(string glob)
        {
            glob = StripRelativeDirInfo(glob);
            List<string> output = new List<string>();
            string head = PathHead(glob) + DirSep;
            string tail = PathTail(glob);
            foreach (string path in GlobToList(head, tail)) output.Add(path);
            return output;
        }

        private static List<string> GlobToList(string head, string tail)
        {
            List<string> output = new List<string>();
            if (PathTail(tail) == tail)
            {
                if (tail.Contains("**"))
                {
                    // ** is customary glob syntax for "match any and all nested subdirectories".
                    // For now, only support ** if it occurs after all directory separators.
                    foreach (string path in Directory.GetFiles(head, tail, SearchOption.AllDirectories))
                    {
                        output.Add(path);
                    }
                    return output;
                }
                foreach (string path in Directory.GetFiles(head, tail)) /*orderby*/
                    output.Add(path);
            }
            else
            {
                foreach (string dir in SafeGetDirectories(head, PathHead(tail))) /*orderby*/
                    foreach (string path in GlobToList(Path.Combine(head, dir), PathTail(tail)))
                        output.Add(path);

            }
            return output;
        }

        public static string StripRelativeDirInfo(string path)
        {
            //Console.WriteLine("path=" + path);
            for (; ; )
            {
                int index = path.IndexOf(DirSep + "..");
                if (index < 0)
                {
                    break;
                }
                else
                {
                    string head = path.Substring(0, index);
                    string tail = path.Substring(index + 3);
                    string parent = Directory.GetParent(head).ToString();
                    path = parent + tail;
                    //Console.WriteLine("path="+path);
                }
            }
            return path;
        }

        public static string[] SafeGetDirectories(string dir, string match)
        {
            try
            {
                return Directory.GetDirectories(dir, match);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR dir=[" + dir + "], match=[" + match + "]");
                throw (e);
            }
        }

        /// <summary>
        /// return a list of files that matches some wildcard pattern, e.g. 
        /// C:\p4\software\dotnet\tools\*\*.sln to get all tool solution files
        /// </summary>
        /// <param name="glob">pattern to match</param>
        /// <returns>all matching paths</returns>
        public static IEnumerable<string> Glob(string glob)
        {
            glob = StripRelativeDirInfo(glob);
            foreach (string path in Glob(PathHead(glob) + DirSep, PathTail(glob)))
                yield return path;
        }

        /// <summary>
        /// uses 'head' and 'tail' -- 'head' has already been pattern-expanded
        /// and 'tail' has not.
        /// </summary>
        /// <param name="head">wildcard-expanded</param>
        /// <param name="tail">not yet wildcard-expanded</param>
        /// <returns></returns>
        private static IEnumerable<string> Glob(string head, string tail)
        {
            if (PathTail(tail) == tail)
            {
                foreach (string path in Directory.GetFiles(head, tail)) /*orderby*/
                    yield return path;
            }
            else
            {
                foreach (string dir in Directory.GetDirectories(head, PathHead(tail))) /*orderby*/
                    foreach (string path in Glob(Path.Combine(head, dir), PathTail(tail)))
                        yield return path;
            }
        }

        /// <summary>
        /// return the first element of a file path
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>first logical unit</returns>
        static string PathHead(string path)
        {
            // RJP
            // Glob was not working when passed a simply filename.txt where it should assume 
            // current directory. Made this default to '.' so if you call path on filename.txt
            // it assumes current dir.
            if (!path.Contains(DirSep.ToString()))
                return ".";

            // handle case of \\share\vol\foo\bar -- return \\share\vol as 'head'
            // because the dir stuff won't let you interrogate a server for its share list
            // FIXME check behavior on Linux to see if this blows up -- I don't think so
            if (path.StartsWith("" + DirSep + DirSep))
                return path.Substring(0, 2) + path.Substring(2).Split(DirSep)[0] + DirSep + path.Substring(2).Split(DirSep)[1];

            return path.Split(DirSep)[0];
        }

        /// <summary>
        /// return everything but the first element of a file path
        /// e.g. PathTail("C:\TEMP\foo.txt") = "TEMP\foo.txt"
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>all but the first logical unit</returns>
        static string PathTail(string path)
        {
            if (!path.Contains(DirSep.ToString()))
                return path;

            return path.Substring(1 + PathHead(path).Length);
        }

        public static void TestGlob()
        {
            string glob = @"F:\_SOURCE\MVM.06102009\MVM\MVM3_overlay_35\bin\Debug\..\..\_TESTS\file_processor\input\*.txt";
            //string glob = @"F:\_ROB\*\*.txt";
            foreach (string v in GlobToList(glob)) Console.WriteLine(v);
        }

        /// <summary>
        /// Move a file, overwriting if it already exists
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        public static void MoveWithReplace(string sourceFileName, string destFileName)
        {
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }
            File.Move(sourceFileName, destFileName);
        }
    }
}
