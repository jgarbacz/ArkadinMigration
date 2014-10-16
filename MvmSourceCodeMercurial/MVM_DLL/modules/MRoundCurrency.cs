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
  <round>
    <input>TEMP.money</input>
    <precision>2</precision>
    <output>TEMP.out</output>
  </round>
      */

    class MRoundCurrency : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string currencySyntax;
        private string outputSyntax;

        // from setup
        private IReadString inputParsed;
        private IReadString currencyParsed;
        private IWriteString outputParsed;
        private IIndex my_index;
        private Dictionary<string, string> my_values = new Dictionary<string, string>();
        private List<string> my_list = new List<string>();

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRoundCurrency m = new MRoundCurrency();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.currencySyntax = me.SelectNodeInnerText("./currency");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.currencyParsed = mc.ParseSyntax(m.currencySyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);

            m.my_index = (IIndex)mc.globalContext.GetNamedClassInst("ALL_CURRENCIES");

            m.my_values["significant_digits"] = "";
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            decimal input,output;
            int precision;
            if (!decimal.TryParse(this.inputParsed.Read(mc), out input)) return;
            //IIndex my_index = (IIndex) mc.globalContext.GetNamedClassInst("ALL_CURRENCIES");
            //List<string> my_list = new List<string>();
            my_list.Clear();
            my_list.Add(this.currencyParsed.Read(mc));
            //Dictionary<string,string> my_values = new Dictionary<string,string>();
            //my_values["significant_digits"] = "";
            my_index.IndexGet(mc, my_list, my_values);
            output = input;
            if (!int.TryParse(my_values["significant_digits"], out precision)) return;
            output = Math.Round(input, precision);
            outputParsed.Write(mc, decimal.Parse(output.ToString()).ToString("G29"));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("round:");
        }
    }
}
