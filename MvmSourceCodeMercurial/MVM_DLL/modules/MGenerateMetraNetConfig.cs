using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*[Module(@"
        <module_config>
            <name>generate_metranet_procs</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='xs:string'/>
            </xsd>
            <doc>
                <category>MetraNet</category>
                <description>Generates and calls table printing for t_av_* tables</description>
            </doc>
        </module_config>
    ")]*/
    class MGenerateMetraNetConfig : IModuleSetup
    {

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // Determine where to generate the files
            string genDir = mc.SyntaxReadString(me.SelectNodeInnerText("./generate_dir", "GLOBAL.generate_dir"));

            // Get access to RMP
            string rmpDir = mc.SyntaxReadString(me.SelectNodeInnerText("./rmp_dir", @"(GLOBAL.rmp_dir ne ''?GLOBAL.rmp_dir:'D:\MetraTech\RMP')"));
            if (rmpDir.Equals("")) throw new Exception("Cannot do generate_metranet_procs for null RMP dir=[" + rmpDir + "]");
            RmpSearcher rmp = new RmpSearcher(rmpDir);

            // Cache all the enums
            DbInfo dbInfo = new DbInfo(me, mc);
            Dictionary<string, Dictionary<string, int>> enumDictionary = GetEnumDictionary(mc, dbInfo);

            // generate the caches we use.
            // enums, account types, param tables, etc.
            {
                string procName = "mt_indexes";
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">".AppendLine());
                proc.Append(
@"
<startup>
  <info>'Build ENUM_MAP'</info>
  <db_select>
"+dbInfo.BackToXmlString()+@"
    <query type='oracle'>
      ""select nm_enum_data, ""~
      ""lower(substr(nm_enum_data,0,instr(nm_enum_data,'/',-1,1)-1)) enum_name, ""~
      ""substr(nm_enum_data,instr(nm_enum_data,'/',-1,1)+1) enum_value, ""~
      ""id_enum_data ""~
      ""from t_enum_data""
    </query>
    <query type='sql'>
      ""SELECT nm_enum_data, ""~
      ""lower(SUBSTRING(nm_enum_data,0,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+1)) enum_name, ""~
      ""SUBSTRING(nm_enum_data,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+2,LEN(nm_enum_data)) enum_value, ""~
      ""id_enum_data ""~
      ""FROM t_enum_data""
    </query>
    <cursor>TEMP.csr</cursor>
    <run>
      <create_memory_index_from_cursor>
        <index>'ENUM_MAP'</index>
        <cursor>TEMP.csr</cursor>
        <key_field>'enum_name'</key_field>
        <key_field>'enum_value'</key_field>
      </create_memory_index_from_cursor>
    </run>
  </db_select>

  <info>'Build ENUM_NAME_OR_ID_TO_ID_MAP'</info>
  <db_select>
" + dbInfo.BackToXmlString() + @"
    <query type='oracle'>
        ""select nm_enum_data,""~
        ""nvl(lower(substr(nm_enum_data,0,instr(nm_enum_data,'/',-1,1)-1)),'junk') enum_name, ""~
        ""to_char(substr(nm_enum_data,instr(nm_enum_data,'/',-1,1)+1)) enum_value, ""~
        ""id_enum_data ""~
        ""from t_enum_data ""~
        ""union all ""~
        ""select nm_enum_data,""~
        ""lower(substr(nm_enum_data,0,instr(nm_enum_data,'/',-1,1)-1)) enum_name, ""~
        ""to_char(id_enum_data) enum_value, ""~
        ""id_enum_data ""~
        ""from t_enum_data ""
    </query>
    <query type='sql'>
      ""SELECT nm_enum_data, ""~
      ""lower(SUBSTRING(nm_enum_data,0,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+1)) enum_name, ""~
      ""SUBSTRING(nm_enum_data,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+2,LEN(nm_enum_data)) enum_value, ""~
      ""id_enum_data ""~
      ""FROM t_enum_data ""~
      ""union all ""~
      ""SELECT nm_enum_data, ""~
      ""lower(SUBSTRING(nm_enum_data,0,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+1)) enum_name, ""~
      ""CAST(id_enum_data as nvarchar) enum_value, ""~
      ""id_enum_data ""~
      ""FROM t_enum_data""
    </query>
    <cursor>TEMP.csr</cursor>
    <run>
      <create_memory_index_from_cursor>
        <index>'ENUM_NAME_OR_ID_TO_ID_MAP'</index>
        <cursor>TEMP.csr</cursor>
        <key_field>'enum_name'</key_field>
        <key_field>'enum_value'</key_field>
      </create_memory_index_from_cursor>
    </run>
  </db_select>

  <info>'Build ID_PARAMTABLE_TO_PARAM_TABLE'</info>
  <db_select>
" + dbInfo.BackToXmlString() + @"
    <query>'select id_paramtable, lower(nm_instance_tablename) param_table from t_rulesetdefinition'</query>
    <cursor>TEMP.csr</cursor>
    <run>
      <create_memory_index_from_cursor>
        <index>'ID_PARAMTABLE_TO_PARAM_TABLE'</index>
        <cursor>TEMP.csr</cursor>
        <key_field>'id_paramtable'</key_field>
      </create_memory_index_from_cursor>
    </run>
  </db_select>

 <info>'Build ID_TYPE_TO_ACCOUNT_TYPE'</info>
  <db_select>
" + dbInfo.BackToXmlString() + @"
    <query>'select id_type, lower(name) account_type from t_account_type'</query>
    <cursor>TEMP.csr</cursor>
    <run>
      <create_memory_index_from_cursor>
        <index>'ID_TYPE_TO_ACCOUNT_TYPE'</index>
        <cursor>TEMP.csr</cursor>
        <key_field>'id_type'</key_field>
      </create_memory_index_from_cursor>
    </run>
  </db_select>

</startup>
</proc>
");
                WriteOutProc(genDir, procName, proc.ToString());
            }


            // generate av procs
            {
                string printingProcName = "mt_print_t_av";
                string insertingProcName = "mt_insert_t_av";
                string enumMappingProcName = "mt_convert_enums_t_av";
                string tryEnumMappingProcName = "mt_try_convert_enums_t_av";
                string defaultsProcName = "mt_default_t_av";
                StringBuilder printingProc = new StringBuilder("<proc name=" + printingProcName.q() + ">".AppendLine());
                StringBuilder insertingProc = new StringBuilder("<proc name=" + insertingProcName.q() + ">".AppendLine());
                StringBuilder enumMappingProc = new StringBuilder("<proc name=" + enumMappingProcName.q() + ">".AppendLine());
                StringBuilder tryEnumMappingProc = new StringBuilder("<proc name=" + tryEnumMappingProcName.q() + ">".AppendLine());
                StringBuilder defaultsProc = new StringBuilder("<proc name=" + defaultsProcName.q() + ">".AppendLine());
                MultiStringBuilder allProcs = new MultiStringBuilder(printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);

                allProcs.AppendLine("<if>");
                allProcs.AppendLine("<condition>OBJECT.account_type eq '' and OBJECT.id_type ne ''</condition>");
                allProcs.AppendLine("<then>");
                allProcs.AppendLine("<index_select>");
                allProcs.AppendLine("<index>'ID_TYPE_TO_ACCOUNT_TYPE'</index>");
                allProcs.AppendLine("<field name='id_type'>OBJECT.id_type</field>");
                allProcs.AppendLine("<cursor>TEMP.csr</cursor>");
                allProcs.AppendLine("<then>");
                allProcs.AppendLine("<do>OBJECT.account_type=OBJECT(TEMP.csr).account_type</do>");
                allProcs.AppendLine("</then>");
                allProcs.AppendLine("</index_select>");
                allProcs.AppendLine("</then>");
                allProcs.AppendLine("</if>");

                allProcs.AppendLine("<if>");
                foreach (var accountType in rmp.AccountTypes)
                {
                    allProcs.AppendLine("<condition>OBJECT.account_type eq " + accountType.q() + "</condition>");
                    allProcs.AppendLine("<then>");
                    foreach (var av in rmp.GetAccountTypeAccountViews(accountType))
                    {
                        string viewName = av.viewName.ToLower();
                        string tableName = "t_av_" + viewName;
                        string serviceDefName = av.serviceDef;
                        ServiceDef serviceDef = rmp.GetAvServiceDefObj(serviceDefName);
                        TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, tableName);
                        FieldDef partOfKeyFieldDef = serviceDef.GetEnumPartOfKeyFieldDef();
                        if (partOfKeyFieldDef != null)
                        {
                            string enumSpaceType = (partOfKeyFieldDef.enumSpace + "/" + partOfKeyFieldDef.enumType).ToLower();
                            string partOfKeyEnumName = partOfKeyFieldDef.name;
                            if (!enumDictionary.ContainsKey(enumSpaceType))
                            {
                                throw new Exception("Error, generating for account_view=[" + av.viewName + "] expecting enum like '" + enumSpaceType + "%' to be in t_enum_data");
                            }
                            foreach (string partOfKeyEnumValue in enumDictionary[enumSpaceType].Keys)
                            {
                                string prefix = MakeValidFieldName(partOfKeyEnumValue + "_");
                                string enumFieldName = (prefix + partOfKeyEnumName).ToLower();
                                string enumObjectField = "OBJECT." + enumFieldName;
                                string enumObjectFieldNmEnumData = "OBJECT." + enumFieldName + "_nmenumdata";
                                allProcs.AppendLine("<if>");
                                allProcs.AppendLine("<condition>" + enumObjectField + " eq '" + partOfKeyEnumValue + "' or " + enumObjectFieldNmEnumData + " eq '" + partOfKeyEnumValue + "'</condition>");
                                allProcs.AppendLine("<then>");
                                GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                                allProcs.AppendLine("</then>");
                                allProcs.AppendLine("</if>");
                            }
                        }
                        else
                        {
                            string prefix = "";
                            GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                        }
                    }
                    allProcs.AppendLine("</then>");
                }
                allProcs.AppendLine("<else>");
                allProcs.AppendLine("<fatal>'Unknown account type ['~OBJECT.account_type~']'</fatal>");
                allProcs.AppendLine("</else>");
                allProcs.AppendLine("</if>");
                allProcs.AppendLine("</proc>");
                WriteOutProcs(genDir, printingProcName, printingProc.ToString(), insertingProcName, insertingProc.ToString(), enumMappingProcName, enumMappingProc.ToString(), tryEnumMappingProcName, tryEnumMappingProc.ToString(), defaultsProcName, defaultsProc.ToString());
            }

            // generate pt procs
            {
                string printingProcName = "mt_print_t_pt";
                string insertingProcName = "mt_insert_t_pt";
                string enumMappingProcName = "mt_convert_enums_t_pt";
                string tryEnumMappingProcName = "mt_try_convert_enums_t_pt";
                string defaultsProcName = "mt_default_t_pt";
                StringBuilder printingProc = new StringBuilder("<proc name=" + printingProcName.q() + ">".AppendLine());
                StringBuilder insertingProc = new StringBuilder("<proc name=" + insertingProcName.q() + ">".AppendLine());
                StringBuilder enumMappingProc = new StringBuilder("<proc name=" + enumMappingProcName.q() + ">".AppendLine());
                StringBuilder tryEnumMappingProc = new StringBuilder("<proc name=" + tryEnumMappingProcName.q() + ">".AppendLine());
                StringBuilder defaultsProc = new StringBuilder("<proc name=" + defaultsProcName.q() + ">".AppendLine());
                MultiStringBuilder allProcs = new MultiStringBuilder(printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);

                allProcs.AppendLine("<if>");
                allProcs.AppendLine("<condition>OBJECT.param_table eq '' and OBJECT.id_paramtable ne ''</condition>");
                allProcs.AppendLine("<then>");
                allProcs.AppendLine("<index_select>");
                allProcs.AppendLine("<index>'ID_PARAMTABLE_TO_PARAM_TABLE'</index>");
                allProcs.AppendLine("<field name='id_paramtable'>OBJECT.id_paramtable</field>");
                allProcs.AppendLine("<cursor>TEMP.csr</cursor>");
                allProcs.AppendLine("<then>");
                allProcs.AppendLine("<do>OBJECT.param_table=OBJECT(TEMP.csr).param_table</do>");
                allProcs.AppendLine("</then>");
                allProcs.AppendLine("</index_select>");
                allProcs.AppendLine("</then>");
                allProcs.AppendLine("</if>");

                allProcs.AppendLine("<if>");
                var piParamTables = GetPiPtMap(mc, dbInfo);
                foreach (var piName in piParamTables.Keys)
                {
                    foreach (string tableName in piParamTables[piName])
                    {
                        if (!rmp.HasPtServiceDefByTableName(tableName))
                        {
                            Console.WriteLine("Warning, not service def for table:" + tableName);
                            continue;
                        }
                        ServiceDef serviceDef = rmp.GetPtServiceDefByTableName(tableName);
                        string serviceDefName = tableName.Substring(5);
                        allProcs.AppendLine("<condition>OBJECT.param_table eq " + tableName.q() + "</condition>");
                        allProcs.AppendLine("<then>");
                        TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, tableName);
                        FieldDef partOfKeyFieldDef = serviceDef.GetEnumPartOfKeyFieldDef();
                        if (partOfKeyFieldDef != null)
                        {
                            string enumSpaceType = (partOfKeyFieldDef.enumSpace + "/" + partOfKeyFieldDef.enumType).ToLower();
                            string partOfKeyEnumName = partOfKeyFieldDef.name;
                            if (!enumDictionary.ContainsKey(enumSpaceType)) throw new Exception("Error, expecting enum like '" + enumSpaceType + "%' to be in t_enum_data");
                            foreach (string partOfKeyEnumValue in enumDictionary[enumSpaceType].Keys)
                            {
                                string prefix = MakeValidFieldName(partOfKeyEnumValue + "_");
                                string enumFieldName = (prefix + partOfKeyEnumName).ToLower();
                                string enumObjectField = "OBJECT." + enumFieldName;
                                allProcs.AppendLine("<if>");
                                allProcs.AppendLine("<condition>" + enumObjectField + " eq '" + partOfKeyEnumValue + "'</condition>");
                                allProcs.AppendLine("<then>");
                                GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                                allProcs.AppendLine("</then>");
                                allProcs.AppendLine("</if>");
                            }
                        }
                        else
                        {
                            string prefix = "";
                            GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                        }
                        allProcs.AppendLine("</then>");
                    }
                }
                allProcs.AppendLine("<else>");
                allProcs.AppendLine("<fatal>'Unknown param_table ['~OBJECT.param_table~']'</fatal>");
                allProcs.AppendLine("</else>");
                allProcs.AppendLine("</if>");
                allProcs.AppendLine("</proc>");
                WriteOutProcs(genDir, printingProcName, printingProc.ToString(), insertingProcName, insertingProc.ToString(), enumMappingProcName, enumMappingProc.ToString(), tryEnumMappingProcName, tryEnumMappingProc.ToString(), defaultsProcName, defaultsProc.ToString());
            }

            // generate svc procs
            {
                string printingProcName = "mt_print_t_svc";
                string insertingProcName = "mt_insert_t_svc";
                string enumMappingProcName = "mt_convert_enums_t_svc";
                string tryEnumMappingProcName = "mt_try_convert_enums_t_svc";
                string defaultsProcName = "mt_default_t_svc";
                StringBuilder printingProc = new StringBuilder("<proc name=" + printingProcName.q() + ">".AppendLine());
                StringBuilder insertingProc = new StringBuilder("<proc name=" + insertingProcName.q() + ">".AppendLine());
                StringBuilder enumMappingProc = new StringBuilder("<proc name=" + enumMappingProcName.q() + ">".AppendLine());
                StringBuilder tryEnumMappingProc = new StringBuilder("<proc name=" + tryEnumMappingProcName.q() + ">".AppendLine());
                StringBuilder defaultsProc = new StringBuilder("<proc name=" + defaultsProcName.q() + ">".AppendLine());
                MultiStringBuilder allProcs = new MultiStringBuilder(printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                allProcs.AppendLine("<if>");
                foreach (string tableName in rmp.SvcTableNameServiceDefMap.Keys)
                {
                    if (!rmp.HasSvcServiceDefByTableName(tableName))
                    {
                        Console.WriteLine("Warning no service defintion for table:" + tableName);
                        continue;
                    }
                    ServiceDef serviceDef = rmp.GetSvcServiceDefByTableName(tableName);
                    string serviceDefName = tableName.Substring(5);
                    allProcs.AppendLine("<condition>OBJECT.service_definition eq " + tableName.ToLower().q() + "</condition>");
                    TableInfo tableInfo;
                    try
                    {
                        tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, tableName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("WARNING: "+ e.Message);
                        continue;
                    }
                    allProcs.AppendLine("<then>");
                    FieldDef partOfKeyFieldDef = serviceDef.GetEnumPartOfKeyFieldDef();
                    if (partOfKeyFieldDef != null)
                    {
                        string enumSpaceType = (partOfKeyFieldDef.enumSpace + "/" + partOfKeyFieldDef.enumType).ToLower();
                        string partOfKeyEnumName = partOfKeyFieldDef.name;
                        if (!enumDictionary.ContainsKey(enumSpaceType)) throw new Exception("Error, expecting enum like '" + enumSpaceType + "%' to be in t_enum_data");
                        foreach (string partOfKeyEnumValue in enumDictionary[enumSpaceType].Keys)
                        {
                            string prefix = MakeValidFieldName(partOfKeyEnumValue + "_");
                            string enumFieldName = (prefix + partOfKeyEnumName).ToLower();
                            string enumObjectField = "OBJECT." + enumFieldName;
                            allProcs.AppendLine("<if>");
                            allProcs.AppendLine("<condition>" + enumObjectField + " eq '" + partOfKeyEnumValue + "'</condition>");
                            allProcs.AppendLine("<then>");
                            GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                            allProcs.AppendLine("</then>");
                            allProcs.AppendLine("</if>");
                        }
                    }
                    else
                    {
                        string prefix = "";
                        GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                    }
                    allProcs.AppendLine("</then>");
                }
                allProcs.AppendLine("<else>");
                allProcs.AppendLine("<fatal>'Unknown svc_name ['~OBJECT.svc_name~']'</fatal>");
                allProcs.AppendLine("</else>");
                allProcs.AppendLine("</if>");
                allProcs.AppendLine("</proc>");
                WriteOutProcs(genDir, printingProcName, printingProc.ToString(), insertingProcName, insertingProc.ToString(), enumMappingProcName, enumMappingProc.ToString(), tryEnumMappingProcName, tryEnumMappingProc.ToString(), defaultsProcName, defaultsProc.ToString());
            }

            // generate ep procs
            {
                string printingProcName = "mt_print_t_ep";
                string insertingProcName = "mt_insert_t_ep";
                string enumMappingProcName = "mt_convert_enums_t_ep";
                string tryEnumMappingProcName = "mt_try_convert_enums_t_ep";
                string defaultsProcName = "mt_default_t_ep";
                StringBuilder printingProc = new StringBuilder("<proc name=" + printingProcName.q() + ">".AppendLine());
                StringBuilder insertingProc = new StringBuilder("<proc name=" + insertingProcName.q() + ">".AppendLine());
                StringBuilder enumMappingProc = new StringBuilder("<proc name=" + enumMappingProcName.q() + ">".AppendLine());
                StringBuilder tryEnumMappingProc = new StringBuilder("<proc name=" + tryEnumMappingProcName.q() + ">".AppendLine());
                StringBuilder defaultsProc = new StringBuilder("<proc name=" + defaultsProcName.q() + ">".AppendLine());
                MultiStringBuilder allProcs = new MultiStringBuilder(printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                allProcs.AppendLine("<if>");
                foreach (string tableName in rmp.EpTableNameServiceDefMap.Keys)
                {
                    ServiceDef serviceDef = rmp.GetEpServiceDefByTableName(tableName);
                    string serviceDefName = tableName.Substring(5);
                    allProcs.AppendLine("<condition>OBJECT.ep_name eq " + serviceDefName.q() + "</condition>");
                    allProcs.AppendLine("<then>");
                    TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, tableName);
                    FieldDef partOfKeyFieldDef = serviceDef.GetEnumPartOfKeyFieldDef();
                    if (partOfKeyFieldDef != null)
                    {
                        string enumSpaceType = (partOfKeyFieldDef.enumSpace + "/" + partOfKeyFieldDef.enumType).ToLower();
                        string partOfKeyEnumName = partOfKeyFieldDef.name;
                        if (!enumDictionary.ContainsKey(enumSpaceType)) throw new Exception("Error, expecting enum like '" + enumSpaceType + "%' to be in t_enum_data");
                        foreach (string partOfKeyEnumValue in enumDictionary[enumSpaceType].Keys)
                        {
                            string prefix = MakeValidFieldName(partOfKeyEnumValue + "_");
                            string enumFieldName = (prefix + partOfKeyEnumName).ToLower();
                            string enumObjectField = "OBJECT." + enumFieldName;
                            allProcs.AppendLine("<if>");
                            allProcs.AppendLine("<condition>" + enumObjectField + " eq '" + partOfKeyEnumValue + "'</condition>");
                            allProcs.AppendLine("<then>");
                            GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                            allProcs.AppendLine("</then>");
                            allProcs.AppendLine("</if>");
                        }
                    }
                    else
                    {
                        string prefix = "";
                        GenerateProcs(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix, printingProc, insertingProc, enumMappingProc, tryEnumMappingProc, defaultsProc);
                    }
                    allProcs.AppendLine("</then>");
                }
                allProcs.AppendLine("<else>");
                allProcs.AppendLine("<fatal>'Unknown ep_name ['~OBJECT.ep_name~']'</fatal>");
                allProcs.AppendLine("</else>");
                allProcs.AppendLine("</if>");
                allProcs.AppendLine("</proc>");
                WriteOutProcs(genDir, printingProcName, printingProc.ToString(), insertingProcName, insertingProc.ToString(), enumMappingProcName, enumMappingProc.ToString(), tryEnumMappingProcName, tryEnumMappingProc.ToString(), defaultsProcName, defaultsProc.ToString());
            }


            // generate select productOfferings
            string poSelectProcName = "select_product_offerings_query";
            string poSelect = "<do>GLOBAL.select_product_offerings_query=" + GenerateSelectProductOfferings(mc, rmp, dbInfo, "").qq() + "</do>";
            string poSelectProc = "<proc name=" + poSelectProcName.q() + ">".AppendLine() + poSelect + "</proc>";
            WriteOutProc(genDir, poSelectProcName, poSelectProc);

            // generate select rc instances query
            string rcInstSelectProcName = "select_rc_instances_query";
            string rcInstSelect = "<do>GLOBAL.select_rc_instances_query=" + GenerateSelectRcInstances(mc, rmp, dbInfo, "").qq() + "</do>";
            string rcInstSelectProc = "<proc name=" + rcInstSelectProcName.q() + ">".AppendLine() + rcInstSelect + "</proc>";
            WriteOutProc(genDir, rcInstSelectProcName, rcInstSelectProc);

            // generate select rc templates query
            string rcTemplateSelectProcName = "select_rc_templates_query";
            string rcTemplateSelect = "<do>GLOBAL.select_rc_templates_query=" + GenerateSelectRcTemplates(mc, rmp, dbInfo, "").qq() + "</do>";
            string rcTemplateSelectProc = "<proc name=" + rcTemplateSelectProcName.q() + ">".AppendLine() + rcTemplateSelect + "</proc>";
            WriteOutProc(genDir, rcTemplateSelectProcName, rcTemplateSelectProc);

            // generate select nrc templates query
            string nrcTemplateSelectProcName = "select_nrc_templates_query";
            string nrcTemplateSelect = "<do>GLOBAL.select_nrc_templates_query=" + GenerateSelectNrcTemplates(mc, rmp, dbInfo, "").qq() + "</do>";
            string nrcTemplateSelectProc = "<proc name=" + nrcTemplateSelectProcName.q() + ">".AppendLine() + nrcTemplateSelect + "</proc>";
            WriteOutProc(genDir, nrcTemplateSelectProcName, nrcTemplateSelectProc);
        }

        public static void GenerateProcs(
            string genDir,
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix,
            StringBuilder printingProc,
            StringBuilder insertingProc,
            StringBuilder enumMappingProc,
            StringBuilder tryEnumMappingProc,
            StringBuilder defaultsProc
)
        {
            printingProc.AppendLine("<call_proc>'" + GeneratePrintToServiceDef(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix) + "'</call_proc>");
            insertingProc.AppendLine("<call_proc>'" + GenerateInsertToServiceDef(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix) + "'</call_proc>");
            enumMappingProc.AppendLine("<call_proc>'" + GenerateApplyServiceDefEnumMapping(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix) + "'</call_proc>");
            tryEnumMappingProc.AppendLine("<call_proc>'" + GenerateTryApplyServiceDefEnumMapping(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix) + "'</call_proc>");
            defaultsProc.AppendLine("<call_proc>'" + GenerateApplyServiceDefDefaults(genDir, rmp, dbInfo, serviceDef, tableInfo, prefix) + "'</call_proc>");

        }

        // track the ones we generated.
        public static void WriteOutProcs(string genDir, params string[] procNameAndConfig)
        {
            for (int i = 0; i < procNameAndConfig.Length; i += 2)
            {
                string procName = procNameAndConfig[i];
                string procConfig = procNameAndConfig[i + 1];
                WriteOutProc(genDir, procName, procConfig);
            }
        }
        public static Dictionary<string, int> GeneratedProcs = new Dictionary<string, int>();
        public static void WriteOutProc(string genDir, string procName, string config)
        {
            string procFileName = Path.Combine(genDir, procName + ".xml");
            //Console.WriteLine("Generating: " + procFileName + "...");
            File.WriteAllText(procFileName, config);
            GeneratedProcs[procFileName] = 1;
        }
        public static bool ProcExists(string genDir, string procName)
        {
            string procFileName = Path.Combine(genDir, procName + ".xml");
            if (GeneratedProcs.ContainsKey(procFileName)) return true;
            if (File.Exists(procFileName)) return true;
            return false;
        }


        /////////////////////////////////////////////////////////////////////////////////////////

        //SELECT c.nm_name template_name, b.n_event_type FROM t_pi_template a, dbo.t_nonrecur b, t_base_props c WHERE a.id_template=b.id_prop AND c.id_prop=b.id_prop
        public static string GenerateSelectNrcTemplates(
          ModuleContext mc,
          RmpSearcher rmp,
          DbInfo dbInfo,
          string prefix
       )
        {
            List<string> fields = new List<string>();
            List<string> from = new List<string>();
            List<string> where = new List<string>();
            fields.Add("'nonrecurring' ep_name");
            fields.Add("b.nm_name pi_template_name");
            fields.Add("b.nm_name");
            fields.Add("b.nm_desc");
            fields.Add("b.nm_display_name");
            fields.Add("b.n_kind");
            fields.Add("a.id_template id_pi_template");
            fields.Add("d.n_event_type");
            fields.Add("z.id_paramtable");
            fields.Add("lower(z.nm_instance_tablename) param_table");
            from.Add("t_pi_template a");
            from.Add("t_base_props b");
            from.Add("t_nonrecur d");
            from.Add("t_ep_nonrecurring e");
            from.Add("t_pi_rulesetdef_map y");
            from.Add("t_rulesetdefinition z");
            where.Add("a.id_template=b.id_prop");
            where.Add("a.id_template=d.id_prop");
            where.Add("a.id_template=e.id_prop");
            where.Add("a.id_pi=y.id_pi");
            where.Add("z.id_paramtable=y.id_pt");

            ServiceDef serviceDef = rmp.EpTableNameServiceDefMap["t_ep_recurring"];
            TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, "t_ep_recurring");
            GetServiceDefToObject(tableInfo, serviceDef, prefix, "e", fields, from, where);
            string query = "select " + fields.Join(",") + " from " + from.Join(",") + " where " + where.Join(" and ");
            return query;
        }


        public static string GenerateSelectRcTemplates(
            ModuleContext mc,
            RmpSearcher rmp,
            DbInfo dbInfo,
            string prefix
         )
        {
            List<string> fields = new List<string>();
            List<string> from = new List<string>();
            List<string> where = new List<string>();
            fields.Add("'recurring' ep_name");
            fields.Add("b.nm_name pi_template_name");
            fields.Add("b.nm_desc");
            fields.Add("b.nm_display_name");
            fields.Add("b.n_kind");
            fields.Add("a.id_template id_pi_template");
            fields.Add("d.b_advance");
            fields.Add("d.b_prorate_on_deactivate");
            fields.Add("d.b_prorate_on_activate");
            fields.Add("d.b_prorate_on_rate_change");
            fields.Add("d.b_fixed_proration_length");
            fields.Add("d.id_usage_cycle");
            fields.Add("d.id_cycle_type");
            fields.Add("d.tx_cycle_mode");
            fields.Add("d.b_charge_per_participant");
            fields.Add("d.n_unit_name");
            fields.Add("d.nm_unit_name");
            fields.Add("d.n_unit_display_name");
            fields.Add("d.nm_unit_display_name");
            fields.Add("d.n_rating_type");
            fields.Add("d.b_integral");
            fields.Add("d.max_unit_value");
            fields.Add("d.min_unit_value");
            from.Add("t_pi_template a");
            from.Add("t_base_props b");
            from.Add("t_recur d");
            from.Add("t_ep_recurring e");
            where.Add("a.id_template=b.id_prop");
            where.Add("a.id_template=d.id_prop");
            where.Add("a.id_template=e.id_prop");
            ServiceDef serviceDef = rmp.EpTableNameServiceDefMap["t_ep_recurring"];
            TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, "t_ep_recurring");
            GetServiceDefToObject(tableInfo, serviceDef, prefix, "e", fields, from, where);
            string query = "select " + fields.Join(",") + " from " + from.Join(",") + " where " + where.Join(" and ");
            return query;
        }


        public static string GenerateSelectRcInstances(
            ModuleContext mc,
            RmpSearcher rmp,
            DbInfo dbInfo,
            string prefix
         )
        {
            List<string> fields = new List<string>();
            List<string> from = new List<string>();
            List<string> where = new List<string>();
            fields.Add("'recurring' ep_name");
            fields.Add("b.nm_name po_name");
            fields.Add("c.nm_name pi_instance_name");
            fields.Add("c.nm_name");
            fields.Add("c.nm_desc");
            fields.Add("c.nm_display_name");
            fields.Add("c.n_kind");
            fields.Add("a.id_pricelist");
            fields.Add("a.b_canicb");
            fields.Add("a.id_pi_template");
            fields.Add("d.b_advance");
            fields.Add("d.b_prorate_on_deactivate");
            fields.Add("d.b_prorate_on_activate");
            fields.Add("d.b_prorate_on_rate_change");
            fields.Add("d.b_fixed_proration_length");
            fields.Add("d.id_usage_cycle");
            fields.Add("d.id_cycle_type");
            fields.Add("d.tx_cycle_mode");
            fields.Add("d.b_charge_per_participant");
            fields.Add("d.n_unit_name");
            fields.Add("d.nm_unit_name");
            fields.Add("d.n_unit_display_name");
            fields.Add("d.nm_unit_display_name");
            fields.Add("d.n_rating_type");
            fields.Add("d.b_integral");
            fields.Add("d.max_unit_value");
            fields.Add("d.min_unit_value");
            from.Add("t_pl_map a");
            from.Add("t_base_props b");
            from.Add("t_base_props c");
            from.Add("t_recur d");
            from.Add("t_ep_recurring e");
            where.Add("a.id_paramtable IS null");
            where.Add("a.id_pi_instance=d.id_prop");
            where.Add("a.id_pi_instance=e.id_prop");
            where.Add("a.id_po=b.id_prop");
            where.Add("a.id_pi_instance = c.id_prop");
            ServiceDef serviceDef = rmp.EpTableNameServiceDefMap["t_ep_recurring"];
            TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, "t_ep_recurring");
            GetServiceDefToObject(tableInfo, serviceDef, prefix, "e", fields, from, where);
            string query = "select " + fields.Join(",") + " from " + from.Join(",") + " where " + where.Join(" and ");
            return query;
        }


        public static string GenerateSelectProductOfferings(
        ModuleContext mc,
        RmpSearcher rmp,
        DbInfo dbInfo,
        string prefix
     )
        {
            List<string> fields = new List<string>();
            List<string> from = new List<string>();
            List<string> where = new List<string>();
            fields.Add("b.nm_name po_name");
            fields.Add("b.nm_name");
            fields.Add("b.nm_desc");
            fields.Add("b.nm_display_name");
            fields.Add("c.n_begintype eff_n_begintype");
            fields.Add("c.dt_start eff_dt_start");
            fields.Add("c.n_beginoffset eff_n_beginoffset");
            fields.Add("c.n_endtype eff_n_endtype");
            fields.Add("c.dt_end eff_date_end");
            fields.Add("c.n_endoffset eff_n_endoffset");
            fields.Add("d.n_begintype avail_n_begintype");
            fields.Add("d.dt_start avail_dt_start");
            fields.Add("d.n_beginoffset avail_n_beginoffset");
            fields.Add("d.n_endtype avail_n_endtype");
            fields.Add("d.dt_end avail_date_end");
            fields.Add("d.n_endoffset avail_n_endoffset");
            fields.Add("a.b_user_subscribe");
            fields.Add("a.b_user_unsubscribe");
            fields.Add("a.b_hidden");
            from.Add("t_po a");
            from.Add("t_base_props b");
            from.Add("t_effectivedate c");
            from.Add("t_effectivedate d");
            from.Add("t_ep_po e");
            where.Add("a.id_po=b.id_prop");
            where.Add("a.id_eff_date=c.id_eff_date");
            where.Add("a.id_avail=d.id_eff_date");
            where.Add("a.id_po=e.id_prop");
            ServiceDef serviceDef = rmp.EpTableNameServiceDefMap["t_ep_po"];
            TableInfo tableInfo = mc.globalContext.schemaMaster.GetTableInfo(mc, dbInfo, "t_ep_po");
            GetServiceDefToObject(tableInfo, serviceDef, prefix, "e", fields, from, where);
            string query = "select " + fields.Join(",") + " from " + from.Join(",") + " where " + where.Join(" and ");
            return query;
        }


        // selects the service entityDef fields doing the tableName to object mapping
        public static void GetServiceDefToObject(TableInfo tableInfo, ServiceDef serviceDef, string prefix, string alias, List<string> fields, List<string> from, List<string> where)
        {
            int tedCtr = 1;
            foreach (var columnName in tableInfo.columnNames)
            {
                string tableColumnName = alias + "." + columnName;
                string fieldName = columnName.ToLower().Substring(2);
                string overrideFieldName = (prefix + fieldName).ToLower();
                var fieldDef = serviceDef.GetFieldDefForColumnName(columnName);
                if (fieldDef == null) continue; // skips id_acc
                if (fieldDef.type.Equals("enum"))
                {
                    string tedAlias = "ted" + (tedCtr++);
                    string nmEnumData = tedAlias + ".nm_enum_data";
                    string enumValue = "SUBSTRING(" + nmEnumData + ",LEN(" + nmEnumData + ")-CHARINDEX('/',REVERSE(" + nmEnumData + "))+2,LEN(" + nmEnumData + "))";
                    fields.Add(enumValue + " " + overrideFieldName);
                    from.Add("t_enum_data " + tedAlias);
                    where.Add(tedAlias + ".id_enum_data=" + tableColumnName);
                }
                else
                {
                    fields.Add(tableColumnName + " " + overrideFieldName);
                }
            }
        }

        // returns the initNamespaceProcName
        public static string GenerateApplyServiceDefEnumMapping(
            string genDir,
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
            )
        {
            string procName = "mt_convert_enums_" + prefix + tableInfo.tableName;
            if (!ProcExists(genDir, procName))
            {
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">");
                proc.Append(GenerateApplyServiceDefEnumMapping(rmp, dbInfo, serviceDef, tableInfo, prefix));
                proc.AppendLine("</proc>");
                WriteOutProc(genDir, procName, proc.ToString());
            }
            return procName;
        }

        // returns config
        public static string GenerateApplyServiceDefEnumMapping(
         RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
         )
        {
            StringBuilder output = new StringBuilder();
            foreach (var f in serviceDef.GetEnumFields())
            {
                string fieldSpaceType = (f.enumSpace + "/" + f.enumType).ToLower();
                string fieldName = f.columnName.ToLower().Substring(2);
                string overrideFieldName = (prefix + fieldName).ToLower();
                string overrideObjectField = "OBJECT." + overrideFieldName;
                string objectFieldNmEnumData = overrideObjectField + "_nmenumdata";
                output.AppendLine("<if>");
                output.AppendLine("<condition>" + overrideObjectField + " ne ''</condition>");
                output.AppendLine("<then>");
                output.AppendLine("<index_select>");
                output.AppendLine("<index>'ENUM_MAP'</index>");
                output.AppendLine("<field name='enum_name'>" + fieldSpaceType.q() + "</field>");
                output.AppendLine("<field name='enum_value'>" + overrideObjectField + "</field>");
                output.AppendLine("<cursor>TEMP.csr</cursor>");
                output.AppendLine("<then>");
                output.AppendLine("<do>" + objectFieldNmEnumData + "=" + overrideObjectField + "</do>");
                output.AppendLine("<do>" + overrideObjectField + "=OBJECT(TEMP.csr).id_enum_data</do>");
                output.AppendLine("<debug>'converted enum_name=[" + fieldSpaceType + "] enum_value=['~" + objectFieldNmEnumData + "~'] to id=['~" + overrideObjectField + "~']'</debug>");
                output.AppendLine("</then>");
                output.AppendLine("<else>");
                output.AppendLine("<fatal>'Unexpected value for " + overrideFieldName + "=['~" + overrideObjectField + "~'] for service definition " + serviceDef.name + ".  Expecting value from lower(T_ENUM_DATA.nm_enum_data) like [" + fieldSpaceType + "/%]'</fatal>");
                output.AppendLine("</else>");
                output.AppendLine("</index_select>");
                output.AppendLine("</then>");
                output.AppendLine("</if>");
            }
            return output.ToString();
        }


        // returns the initNamespaceProcName
        public static string GenerateTryApplyServiceDefEnumMapping(string genDir, RmpSearcher rmp, DbInfo dbInfo, ServiceDef serviceDef, TableInfo tableInfo, string prefix)
        {
            string procName = "mt_try_convert_enums_" + prefix + tableInfo.tableName;
            if (!ProcExists(genDir, procName))
            {
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">");
                proc.Append(GenerateTryApplyServiceDefEnumMapping(rmp, dbInfo, serviceDef, tableInfo, prefix));
                proc.AppendLine("</proc>");
                WriteOutProc(genDir, procName, proc.ToString());
            }
            return procName;
        }

        // returns config
        public static string GenerateTryApplyServiceDefEnumMapping(
         RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
         )
        {
            StringBuilder output = new StringBuilder();
            foreach (var f in serviceDef.GetEnumFields())
            {
                string fieldSpaceType = (f.enumSpace + "/" + f.enumType).ToLower();
                string fieldName = f.columnName.ToLower().Substring(2);
                string overrideFieldName = (prefix + fieldName).ToLower();
                string overrideObjectField = "OBJECT." + overrideFieldName;
                string objectFieldNmEnumData = overrideObjectField + "_nmenumdata";
                output.AppendLine("<if>");
                output.AppendLine("<condition>" + overrideObjectField + " ne ''</condition>");
                output.AppendLine("<then>");
                output.AppendLine("<index_select>");
                output.AppendLine("<index>'ENUM_NAME_OR_ID_TO_ID_MAP'</index>");
                output.AppendLine("<field name='enum_name'>" + fieldSpaceType.q() + "</field>");
                output.AppendLine("<field name='enum_value'>" + overrideObjectField + "</field>");
                output.AppendLine("<cursor>TEMP.csr</cursor>");
                output.AppendLine("<then>");
                output.AppendLine("<do>" + objectFieldNmEnumData + "=" + overrideObjectField + "</do>");
                output.AppendLine("<do>" + overrideObjectField + "=OBJECT(TEMP.csr).id_enum_data</do>");
                output.AppendLine("<debug>'converted enum_name=[" + fieldSpaceType + "] enum_value=['~" + objectFieldNmEnumData + "~'] to id=['~" + overrideObjectField + "~']'</debug>");
                output.AppendLine("</then>");
                output.AppendLine("<else>");
                output.AppendLine("<fatal>'Unexpected value for " + overrideFieldName + "=['~" + overrideObjectField + "~'] for service definition " + serviceDef.name + ".  Expecting value from lower(T_ENUM_DATA.nm_enum_data) like [" + fieldSpaceType + "/%]'</fatal>");
                output.AppendLine("</else>");
                output.AppendLine("</index_select>");
                output.AppendLine("</then>");
                output.AppendLine("</if>");
            }
            return output.ToString();
        }



        // returns the initNamespaceProcName
        public static string GenerateApplyServiceDefDefaults(string genDir, RmpSearcher rmp, DbInfo dbInfo, ServiceDef serviceDef, TableInfo tableInfo, string prefix)
        {
            string procName = "mt_default_" + prefix + tableInfo.tableName;
            if (!ProcExists(genDir, procName))
            {
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">");
                proc.Append(GenerateApplyServiceDefDefaults(rmp, dbInfo, serviceDef, tableInfo, prefix));
                proc.AppendLine("</proc>");
                WriteOutProc(genDir, procName, proc.ToString());
            }
            return procName;
        }

        public static string GenerateApplyServiceDefDefaults(RmpSearcher rmp, DbInfo dbInfo, ServiceDef serviceDef, TableInfo tableInfo, string prefix)
        {
            StringBuilder output = new StringBuilder();
            foreach (var f in serviceDef.fieldDefs)
            {
                if (f.defaultValue != null && (!f.defaultValue.Equals("")))
                {
                    string fieldName = f.columnName.ToLower().Substring(2);
                    string overrideFieldName = (prefix + fieldName).ToLower();
                    string overrideObjectField = "OBJECT." + overrideFieldName;
                    output.AppendLine("<do>" + overrideObjectField + "=" + overrideObjectField + " ne ''? " + overrideObjectField + ":" + f.defaultValue.q() + "</do>");
                }
            }
            return output.ToString();
        }


        // returns the initNamespaceProcName
        public static string GeneratePrintToServiceDef(
            string genDir,
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
            )
        {
            string procName = "mt_print_" + prefix + tableInfo.tableName;
            if (!ProcExists(genDir, procName))
            {
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">");
                proc.Append(GeneratePrintToServiceDef(rmp, dbInfo, serviceDef, tableInfo, prefix));
                proc.AppendLine("</proc>");
                WriteOutProc(genDir, procName, proc.ToString());
            }
            return procName;
        }

        // return the config
        public static string GeneratePrintToServiceDef(
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
            )
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<print_table>");
            output.Append(dbInfo.BackToXmlString());
            output.AppendLine("<name>" + tableInfo.tableName.q() + "</name>");
            output.AppendLine("<regex pattern='^(c_)(.*)$'>OBJECT." + prefix + "$2</regex>");
            output.AppendLine("</print_table>");
            return output.ToString();
        }

        // returns the initNamespaceProcName
        public static string GenerateInsertToServiceDef(
            string genDir,
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
            )
        {
            string procName = "mt_insert_" + prefix + tableInfo.tableName;
            if (!ProcExists(genDir, procName))
            {
                StringBuilder proc = new StringBuilder("<proc name=" + procName.q() + ">");
                proc.Append(GenerateInsertToServiceDef(rmp, dbInfo, serviceDef, tableInfo, prefix));
                proc.AppendLine("</proc>");
                WriteOutProc(genDir, procName, proc.ToString());
            }
            return procName;
        }

        // return the config
        public static string GenerateInsertToServiceDef(
            RmpSearcher rmp,
            DbInfo dbInfo,
            ServiceDef serviceDef,
            TableInfo tableInfo,
            string prefix
            )
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<db_insert>");
            output.Append(dbInfo.BackToXmlString());
            output.AppendLine("<name>" + tableInfo.tableName.q() + "</name>");
            output.AppendLine("<regex pattern='^(c_)(.*)$'>OBJECT." + prefix + "$2</regex>");
            output.AppendLine("</db_insert>");
            return output.ToString();
        }


        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("generate_metranet_config:");
        }


        public static List<string> GetEnumValues(ModuleContext mc, IDbLoginInfo dbInfo, string enumSpace, string enumType)
        {
            string nameSpaceType = enumSpace + "/" + enumType;
            string queryString;
            if (dbInfo.GetType(mc).Equals("oracle"))
            {
                queryString =
                @"select substr(nm_enum_data,instr(nm_enum_data,'/',-1,1)+1) enum_value 
                from t_enum_data 
                where substr(nm_enum_data,0,instr(nm_enum_data,'/',-1,1)-1) = " + nameSpaceType.q();
            }
            else
            {
                queryString =
                @"SELECT SUBSTRING(nm_enum_data,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+2,LEN(nm_enum_data)) enum_value
                FROM t_enum_data
                where  SUBSTRING(nm_enum_data,0,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+1)= " + nameSpaceType.q();
            }

            var results = DbUtils.DbQueryToList(mc, dbInfo, queryString);
            return results;
        }


        // 
        // enumDictionary{lower(enumSpace/enumType)}{enumValue}=id_enum_data;
        public static Dictionary<string, Dictionary<string, int>> GetEnumDictionary(ModuleContext mc, IDbLoginInfo dbInfo)
        {
            Dictionary<string, Dictionary<string, int>> enumDictionary = new Dictionary<string, Dictionary<string, int>>();
            string db = dbInfo.GetDb(mc);
            string server = dbInfo.GetServer(mc);
            string user = dbInfo.GetUser(mc);
            string pw = dbInfo.GetPw(mc);
            string type = dbInfo.GetType(mc);
            if (type.Equals("oracle"))
            {
                enumDictionary = MGenerateMetraNetConfigOra.GetEnumDictionary(mc, dbInfo);
                return enumDictionary;
            }
            else if (type.Equals("sql"))
            {
                string queryString =
                @"SELECT " +
                @"SUBSTRING(nm_enum_data,0,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+1) enum_name, " +
                @"SUBSTRING(nm_enum_data,LEN(nm_enum_data)-CHARINDEX('/',REVERSE(nm_enum_data))+2,LEN(nm_enum_data)) enum_value, " +
                @"id_enum_data " +
                @"from t_enum_data ";
                string connString = @"Pooling=true;Server=" + server + ";Trusted_Connection=no;Database=" + db + ";Uid=" + user + ";Pwd=" + pw + ";";
                //Console.WriteLine("connecting with String: {0}", connString);
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(connString);
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, cannot connect using connString=[" + connString + "], msg=" + e.Message, e);
                    }
                    SqlCommand command;
                    try
                    {
                        command = new SqlCommand(queryString, conn);
                        command.CommandTimeout = 0;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + connString + "], msg=" + e.Message, e);
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string enumName = reader.GetString(0).ToLower();
                        string enumValue = reader.GetString(1);
                        int idEnumData = reader.GetInt32(2);
                        if (!enumDictionary.ContainsKey(enumName)) enumDictionary[enumName] = new Dictionary<string, int>();
                        enumDictionary[enumName][enumValue] = idEnumData;
                    }
                }
                finally
                {
                    if (conn != null) conn.Close();
                }
                return enumDictionary;
            }
            return null;
        }

        // piParamtables{lower(pi_name)}=[lower(pt_name),lower(pt_name)...];
        public static Dictionary<string, List<string>> GetPiPtMap(ModuleContext mc, IDbLoginInfo dbInfo)
        {
            Dictionary<string, List<string>> piParamtables = new Dictionary<string, List<string>>();
            string db = dbInfo.GetDb(mc);
            string server = dbInfo.GetServer(mc);
            string user = dbInfo.GetUser(mc);
            string pw = dbInfo.GetPw(mc);
            string type = dbInfo.GetType(mc);
            if (type.Equals("oracle"))
            {
                piParamtables = MGenerateMetraNetConfigOra.GetPiPtMap( mc,  dbInfo);
                return piParamtables;
            }
            else if (type.Equals("sql"))
            {
                string queryString =
                  @"select lower(b.nm_name) pi_name, lower(c.nm_instance_tablename) pt_name " +
                  @"from t_pi_rulesetdef_map a, t_base_props b,t_rulesetdefinition c where a.id_pi=b.id_prop and a.id_pt=c.id_paramtable " +
                  @"order by b.nm_name ";
                string connString = @"Pooling=true;Server=" + server + ";Trusted_Connection=no;Database=" + db + ";Uid=" + user + ";Pwd=" + pw + ";";
                //Console.WriteLine("connecting with String: {0}", connString);
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(connString);
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, cannot connect using connString=[" + connString + "], msg=" + e.Message, e);
                    }
                    SqlCommand command;
                    try
                    {
                        command = new SqlCommand(queryString, conn);
                        command.CommandTimeout = 0;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + connString + "], msg=" + e.Message, e);
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string piName = reader.GetString(0);
                        string ptName = reader.GetString(1);
                        if (!piParamtables.ContainsKey(piName)) piParamtables[piName] = new List<string>();
                        piParamtables[piName].Add(ptName);
                    }
                }
                finally
                {
                    if (conn != null) conn.Close();
                }
                return piParamtables;
            }
            return null;
        }

        public static string MakeValidFieldName(string inputValue)
        {
            string outputValue = System.Text.RegularExpressions.Regex.Replace(inputValue, "[^a-zA-Z0-9]", "_");
            return outputValue;
        }
    }
}
