using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MVM
{
    /*
     * <push_index>
     *      <process_id>GLOBAL.master</process_id>
     *      <clear_source>1|0</clear_source>
     *      <source>
     *           <index>STRUCT</index>
     *           <field name="akey">'blah'</field>
     *      </source>
     *      <target>
     *           <index>OTHER</index>
     *           <field name="xxx">'blow'</field>
     *      </target>
     * </push_index>
     */
    class MPushIndex : IModuleSetup
    {
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string processIdSyntax = me.SelectNodeInnerText("./process_id");
            IReadString processIdParsed = mc.ParseSyntax(processIdSyntax);

            string clearSourceSyntax = me.SelectNodeInnerText("./clear_source");
            IReadString clearSourceParsed = mc.ParseSyntax(clearSourceSyntax);

            XmlElement srcElement = me.SelectSingleElem("./source");
            string srcIndexName = mc.ParseSyntax(srcElement.SelectNodeInnerText("./index")).Read(mc);
            Dictionary<string, string> srcPassedFields = srcElement.SelectNodes("field").ToNameTextDictionary();
            IIndex srcIndex = (IIndex)mc.globalContext.GetNamedClassInst(srcIndexName);
            List<string> srcOrderedFields = srcIndex.GetOrderedValueFields();
            List<string> srcOrderedKeyFields = srcIndex.GetOrderedKeyFields();

            XmlElement tgtElement = me.SelectSingleElem("./target");
            string tgtIndexName = mc.ParseSyntax(tgtElement.SelectNodeInnerText("./index")).Read(mc);
            string tgtIndexLocalNameSyntax = tgtElement.SelectNodeInnerText("./local_name");
            string tgtIndexLocalName;
            if (tgtIndexLocalNameSyntax.IsNullOrEmpty())
            {
                tgtIndexLocalName = tgtIndexName;
            }
            else
            {
                tgtIndexLocalName = mc.ParseSyntax(tgtIndexLocalNameSyntax).Read(mc);
            }
            Dictionary<string, string> tgtPassedFields = tgtElement.SelectNodes("field").ToNameTextDictionary();
            IIndex tgtIndex = (IIndex)mc.globalContext.GetNamedClassInst(tgtIndexLocalName);
            List<string> tgtOrderedFields = tgtIndex.GetOrderedValueFields();

            // mapping of target recordIdxes that need to be populated with readable values
            List<KeyValuePair<int, IReadString>> tgtIdxSrcVarMap = new List<KeyValuePair<int, IReadString>>();

            // mapping of target recordIdxes that need to be populated with values from the source cursor
            List<KeyValuePair<int, int>> tgtIdxSrcIdxMap = new List<KeyValuePair<int, int>>();

            // need to make sure that we have a value (or default value) for every target field, else error.
            for (int tgtIdx = 0; tgtIdx < tgtOrderedFields.Count; tgtIdx++)
            {
                string tgtFieldName = tgtOrderedFields[tgtIdx];
                if (tgtPassedFields.ContainsKey(tgtFieldName))
                {
                    string defaultSyntax = tgtPassedFields[tgtFieldName];
                    IReadString defaultParsed = mc.ParseSyntax(defaultSyntax);
                    tgtIdxSrcVarMap.Add(new KeyValuePair<int, IReadString>(tgtIdx, defaultParsed));
                }
                else if (srcOrderedFields.Contains(tgtFieldName))
                {
                    int srcIdx = srcOrderedFields.IndexOf(tgtFieldName);
                    tgtIdxSrcIdxMap.Add(new KeyValuePair<int, int>(tgtIdx, srcIdx));
                }
                else
                {
                    throw new Exception("Cannot push " + srcIndexName + " to " + tgtIndexName + " with out specifying value for " + tgtFieldName);
                }
            }

            // If the user didn't pass any keys, assume push all.
            if (srcPassedFields.Count == 0)
            {
                PushMemoryIndex m = new PushMemoryIndex(
                   processIdParsed,
                   srcIndexName,
                   tgtIdxSrcVarMap,
                   tgtIdxSrcIdxMap,
                   tgtIndexName,
                   tgtOrderedFields,
                   clearSourceParsed);
                run.Add(m);
                return;
            }

            // Otherwise we're pushing a slice
            {
                List<IReadString> srcKeyFields = new List<IReadString>();
                foreach (string keyField in srcOrderedKeyFields)
                {
                    if (!srcPassedFields.ContainsKey(keyField)) throw new Exception("Error need to pass key field " + keyField);
                    string srcPassedSyntax = srcPassedFields[keyField];
                    IReadString srcPassedParsed = mc.ParseSyntax(srcPassedSyntax);
                    srcKeyFields.Add(srcPassedParsed);
                }
                PushMemoryIndexSlice m = new PushMemoryIndexSlice(
                    processIdParsed,
                    srcIndexName,
                    srcKeyFields,
                    tgtIdxSrcVarMap,
                    tgtIdxSrcIdxMap,
                    tgtIndexName,
                    tgtOrderedFields,
                    clearSourceParsed);
                run.Add(m);
                return;
            }
        }
    }


    /// <summary>
    /// This class pushes an entire MemoryIndex
    /// </summary>
    public class PushMemoryIndex:IModuleRun
    {
        public PushMemoryIndex(
            IReadString processIdParsed, 
            string srcIndexName, List<KeyValuePair<int, IReadString>> 
            tgtIdxSrcVarMap, 
            List<KeyValuePair<int, int>> tgtIdxSrcIdxMap, 
            string tgtIndexName, 
            List<string> tgtOrderedFields,
            IReadString clearSourceParsed
            )
        {
            this.processIdParsed = processIdParsed;
            this.srcIndexName = srcIndexName;
            this.tgtIdxSrcIdxMap = tgtIdxSrcIdxMap;
            this.tgtIdxSrcVarMap = tgtIdxSrcVarMap;
            this.tgtIndexName = tgtIndexName;
            this.tgtOrderedFields = tgtOrderedFields;
            this.clearSourceParsed = clearSourceParsed;
            this.tgtFieldCount = this.tgtOrderedFields.Count;
     }

        public IReadString processIdParsed;
        public string srcIndexName;
        public List<KeyValuePair<int, IReadString>> tgtIdxSrcVarMap;
        public List<KeyValuePair<int, int>> tgtIdxSrcIdxMap;
        public string tgtIndexName;
        List<string> tgtOrderedFields;
        public int tgtFieldCount;
        public IReadString clearSourceParsed;

        public void Run(ModuleContext mc)
        {
            bool clearSource = this.clearSourceParsed.Read(mc).Equals("1");

            // need to lookup the socketHandler;
            string nodeId = this.processIdParsed.Read(mc);
            int nodeIdInt = nodeId.ToInt();
            SocketHandler socketHandler = mc.mvm.mvmCluster.GetClusterNode(nodeIdInt).SocketHandler;


            // need to build up a string array 
            string[] tgtRecord = new string[this.tgtFieldCount];
            
            // need to preload the target record with static context
            foreach (var entry in tgtIdxSrcVarMap)
            {
                tgtRecord[entry.Key] = entry.Value.Read(mc);
            }
            
            IIndex srcIndex = mc.globalContext.GetNamedClassInst(this.srcIndexName) as IIndex;
            srcIndex.PushAll(mc.mvm,socketHandler, clearSource, tgtIndexName, tgtRecord, tgtIdxSrcIdxMap);
        }
    }


    /// <summary>
    /// This class pushes a slice of a MemoryIndex
    /// </summary>
    public class PushMemoryIndexSlice : IModuleRun
    {
        public PushMemoryIndexSlice(IReadString processIdParsed, 
            string srcIndexName,
            List<IReadString> srcKeyFields, 
            List<KeyValuePair<int, IReadString>> tgtIdxSrcVarMap, 
            List<KeyValuePair<int, int>> tgtIdxSrcIdxMap, 
            string tgtIndexName, List<string> tgtOrderedFields,
            IReadString clearSourceParsed
            )
        {
            this.processIdParsed = processIdParsed;
            this.srcIndexName = srcIndexName;
            this.srcKeyFields = srcKeyFields;
            this.tgtIdxSrcIdxMap = tgtIdxSrcIdxMap;
            this.tgtIdxSrcVarMap = tgtIdxSrcVarMap;
            this.tgtIndexName = tgtIndexName;
            this.tgtOrderedFields = tgtOrderedFields;
            this.clearSourceParsed = clearSourceParsed;
            this.tgtFieldCount = this.tgtOrderedFields.Count;
        }

        public IReadString processIdParsed;
        public string srcIndexName;
        public List<IReadString> srcKeyFields;
        public List<KeyValuePair<int, IReadString>> tgtIdxSrcVarMap;
        public List<KeyValuePair<int, int>> tgtIdxSrcIdxMap;
        public string tgtIndexName;
        List<string> tgtOrderedFields;
        public int tgtFieldCount;
        public IReadString clearSourceParsed;

        public void Run(ModuleContext mc)
        {
            bool clearSource = this.clearSourceParsed.Read(mc).Equals("1");

            // need to lookup the socketHandler;
            string processId = this.processIdParsed.Read(mc);
            SocketAccesor socketAccessor = (SocketAccesor)mc.globalContext.GetNamedClassInst(processId);
            SocketHandler socketHandler = socketAccessor.GetSocketHandler(5000);

            // need to build up the target record template
            string[] tgtRecordTemplate = new string[this.tgtFieldCount];
            foreach (var entry in tgtIdxSrcVarMap)
            {
                tgtRecordTemplate[entry.Key] = entry.Value.Read(mc);
            }

            IIndex srcIndex = mc.globalContext.GetNamedClassInst(this.srcIndexName) as IIndex;

            // need to build up ordered keys
            string[] orderedKeyValues = new string[this.srcKeyFields.Count];
            for (int i = 0; i < orderedKeyValues.Length; i++)
            {
                orderedKeyValues[i] = this.srcKeyFields[i].Read(mc);
            }

            // Call push on the srcIndex
            srcIndex.PushSlice(mc.mvm, orderedKeyValues, socketHandler, clearSource, tgtIndexName, tgtRecordTemplate, tgtIdxSrcIdxMap);
        }
    }
}
