using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

//using MetraTech.Foundation.Security;
using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <get_mt_db_info>
    <server_type>'Netmeter'</server_type>
    <database_type>GLOBAL.database_type</database_type>
    <database_server>GLOBAL.database_server</database_server>
    <database_name>GLOBAL.database_name</database_name>
    <database_user>GLOBAL.database_user</database_user>
    <database_password>GLOBAL.database_password</database_password>
  </get_mt_db_info>
      */

    class MGetMtDbInfo : IModuleSetup, IModuleRun
    {
        private string serverTypeSyntax;
        private IReadString serverTypeParsed;
        private string databaseTypeSyntax;
        private IWriteString databaseTypeParsed;
        private string databaseServerSyntax;
        private IWriteString databaseServerParsed;
        private string databaseNameSyntax;
        private IWriteString databaseNameParsed;
        private string databaseUserSyntax;
        private IWriteString databaseUserParsed;
        private string databasePasswordSyntax;
        private IWriteString databasePasswordParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetMtDbInfo m = new MGetMtDbInfo();
            m.serverTypeSyntax = me.SelectNodeInnerText("./server_type","'Netmeter'");
            m.serverTypeParsed = mc.ParseSyntax(m.serverTypeSyntax);
            m.databaseTypeSyntax = me.SelectNodeInnerText("./database_type");
            m.databaseTypeParsed = mc.ParseWritableSyntax(m.databaseTypeSyntax);
            m.databaseServerSyntax = me.SelectNodeInnerText("./database_server");
            m.databaseServerParsed = mc.ParseWritableSyntax(m.databaseServerSyntax);
            m.databaseNameSyntax = me.SelectNodeInnerText("./database_name");
            m.databaseNameParsed = mc.ParseWritableSyntax(m.databaseNameSyntax);
            m.databaseUserSyntax = me.SelectNodeInnerText("./database_user");
            m.databaseUserParsed = mc.ParseWritableSyntax(m.databaseUserSyntax);
            m.databasePasswordSyntax = me.SelectNodeInnerText("./database_password");
            m.databasePasswordParsed = mc.ParseWritableSyntax(m.databasePasswordSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string serverType = this.serverTypeParsed.Read(mc);
            string databaseType, databaseServer, databaseName, databaseUser, databasePassword;
            
            //MtReflection.GetMtDbInfo("Netmeter", mc.mvm.rmpBinDir, out databaseType, out databaseServer, out databaseName, out databaseUser, out databasePassword);

            // Getting compile errors on this; just using same code used in current MVM. -- TCF
            /*string rmpDir = mc.mvm.rmpDir;
            var config = ServerConfigManager.GetServerConfig(rmpDir, "NetMeter");
            databaseType = config.DatabaseType;
            databaseServer = config.ServerName;
            databaseName = config.DatabaseName;
            databaseUser = config.Username;
            databasePassword = config.Password;*/
            string rmpBin = null;

            MtReflection.GetMtDbInfo("Netmeter", rmpBin, out databaseType, out databaseServer, out databaseName, out databaseUser, out databasePassword);
            if (databaseType.ToLower().Contains("sql"))
            {
                databaseType = "sql";
            }
            else
            {
                databaseType = "oracle";
                databaseName = databaseServer;
            }

            this.databaseTypeParsed.Write(mc,databaseType);
            this.databaseServerParsed.Write(mc, databaseServer);
            this.databaseNameParsed.Write(mc,databaseName);
            this.databaseUserParsed.Write(mc,databaseUser);
            this.databasePasswordParsed.Write(mc,databasePassword);
        }
    }
}
