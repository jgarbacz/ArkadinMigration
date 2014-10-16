using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MValidateField: IModuleSetup, IModuleRun
    {
        public enum DataType { integer, numeric, real, varchar, enumeration, date };
        public enum BooleanType { none, digit, letter };

        private string fieldSyntax;
        private string objectIdSyntax;
        private string typeSyntax;
        private string namespaceSyntax;
        private string formatSyntax;
        private string lengthSyntax;
        private string scaleSyntax;
        private string nullableSyntax;
        private string defaultSyntax;
        private string outputSyntax;
        private string replaceSyntax;

        private IReadString fieldParsed;
        private IReadString objectIdParsed = null;
        private IReadString typeParsed;
        private IReadString namespaceParsed;
        private IReadString formatParsed;
        private IReadString lengthParsed;
        private IReadString scaleParsed;
        private IReadString nullableParsed;
        private IReadString defaultParsed;
        private IReadString outputParsed;
        private IReadString replaceParsed;

        private string fieldName;
        private string type;
        private string nmspace;
        private string length;
        private string scale;
        private string nullable;
        private string defaultValue;
        private string outputFieldName;
        private bool replace;

        private DataType validationType;
        private long maxValue = 0;
        private long minValue = 0;
        private int numPrecision = 0;
        private int numScale = 0;
        private int charLength = 0;
        private string dateFormat = null;
        private IIndex enumIndex = null;
        private BooleanType boolType = BooleanType.none;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MValidateField m = new MValidateField();

            m.fieldSyntax = me.SelectNodeInnerText("./field");
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.typeSyntax = me.SelectNodeInnerText("./datatype", "''");
            m.namespaceSyntax = me.SelectNodeInnerText("./namespace", "''");
            m.formatSyntax = me.SelectNodeInnerText("./format", "''");
            m.lengthSyntax = me.SelectNodeInnerText("./length", "0");
            m.scaleSyntax = me.SelectNodeInnerText("./scale", "0");
            m.nullableSyntax = me.SelectNodeInnerText("./nullable", "1");
            m.defaultSyntax = me.SelectNodeInnerText("./default", "''");
            m.outputSyntax = me.SelectNodeInnerText("./output_field");
            m.replaceSyntax = me.SelectNodeInnerText("./replace", "1");

            m.fieldParsed = mc.ParseSyntax(m.fieldSyntax);
            if (!m.objectIdSyntax.IsNullOrEmpty())
            {
                m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            }
            m.typeParsed = mc.ParseSyntax(m.typeSyntax);
            m.namespaceParsed = mc.ParseSyntax(m.namespaceSyntax);
            m.formatParsed = mc.ParseSyntax(m.formatSyntax);
            m.lengthParsed = mc.ParseSyntax(m.lengthSyntax);
            m.scaleParsed = mc.ParseSyntax(m.scaleSyntax);
            m.nullableParsed = mc.ParseSyntax(m.nullableSyntax);
            try
            {
                m.defaultParsed = mc.ParseSyntax(m.defaultSyntax);
            }
            catch
            {
                // Ignore errors if we can't interpret the default value
                m.defaultParsed = mc.ParseSyntax("''");
            }
            m.outputParsed = mc.ParseSyntax(m.outputSyntax);
            m.replaceParsed = mc.ParseSyntax(m.replaceSyntax);

            m.fieldName = m.fieldParsed.Read(mc).Trim();
            m.type = m.typeParsed.Read(mc).Trim().ToLower();
            m.nmspace = m.namespaceParsed.Read(mc).Trim().ToLower();
            m.dateFormat = m.formatParsed.Read(mc).Trim();
            m.length = m.lengthParsed.Read(mc).Trim();
            m.scale = m.scaleParsed.Read(mc).Trim();
            m.nullable = m.nullableParsed.Read(mc).Trim();
            m.defaultValue = m.defaultParsed != null ? m.defaultParsed.Read(mc).Trim() : "";
            m.outputFieldName = m.outputParsed.Read(mc).Trim();
            m.replace = m.replaceParsed.Read(mc).Trim().Equals("1");

            if (m.type.Equals("bigint"))
            {
                m.maxValue = 9223372036854775807;
                m.minValue = -9223372036854775808;
            }
            else if (m.type.Equals("int"))
            {
                m.maxValue = 2147483647;
                m.minValue = -2147483648;
            }
            else if (m.type.Equals("smallint"))
            {
                m.maxValue = 32767;
                m.minValue = -32768;
            }
            else if (m.type.Equals("tinyint"))
            {
                m.maxValue = 255;
                m.minValue = 0;
            }
            else if (m.type.Equals("bit"))
            {
                m.maxValue = 1;
                m.minValue = 0;
            }
            else if (m.type.Equals("decimal") || m.type.StartsWith("num"))
            {
                m.validationType = DataType.numeric;
                m.numPrecision = Int32.Parse(m.length);
                m.numScale = Int32.Parse(m.scale);
            }
            else if (m.type.In("float", "real"))
            {
                m.validationType = DataType.real;
            }
            else if (m.type.Contains("char"))
            {
                m.validationType = DataType.varchar;
                m.charLength = Int32.Parse(m.length);
            }
            else if (m.type.Contains("enum"))
            {
                m.validationType = DataType.enumeration;
                m.enumIndex = (IIndex)mc.globalContext.GetNamedClassInst("ENUMS_BY_NAME");
            }
            else if (m.type.Contains("date"))
            {
                m.validationType = DataType.date;
            }

            if (m.maxValue > 0)
            {
                m.validationType = DataType.integer;
            }

            if (m.charLength == 1)
            {
                if (m.defaultValue.Equals("0") || m.defaultValue.Equals("1"))
                {
                    m.boolType = BooleanType.digit;
                }
                else if (m.defaultValue.EqualsIgnoreCase("T") || m.defaultValue.EqualsIgnoreCase("F"))
                {
                    m.boolType = BooleanType.letter;
                }
            }

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string input;
            if (this.objectIdParsed != null)
            {
                using (IObjectData obj = mc.objectCache.CheckOut(this.objectIdParsed.Read(mc)))
                {
                    input = obj[fieldName];
                }
            }
            else
            {
                input = mc.objectData[fieldName];
            }

            if (input.Equals(""))
            {
                if (!nullable.Equals("1"))
                {
                    mc.objectData[outputFieldName] += "Field " + fieldName + " violates null constraint. ";
                }
                return;
            }

            long result;
            bool success;
            switch (validationType)
            {
                case DataType.integer:
                    {
                        success = Int64.TryParse(input.StripFractionalTrailingZeros(), out result);
                        if (!(success && result >= minValue && result <= maxValue))
                        {
                            mc.objectData[outputFieldName] += "Field " + fieldName + " [" + input + "] is not a valid integer. ";
                        }
                        break;
                    }
                case DataType.numeric:
                    {
                        string[] split_input = input.Split('.');
                        if (split_input.Length > 1)
                        {
                            string end = split_input[1].TrimEnd('0');
                            if (end.Length > 0)
                            {
                                success = Int64.TryParse(end, out result);
                                if (!(success && end.Length <= numScale))
                                {
                                    mc.objectData[outputFieldName] += "Field " + fieldName + " [" + input + "] has invalid scale. ";
                                }
                            }
                        }
                        string start = split_input[0].TrimStart('0');
                        if (start.Length == 0)
                        {
                            break;
                        }
                        success = Int64.TryParse(start, out result);
                        if (!(success && start.Length <= (numPrecision + (result < 0 ? 1 : 0))))
                        {
                            mc.objectData[outputFieldName] += "Field " + fieldName + " [" + input + "] has invalid precision. ";
                        }
                        break;
                    }
                case DataType.varchar:
                    {
                        if (charLength == 1 && defaultValue.NotNullOrEmpty())
                        {
                            // Special case of a MetraNet boolean field, which we convert to the appropriate true/false representation
                            if (boolType == BooleanType.digit)
                            {
                                mc.objectData[fieldName] = MRecordType.IsTrue(input) ? "1" : "0";
                                break;
                            }
                            else if (boolType == BooleanType.letter)
                            {
                                mc.objectData[fieldName] = MRecordType.IsTrue(input) ? "T" : "F";
                                break;
                            }
                        }
                        if (input.Length > charLength)
                        {
                            mc.objectData[outputFieldName] += "Field " + fieldName + " [" + input + "] overflows database maximum length. ";
                        }
                        break;
                    }
                case DataType.enumeration:
                    {
                        List<string> keys = new List<string>();
                        keys.Add(this.nmspace);
                        keys.Add(input.ToLower());
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        values["id_enum_data"] = "";
                        string retval = enumIndex.IndexGet(mc, keys, values);
                        if (retval.Equals("1"))
                        {
                            if (this.replace)
                            {
                                mc.objectData[fieldName] = values["id_enum_data"];
                            }
                        }
                        else
                        {
                            mc.objectData[outputFieldName] += "Field " + fieldName + " [" + this.nmspace + "/" + input + "] fails enum lookup. ";
                        }
                        break;
                    }
                case DataType.date:
                    {
                        DateTime mydate;
                        success = DateTime.TryParseExact(input, this.dateFormat, null, System.Globalization.DateTimeStyles.None, out mydate);
                        if (!success)
                        {
                            mc.objectData[outputFieldName] += "Field " + fieldName + " [" + input + "] is not a valid date. ";
                        }
                        break;
                    }
            }
        }
    }
}
