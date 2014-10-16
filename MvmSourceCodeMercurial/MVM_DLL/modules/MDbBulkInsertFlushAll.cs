using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace MVM
{
   

    // flush all bulk loaders
    class MDbBulkInsertFlushAll :MDbCommon, IModuleSetup,IModuleRun
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbBulkInsertFlushAll m = new MDbBulkInsertFlushAll();
            m.ParseDbCommon(me, mc, "'bulk_insert_flush_all'");
            string beginMsg = "'call bulk_db_insert_flush_all'";
            string endMsg = "'done bulk_db_insert_flush_all'";
            run.Add(mc.GetLogModule(beginMsg, m.logLevel));
            run.Add(m);
            run.Add(mc.GetLogModule(endMsg, m.logLevel));
        }
        public void Run(ModuleContext mc)
        {
            int numRows = 0;
            foreach (IBulkLoader bulkLoader in mc.globalContext.schemaMaster.GetAllBulkLoaders())
            {
                numRows += bulkLoader.Flush();
            }
            this.WriteNumRows(mc, numRows);
        }
    }
}
