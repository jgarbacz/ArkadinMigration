using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

/*
 * Works just like db_select but allows the tableName name to be dynamic.
 * Implemented this by having it gen a procContext for each unique tableName name. 
 * We store the proc_ids in a hash so we can reuse procs.
*/
namespace MVM
{
    class MDbSelectDynamic : IModuleSetup, IModuleRun
    {
        private Dictionary<string, int> subProcs = new Dictionary<string, int>();
        private XmlElement subProcElem;

        private string querySyntax;
        private IReadString queryParsed;

        private string sqlQuerySyntax;
        private IReadString sqlQueryParsed;

        private string oraQuerySyntax;
        private IReadString oraQueryParsed;


        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbSelectDynamic m = new MDbSelectDynamic();
            m.subProcElem = me.CreateElement("proc");
            m.subProcElem.SetAttribute("name", "generated");
           
            
            XmlElement dbSelect = me.CreateElement("db_select");
            foreach(XmlNode n in me.ChildNodes){
                dbSelect.AppendChild(n.CloneNode(true));
            }
            m.subProcElem.AppendChild(dbSelect);

            m.querySyntax = me.SelectNodeInnerText("query[not(type)]");
            m.oraQuerySyntax = me.SelectNodeInnerText("query[@type='oracle']");
            m.sqlQuerySyntax = me.SelectNodeInnerText("query[@type='sql']");
            m.queryParsed = mc.ParseSyntax(m.querySyntax);
            m.sqlQueryParsed = mc.ParseSyntax(m.sqlQuerySyntax);
            m.oraQueryParsed = mc.ParseSyntax(m.oraQuerySyntax);

            run.Add(new MScopeSnapshot());
            run.Add(m);
            run.Add(new MScopeUnsnapshot());
        }

        public void Run(ModuleContext mc)
        {
            string text =
                (this.queryParsed != null ? this.queryParsed.Read(mc) : "")
                + "|" + (this.oraQueryParsed != null ? this.oraQueryParsed.Read(mc) : "")
                + "|" + (this.sqlQueryParsed != null ? this.sqlQueryParsed.Read(mc) : "");
              
            int procId;
            lock (this.subProcs)
            {
                if (!subProcs.TryGetValue(text, out procId))
                {
                    string procName = mc.GetGenSym("MDbSelectDynamic");
                    mc.schedulerMaster.ReadXmlProcFromElem(mc.NameSpace, procName, this.subProcElem.CloneNode(true) as XmlElement, mc.worker, mc.structuralNameSpace, mc.configLocator);
                    procId = mc.GetProcId(procName);
                    //Console.WriteLine("generated: " + procName + "," + procId);
                    subProcs[text] = procId;
                }
            }
           // Console.WriteLine("calling subproc: "  + procId);
            mc.CallProcForCurrentObjectNested(procId);// added snap/unsnap in setup
        }
    }
}
