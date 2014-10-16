using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
namespace MVM
{
    class MiscMvmFunctions
    {

        /// <summary>
        /// Look up in ENUMS_BY_NAME index to see if the passed namespace and enum are valid and returns true or false.
        /// example: is_enum('my/countries','USA')
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_positional_arguments"></param>
        /// <returns></returns>
        [MvmExport("is_enum")]
        public static bool is_enum(ModuleContext _module_context, List<IReadable> _positional_arguments)
        {
            MemoryIndexSync index = (MemoryIndexSync)_module_context.globalContext.GetNamedClassInst("ENUMS_BY_NAME");
            string enumNamespace = _positional_arguments[0].ReadObject(_module_context).ToString();
            string findVal = _positional_arguments[1].ReadObject(_module_context).ToString();
            bool result = index.index.ContainsKey(new StringArray(enumNamespace, findVal));
            return result;
        }

        /// <summary>
        /// Tests to see if the first value is in the second which must evaluate to a comma separated list.
        ///  <test_output>'is true='~in(TEMP.one,TEMP.one_two)</test_output>
        /// <test_output>'is false='~in(TEMP.one,TEMP.three_four)</test_output>
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_positional_arguments"></param>
        /// <returns></returns>
        [MvmExport("in")]
        public static int dynamic_in(ModuleContext _module_context, List<IReadable> _positional_arguments)
        {

            string findVal = _positional_arguments[0].ReadObject(_module_context).ToString();
            Dictionary<string, bool> findVals = new Dictionary<string, bool>();
            for (int i = 1; i < _positional_arguments.Count; i++)
            {
                string inVal = _positional_arguments[i].ReadObject(_module_context).ToString();
                foreach (var item in inVal.Split(','))
                {
                    findVals[item] = true;
                }
            }

            if (findVals.ContainsKey(findVal))
                return 1;
            else
                return 0;
        }

        /// <summary>
        ///  Prints to the mvm log and returns true
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="text">Text to print</param>
        /// <returns></returns>
        [MvmExport("print")]
        public static bool print(ModuleContext _module_context, string text)
        {
            _module_context.mvm.Log(text);
            return true;
        }

        /// <summary>
        /// Cuts a section from the main logfile and puts it into a new file
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <param name="toFile"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("snip_log")]
        public static void SnipLog(ModuleContext mc, string startString, string endString, string toFile)
        {
            mc.mvm.FlushNLog();
            var logFile = mc.mvm.GetLogFileName();
            string[] text = File.ReadAllLines(logFile);

            using (StreamWriter writer = new StreamWriter(toFile))
            {
                bool on = false;
                foreach (var line in text)
                {

                    if (!on)
                    {
                        if (line.Contains(startString))
                        {
                            on = true;
                            writer.WriteLine(line);
                        }
                    }
                    else
                    {
                        writer.WriteLine(line);
                        if (line.Contains(endString))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
