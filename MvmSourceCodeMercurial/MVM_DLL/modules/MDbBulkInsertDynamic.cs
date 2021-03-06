﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

/*
 * Works just like db_insert but allows the tableName name to be dynamic.
 * Implemented this by having it gen a procContext for each unique tableName name. 
 * We store the proc_ids in a hash so we can reuse procs.
*/
namespace MVM
{
    class MDbBulkInsertDynamic : IModuleSetup, IModuleRun
    {
        private Dictionary<string, int> tableSubProcs = new Dictionary<string, int>();
        private XmlElement subProcElem;
        private string tableSyntax;
        private IReadString tableParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbBulkInsertDynamic m = new MDbBulkInsertDynamic();
            m.subProcElem = me.CreateElement("proc");
            m.subProcElem.SetAttribute("name", "generated");
            XmlElement dbInsert = me.CreateElement("db_bulk_insert");
            foreach(XmlNode n in me.ChildNodes){
                dbInsert.AppendChild(n.CloneNode(true));
            }
            m.subProcElem.AppendChild(dbInsert);
            m.tableSyntax = me.SelectNodeInnerText("./name");
            m.tableParsed = mc.ParseSyntax(m.tableSyntax);

            // since we know we are calling a proc with nested scope we need to snap/unsnap scope
            run.Add(new MScopeSnapshot());
            run.Add(m);
            run.Add(new MScopeUnsnapshot());
        }

        public void Run(ModuleContext mc)
        {
            string table = this.tableParsed.Read(mc);
            int procId;
            lock (this.tableSubProcs)
            {
                if (!tableSubProcs.TryGetValue(table, out procId))
                {
                    string procName = mc.GetGenSym("MDbBulkInsertDynamic");
                    mc.schedulerMaster.ReadXmlProcFromElem(mc.NameSpace, procName, this.subProcElem.CloneNode(true) as XmlElement, mc.worker, mc.structuralNameSpace, mc.configLocator);
                    procId = mc.GetProcId(procName);
                    tableSubProcs[table] = procId;
                }
            }
            mc.CallProcForCurrentObjectNested(procId); // added snap/unsnap in setup
        }
    }
}
