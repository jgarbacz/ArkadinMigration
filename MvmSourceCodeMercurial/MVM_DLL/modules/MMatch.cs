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
        <match>
          <input>GLOBAL.input_file</input>
          <regex>’INTR(/d+)O_LBX_OUTBNR_(..)FSLBX.(\d+).txt’</regex>
          <success>OBJECT.is_boa</success>
          <capture>GLOBAL.lockbox_number</capture>
          <capture>OBJECT.lockbox_office</capture>
        </match>
      */

    class MMatch : IModuleSetup, IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string regexSyntax;
        private string successSyntax;
        private string caseSyntax;
        private List<string> captureSyntax;

        // from setup
        private IReadString inputParsed;
        private IReadString regexParsed;
        private IWriteString successParsed;
        private IReadString caseParsed;
        private List<IWriteString> captureParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MMatch m = new MMatch();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.regexSyntax = me.SelectNodeInnerText("./regex");
            m.caseSyntax = me.SelectNodeInnerText("./ignore_case", "''");
            m.successSyntax = me.SelectNodeInnerText("./success");
            m.captureSyntax = me.SelectNodesInnerText("./capture");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.regexParsed = mc.ParseSyntax(m.regexSyntax);
            m.caseParsed = mc.ParseSyntax(m.caseSyntax);
            m.successParsed = m.successSyntax.Equals("") ? null : mc.ParseWritableSyntax(m.successSyntax);
            m.captureParsed = mc.ParseWritableSyntax(m.captureSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string inputValue = this.inputParsed.Read(mc);
            string pattern = this.regexParsed.Read(mc);
            Regex regex = null;
            if (this.caseParsed.Read(mc).Equals("1"))
            {
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
            }
            else
            {
                regex = new Regex(pattern);
            }
            Match match = regex.Match(inputValue);
            if (match.Success)
            {
                this.successParsed.Write(mc, "1");
                int i = 1;
                foreach (IWriteString writable in this.captureParsed)
                {
                    writable.Write(mc,match.Groups[i++].Value);
                }
            }
            else
            {
                this.successParsed.Write(mc, "0");
            }
        }
        }
    }
