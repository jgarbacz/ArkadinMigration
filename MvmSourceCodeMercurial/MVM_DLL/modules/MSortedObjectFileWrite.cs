using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
    <sorted_object_file_write>
      <file>TEMP.filename</file>
      <object_id>TEMP.csr</object_id>
      <in_order>'forward|reverse'</in_order>
      <key_field>'sort_key_field_name'</key_field>
    </sorted_object_file_write>
    */

    class MSortedObjectFileWrite : IModuleSetup, IModuleRun
    {
        // from xml
        private string fileSyntax;
        private string objectIdSyntax;
        private string orderSyntax;
        private List<string> keyFieldsSyntax = new List<string>();

        // from setup
        private IReadString fileParsed;
        private IReadString objectIdParsed;
        private IReadString orderParsed;
        private List<IReadString> keyFieldsParsed = new List<IReadString>();

        private SortOrder inOrder;
        private List<string> orderedKeyFields = new List<string>();

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSortedObjectFileWrite m = new MSortedObjectFileWrite();
            // xml extraction
            m.fileSyntax = me.SelectNodeInnerText("./file");
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id", "OBJECT.object_id");
            m.orderSyntax = me.SelectNodeInnerText("./in_order", "0");
            m.keyFieldsSyntax = me.SelectNodesInnerText("./key_field");

            // parsing
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.orderParsed = mc.ParseSyntax(m.orderSyntax);
            m.keyFieldsParsed = mc.ParseSyntax(m.keyFieldsSyntax);

            string in_order = m.orderParsed.Read(mc);
            m.inOrder = SortOrder.None;
            if (in_order.StartsWith("f"))
            {
                m.inOrder = SortOrder.Forward;
            }
            else if (in_order.StartsWith("r"))
            {
                m.inOrder = SortOrder.Reverse;
            }
            foreach (IReadString keyFieldParsed in m.keyFieldsParsed)
            {
                m.orderedKeyFields.Add(keyFieldParsed.Read(mc));
            }
            if (m.orderedKeyFields.Count > 1)
            {
                throw new Exception("MSortedObjectFileWrite doesn't support multiple keys!");
            }

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file = this.fileParsed.Read(mc);
            MergeSortReaderWriter<Text, IObjectData> w = mc.threadContext.GetMergeSortWriter(file);
            string objectId = this.objectIdParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(objectId))
            {
                w.Add(new KeyValuePair<Text, IObjectData>(new Text(obj[this.orderedKeyFields[0]]), obj), this.inOrder);
            }
        }
    }
}
