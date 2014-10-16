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
  <split>
    <source>"a,b,c"</source>
    <delimiter>","</delimiter>
    <limit>2</limit> // defaults to number of targets, but may be fewer
    <target>TEMP.part1</target>
    <target>TEMP.part2</target>
    <target>TEMP.part3</target>
  </split>
     
     * TEMP.part1 -> 'a'
     * TEMP.part2 -> 'b,c'
     * TEMP.part3 -> ''
      */

    class MSplit : IModuleSetup, IModuleRun
    {
        // from xml
        private string sourceSyntax;
        private string delimiterSyntax;
        private string limitSyntax;
        private List<string> targetsSyntax;

        // from setup
        private IReadString sourceParsed;
        private IReadString delimiterParsed;
        private IReadString limitParsed;
        private List<IWriteString> targetsParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSplit m = new MSplit();
            m.sourceSyntax = me.SelectNodeInnerText("./source");
            m.delimiterSyntax = me.SelectNodeInnerText("./delimiter");
            m.targetsSyntax = me.SelectNodesInnerText("./target");
            int numTargets = m.targetsSyntax.Count;
            m.limitSyntax = me.SelectNodeInnerText("./limit", numTargets.ToString());
            m.sourceParsed = mc.ParseSyntax(m.sourceSyntax);
            m.delimiterParsed = mc.ParseSyntax(m.delimiterSyntax);
            m.limitParsed = mc.ParseSyntax(m.limitSyntax);
            m.targetsParsed=mc.ParseWritableSyntax(m.targetsSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string source = this.sourceParsed.Read(mc);
            int limit = this.limitParsed.Read(mc).ToInt();
            string delimiter=this.delimiterParsed.Read(mc);
            string[] delimiters=new string[]{delimiter};
            string[] parts = source.Split(delimiters, limit,StringSplitOptions.None);
            for (int i = 0; i < limit && i < parts.Length; i++)
            {
                this.targetsParsed[i].Write(mc, parts[i]);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("split:");
        }
    }
}
