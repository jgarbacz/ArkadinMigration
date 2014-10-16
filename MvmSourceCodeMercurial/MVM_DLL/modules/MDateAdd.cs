using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <date_add>
    <input>TEMP.in</input>
    <unit>'DD'</unit>
    <increment>32</increment>
    <output>TEMP.out</output>
  </date_add>
      */

    class MDateAdd : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string unitSyntax;
        private string incrementSyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IReadString unitParsed;
        private IReadString incrementParsed;
        private IWriteString outputParsed;

        // assumed to be static
        private string dateFormat;

        private int dateType;

        private string dbType;
        private string defaultDateFormat;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDateAdd m = new MDateAdd();

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
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.unitSyntax = me.SelectNodeInnerText("./unit");
            m.incrementSyntax = me.SelectNodeInnerText("./increment");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.unitParsed = mc.ParseSyntax(m.unitSyntax);
            m.incrementParsed = mc.ParseSyntax(m.incrementSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);

            if (mc.globalContext["global_date_format"].Equals(""))
            {
                m.dateFormat = defaultDateFormat;
                mc.globalContext["global_date_format"] = defaultDateFormat;
                //throw new Exception("GLOBAL.global_date_format is not set!");
            }
            else
            {
                m.dateFormat = mc.globalContext["global_date_format"];
            }

            
            if (m.dateFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.dateType = 1;
            else if (m.dateFormat.Equals("YYYYMMDDHH24MISS")) m.dateType = 2;
            else if (m.dateFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.dateType = 3;
            else throw new Exception("Global date format=" + m.dateFormat + " is not supported");

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string input = this.inputParsed.Read(mc);
            if (input.Equals(""))
            {
                this.outputParsed.Write(mc, "");
                return;
            }

            //if (mc.globalContext["global_date_format"].Equals(""))
            //{
            //    throw new Exception("GLOBAL.global_date_format is not set!");
            //}

            string unit = this.unitParsed.Read(mc);
            string incrementStr=this.incrementParsed.Read(mc);
            int increment=0;
            if (!incrementStr.Equals("")){
                increment=int.Parse(incrementStr);
            }
            //string global_date_format = mc.globalContext["global_date_format"];
            //Console.WriteLine("INPUT======" + input);

            switch (this.dateType)
            {
                // if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.dateType = 1;
                case 1:
                    {
                        int year = int.Parse(input.Substring(0, 4));
                        int month = int.Parse(input.Substring(5, 2));
                        int day = int.Parse(input.Substring(8, 2));
                        int hour = int.Parse(input.Substring(11, 2));
                        int minute = int.Parse(input.Substring(14, 2));
                        int second = int.Parse(input.Substring(17, 2));
                        int millisecond = int.Parse(input.Substring(20, 3));
                        System.DateTime inputDt = new System.DateTime(year, month, day, hour, minute, second, millisecond);
                        System.DateTime outputDt;
                        if (unit.ToUpper().Equals("YYYY")) outputDt = inputDt.AddYears(increment);
                        else if (unit.ToUpper().Equals("MM")) outputDt = inputDt.AddMonths(increment);
                        else if (unit.ToUpper().Equals("DD")) outputDt = inputDt.AddDays(increment);
                        else if (unit.ToUpper().Equals("HH")) outputDt = inputDt.AddHours(increment);
                        else if (unit.ToUpper().Equals("MI")) outputDt = inputDt.AddMinutes(increment);
                        else if (unit.ToUpper().Equals("SS")) outputDt = inputDt.AddSeconds(increment);
                        else throw new Exception("Unexpected date add unit [" + unit + "]");
                        string outputDtStr = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", outputDt);
                        outputParsed.Write(mc, outputDtStr);
                    }
                    break;
                case 2:
                    {
                        int year = int.Parse(input.Substring(0, 4));
                        int month = int.Parse(input.Substring(4, 2));
                        int day = int.Parse(input.Substring(6, 2));
                        int hour = int.Parse(input.Substring(8, 2));
                        int minute = int.Parse(input.Substring(10, 2));
                        int second = int.Parse(input.Substring(12, 2));
                        System.DateTime inputDt = new System.DateTime(year, month, day, hour, minute, second);
                        System.DateTime outputDt;
                        if (unit.ToUpper().Equals("YYYY")) outputDt = inputDt.AddYears(increment);
                        else if (unit.ToUpper().Equals("MM")) outputDt = inputDt.AddMonths(increment);
                        else if (unit.ToUpper().Equals("DD")) outputDt = inputDt.AddDays(increment);
                        else if (unit.ToUpper().Equals("HH")) outputDt = inputDt.AddHours(increment);
                        else if (unit.ToUpper().Equals("MI")) outputDt = inputDt.AddMinutes(increment);
                        else if (unit.ToUpper().Equals("SS")) outputDt = inputDt.AddSeconds(increment);
                        else throw new Exception("Unexpected date add unit [" + unit + "]");
                        string outputDtStr = String.Format("{0:yyyyMMddHHmmss}", outputDt);
                        outputParsed.Write(mc, outputDtStr);
                    }
                    break;
                // if (m.fromFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.dateType = 3;
                case 3:
                    {
                        int year = int.Parse(input.Substring(0, 4));
                        int month = int.Parse(input.Substring(5, 2));
                        int day = int.Parse(input.Substring(8, 2));
                        int hour = int.Parse(input.Substring(11, 2));
                        int minute = int.Parse(input.Substring(14, 2));
                        int second = int.Parse(input.Substring(17, 2));
                        System.DateTime inputDt = new System.DateTime(year, month, day, hour, minute, second);
                        System.DateTime outputDt;
                        if (unit.ToUpper().Equals("YYYY")) outputDt = inputDt.AddYears(increment);
                        else if (unit.ToUpper().Equals("MM")) outputDt = inputDt.AddMonths(increment);
                        else if (unit.ToUpper().Equals("DD")) outputDt = inputDt.AddDays(increment);
                        else if (unit.ToUpper().Equals("HH")) outputDt = inputDt.AddHours(increment);
                        else if (unit.ToUpper().Equals("MI")) outputDt = inputDt.AddMinutes(increment);
                        else if (unit.ToUpper().Equals("SS")) outputDt = inputDt.AddSeconds(increment);
                        else throw new Exception("Unexpected date add unit [" + unit + "]");
                        string outputDtStr = String.Format("{0:yyyy-MM-dd HH:mm:ss}", outputDt);
                        outputParsed.Write(mc, outputDtStr);
                    }
                    break;
                default:
                    //throw new Exception("Unexpected global date format [" + global_date_format + "]");
                    throw new Exception("Unexpected global date format [" + defaultDateFormat + "]");
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("date_add:");
        }
    }
}
