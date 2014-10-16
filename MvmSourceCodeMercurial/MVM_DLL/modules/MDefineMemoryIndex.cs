using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

// TEMP
using System.Reflection;

/*
<define_memory_index capacity='4'>
<index>'MY_INDEX'</index>
<type>'lifo'|'fifo'</type>
<key_field>'field_1'</key_field>
<key_field>'field_2'</key_field>
<capacity>1000</capacity>
<field>'field_3'</field>
<field>'field_4'</field>
</define_memory_index>
*/
namespace MVM
{
    public class MDefineMemoryIndex : IModuleSetup, IModuleRun
    {
        // from xml
        private string indexSyntax;
        private string fileSyntax;
        private string outputFileSyntax;
        private List<string> keyFieldsSyntax = new List<string>();
        private List<string> valFieldsSyntax = new List<string>();
        // new value added by TCF
        private int capacity;
        // from setup
        private string indexName;
        private string fileName;

        private IWriteString outputFileParsed;
        private List<IReadString> keyFieldsParsed = new List<IReadString>();
        private List<IReadString> valFieldsParsed = new List<IReadString>();

        private string defaultCursorOrderString;
        private CursorOrder defaultCursorOrder = CursorOrder.Forward;

        private string useNestedKeys;
        private string useContext;
        private string unique;
        private bool replace;

        private bool synchronized;
        private List<int> objectIdFieldIdxes;

        string searchKeyFieldName = null;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MDefineMemoryIndex m = new MDefineMemoryIndex();
            // xml extraction
            m.indexSyntax = me.SelectNodeInnerText("./index");
            m.fileSyntax = me.SelectNodeInnerText("./file");
            m.outputFileSyntax = me.SelectNodeInnerText("./output_file");
            m.keyFieldsSyntax = me.SelectNodesInnerText("./key_field");
            m.valFieldsSyntax = me.SelectNodesInnerText("./field");
            // parse out capacity; default to 4
            // Added by TCF to explicitly set the capacity.
            if (me.HasChildElement("capacity")) {
                IReadString capstr = mc.ParseSyntax(me.SelectNodeInnerText("./capacity"));
                string cap = capstr.Read(mc);
                if (cap != null && cap.IsNumeric()) m.capacity = Convert.ToInt32(capacity);
                else m.capacity = 4;
                mc.mvm.Log(String.Format("Defined index {0} with a capacity of {1}.", m.indexSyntax, cap));
                mc.mvm.Log("Got a capacity of " + cap);
            }
            m.synchronized = mc.SyntaxReadString(me.GetAttributeDefaulted("synchronized", "0")).Equals("1");

            // parsing
            if (!mc.IsLiteralString(m.indexSyntax)) throw new Exception("Error, expecting the index name to be a literal string: [" + m.indexSyntax + "]");
            m.indexName = mc.GetLiteralString(m.indexSyntax);
            if (!m.fileSyntax.IsNullOrEmpty())
            {
                m.fileName = mc.ParseSyntax(m.fileSyntax).Read(mc);
            }
            else if (m.fileSyntax != null)
            {
                string tempDir = mc.globalContext["mvm_temp_dir"];
                if (tempDir.Equals(""))
                {
                    tempDir = Path.GetTempPath();
                }
                // GetRandomFileName() will give names like sadflkjgh.oiu, which when given to raptor
                // results in files like sadflkjgh.mgbmp, which we then can't auto-delete, so kill the dots
                m.fileName = tempDir + Path.PathSeparator + Path.GetRandomFileName().Replace('.', 'x');
            }
            if (!m.outputFileSyntax.IsNullOrEmpty())
            {
                m.outputFileParsed = mc.ParseWritableSyntax(m.outputFileSyntax);
                m.outputFileParsed.Write(mc, m.fileName);
            }
            m.keyFieldsParsed = mc.ParseSyntax(m.keyFieldsSyntax);
            m.valFieldsParsed = mc.ParseSyntax(m.valFieldsSyntax);

            // cursor newOrder
            m.defaultCursorOrderString = mc.SyntaxReadString(me.SelectNodeInnerText("./default_cursor_order", "'forward'"));
            if (m.defaultCursorOrderString.Equals("reverse")) m.defaultCursorOrder = CursorOrder.Reverse;
            else if (m.defaultCursorOrderString.Equals("forward")) m.defaultCursorOrder = CursorOrder.Forward;
            else throw new Exception("Unexpected default_cursor_order [" + m.defaultCursorOrderString + "]");

            // is structure nested
            string useNestedKeysSyntax = me.SelectNodeInnerText("./use_nested_keys", "''");
            m.useNestedKeys = mc.SyntaxReadString(useNestedKeysSyntax);

            // default if we should use context to figure implicitly get keys.
            m.useContext = mc.SyntaxReadString(me.SelectNodeInnerText("./use_context", "0"));

            // should we enforce uniqueness in this index?
            m.unique = mc.SyntaxReadString(me.SelectNodeInnerText("./unique", "0"));

            // drop the index first if it already exists
            m.replace = mc.SyntaxReadString(me.SelectNodeInnerText("./replace", "0")).Equals("1");

