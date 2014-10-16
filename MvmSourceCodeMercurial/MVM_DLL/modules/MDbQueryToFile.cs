using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Diagnostics;

// Setup:
// -Download instant client from oracle 
// -Drop phil's binary into the instant client dir
// -create a tnsname.ora file
// -point TNS_ADMIN env var to directory where tnsnames.ora is located.
// -point the path into the instant client dir
// datacopy.exe -d oracle:jonah/jonah123@ORCL -q 'select 19 from dual' -D 'x.txt' -C 'x_ctrl.txt'

/*
<db_query_to_file>
<db>GLOBAL.db</db>
<user>GLOBAL.user</user>
<pw>GLOBAL.pw</pw> 
<query>select * from T_ACC_USAGE</query>    # input
<order_by>id_acc,id_type</order_by>
<file>TEMP.input_file_name</file>           # input
<ctrl>TEMP.output_ctrl_name</ctrl>          # input
</db_query_to_file>
 * 
*/


namespace MVM
{
    class MDbQueryToFile: IModuleSetup,IModuleRun
    {
        private string querySyntax;
        private string orderSyntax;
        private string fileSyntax;
        private string ctrlSyntax;
        private string fdelSyntax;
        private string rdelSyntax;
        private IReadString queryParsed;
        private IReadString orderParsed;
        private IReadString fileParsed;
        private IReadString ctrlParsed;
        private IReadString fdelParsed;
        private IReadString rdelParsed;
        private DbInfo dbInfo;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDbQueryToFile m = new MDbQueryToFile();
            m.dbInfo = new DbInfo(me, mc);
            m.querySyntax = me.SelectNodeInnerText("./query");
            m.orderSyntax = me.SelectNodeInnerText("./order_by");
            m.fileSyntax = me.SelectNodeInnerText("./file");
            m.ctrlSyntax = me.SelectNodeInnerText("./ctrl");
            m.fdelSyntax = me.SelectNodeInnerText("./field_delim","GLOBAL.field_delim");
            m.rdelSyntax = me.SelectNodeInnerText("./record_delim","GLOBAL.record_delim");
            m.queryParsed = mc.ParseSyntax(m.querySyntax);
            m.orderParsed = m.orderSyntax!=null?mc.ParseSyntax(m.orderSyntax):null;
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            m.ctrlParsed = mc.ParseSyntax(m.ctrlSyntax);
            m.fdelParsed = mc.ParseSyntax(m.fdelSyntax);
            m.rdelParsed = mc.ParseSyntax(m.rdelSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string type = this.dbInfo.GetType(mc);
            string server = this.dbInfo.GetServer(mc);
            string db = this.dbInfo.GetDb(mc);
            string user = this.dbInfo.GetUser(mc);
            string pw = this.dbInfo.GetPw(mc);
            string query = this.queryParsed.Read(mc);
            string order = this.orderParsed!=null?this.orderParsed.Read(mc):null;
            string file = this.fileParsed.Read(mc);
            string ctrl = this.ctrlParsed.Read(mc);
            string fdel = this.fdelParsed.Read(mc);
            string rdel = this.rdelParsed.Read(mc);
            if (fdel.Equals("")) fdel = "\t";
            if (fdel.Equals("")) fdel = "\r\n";
            DbUtils.QueryToFile(type,server,db,user,pw,query,order,file,ctrl,fdel,rdel);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("db_query_to_file:" + this.querySyntax);
        }
    }
}
