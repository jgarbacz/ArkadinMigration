using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace MVM
{
    class MDbBulkInsertAbort : MDbCommon, IModuleSetup, IModuleRun
    {
        IBulkLoader bulkLoader;
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // if not name passed then abort all
            if (me.SelectNodeInnerText("./name") == null)
            {
                new MDbBulkInsertAbortAll().Setup(me, mc, run);
                return;
            }

            MDbBulkInsertAbort m = new MDbBulkInsertAbort();
            m.ParseDbCommon(me, mc, me.SelectNodeInnerText("./name"));
            string commitSizeSyntax = me.SelectNodeInnerText("commit_size", int.MaxValue.ToString());
            string commitSizeStr = mc.ParseSyntax(commitSizeSyntax).Read(mc).Nvl(int.MaxValue.ToString());
            int commitSize = int.Parse(commitSizeStr);
            m.bulkLoader = mc.globalContext.schemaMaster.GetBulkLoader(m.type, m.server, m.db, m.user, m.pw, m.name, commitSize);

            string beginMsg = "'call bulk_db_insert_abort " + m.name;
            string endMsg = "'done bulk_db_insert_abort " + m.name;
            run.Add(mc.GetLogModule(beginMsg, m.logLevel));
            run.Add(m);
            run.Add(mc.GetLogModule(endMsg, m.logLevel));
        }

        public void Run(ModuleContext mc)
        {
            try
            {
                int numRows = this.bulkLoader.Abort();
                this.WriteNumRows(mc, numRows);
            }
            catch (Exception e)
            {
                string msg = "db_bulk_insert_abort errored.";
                this.ProcessException(mc, msg, e);
            }
        }
    }

}