            // is structure synchronized
            m.synchronized = mc.SyntaxReadString(me.GetAttributeDefaulted("synchronized", "0")).Equals("1");

            // what field indexes are object ids.
            List<XmlElement> orderedFieldElems = new List<XmlElement>();
            me.SelectElements("./key_field").ToList().ForEach(n => orderedFieldElems.Add(n));
            me.SelectElements("./field").ToList().ForEach(n => orderedFieldElems.Add(n));

            m.objectIdFieldIdxes = orderedFieldElems
                .SelectIndexValuePairs()
                .Where(e => e.value.GetAttribute("type").Equals("object_id"))
                .Select(e => e.index)
                .ToList();

            m.searchKeyFieldName = orderedFieldElems
                .Where(e => e.GetAttribute("type").Equals("search"))
                .Select(e => e.InnerText).FirstOrDefault();
            if (m.searchKeyFieldName != null)
                m.searchKeyFieldName = mc.GetLiteralString(m.searchKeyFieldName);

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            List<string> orderedKeyFields = new List<string>();
            foreach (IReadString keyFieldParsed in this.keyFieldsParsed) orderedKeyFields.Add(keyFieldParsed.Read(mc));
            List<string> orderedValFields = new List<string>();
            foreach (IReadString valFieldParsed in this.valFieldsParsed) orderedValFields.Add(valFieldParsed.Read(mc));

            // store both the key and the value
            List<string> orderedFieldNames = new List<string>();
            orderedFieldNames.AddRange(orderedKeyFields);
            orderedFieldNames.AddRange(orderedValFields);

            if (replace)
            {
                object idx;
                if (mc.globalContext.GetNamedClassInst(this.indexName, out idx))
                {
                    IIndex iindex = idx as IIndex;
                    if (iindex != null)
                    {
                        iindex.IndexDrop(mc);
                    }
                }
            }

            //if (orderedFieldNames.Contains("object_id"))
            //{
            //    throw new Exception("Error, fields cannot be named 'object_id' in memory index [" + this.indexName + "]");
            //}

            // create the index
            IIndex index;
            if (this.searchKeyFieldName != null)
            {
                mc.mvm.Log("Creating a SearchMemoryIndex called " + this.indexName);
                index = new SearchMemoryIndex(this.indexName, orderedFieldNames, orderedKeyFields, this.searchKeyFieldName, defaultCursorOrder, useContext.Equals("1"), objectIdFieldIdxes);
            }
            else if (!this.fileName.IsNullOrEmpty())
            {
                mc.mvm.Log("Creating a RaptorDBIndex called " + this.indexName);
                index = new RaptorDBIndex(mc.mvm, this.indexName, this.fileName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext.Equals("1"), unique.Equals("1"), objectIdFieldIdxes);
            }
            else if (synchronized)
            {
                if (useNestedKeys.Equals("1"))
                    throw new Exception("Error, memory index with nested keys does not support synchronized: " + this.indexName);
                else
                {
                    mc.mvm.Log("Creating a MemoryIndexSync in clause A called " + this.indexName);
                    index = new MemoryIndexSync(mc.mvm, this.indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext.Equals("1"), unique.Equals("1"), objectIdFieldIdxes, this.capacity);
                }
            }
            else
            {
                if (useNestedKeys.Equals("1"))
                {
                    mc.mvm.Log("Creating a NestedMemoryIndex called + " + this.indexName);
                    index = new NestedMemoryIndex(this.indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext.Equals("1"), unique.Equals("1"));
                }
                else
                {
                    // for now deprecate this and see if speed matters.
                    //index = new MemoryIndex(orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext.Equals("1"), objectIdFieldIdxes);
                    try
                    {
                        /*mc.mvm.Log("MDefineMemoryIndex Memory Check #2:");
                        long memory = GC.GetTotalMemory(true);
                        mc.mvm.Log(String.Format("Memory used: {0}", memory.ToString()));
                        long memory2 = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
                        mc.mvm.Log(String.Format("Memory(2) used: {0}", memory2.ToString()));*/

                        //mc.mvm.Log("Creating a MemoryIndexSync in clause B called " + this.indexName);
                        index = new MemoryIndexSync(mc.mvm, this.indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, useContext.Equals("1"), unique.Equals("1"), objectIdFieldIdxes, this.capacity);

                        /*mc.mvm.Log("MDefineMemoryIndex Memory Check #3:");
                        mc.mvm.Log(String.Format("Memory used: {0}", GC.GetTotalMemory(true).ToString()));
                        mc.mvm.Log(String.Format("Memory(2) used: {0}", System.Diagnostics.Process.GetCurrentProcess().WorkingSet64.ToString()));*/
                    }
                    catch (OutOfMemoryException oom)
                    {
                        mc.mvm.DumpMemory();
                        throw oom;
                    }
                    catch (Exception e)
                    {
                        // TBD if we want to do anything else here
                        throw e;
                    }
                }
            }

            // register the index
            try
            {
                mc.globalContext.SetNamedClassInst(this.indexName, index);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot define memory index [" +
                    this.indexName +
                    "] because this name is already in use: " +
                    e.Message);
            }
        }
    }
}
