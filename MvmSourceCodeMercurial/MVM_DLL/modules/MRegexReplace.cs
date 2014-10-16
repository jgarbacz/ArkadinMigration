using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.IO;


using Antlr.Runtime.Tree;

namespace MVM
{
    /*
        <regex_replace>
        <input>'t_pt_blah'</input>
        <pattern>'(pt_)(*.)$</pattern>
        <replacement>$2<replacement>
        <max_count>1</max_count>
        <start_at>2</start_at>
        <output>TEMP.new_value</output>
        <regex_replace>
      */

    class MRegexReplace : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string patternSyntax;
        private string replacementSyntax;
        private string maxCountSyntax;
        private string startAtSyntax;
        private string outputSyntax;
        private string successSyntax;

        // from setup
        private IReadString inputParsed;
        private IReadString patternParsed;
        private IReadString replacementParsed;
        private IReadString maxCountParsed;
        private IReadString startAtParsed;
        private IWriteString outputParsed;
        private IWriteString successParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MRegexReplace m = new MRegexReplace();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.patternSyntax = me.SelectNodeInnerText("./pattern");
            m.replacementSyntax = me.SelectNodeInnerText("./replacement");
            m.maxCountSyntax = me.SelectNodeInnerText("./max_count");
            m.startAtSyntax = me.SelectNodeInnerText("./start_at");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.successSyntax = me.SelectNodeInnerText("./success");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.patternParsed = mc.ParseSyntax(m.patternSyntax);
            m.replacementParsed = mc.ParseSyntax(m.replacementSyntax);
            m.maxCountParsed = mc.ParseSyntax(m.maxCountSyntax);
            m.startAtParsed = mc.ParseSyntax(m.startAtSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            m.successParsed = mc.ParseWritableSyntax(m.successSyntax);
           // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string inputValue = this.inputParsed.Read(mc);
            string pattern = this.patternParsed.Read(mc);
            string replacement = this.replacementParsed.Read(mc);
            string maxCount = this.maxCountParsed.ReadOrNull(mc);
            string startAt = this.startAtParsed.ReadOrNull(mc);
            Regex regex = new Regex(pattern);
            if (startAt != null)
            {
                int iMaxCount = maxCount.ToInt();
                int iStartAt = startAt.ToInt();
                this.outputParsed.Write(mc,regex.Replace(inputValue,replacement,iMaxCount,iStartAt));
            }
            else if (maxCount != null)
            {
                int iMaxCount = maxCount.ToInt();
                this.outputParsed.Write(mc, regex.Replace(inputValue, replacement, iMaxCount));
            }
            else
            {
                this.outputParsed.Write(mc, regex.Replace(inputValue, replacement));
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("regex_replace:");
        }
    }
}
