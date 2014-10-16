using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

/*
 * Works just like db_execute but allows the statement to be dynamic.
 * Implemented this by having it gen a procContext for each unique statementName name. 
 * We store the proc_ids in a hash so we can reuse procs.
*/
namespace MVM
{
    class MDbExecuteDynamic : IModuleSetup, IModuleRun
    {
        private Dictionary<string, int> statementSubProcs = new Dictionary<string, int>();
        private XmlElement subProcElem;
        private string statementSyntax;
        private IReadString statementParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbExecuteDynamic m = new MDbExecuteDynamic();
            m.subProcElem = me.CreateElement("proc");
            m.subProcElem.SetAttribute("name", "generated");
            XmlElement dbExecute = me.CreateElement("db_execute");
            foreach(XmlNode n in me.ChildNodes){
                dbExecute.AppendChild(n.CloneNode(true));
            }
            m.subProcElem.AppendChild(dbExecute);
            m.statementSyntax = me.SelectNodeInnerText("./statement");
            m.statementParsed = mc.ParseSyntax(m.statementSyntax);

            run.Add(new MScopeSnapshot());
            run.Add(m);
            run.Add(new MScopeUnsnapshot());
        }

        public void Run(ModuleContext mc)
        {
            string statement = this.statementParsed.Read(mc);
            int procId;
            lock (this.statementSubProcs)
            {
                if (!statementSubProcs.TryGetValue(statement, out procId))
                {
                    string procName = mc.GetGenSym("MDbExecuteDynamic");
                    mc.schedulerMaster.ReadXmlProcFromElem(mc.NameSpace, procName, this.subProcElem.CloneNode(true) as XmlElement, mc.worker, mc.structuralNameSpace, mc.configLocator);
                    procId = mc.GetProcId(procName);
                    statementSubProcs[statement] = procId;
                }
            }
            mc.CallProcForCurrentObjectNested(procId);// added snap/unsnap in setup
        }
    }
}
