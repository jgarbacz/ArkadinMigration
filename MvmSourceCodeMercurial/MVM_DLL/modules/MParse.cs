using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     <parse>
        <input>OBJECT.record</input>
        <format fixed_length='true' trim='true'>
          <field name='record_type' length='1'/>
          <field name='first' length='2'/>
          <field name='second' length='2'/>
        </format>
      </parse>
    */

    class MParse : IModuleSetup, IModuleRun
    {
        private string inputSyntax;
        private IReadString inputParsed;
        private XmlSpecifiedFormat xmlSpecifiedFormat;
        private IFormatParser objectFieldParser;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MParse m = new MParse();
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.xmlSpecifiedFormat = new XmlSpecifiedFormat(me.SelectSingleElem("./format"));
            m.objectFieldParser = m.xmlSpecifiedFormat.GetFormatParser();
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string inputValue = this.inputParsed.Read(mc);
            this.objectFieldParser.Parse(mc, inputValue);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("parse:");
        }
    }

    /// <summary>
    /// XmlSpecifiedFormat is the standard way we specify <format/> for different modules such as
    /// reading/writing to files, strings, and buffers.
    /// </summary>
    public class XmlSpecifiedFormat
    {
        XmlElement formatElem;
        public XmlSpecifiedFormat(XmlElement formatElem)
        {
            this.formatElem = formatElem;
        }

        /// <summary>
        /// Returns a parser able to parse the passed format.
        /// </summary>
        /// <returns></returns>
        public IFormatParser GetFormatParser()
        {
            if (!this.formatElem.GetAttribute("fixed_length").IsNullOrEmpty())
            {
                string fieldDefault = this.formatElem.GetAttributeDefaulted("default", null);
                bool trim = this.formatElem.GetAttributeDefaulted("trim", "false").Equals("true") ? true : false;
                List<string> fieldNames = new List<string>();
                List<int> fieldLengths = new List<int>();
                foreach (XmlElement elem in this.formatElem.SelectNodes("./field"))
                {
                    string fieldName = elem.GetAttribute("name");
                    int fieldLength = int.Parse(elem.GetAttribute("length"));
                    fieldNames.Add(fieldName);
                    fieldLengths.Add(fieldLength);
                }
                return new FixedLengthParser(fieldNames, fieldLengths, fieldDefault, trim);
            }
            if (!this.formatElem.GetAttribute("delimited").IsNullOrEmpty())
            {
                string fieldDefault = this.formatElem.GetAttributeDefaulted("default", null);
                string fieldDelim = this.formatElem.HasAttribute("field_delim") ? this.formatElem.GetAttribute("field_delim") : "";
                string fieldDelimRegex = this.formatElem.HasAttribute("field_delim_regex") ? this.formatElem.GetAttribute("field_delim_regex") : "";
                string recordDelim = this.formatElem.HasAttribute("record_delim") ? this.formatElem.GetAttribute("record_delim") : FileUtils2.DefaultNewline;
                bool trim = this.formatElem.GetAttributeDefaulted("trim", "false").Equals("true");
                List<string> fieldNames = new List<string>();
                foreach (XmlElement elem in this.formatElem.SelectNodes("./field"))
                {
                    fieldNames.Add(elem.GetAttribute("name"));
                }
                return new DelimitedParser(fieldNames, fieldDefault, fieldDelim, fieldDelimRegex, recordDelim, trim);
            }
            throw new Exception("format type not supported");
        }

        /// <summary>
        /// Returns the appropriate format writer.
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public IFormatWriter GetFormatWriter(ModuleContext mc)
        {
            // fixed_length
            if (this.formatElem.HasAttribute("fixed_length"))
            {
                List<string> fieldNames = new List<string>();
                List<int> fieldLengths = new List<int>();
                List<IReadString> fieldValues = new List<IReadString>();
                string recordDelim = this.formatElem.HasAttribute("record_delim") ? this.formatElem.GetAttribute("record_delim") : FileUtils2.DefaultNewline;
                foreach (XmlElement elem in this.formatElem.SelectNodes("./field"))
                {
                    string fieldName = elem.GetAttribute("name");
                    int fieldLength = int.Parse(elem.GetAttribute("length"));
                    string overrideValue = elem.InnerText;
                    string fieldValueSyntax = "OBJECT." + fieldName;
                    if (!overrideValue.Equals("")) fieldValueSyntax = overrideValue;
                    IReadString fieldValueParsed = mc.ParseSyntax(fieldValueSyntax);
                    fieldNames.Add(fieldName);
                    fieldLengths.Add(fieldLength);
                    fieldValues.Add(fieldValueParsed);
                }
                return new FixedLengthWriter(fieldNames, fieldLengths, fieldValues, recordDelim);
            }
            // Delimited
            if (this.formatElem.HasAttribute("field_delim")  || this.formatElem.HasAttribute("record_delim"))
            {
                List<string> fieldNames = new List<string>();
                List<IReadString> fieldValues = new List<IReadString>();
                bool trim = this.formatElem.GetAttributeDefaulted("trim", "false").Equals("true") ? true : false;
                string fieldDelim = this.formatElem.HasAttribute("field_delim") ? this.formatElem.GetAttribute("field_delim") : ",";
                string recordDelim = this.formatElem.HasAttribute("record_delim") ? this.formatElem.GetAttribute("record_delim") : FileUtils2.DefaultNewline;
                foreach (XmlElement elem in this.formatElem.SelectNodes("./field"))
                {
                    string fieldName = elem.GetAttribute("name");
                    string overrideValue = elem.InnerText;
                    string fieldValueSyntax = "OBJECT." + fieldName;
                    if (!overrideValue.Equals("")) fieldValueSyntax = overrideValue;
                    IReadString fieldValueParsed = mc.ParseSyntax(fieldValueSyntax);
                    fieldNames.Add(fieldName);
                    fieldValues.Add(fieldValueParsed);
                }
                return new DelimitedWriter(fieldNames, fieldValues, fieldDelim, recordDelim, trim);

            }
            throw new Exception("format type not supported");
        }
    }

    /// <summary>
    /// Writes format to a StreamWriter
    /// </summary>
    public interface IFormatWriter
    {
        void Write(ModuleContext mc, StreamWriter streamWriter);
    }

    /// <summary>
    /// Parses string into fields
    /// </summary>
    public interface IFormatParser
    {
        bool Parse(ModuleContext mc, string inputValue);
    }

    /// <summary>
    /// Reads fixed length records
    /// </summary>
    public class FixedLengthParser : IFormatParser
    {
        public readonly List<string> fieldNames;
        public readonly List<int> fieldLengths;
        public readonly string fieldDefault;
        public readonly bool trim;
        public FixedLengthParser(List<string> fieldNames, List<int> fieldLengths, string fieldDefault, bool trim)
        {
            this.fieldDefault = fieldDefault;
            this.fieldLengths = fieldLengths;
            this.fieldNames = fieldNames;
            this.trim = trim;
        }
        public bool Parse(ModuleContext mc, string inputValue)
        {
            int pos = 0;
            for (int i = 0; i < fieldNames.Count; i++)
            {
                string fieldName = fieldNames[i];
                int fieldLength = fieldLengths[i];
                string fieldValue = inputValue.Substring(pos, fieldLength);
                pos += fieldLength;
                if (trim) fieldValue = fieldValue.Trim();
                if (fieldDefault != null)
                {
                    if (fieldValue.Equals("")) fieldValue = fieldDefault;
                }
                mc.objectData[fieldName] = fieldValue;
            }
            return true;//TBD, make this indicate success/error on parse
        }
    }

    /// <summary>
    ///  Writes out fixed length records
    /// </summary>
    public class FixedLengthWriter : IFormatWriter
    {
        public readonly List<string> fieldNames;
        public readonly List<int> fieldLengths;
        public readonly List<IReadString> fieldValues;
        public readonly string recordDelim;
        public FixedLengthWriter(List<string> fieldNames, List<int> fieldLengths, List<IReadString> fieldValues, string recordDelim)
        {
            this.fieldValues = fieldValues;
            this.fieldLengths = fieldLengths;
            this.fieldNames = fieldNames;
            this.recordDelim = recordDelim;
        }
        public void Write(ModuleContext mc, StreamWriter streamWriter)
        {
            for (int i = 0; i < fieldNames.Count; i++)
            {
                //string fieldName = fieldNames[i];
                int fieldLength = fieldLengths[i];
                string fieldValue = fieldValues[i].Read(mc);
                streamWriter.Write(fieldValue.PadRightFixed(fieldLength));
            }
            streamWriter.Write(this.recordDelim);
        }
    }

    /// <summary>
    /// Reads delimited records
    /// </summary>
    public class DelimitedParser : IFormatParser
    {
        public readonly List<string> fieldNames;
        public readonly string fieldDefault;
        public readonly char[] fieldDelim;
        public readonly string fieldDelimRegex;
        public readonly string recordDelim;
        public readonly Regex fieldRegex;
        public readonly bool trim;
        public readonly bool useRegex = false;
        public DelimitedParser(List<string> fieldNames, string fieldDefault, string fieldDelim, string fieldDelimRegex, string recordDelim, bool trim)
        {
            this.fieldNames = fieldNames;
            this.fieldDefault = fieldDefault;
            this.recordDelim = recordDelim;
            this.trim = trim;
            if (fieldDelim.IsNullOrEmpty() && fieldDelimRegex.IsNullOrEmpty())
            {
                throw new Exception("DelimitedParser must have either field delimiter or field regex!");
            }
            if (!fieldDelimRegex.IsNullOrEmpty())
            {
                this.useRegex = true;
                this.fieldDelimRegex = fieldDelimRegex;
                this.fieldRegex = new Regex(fieldDelimRegex, RegexOptions.Compiled);
            }
            else
            {
                this.fieldDelim = fieldDelim.ToCharArray();
            }
        }
        public bool Parse(ModuleContext mc, string inputValue)
        {
            bool retval = true;
            string[] fields;
            if (this.useRegex)
            {
                fields = this.fieldRegex.Split(inputValue);
            }
            else
            {
                fields = inputValue.Split(fieldDelim);
            }
            if (fields.Length != fieldNames.Count)
            {
                retval = false;
            }
            for (int i = 0; i < fieldNames.Count; i++)
            {
                string fieldValue;
                if (i < fields.Length)
                {
                    fieldValue = fields[i];
                }
                else
                {
                    fieldValue = "";
                }
                if (trim)
                {
                    fieldValue = fieldValue.Trim();
                }
                if (fieldDefault != null && fieldValue.IsNullOrEmpty())
                {
                    fieldValue = fieldDefault;
                }
                mc.objectData[fieldNames[i]] = fieldValue;
            }
            return retval;
        }
    }

    /// <summary>
    /// Writes delimited records to a StreamWriter
    /// </summary>
    public class DelimitedWriter : IFormatWriter
    {
        public readonly List<string> fieldNames;
        public readonly List<int> fieldLengths;
        public readonly List<IReadString> fieldValues;
        public readonly string fieldDelim;
        public readonly string recordDelim;
        public readonly bool trim;
        public DelimitedWriter(List<string> fieldNames, List<IReadString> fieldValues, string fieldDelim, string recordDelim, bool trim)
        {
            this.fieldValues = fieldValues;
            this.fieldNames = fieldNames;
            this.fieldDelim = fieldDelim;
            this.recordDelim = recordDelim;
            this.trim = trim;
        }
        public void Write(ModuleContext mc, StreamWriter streamWriter)
        {
            if (fieldNames.Count > 0)
            {
                string fieldValue = fieldValues[0].Read(mc);
                if (trim) fieldValue = fieldValue.Trim();
                streamWriter.Write(fieldValue);
                for (int i = 1; i < fieldNames.Count; i++)
                {
                    fieldValue = fieldValues[i].Read(mc);
                    streamWriter.Write(fieldDelim);
                    streamWriter.Write(fieldValue);
                }
            }
            streamWriter.Write(recordDelim);
        }
    }
}
