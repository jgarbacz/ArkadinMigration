using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

/*
  
<inherit_object>
<source>TEMP.csr</source>
<target>TEMP.oid</target> // default to OBJECT.object_id
</inherit_object>
 
 */
using System.Text.RegularExpressions;
namespace MVM
{
    class MInheritObject: IModuleSetup,IModuleRun
    {
        public string sourceSyntax;
        public string targetSyntax;
        public IReadString sourceParsed;
        public IReadString targetParsed;
        bool inheritHistory = false;

        List<string> excludePatternsSyntax;
       

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            // xml extraction
            MInheritObject m = new MInheritObject();
            m.sourceSyntax = me.SelectNodeInnerText("./source");
            m.targetSyntax = me.SelectNodeInnerText("./target","OBJECT.object_id");
            m.inheritHistory = mc.SyntaxReadString(me.SelectNodeInnerText("./include_history", "0")).Equals("1");
            m.sourceParsed = mc.ParseSyntax(m.sourceSyntax);
            m.targetParsed = mc.ParseSyntax(m.targetSyntax);
            m.excludePatternsSyntax = me.SelectNodesInnerText("./exclude_regex");
            if (m.excludePatternsSyntax.Count>0)
            {
                List<Regex> excludePatternsRegex = new List<Regex>();
                foreach (var s in m.excludePatternsSyntax)
                {
                    string pattern = mc.ParseSyntax(s).Read(mc);
                    Regex regex = new Regex(pattern);
                    excludePatternsRegex.Add(regex);
                }
                run.Add(new InheritWithExclusion(m.sourceParsed,m.targetParsed,excludePatternsRegex));
                return;
            }

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string sourceOid = sourceParsed.Read(mc);
            string targetOid = targetParsed.Read(mc);
            if (inheritHistory)
            {
                using(var tgt = mc.objectCache.CheckOut(targetOid)){
                    var tgtDelta = tgt as ObjectDataFormattedDelta;
                    tgtDelta.InheritAll(sourceOid);
                }
            }
            else
            {
                mc.InheritObject(sourceOid, targetOid);
            }
        }


        public class InheritWithExclusion : IModuleRun
        {
            public IReadString sourceParsed;
            public IReadString targetParsed;
            public List<Regex> excludePatternsRegex;
            public InheritWithExclusion(IReadString sourceParsed, IReadString targetParsed, List<Regex> excludePatternsRegex)
            {
                this.sourceParsed = sourceParsed;
                this.targetParsed = targetParsed;
                this.excludePatternsRegex=excludePatternsRegex;
            }
            public void Run(ModuleContext mc)
            {
                string sourceOid = sourceParsed.Read(mc);
                string targetOid = targetParsed.Read(mc);
                mc.InheritObject(sourceOid, targetOid,excludePatternsRegex);
            }
        }
    }
}
