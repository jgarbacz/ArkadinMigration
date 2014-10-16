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
<date_convert>
<from format='YYYY-MM-DD HH24:MI:SS'>OBJECT.date</from>
<to format='YYYYMMDDHH24MISS'>OBJECT.date</to>
</date_convert>
     */
    class MDateConvert : IModuleSetup, IModuleRun
    {
        // from xml
        private string fromSyntax;
        private string toSyntax;

        // from setup
        private IReadString fromParsed;
        private IWriteString toParsed;

        // assumed to be static
        private string fromFormat;
        private string toFormat;

        private int convertType;

        private string dbType;
        private string defaultDateFormat;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDateConvert m = new MDateConvert();

            string targetLoginOid = mc.mvm.globalContext["default_database_login"];
            if (!targetLoginOid.IsNullOrEmpty())
            {
                using (IObjectData targetLogin = mc.mvm.objectCache.CheckOut(targetLoginOid))
                {
                    dbType = targetLogin["database_type"];
                }
            }
            else
            {
                targetLoginOid = mc.mvm.globalContext["target_login"];
                if (!targetLoginOid.IsNullOrEmpty())
                {
                    using (IObjectData targetLogin = mc.mvm.objectCache.CheckOut(targetLoginOid))
                    {
                        dbType = targetLogin["database_type"];
                    }
                }
            }

            if (dbType.Equals("sql"))
            {
                defaultDateFormat = "YYYY-MM-DD HH24:MI:SS.mmm";
            }
            else
            {
                defaultDateFormat = "YYYYMMDDHH24MISS";
            }

            // xml extraction
            XmlElement fromElem = me.SelectSingleElem("./from");
            XmlElement toElem = me.SelectSingleElem("./to");

            m.fromSyntax = fromElem.InnerText;
            m.toSyntax = toElem.InnerText;
            m.fromParsed = mc.ParseSyntax(m.fromSyntax);
            m.toParsed = mc.ParseWritableSyntax(m.toSyntax);

            m.fromFormat = fromElem.GetAttributeDefault("format", defaultDateFormat);
            m.toFormat = toElem.GetAttributeDefault("format", defaultDateFormat);

            if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 1;
            else if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 1;
            else if (m.fromFormat.Equals("DD/MM/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 2;
            else if (m.fromFormat.Equals("MM/DD/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 3;
            else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("MM/DD/YYYY")) m.convertType = 4;
            else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.convertType = 5;
            else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.convertType = 6;
            else
            {
                // Assume that any other formats are arbitrary C# date formats and try to do the conversion
                m.convertType = 99;
            }

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string from = this.fromParsed.Read(mc);
            if (from.Equals(""))
            {
                this.toParsed.Write(mc, "");
                return;
            }
            string to = "";
            switch (this.convertType)
            {
                // if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 1;
                case 1:
                    to = from.Substring(0, 4) + from.Substring(5, 2) + from.Substring(8, 2) + from.Substring(11, 2) + from.Substring(14, 2) + from.Substring(17, 2);
                    break;
                // else if (m.fromFormat.Equals("DD/MM/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 2;
                case 2:
                    to = from.Substring(6, 4) + from.Substring(3, 2) + from.Substring(0, 2) + "000000";
                    break;
                // else if (m.fromFormat.Equals("MM/DD/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 3;
                case 3:
                    to = from.Substring(5, 4) + from.Substring(0, 2) + from.Substring(3, 2) + "000000";
                    break;
                // else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("MM/DD/YYYY")) m.convertType = 4;
                case 4:
                    to = from.Substring(4, 2) + "/" + from.Substring(6, 2) + "/" + from.Substring(0, 4);
                    break;
                // else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.convertType = 5;
                case 5:
                    to = from.Substring(0, 4) + "-" + from.Substring(4, 2) + "-" + from.Substring(6, 2) + " " + from.Substring(8, 2) + ":" + from.Substring(10, 2) + ":" + from.Substring(12, 2);
                    break;
                // else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.convertType = 6;
                case 6:
                    to = from.Substring(0, 4) + "-" + from.Substring(4, 2) + "-" + from.Substring(6, 2) + " " + from.Substring(8, 2) + ":" + from.Substring(10, 2) + ":" + from.Substring(12, 2) + ".000";
                    break;
                case 99:
                    try
                    {
                        to = DateTime.ParseExact(from, this.fromFormat, null).ToString(this.toFormat);
                    }
                    catch (System.FormatException e)
                    {
                        throw new Exception("Cannot convert from " + this.fromFormat + " to " + this.toFormat + " with date " + from + ": " + e);
                        // We will return empty string if the conversion failed
                    }
                    break;
                default:
                    throw new Exception("unexpected date convert type=" + this.convertType);
            }
            this.toParsed.Write(mc, to);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("date_convert:" + this.fromSyntax);
        }
    }
}
