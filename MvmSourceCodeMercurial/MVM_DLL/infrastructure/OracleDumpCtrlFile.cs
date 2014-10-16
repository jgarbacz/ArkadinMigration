using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MVM
{
    /*
   load data infile 'junk' "str '|\r\n'" append into tableName owner_accessory fields terminated by '|' trailing nullcols (
    )

    */
    class OracleDumpCtrlFile
    {
        public static readonly string rdelPat = "\"str +'(.+?)'\"";
        public static readonly string fdelPat = "fields terminated by '(.+?)'";
        public static readonly string fieldPattern = @"^([a-zA-Z0-9_]+).*$";
        public static readonly string tableNamePat = "into table ([a-zA-Z0-9_]+)";
        public static readonly Regex fdelRegex = new Regex(fdelPat, RegexOptions.Compiled);
        public static readonly Regex rdelRegex = new Regex(rdelPat, RegexOptions.Singleline);
        public static readonly Regex fieldRegex = new Regex(fieldPattern, RegexOptions.Compiled);
        public static readonly Regex tableNameRegex = new Regex(tableNamePat, RegexOptions.Compiled);

        public List<string> orderedFieldNames = new List<string>();
        public string fieldDelim { get; set; }
        public string recordDelim { get; set; }
        public string tableName { get; set; }

        // copy constructor
        public OracleDumpCtrlFile(OracleDumpCtrlFile copy)
        {
            this.orderedFieldNames = copy.orderedFieldNames.CopyShallow();
            this.fieldDelim = copy.fieldDelim;
            this.recordDelim = copy.recordDelim;
            this.tableName = copy.tableName;
        }

        public static string HexStringToString(string hex)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string hexVal in hex.Cluster(2))
            {
                int intVal = Convert.ToInt32(hexVal, 16);
                char c = Convert.ToChar(intVal);
                sb.Append(c);
            }
            return sb.ToString();
        }


        public OracleDumpCtrlFile(string file)
        {
            StreamReader r = File.OpenText(file);
            string ctlText = r.ReadToEnd();
            r.Close();

            if (ctlText == null) throw new Exception("Error, ctrl file is empty [" + file + "]");

            // find the tableName
            {
                Match m = tableNameRegex.Match(ctlText);
                if (m.Success)
                {
                    this.tableName = m.Groups[1].ToString();
                }
                else
                {
                    throw new Exception("Error, cannot determine table name pat=[" + tableNamePat + "] in ctl file: " + file + ".\r\ntext=" + ctlText);
                }
            }


            // find the field delim
            {
                Match m = fdelRegex.Match(ctlText);
                if (m.Success)
                {
                    this.fieldDelim = m.Groups[1].ToString();
                }
                else
                {
                    throw new Exception("Error, cannot determine field delimiter pat=[" + fdelPat + "] in ctl file: " + file + ".\r\ntext=" + ctlText);
                }
            }

            // find the record delim
            //Console.WriteLine("rdelRegex=" + rdelRegex.ToString());
            //Console.WriteLine("ctrltxt=" + ctlText);
            {
                Match m = rdelRegex.Match(ctlText);
                if (m.Success)
                {
                    this.recordDelim = m.Groups[1].ToString();
                }
                else
                {
                    //Console.WriteLine("DEFAULT THE RECORD DELIM");
                    this.recordDelim = "\r\n";
                }
            }

            //Console.WriteLine("fd=[" + this.fieldDelim + "]");
            //Console.WriteLine("rd=[" + this.recordDelim + "]");

            this.fieldDelim = this.fieldDelim.InterpolateEscapes();
            this.recordDelim = this.recordDelim.InterpolateEscapes();

            // process the fields
            StringReader reader = new StringReader(ctlText);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.EndsWith("(")) break;
            }
            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                line = line.Trim();
                if (line.StartsWith(")")) continue;
                if (line.Equals("")) continue;
                Match m = fieldRegex.Match(line);
                if (m.Success)
                {
                    string fieldName = m.Groups[1].ToString();
                    this.orderedFieldNames.Add(fieldName);
                }
                else
                {
                    throw new Exception("Error, unexpected line format [" + line + "]");
                }
            }
        }

        public List<string> GetOrderedFieldNames()
        {
            return this.orderedFieldNames;
        }


        public void ToFile(string fileName)
        {
            string fDel = this.fieldDelim.InterpolateEscapesReverse();
            string rDel = this.recordDelim.InterpolateEscapesReverse();
            string tableName = this.tableName == null || this.tableName.Equals("") ? "TABLE" : this.tableName;

            DbUtilsOra.WriteOraSqlldrCtrl(fileName, tableName, fDel, rDel, orderedFieldNames, null, null);
        }


        public static void Test()
        {
            string ctrlFile = "C:\\_ROB\\mvm\\test_ora_ctl\\ctl.txt";
            OracleDumpCtrlFile cf = new OracleDumpCtrlFile(ctrlFile);
            Console.WriteLine("fields:");
            Console.WriteLine("fd=[" + cf.fieldDelim + "]");
            Console.WriteLine("rd=[" + cf.recordDelim + "]");
            Console.WriteLine(cf.GetOrderedFieldNames().Join("\r\n"));
        }
    }
}
