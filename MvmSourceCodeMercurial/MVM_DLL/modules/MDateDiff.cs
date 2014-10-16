using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /* NOTE: Returns diff in seconds! And if no format is specified, the DB type will be used to determine the date format.
  <date_diff>
    <first_date format='YYYY-MM-DD HH24:MI:SS'>TEMP.in</first_date>
    <second_date format='YYYY-MM-DD HH24:MI:SS'>TEMP.in</second_date>
    <output>TEMP.out</output>
  </date_diff>
      */

    class MDateDiff : IModuleSetup, IModuleRun
    {
        // from xml
        private string first_dateSyntax;
        private string second_dateSyntax;
        private string outputSyntax;

        // from setup
        private IReadString first_dateParsed;
        private IReadString second_dateParsed;
        private IWriteString outputParsed;

        // assumed to be static
        private string firstFormat;
        private string secondFormat;
        private string outputTypeStr;

        private int firstConvertType;
        private int secondConvertType;
        private int outputType;

        private int firstFormatLength;
        private int secondFormatLength;

        private string dbType;
        private string defaultDateFormat;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDateDiff m = new MDateDiff();


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

            if (dbType.Equals("sql")) {
                defaultDateFormat="YYYY-MM-DD HH24:MI:SS.mmm";
            } else {
                defaultDateFormat="YYYYMMDDHH24MISS";
            }

            XmlElement firstElem = me.SelectSingleElem("./first_date");
            XmlElement secondElem = me.SelectSingleElem("./second_date");
            XmlElement outputElem = me.SelectSingleElem("./output");
            // xml extraction
            m.first_dateSyntax = firstElem.InnerText;
            m.second_dateSyntax = secondElem.InnerText;
            m.outputSyntax = outputElem.InnerText;
            // parsing
            m.first_dateParsed = mc.ParseSyntax(m.first_dateSyntax);
            m.second_dateParsed = mc.ParseSyntax(m.second_dateSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);

            m.firstFormat = firstElem.GetAttributeDefault("format", defaultDateFormat);
            m.secondFormat = secondElem.GetAttributeDefault("format", defaultDateFormat);
            m.outputTypeStr = outputElem.GetAttributeDefault("type", "seconds");

            if (m.firstFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm"))
            {
                m.firstConvertType = 1;
                m.firstFormatLength = 23;
            }
            else if (m.firstFormat.Equals("YYYYMMDDHH24MISS"))
            {
                m.firstConvertType = 2;
                m.firstFormatLength = 14;
            }
            else if (m.firstFormat.Equals("MM/DD/YYYY"))
            {
                m.firstConvertType = 3;
                m.firstFormatLength = 10;
            }
            else if (m.firstFormat.Equals("YYYY-MM-DD HH24:MI:SS"))
            {
                m.firstConvertType = 4;
                m.firstFormatLength = 19;
            }
            else throw new Exception("date_diff first format=" + m.firstFormat + " is not supported");

            if (m.secondFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm"))
            {
                m.secondConvertType = 1;
                m.secondFormatLength = 23;
            }
            else if (m.secondFormat.Equals("YYYYMMDDHH24MISS"))
            {
                m.secondConvertType = 2;
                m.secondFormatLength = 14;
            }
            else if (m.secondFormat.Equals("MM/DD/YYYY"))
            {
                m.secondConvertType = 3;
                m.secondFormatLength = 10;
            }
            else if (m.secondFormat.Equals("YYYY-MM-DD HH24:MI:SS"))
            {
                m.secondConvertType = 4;
                m.secondFormatLength = 19;
            }
            else throw new Exception("date_diff second format=" + m.secondFormat + " is not supported");

            if (m.outputTypeStr.Equals("seconds")) m.outputType = 1;
            else if (m.outputTypeStr.Equals("minutes")) m.outputType = 2;
            else if (m.outputTypeStr.Equals("hours")) m.outputType = 3;
            else if (m.outputTypeStr.Equals("days")) m.outputType = 4;
            else if (m.outputTypeStr.Equals("weeks")) m.outputType = 5;
            else if (m.outputTypeStr.Equals("months")) m.outputType = 6;
            else if (m.outputTypeStr.Equals("years")) m.outputType = 7;
            else throw new Exception("date_diff output type [" + m.secondFormat + "] is not supported");

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string first_date = this.first_dateParsed.Read(mc);
            string second_date = this.second_dateParsed.Read(mc);

            if (first_date.Equals("") || second_date.Equals(""))
            {
                outputParsed.Write(mc, "");
                return;
            }

            if ((first_date.Length != firstFormatLength && firstConvertType != 1) || (firstConvertType == 1 && (first_date.Length != 19 && first_date.Length != firstFormatLength)))
            {
                throw new Exception("Passed first date [" + first_date + "] does not match format ["+ firstFormat +"]");
                //outputParsed.Write(mc, "");
                //return;
            }

            if ((second_date.Length != secondFormatLength && secondConvertType != 1) || (secondConvertType == 1 && (second_date.Length != 19 && second_date.Length != secondFormatLength)))
            {
                throw new Exception("Passed second date [" + second_date + "] does not match format [" + secondFormat + "]");
                //outputParsed.Write(mc, "");
                //return;
            }



            int firstYear;
            int firstMonth;
            int firstDay;
            int firstHour;
            int firstMinute;
            int firstSecond;
            int firstMillisecond;
            int secondYear;
            int secondMonth;
            int secondDay;
            int secondHour;
            int secondMinute;
            int secondSecond;
            int secondMillisecond;
            firstMillisecond = 0;
            secondMillisecond = 0;
            switch (this.firstConvertType)
            {
                // if (m.firstFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.firstConvertType = 1;
                case 1:
                    firstYear = int.Parse(first_date.Substring(0,4));
                    firstMonth = int.Parse(first_date.Substring(5,2));
                    firstDay = int.Parse(first_date.Substring(8,2));
                    firstHour = int.Parse(first_date.Substring(11,2));
                    firstMinute = int.Parse(first_date.Substring(14,2));
                    firstSecond = int.Parse(first_date.Substring(17,2));
                    if (first_date.Length > 19)
                    {
                        firstMillisecond = int.Parse(first_date.Substring(20, 3));
                    }
                    break;
                // else if (m.firstFormat.Equals("YYYYMMDDHH24MISS")) m.firstConvertType = 2;
                case 2:
                    firstYear = int.Parse(first_date.Substring(0, 4));
                    firstMonth = int.Parse(first_date.Substring(4, 2));
                    firstDay = int.Parse(first_date.Substring(6, 2));
                    firstHour = int.Parse(first_date.Substring(8, 2));
                    firstMinute = int.Parse(first_date.Substring(10, 2));
                    firstSecond = int.Parse(first_date.Substring(12, 2));
                    break;
                // else if (m.firstFormat.Equals("MM/DD/YYYY")) m.firstConvertType = 3;
                case 3:
                    firstYear = int.Parse(first_date.Substring(6, 4));
                    firstMonth = int.Parse(first_date.Substring(0, 2));
                    firstDay = int.Parse(first_date.Substring(3, 2));
                    firstHour = 0;
                    firstMinute = 0;
                    firstSecond = 0;
                    break;
                // if (m.firstFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.firstConvertType = 4;
                case 4:
                    firstYear = int.Parse(first_date.Substring(0, 4));
                    firstMonth = int.Parse(first_date.Substring(5, 2));
                    firstDay = int.Parse(first_date.Substring(8, 2));
                    firstHour = int.Parse(first_date.Substring(11, 2));
                    firstMinute = int.Parse(first_date.Substring(14, 2));
                    firstSecond = int.Parse(first_date.Substring(17, 2));
                    break;
                default:
                    throw new Exception("unexpected first date format=" + this.firstConvertType);
            }

            switch (this.secondConvertType)
            {
                // if (m.secondFormat.Equals("YYYY-MM-DD HH24:MI:SS.mmm")) m.secondConvertType = 1;
                case 1:
                    secondYear = int.Parse(second_date.Substring(0, 4));
                    secondMonth = int.Parse(second_date.Substring(5, 2));
                    secondDay = int.Parse(second_date.Substring(8, 2));
                    secondHour = int.Parse(second_date.Substring(11, 2));
                    secondMinute = int.Parse(second_date.Substring(14, 2));
                    secondSecond = int.Parse(second_date.Substring(17, 2));
                    if (second_date.Length > 19)
                    {
                        secondMillisecond = int.Parse(second_date.Substring(20, 3));
                    }
                    break;
                // else if (m.secondFormat.Equals("YYYYMMDDHH24MISS")) m.secondConvertType = 2;
                case 2:
                    secondYear = int.Parse(second_date.Substring(0, 4));
                    secondMonth = int.Parse(second_date.Substring(4, 2));
                    secondDay = int.Parse(second_date.Substring(6, 2));
                    secondHour = int.Parse(second_date.Substring(8, 2));
                    secondMinute = int.Parse(second_date.Substring(10, 2));
                    secondSecond = int.Parse(second_date.Substring(12, 2));
                    break;
                // else if (m.secondFormat.Equals("MM/DD/YYYY")) m.secondConvertType = 3;
                case 3:
                    secondYear = int.Parse(second_date.Substring(6, 4));
                    secondMonth = int.Parse(second_date.Substring(0, 2));
                    secondDay = int.Parse(second_date.Substring(3, 2));
                    secondHour = 0;
                    secondMinute = 0;
                    secondSecond = 0;
                    break;
                // if (m.secondFormat.Equals("YYYY-MM-DD HH24:MI:SS")) m.secondConvertType = 4;
                case 4:
                    secondYear = int.Parse(second_date.Substring(0, 4));
                    secondMonth = int.Parse(second_date.Substring(5, 2));
                    secondDay = int.Parse(second_date.Substring(8, 2));
                    secondHour = int.Parse(second_date.Substring(11, 2));
                    secondMinute = int.Parse(second_date.Substring(14, 2));
                    secondSecond = int.Parse(second_date.Substring(17, 2));
                    break;
                default:
                    throw new Exception("unexpected second date format=" + this.secondConvertType);
            }
            System.DateTime firstDt;
            System.DateTime secondDt;
            System.Int64 diff_in_seconds;


            switch (this.outputType)
            {
                // if (m.outputTypeStr.Equals("seconds")) m.outputType = 1;
                case 1:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    diff_in_seconds = (firstDt.Ticks - secondDt.Ticks) / 10000000;
                    outputParsed.Write(mc, diff_in_seconds.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("minutes")) m.outputType = 2;
                case 2:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    diff_in_seconds = (firstDt.Ticks - secondDt.Ticks) / 10000000;
                    System.Double diff_in_minutes = diff_in_seconds / 60D;
                    outputParsed.Write(mc, diff_in_minutes.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("hours")) m.outputType = 3;
                case 3:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    diff_in_seconds = (firstDt.Ticks - secondDt.Ticks) / 10000000;
                    System.Double diff_in_hours = diff_in_seconds /60D /60;
                    outputParsed.Write(mc, diff_in_hours.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("days")) m.outputType = 4;
                case 4:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    diff_in_seconds = (firstDt.Ticks - secondDt.Ticks) / 10000000;
                    System.Double diff_in_days = diff_in_seconds / 60D / 60 / 24;
                    outputParsed.Write(mc, diff_in_days.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("weeks")) m.outputType = 5;
                case 5:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    diff_in_seconds = (firstDt.Ticks - secondDt.Ticks) / 10000000;
                    System.Double diff_in_weeks = diff_in_seconds / 60D / 60 / 24 / 7;
                    outputParsed.Write(mc, diff_in_weeks.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("months")) m.outputType = 6;
                case 6:
                    firstDt = new System.DateTime(firstYear, firstMonth, firstDay, firstHour, firstMinute, firstSecond, firstMillisecond);
                    secondDt = new System.DateTime(secondYear, secondMonth, secondDay, secondHour, secondMinute, secondSecond, secondMillisecond);
                    int daysInMonth;
                    int relMonth;
                    int relYear;
                    int baseMonth;
                    int baseYear;
                    int addMonth;
                    Double partialMonths;
                    Double tempFirstDays = firstDay + firstHour / 24D + firstMinute / 24 / 60 + firstSecond / 24 / 60 / 60;
                    Double tempSecondDays = secondDay + secondHour / 24D + secondMinute / 24 / 60 + secondSecond / 24 / 60 / 60;

                    if (firstDt.Ticks > secondDt.Ticks)
                    {
                        if (tempFirstDays > tempSecondDays || firstMonth == secondMonth)
                        {
                            relMonth = firstMonth;
                            relYear = firstYear;
                            baseYear = secondYear;
                            baseMonth = secondMonth;
                            addMonth = 0;

                        }
                        else
                        {
                            relMonth = firstMonth - 1;
                            relYear = firstYear;
                            baseYear = secondYear;
                            baseMonth = secondMonth;
                            addMonth = 1;
                        }
                    }
                    else if (tempSecondDays > tempFirstDays || firstMonth == secondMonth)
                    {
                        relMonth = secondMonth;
                        relYear = secondYear;
                        baseYear = firstYear;
                        baseMonth = firstMonth;
                        addMonth = 0;
                    }
                    else
                    {
                        relMonth = secondMonth - 1;
                        relYear = secondYear;
                        baseYear = firstYear;
                        baseMonth = firstMonth;
                        addMonth = 1;
                    }
                    if (relMonth == 0)
                    {
                        relMonth = 12;
                        relYear -= 1;
                    }
                    if (relMonth == 11 || relMonth == 4 || relMonth == 6 || relMonth == 9) daysInMonth = 30;
                    else if (relMonth == 2 && firstYear % 4 == 0 && (firstYear % 100 != 0 || firstYear % 400 == 0)) daysInMonth = 29;
                    else if (relMonth == 2) daysInMonth = 28;
                    else daysInMonth = 31;

                    if (firstDt.Ticks < secondDt.Ticks)
                    {
                        partialMonths = 0D - ((relYear - baseYear) * 12 + (relMonth - baseMonth) + ((addMonth * daysInMonth) + secondDay - firstDay + ((secondHour + secondMinute / 60D + secondSecond / 3600D) - (firstHour + firstMinute / 60D + firstSecond / 3600D)) / 24D) / daysInMonth);
                    }
                    else
                    {
                        partialMonths = (relYear - baseYear) * 12 + (relMonth - baseMonth) + ((addMonth * daysInMonth) + firstDay - secondDay + ((firstHour + firstMinute / 60D + firstSecond / 3600D) - (secondHour + secondMinute / 60D + secondSecond / 3600D)) / 24D) / daysInMonth;
                    }
                    outputParsed.Write(mc, partialMonths.ToString());
                    break;
                // else if (m.outputTypeStr.Equals("years")) m.outputType = 7;
                case 7:
                    Double partialYears;
                    Double firstDays;
                    Double secondDays;

                    int firstLeapYear = 0;
                    if (firstYear % 4 == 0 && (firstYear % 100 != 0 || firstYear % 400 == 0)) firstLeapYear = 1;

                    int secondLeapYear = 0;
                    if (secondYear % 4 == 0 && (secondYear % 100 != 0 || secondYear % 400 == 0)) secondLeapYear = 1;

                    switch (firstMonth)
                    {
                        case 1:
                            firstDays = 0;
                            break;
                        case 2:
                            firstDays = 31;
                            break;
                        case 3:
                            firstDays = 59 + firstLeapYear;
                            break;
                        case 4:
                            firstDays = 90 + firstLeapYear;
                            break;
                        case 5:
                            firstDays = 120 + firstLeapYear;
                            break;
                        case 6:
                            firstDays = 151 + firstLeapYear;
                            break;
                        case 7:
                            firstDays = 181 + firstLeapYear;
                            break;
                        case 8:
                            firstDays = 212 + firstLeapYear;
                            break;
                        case 9:
                            firstDays = 243 + firstLeapYear;
                            break;
                        case 10:
                            firstDays = 273 + firstLeapYear;
                            break;
                        case 11:
                            firstDays = 304 + firstLeapYear;
                            break;
                        case 12:
                            firstDays = 334 + firstLeapYear;
                            break;
                        default:
                            throw new Exception("invalid first month =" + firstMonth);
                    }

                    firstDays += firstDay + firstHour / 24D + firstMinute / 24D / 60 + firstSecond / 24D / 60 / 60;

                    switch (secondMonth)
                    {
                        case 1:
                            secondDays = 0;
                            break;
                        case 2:
                            secondDays = 31;
                            break;
                        case 3:
                            secondDays = 59 + secondLeapYear;
                            break;
                        case 4:
                            secondDays = 90 + secondLeapYear;
                            break;
                        case 5:
                            secondDays = 120 + secondLeapYear;
                            break;
                        case 6:
                            secondDays = 151 + secondLeapYear;
                            break;
                        case 7:
                            secondDays = 181 + secondLeapYear;
                            break;
                        case 8:
                            secondDays = 212 + secondLeapYear;
                            break;
                        case 9:
                            secondDays = 243 + secondLeapYear;
                            break;
                        case 10:
                            secondDays = 273 + secondLeapYear;
                            break;
                        case 11:
                            secondDays = 304 + secondLeapYear;
                            break;
                        case 12:
                            secondDays = 334 + secondLeapYear;
                            break;
                        default:
                            throw new Exception("invalid second month =" + secondMonth);
                    }

                    secondDays += secondDay + secondHour / 24D + secondMinute / 24D / 60 + secondSecond / 24D / 60 / 60;

                    if (firstYear > secondYear || (firstYear == secondYear && firstDays > secondDays))
                    {
                        partialYears = firstYear - secondYear + firstDays / (365D + firstLeapYear) - secondDays / (365D + secondLeapYear);
                    }
                    else
                    {
                        partialYears = 0 - (secondYear - firstYear + secondDays / (365D + secondLeapYear) - firstDays / (365D + firstLeapYear));
                    }
                    outputParsed.Write(mc, partialYears.ToString());
                    break;
                default:
                    throw new Exception("unexpected output type=" + this.outputType);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("date_diff:");
        }
    }
}
