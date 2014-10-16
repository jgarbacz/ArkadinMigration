using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
/*

<create_memory_index_from_cursor>
<index>'MY_INDEX'</index>
<cursor>TEMP.csr</cursor>
<key_field>'field_1'</key_field>
<key_field>'field_2'</key_field>
</create_memory_index_from_cursor>

*/
namespace MVM
{
    public class MCreateMemoryIndexFromCursor: IModuleSetup,IModuleRun
    {
        // from xml
        private string indexSyntax;
        private string cursorSyntax;
        private List<string> keyFieldsSyntax = new List<string>();
        // from setup
        private string indexName;
        private IReadString cursorParsed;
        private IWriteString cursorParsedWrite;
        private List<IReadString> keyFieldsParsed = new List<IReadString>();

        private string defaultCursorOrderString;
        private CursorOrder defaultCursorOrder = CursorOrder.Forward;

        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCreateMemoryIndexFromCursor m = new MCreateMemoryIndexFromCursor();
            // xml extraction
            m.indexSyntax = me.SelectNodeInnerText("./index");
            m.cursorSyntax = me.SelectNodeInnerText("./cursor");
            m.keyFieldsSyntax = me.SelectNodesInnerText("./key_field");

            // parsing
            if (!mc.IsLiteralString(m.indexSyntax)) throw new Exception("Error, expecting the index name to be a literal string: ["+m.indexSyntax+"]");
            m.indexName = mc.GetLiteralString(m.indexSyntax);
            m.cursorParsed = mc.ParseSyntax(m.cursorSyntax);
            m.cursorParsedWrite = mc.ParseWritableSyntax(m.cursorSyntax);
            m.keyFieldsParsed = mc.ParseSyntax(m.keyFieldsSyntax);

            // cursor newOrder
            m.defaultCursorOrderString = mc.SyntaxReadString(me.SelectNodeInnerText("./cursor_order", "'forward'"));
            if (m.defaultCursorOrderString.Equals("reverse")) m.defaultCursorOrder = CursorOrder.Reverse;
            else if (m.defaultCursorOrderString.Equals("forward")) m.defaultCursorOrder = CursorOrder.Forward;
            else throw new Exception("Unexpected default_cursor_order [" + m.defaultCursorOrderString + "]");

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string cursorOid = this.cursorParsed.Read(mc);
            List<string> orderedKeyFields = new List<string>();
            foreach (IReadString keyFieldParsed in this.keyFieldsParsed) orderedKeyFields.Add(keyFieldParsed.Read(mc));

            
            // If the cursor does not exist, the still define the structure but do so 
            // lookup the cursor and get the cursor fields
            ICursor cursor=null;
            MemoryIndexSync index=null;
            if (!mc.LookupCursorViaOid(cursorOid, out cursor) || cursor.Eof)
            {
                // create the index with only key fields since we do not know any value fields.
                List<string> orderedFieldNames = orderedKeyFields;
                index = new MemoryIndexSync(mc.mvm,this.indexName,orderedFieldNames, orderedKeyFields, defaultCursorOrder, true, false, new List<int>());
            }
            else
            {
                // make sure the passed cursor is Ienumerable
                IEnumerable<IObjectData> linqCursor = cursor as IEnumerable<IObjectData>;
                if (linqCursor == null)
                    throw new Exception("Error cannot create memory index [" + this.indexName + "] from cursors that do not support IEnumerable<IObjectData>");

                // get the fields
                List<string> orderedFieldNames=null;
           
                // load it w/ the values from the cursor
              
                foreach (var csrObj in linqCursor)
                {
                    List<string> orderedFieldValues = new List<string>();
                    if (orderedFieldNames == null)
                    {
                        orderedFieldNames = csrObj.SelectExternalFields().Select(kv=>kv.Key).ToList();
                        index = new MemoryIndexSync(mc.mvm,this.indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, true, false, new List<int>());
                    }
                    for(int i=0;i<orderedFieldNames.Count;i++)
                    {
                        string fieldName=orderedFieldNames[i];
                        orderedFieldValues.Add(csrObj[fieldName]);
                    }
                    //mc.mvm.Log("IIIIIIIIIIIIIIIIINSERT:"+orderedFieldValues.JoinStrings("|"));
                    index.IndexInsert(mc,orderedFieldValues);
                }
                //mc.mvm.Log("KEY COUNT=" + index.index.Keys.Count);

                // if the index is still null, create an index using just the specified keys
                if (index == null)
                {
                    orderedFieldNames = orderedKeyFields;
                    index = new MemoryIndexSync(mc.mvm,this.indexName, orderedFieldNames, orderedKeyFields, defaultCursorOrder, true, false, new List<int>());
                }
                this.cursorParsedWrite.Write(mc, cursor.CursorOid);
            }

            // register the index
            try
            {
                mc.globalContext.SetNamedClassInst(this.indexName, index);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot create memory index [" + 
                    this.indexName + 
                    "] from cursor because this name is already in use: " +
                    e.Message);
            }
        }
    }
    }
