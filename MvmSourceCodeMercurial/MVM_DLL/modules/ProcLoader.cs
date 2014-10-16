using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace MVM
{
    public class LazyLoader : IModuleGlobal, IModuleSetup
    {
        public string assemblyName;
        public string className;
        public LazyLoader(string assemblyName, string className)
        {
            this.assemblyName = assemblyName;
            this.className = className;
        }

        public IModuleSetup LoadModuleSetup()
        {
            Assembly assembly = Reflector.LoadAssemblyFrom(this.assemblyName);
            Type t = assembly.GetType(this.className, true, true);
            object obj = Reflector.CreateInstance(t);
            return (IModuleSetup)obj;
        }


        #region IModuleGlobal Members

        public string Global(ProcInfo procInfo, XmlElement me, SchedulerMaster schedulerMaster, Worker worker)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IModuleSetup Members

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    /// <summary>
    /// Summary description for ModuleVM.
    /// </summary>
    public class ProcLoader
    {
        // turns on and off xml validation
        public static bool VALIDATE_XML = System.Environment.OSVersion.Platform.ToString().ToLower().StartsWith("win");

        // Stores whether we are tracing the execution
        private bool trace = false;

        // Maps newModule name to the newModule setup handler
        public Dictionary<string, IModuleSetup> moduleMap = new Dictionary<string, IModuleSetup>();
        public Dictionary<string, IModuleGlobal> moduleGlobalMap = new Dictionary<string, IModuleGlobal>();

        public readonly XmlConfigParser xmlConfigParser;
        public readonly MvmEngine mvm;
        public ProcLoader(MvmEngine mvm)
        {
            this.mvm = mvm;
            this.xmlConfigParser = new XmlConfigParser(mvm);
       
            // globalContext newModules
            moduleGlobalMap.Add("entity", new MDefineEntity());
            moduleGlobalMap.Add("entity_group", new MDefineEntityGroup());
            moduleGlobalMap.Add("entity_map_type", new MDefineEntityMapType());

            // lexical newModules
            moduleMap.Add("startup_cluster", new MStartupCluster());
            moduleMap.Add("mvm_cluster_set", new MNothing());
            moduleMap.Add("mvm_cluster_get", new MNothing());
            moduleMap.Add("server_credentials_set", new MNothing());
            moduleMap.Add("server_credentials_get", new MNothing());

            moduleMap.Add("delete_object", new MDeleteObject());
            moduleMap.Add("remove_spawned_object_from_proc", new MRemoveSpawnedObjectFromProc());
            moduleMap.Add("object_ref_count_get", new MObjectRefCountGet());
            moduleMap.Add("object_ref_get", new MObjectRefGet());
            moduleMap.Add("object_ref_remove", new MObjectRefRemove());

            moduleMap.Add("union_all_select", new MUnionAllSelect());
            moduleMap.Add("select", new MPipeline());
            moduleMap.Add("cursor_inst_create_eof_if_none", new MCursorInstCreateEofIfNone());
            moduleMap.Add("cursor_inst_id_to_object_id", new MCursorInstIdToObjectId());
            moduleMap.Add("cursor_object_id_to_inst_id", new MCursorObjectIdToInstId());

            moduleMap.Add("console", new MConsole());
            moduleMap.Add("object_delta_clear_originals", new MObjectDeltaClearOriginals());
            moduleMap.Add("object_delta_table_status", new MObjectDeltaTableStatus());
            moduleMap.Add("object_delta_flush_all", new MObjectDeltaFlushAll());
            moduleMap.Add("object_delta_persist", new MObjectDeltaPersist());
            moduleMap.Add("object_delta_persist_audit", new MObjectDeltaPersistAudit());
            moduleMap.Add("object_delta_from_table", new MObjectDeltaFromTable());
            moduleMap.Add("object_delta_add_update_triggered_table", new MObjectDeltaAddUpdateTriggeredTable());
            moduleMap.Add("object_delta_pack_originals", new MObjectDeltaPackOriginals());
            moduleMap.Add("object_delta_unpack_originals", new MObjectDeltaUnpackOriginals());
            moduleMap.Add("object_delta_get_persisted", new MObjectDeltaGetPersisted());
            moduleMap.Add("object_delta_get_original", new MObjectDeltaGetOriginal());
            moduleMap.Add("object_delta_set_original", new MObjectDeltaSetOriginal());
            moduleMap.Add("object_delta_has_changes", new MObjectDeltaHasPersistedChanges());
            moduleMap.Add("object_delta_persisted_tables_select", new MObjectDeltaPersistedTablesSelect());
            moduleMap.Add("object_delta_select", new MObjectDeltaSelect());
            moduleMap.Add("object_clear", new MObjectClear());
            moduleMap.Add("object_null_fields", new MObjectNullFields());
            moduleMap.Add("directory_delete", new MDirectoryDelete());
            moduleMap.Add("usage_hook_add_object", new MUsageHookAddObject());
            moduleMap.Add("usage_hook_save_local", new MUsageHookSaveLocal());
            moduleMap.Add("usage_hook_fetch_local", new MUsageHookFetchLocal());
            moduleMap.Add("usage_hook_fetch_flush", new MUsageHookFetchFlush());
            moduleMap.Add("usage_hook_release", new MUsageHookRelease());
            moduleMap.Add("usage_hook_select", new MUsageHookSelect());
            moduleMap.Add("usage_hook_create", new MUsageHookCreate());
            moduleMap.Add("sorted_object_file_write", new MSortedObjectFileWrite());
            moduleMap.Add("sorted_object_file_close", new MSortedObjectFileClose());
            moduleMap.Add("sorted_object_file_select", new MSortedObjectFileSelect());
            moduleMap.Add("write_object_to_file", new MWriteObjectToFile());
            moduleMap.Add("setup_buffered_file_system", new MSetupBufferedFileSystem());
            moduleMap.Add("test_message_speed", new MTestMessageSpeed());
            moduleMap.Add("test_outbox_speed", new MTestOutboxSpeed());
            moduleMap.Add("map_reduce", new MMapReduce());
            
            moduleMap.Add("shutdown_cluster", new MNothing());
            
            moduleMap.Add("slave_node_id_select", new MSlaveNodeIdSelect());
            moduleMap.Add("all_node_id_select", new MSlaveNodeIdSelect());
            moduleMap.Add("define_mvm_cluster", new MDefineMvmCluster());
            moduleMap.Add("push_index", new MPushIndex());
            moduleMap.Add("clear_batch", new MClearBatch());
            moduleMap.Add("wait_for_batch_complete", new MWaitForBatchComplete());
            moduleMap.Add("set_server_mode_off", new MNothing());
            moduleMap.Add("set_server_mode_on", new MNothing());
            moduleMap.Add("send_remote_results", new MSendRemoteResults());
            moduleMap.Add("remote_queue_proc", new MRemoteQueueProc());
            moduleMap.Add("startup_slaves", new MNothing());
            //moduleMap.Add("startup_slave_listener", new MStartupSlaveListener());
            moduleMap.Add("extract_embedded_resource", new MExtractEmbeddedFile());
            //moduleMap.Add("mvm_socket_select", new MMvmSocketSelect());
            //moduleMap.Add("parallel_process_cursor", new MParallelProcessCursor());
            moduleMap.Add("duplicate", new MDuplicate());
            moduleMap.Add("stopwatch", new MStopwatch());
            moduleMap.Add("stopwatch_start", new MStopwatchStart());
            moduleMap.Add("stopwatch_stop", new MStopwatchStop());
            moduleMap.Add("stopwatch_reset", new MStopwatchReset());
            moduleMap.Add("goto_label", new MGotoLabel());
            moduleMap.Add("label", new MLabel());
            moduleMap.Add("join", new MJoin());
            moduleMap.Add("execute_immediate", new MExecuteImmediate());
            moduleMap.Add("mt_msixdef_select", new MMtMsixdefSelect());

            moduleMap.Add("proc_inst_get_params", new MProcInstGetParams());
            moduleMap.Add("proc_inst_delete", new MProcInstDelete());
            moduleMap.Add("proc_inst_is_complete", new MProcInstIsComplete());
            moduleMap.Add("proc_inst_resume", new MProcInstResume());
            
            moduleMap.Add("yield", new MYield());
            moduleMap.Add("get_stack_trace", new MGetStackTrace());
            moduleMap.Add("get_proc_name", new MGetProcName());
            moduleMap.Add("get_full_proc_name", new MGetFullProcName());
            moduleMap.Add("get_proc_namespace", new MGetProcNameSpace());
            moduleMap.Add("xl_next_row", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlNextRow"));
            moduleMap.Add("xl_append_field", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlAppendField"));
            moduleMap.Add("xl_delete_worksheet", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlDeleteWorksheet"));
            moduleMap.Add("xl_append_row", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlAppendRow"));
            moduleMap.Add("xl_open_workbook", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlOpenWorkbook"));
            moduleMap.Add("xl_close_workbook", new LazyLoader("MvmSyncFusion.dll", "MVM.MXlCloseWorkbook"));
            moduleMap.Add("cursor_field_name_select", new MCursorFieldNameSelect());
            moduleMap.Add("pipe_row", new MPipeRow());
            moduleMap.Add("proc_select", new MProcSelect());
            moduleMap.Add("open_pipe_cursor", new MOpenPipeCursor());
            moduleMap.Add("get_exception_info", new MGetExceptionInfo());
            moduleMap.Add("get_exception_name", new MGetExceptionName());
            moduleMap.Add("get_exception_trace", new MGetExceptionTrace());
            moduleMap.Add("get_exception_message", new MGetExceptionMessage());
            moduleMap.Add("try", new MTry());
            moduleMap.Add("throw", new MThrow());
            moduleMap.Add("read_text_file", new MReadTextFile());
            moduleMap.Add("get_mvm_generated_dir", new MGetMvmGeneratedDir());
            moduleMap.Add("get_rmp_bin_dir", new MGetRmpBinDir());
            moduleMap.Add("get_metratech_dir", new MGetMetraTechDir());
            moduleMap.Add("get_proc_extension_dir", new MGetProcExtensionDir());
            moduleMap.Add("get_proc_file_name", new MGetProcFileName());
            moduleMap.Add("mash_id", new MMashId());
            moduleMap.Add("get_mt_db_info", new MGetMtDbInfo());
            moduleMap.Add("compile_dll", new MCompileDll());
            moduleMap.Add("path_head", new MPathHead());
            moduleMap.Add("path_tail", new MPathTail());
            moduleMap.Add("print_text_to_file", new MPrintTextToFile());
            moduleMap.Add("host_wcf_services", new MHostWcfServices());
            moduleMap.Add("shutdown_wcf_host", new MShutDownWcfHost());
            moduleMap.Add("cursor_order_by", new MCursorOrderBy2());
            moduleMap.Add("cursor_pipe_row", new MCursorPipeRow());
            moduleMap.Add("cursor_where", new MCursorWhere());
            moduleMap.Add("cursor_top", new MCursorTop());
            moduleMap.Add("cursor_execute", new MCursorExecute());
            moduleMap.Add("cursor_is_eof", new MCursorIsEof());
            moduleMap.Add("c", new MNothing());
            moduleMap.Add("objects_equal", new MObjectsEqual());
            moduleMap.Add("dump_memory", new MDumpMemory());
            moduleMap.Add("split", new MSplit());
            moduleMap.Add("generate_entities", new MGenerateEntities());
            moduleMap.Add("call_dotnet_static_method", new MCallDotNetStaticMethod());
            moduleMap.Add("get_machine_name", new MGetMachineName());
            moduleMap.Add("get_assembly_uri", new MGetAssemblyUri());
            moduleMap.Add("rename_object_fields", new MRenameObjectFields());
            moduleMap.Add("rename_object_field", new MRenameObjectField());
            moduleMap.Add("copy_object_field", new MCopyObjectField());
            moduleMap.Add("remove_object_field", new MRemoveObjectField());
            moduleMap.Add("get_object_field", new MGetObjectField());
            moduleMap.Add("set_object_field", new MSetObjectField());
            moduleMap.Add("regex_replace", new MRegexReplace());
            moduleMap.Add("produce_work", new MProduceWork());
            moduleMap.Add("system_command_select", new MSystemCommandSelect());
            moduleMap.Add("create_directory", new MCreateDirectory());
            moduleMap.Add("file_close", new MFileClose());
            moduleMap.Add("file_touch", new MFileTouch());
            moduleMap.Add("file_delete", new MFileDelete());
            moduleMap.Add("try_file_delete", new MTryFileDelete());
            moduleMap.Add("file_exists", new MFileExists());
            moduleMap.Add("file_info", new MFileInfo());
            moduleMap.Add("file_substring", new MFileSubstring());
            moduleMap.Add("directory_exists", new MDirectoryExists());
            moduleMap.Add("file_copy", new MFileCopy());
            moduleMap.Add("file_move", new MFileMove());
            moduleMap.Add("get_length", new MGetLength());
            moduleMap.Add("path_parent", new MPathParent());
            moduleMap.Add("path_to_absolute", new MPathToAbsolute());
            moduleMap.Add("path_basename", new MPathBasename());
            moduleMap.Add("match", new MMatch());
            moduleMap.Add("match_static", new MMatchStatic());
            moduleMap.Add("validate_field", new MValidateField());
            moduleMap.Add("print_file", new MPrintFile());
            moduleMap.Add("parse", new MParse());
            moduleMap.Add("get_current_directory", new MGetCurrentDirectory());
            moduleMap.Add("glob_select", new MGlobSelect());
            moduleMap.Add("time", new MTime());
            moduleMap.Add("time_start", new MTimeStart());
            moduleMap.Add("time_end", new MTimeEnd());
            moduleMap.Add("ide_break", new MIdeBreak());
            moduleMap.Add("sterilize_cursor", new MSterilizeCursor());
            moduleMap.Add("copy_cursor_to_temp", new MCopyCursorToTemp());
            moduleMap.Add("multi_thread_cursor", new MMultiThreadCursor());
            moduleMap.Add("round", new MRound());
            moduleMap.Add("round_currency", new MRoundCurrency());
            moduleMap.Add("float_divide", new MFloatDivide());
            moduleMap.Add("date_add", new MDateAdd());
            moduleMap.Add("date_diff", new MDateDiff());
            moduleMap.Add("garbage_collect", new MGarbageCollect());
            moduleMap.Add("float_add", new MFloatAdd());
            moduleMap.Add("capitalize", new MCapitalize());
            moduleMap.Add("uncapitalize", new MUncaplitalize());
            moduleMap.Add("replace", new MReplace());
            moduleMap.Add("print", new MLog());
            moduleMap.Add("log", new MLog());
            moduleMap.Add("fatal", new MLog());
            moduleMap.Add("severe", new MLog());
            moduleMap.Add("warning", new MLog());
           
            moduleMap.Add("info", new MLog());
            moduleMap.Add("config", new MLog());
            moduleMap.Add("debug", new MLog());
            moduleMap.Add("fine", new MLog());
            moduleMap.Add("finer", new MLog());
            moduleMap.Add("finest", new MLog());
            moduleMap.Add("counter_select", new MCounterSelect());
            moduleMap.Add("inline_call_proc_for_current_object", new InlineCallProcForCurrentObject());
            moduleMap.Add("parallel", new MParallel());
            moduleMap.Add("get_cluster_object_id", new MGetClusterObjectId());
            moduleMap.Add("get_date", new MGetDate());
            moduleMap.Add("sleep", new MSleep());
            moduleMap.Add("inherit_object", new MInheritObject());
            moduleMap.Add("db_call_proc", new MDbCallProc());
            moduleMap.Add("db_bulk_insert_dynamic", new MDbBulkInsertDynamic());
            moduleMap.Add("db_bulk_insert", new MDbBulkInsert());
            moduleMap.Add("db_bulk_insert_abort", new MDbBulkInsertAbort());
            moduleMap.Add("db_bulk_insert_abort_all", new MDbBulkInsertAbortAll());
            moduleMap.Add("db_bulk_insert_flush", new MDbBulkInsertFlush());
            moduleMap.Add("db_bulk_insert_flush_all", new MDbBulkInsertFlushAll());
            moduleMap.Add("db_upsert", new MDbUpsert());
            moduleMap.Add("db_upsert_dynamic", new MDbUpsertDynamic());
            moduleMap.Add("db_insert", new MDbInsert());
            moduleMap.Add("db_insert_dynamic", new MDbInsertDynamic());
            moduleMap.Add("db_execute", new MDbExecute());
            moduleMap.Add("db_execute_dynamic", new MDbExecuteDynamic());
            moduleMap.Add("remove_named_class", new MRemoveNamedClass());
            moduleMap.Add("trim", new MTrim());
            moduleMap.Add("to_upper", new MToUpper());
            moduleMap.Add("to_lower", new MToLower());
            moduleMap.Add("date_convert", new MDateConvert());
            moduleMap.Add("date_to_string", new MDateToString());
            moduleMap.Add("string_to_date", new MStringToDate());
            moduleMap.Add("substring", new MSubstring());
            moduleMap.Add("file_convert", new MFileConvert());
            moduleMap.Add("file_grep", new MFileGrep());
            moduleMap.Add("get_file", new MGetFile());
            moduleMap.Add("file_sort", new MFileSort());
            moduleMap.Add("md5", new MMd5Hash());
            moduleMap.Add("get_guid", new MGetGuid());
            moduleMap.Add("run_once", new MRunOnce());
            moduleMap.Add("inherit_cursor_default", new MInheritCursorDefault());
            moduleMap.Add("inherit_cursor", new MInheritCursor());
            moduleMap.Add("index_insert", new MIndexInsert());
            moduleMap.Add("index_insert_if_none", new MIndexInsertIfNone());
            moduleMap.Add("index_clear", new MIndexClear());
            moduleMap.Add("index_close", new MIndexClose());
            moduleMap.Add("index_remove", new MIndexRemove());
            moduleMap.Add("index_select_keys", new MIndexSelectKeys());
            moduleMap.Add("index_get", new MIndexGet());
            moduleMap.Add("define_memory_index", new MDefineMemoryIndex());
            moduleMap.Add("load_procs_from_files", new MLoadProcsFromFiles());
            moduleMap.Add("load_procs_from_glob", new MLoadProcsFromGlob());
            moduleMap.Add("print_table", new MPrintTable());
            moduleMap.Add("print_table_for_ctrl", new MPrintTableForCtrl());
            moduleMap.Add("run", new MRun());
            moduleMap.Add("db_select_dynamic", new MDbSelectDynamic());
            moduleMap.Add("db_query_to_file", new MDbQueryToFile());
            moduleMap.Add("create_memory_index_from_cursor", new MCreateMemoryIndexFromCursor());
            moduleMap.Add("index_select", new MIndexSelect());
            moduleMap.Add("create_index_on_sorted_file", new MCreateIndexOnSortedFile());
            moduleMap.Add("get_worker_no", new MGetWorkerNo());
            moduleMap.Add("print_record", new MPrintRecord());
            moduleMap.Add("produce", new MProduce());
            moduleMap.Add("nothing", new MNothing());
            //moduleMap.Add("create_cluster", new MCreateCluster());
            moduleMap.Add("cursor_next", new MCursorNext());
            moduleMap.Add("cursor_clear", new MCursorClear());
            moduleMap.Add("file_select", new MFileSelect());
            moduleMap.Add("pop_scope", new MScopePop());
            moduleMap.Add("push_scope", new MScopePush());
            moduleMap.Add("remove_object_from_cache", new MRemoveObjectFromCache());
            moduleMap.Add("do", new MDo());
            moduleMap.Add("get_proc_text", new MGetProcText());
            moduleMap.Add("record_type", new MRecordType());
            moduleMap.Add("call_proc", new MCallProc());
            moduleMap.Add("call_proc_for_object_nested", new MCallProc());
            moduleMap.Add("call_proc_for_object_nested_no_callback", new MCallProc());
            moduleMap.Add("call_proc_for_object_nested_no_callback_on_stack", new MCallProc());
            moduleMap.Add("call_proc_for_current_object_nested", new MCallProc());
            moduleMap.Add("call_proc_for_current_object", new MCallProc());
            moduleMap.Add("setup_call_proc_for_current_object", new MCallProc());
            moduleMap.Add("call_proc_for_object", new MCallProc());
            moduleMap.Add("setup_call_proc_for_object", new MCallProc());
            moduleMap.Add("call_dynamic_proc_for_object", new MCallProc());
            moduleMap.Add("call_dynamic_proc_for_current_object", new MCallProc());
            moduleMap.Add("call_proc_for_current_cluster", new MCallProc());
            moduleMap.Add("call_proc_for_cluster", new MCallProc());
            moduleMap.Add("queue_proc_for_current_object", new MCallProc());
            moduleMap.Add("queue_proc_for_current_object_nested", new MCallProc());
            moduleMap.Add("queue_proc_for_object", new MCallProc());
            moduleMap.Add("queue_proc_for_object_nested", new MCallProc());
            moduleMap.Add("queue_proc_for_current_object_nested_on_stack", new MCallProc());
            moduleMap.Add("queue_proc_for_object_in_cluster", new MCallProc());
            moduleMap.Add("queue_proc_for_current_cluster", new MCallProc());
            moduleMap.Add("queue_proc_for_cluster", new MCallProc());
            moduleMap.Add("spawn", new MSpawn());
            moduleMap.Add("synchronized", new MSynchronized());
            moduleMap.Add("sync_start", new MSyncStart());
            moduleMap.Add("sync_end", new MSyncEnd());
            moduleMap.Add("if", new MIf2());
            //moduleMap.Add("if2", new MIf());
            moduleMap.Add("while", new MWhile2());
            //moduleMap.Add("while2", new MWhile());
            moduleMap.Add("callback", new MCallback());
            moduleMap.Add("callback_create", new MCallbackCreate());
            moduleMap.Add("callback_create_with_scope_pop", new MCallbackCreateWithScopePop());
            moduleMap.Add("push_before", new MPushBefore());
            moduleMap.Add("push_after", new MPushAfter());
            moduleMap.Add("return", new MReturn());

            // Added by TCF September 2014
            moduleMap.Add("get_server_data", new MGetServerData());
        }

        /// <summary>
        /// After all dynamic module info has been loaded, we can create and load the module XSD
        /// </summary>
        public void LoadXSD()
        {
            xmlConfigParser.LoadXSD();
        }

        /// <summary>
        /// Adds a module declaration, for preparing the module XSD
        /// </summary>
        public void AddModuleDeclaration(XmlElement decl)
        {
            xmlConfigParser.AddModuleDeclaration(decl);
        }

        /// <summary>
        /// Adds a module type, for preparing the module XSD
        /// </summary>
        public void AddModuleType(XmlElement type)
        {
            xmlConfigParser.AddModuleType(type);
        }

        /// <summary>
        /// Adds a module name, for preparing the module autodocs
        /// </summary>
        public void AddModuleName(string name, string[] category)
        {
            xmlConfigParser.AddModuleName(name, category);
        }

        // parses the xml file and return the document element. If there
        // is an error then we will return xml like this:
        // <error>
        // <error_message>here is why parsing failed...</error_message>
        // <procContext name='xxxx' namespace='yyyy'/>
        // <procContext name='xxxx' namespace='yyyy'/>
        // </error>
        public XmlElement ParseInputXmlFile(string xmlFileName)
        {
            return xmlConfigParser.ParseXmlFile(xmlFileName);
        }

        public XmlElement ParseInputXmlString(string xmlString)
        {
            return xmlConfigParser.ParseXmlString(xmlString);
        }

        // Registers a module
        // need name,handler, and xsd info.
        public void RegisterModule(string moduleName, IModuleSetup moduleSetup)
        {
            if (moduleMap.ContainsKey(moduleName))
            {
                throw new Exception("Error, module=[" + moduleName + "] is already defined!");
            }
            moduleMap[moduleName] = moduleSetup;
        }

        // Read an individual xml newModule
        public IModuleRun ReadXmlModule(XmlElement moduleElement)
        {
            string moduleName = moduleElement.LocalName;
            if (this.trace == true) Console.WriteLine("reading module " + moduleName);
            //else Console.WriteLine("OFF newModule "+moduleName);
            if (moduleMap.ContainsKey(moduleName))
            {
                IModuleSetup setupModule = (IModuleSetup)moduleMap[moduleName];
                LazyLoader lazyLoader = setupModule as LazyLoader;
                if (lazyLoader != null) setupModule = lazyLoader.LoadModuleSetup();
                IModuleRun moduleInst = new MXmlModule(moduleElement, setupModule);
                return moduleInst;
            }
            else
            {
                throw new Exception("Unknown module [" + moduleName + "]!");
            }
        }

        /// <summary>
        /// Given a parent node this parses each child node into its newModule instance and return a list of them
        /// </summary>
        public List<IModuleRun> ReadXmlModules(XmlElement elem)
        {
            List<IModuleRun> modules = new List<IModuleRun>();
            XmlNodeList nodes = elem.SelectNodes("./*");
            foreach (XmlNode node in nodes)
            {
                XmlElement moduleElem = (XmlElement)node;
                // flatten any serial blocks
                if (moduleElem.LocalName.Equals("serial"))
                {
                    modules.AddRange(ReadXmlModules(moduleElem));
                }
                // skip procContext params
                else if (moduleElem.LocalName.Equals("param"))
                {
                }
                else
                {
                    IModuleRun moduleInst = ReadXmlModule(moduleElem);
                    modules.Add(moduleInst);
                }
            }
            return modules;
        }

        // expects the string to be well formed. top tag ignored
        public List<IModuleRun> ReadXmlModules(string xmlString)
        {
            XmlElement fakeProcElem = this.ParseInputXmlString(xmlString);
            return this.ReadXmlModules(fakeProcElem);
        }

        // expects the string to be well formed
        public IModuleRun ReadXmlModule(string xmlString)
        {
            XmlElement moduleElem = this.ParseInputXmlString(xmlString);
            return this.ReadXmlModule(moduleElem);
        }
    }

}
