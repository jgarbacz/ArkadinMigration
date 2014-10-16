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
<string_to_date>
    <input format='YYYY-MM-DD HH24:MI:SS'>OBJECT.date</input>
    <output>OBJECT.date</output>
</string_to_date>


     */
    class MStringToDate : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        // assumed to be static
        private string fromFormat;
        private string toFormat;

        private int convertType;

        private string dbType;
        private string defaultDateFormat;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MStringToDate m = new MStringToDate();

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
                defaultDateFormat = "YYYY-MM-DD HH24:MI:SS";
            }
            else
            {
                defaultDateFormat = "YYYYMMDDHH24MISS";
            }


            // xml extraction
            XmlElement inputElem = me.SelectSingleElem("./input");
            XmlElement outputElem = me.SelectSingleElem("./output");

            m.inputSyntax = inputElem.InnerText;
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.outputSyntax = outputElem.InnerText;
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);

            if (mc.globalContext["global_date_format"].Equals(""))
            {
                m.toFormat = defaultDateFormat;
                m.fromFormat = inputElem.GetAttributeDefault("format", defaultDateFormat);
                mc.globalContext["global_date_format"] = defaultDateFormat;
                //throw new Exception("GLOBAL.global_date_format is not set!");
            }
            else
            {
                m.fromFormat = inputElem.GetAttributeDefault("format", mc.globalContext["global_date_format"]);
                m.toFormat = mc.globalContext["global_date_format"];
            }

            if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 1;
            else if (m.fromFormat.Equals("DD/MM/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 2;
            else if (m.fromFormat.Equals("MM/DD/YYYY") && m.toFormat.Equals("YYYYMMDDHH24MISS")) m.convertType = 3;
            //else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("MM/DD/YYYY")) m.convertType = 4;
            else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.convertType = 5;
            else if (m.fromFormat.Equals(m.toFormat)) m.convertType = 6;
            else throw new Exception("string_to_date from format [" + m.fromFormat + "] to format ["+m.toFormat+"] is not supported");

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string from = this.inputParsed.Read(mc);
            if (from.Equals(""))
            {
                //this.toParsed.Write(mc, "");
                return;
            }
            string to;
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
                    to = from.Substring(6, 4) + from.Substring(0, 2) + from.Substring(3, 2) + "000000";
                    break;
                // else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("MM/DD/YYYY")) m.convertType = 4;
                case 4:
                    to = from.Substring(4, 2) + "/" + from.Substring(6, 2) + "/" + from.Substring(0, 4);
                    break;
                // else if (m.fromFormat.Equals("YYYYMMDDHH24MISS") && m.toFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.convertType = 5;
                case 5:
                    to = from.Substring(0, 4) + "-" + from.Substring(4, 2) + "-" + from.Substring(6, 2) + " " + from.Substring(8, 2) + ":" + from.Substring(10, 2) + ":" + from.Substring(12, 2);
                    break;
                case 6:
                    to = from;
                    break;
                default:
                    throw new Exception("unexpected date convert type=" + this.convertType);
            }
            this.outputParsed.Write(mc, to);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("date_convert:" + this.inputSyntax);
        }
    }
}

