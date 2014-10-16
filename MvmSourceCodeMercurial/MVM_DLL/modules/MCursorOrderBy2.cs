using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
namespace MVM
{
    /*
     * <cursor_order_by>
     * <cursor_inst_id><cursor_inst_id>
     * <cursor_order_by>
     */
    public class MCursorOrderBy2 : IModuleSetup, IModuleRun
    {
        private List<string> orderBysSyntax = new List<string>();
        private List<IReadString> orderBysParsed = new List<IReadString>();
        private List<string> orderBysDirection = new List<string>();
        private List<string> orderBysType = new List<string>();
        private int orderBysCount;
        private List<IReadString> cursorValuesParsed = new List<IReadString>();
        private CursorSetupCommon cursorSetup;

        public IMergeableComparer<DynamicKey> dynamicKeyComparer;
        public ISerializer<DynamicKey> dynamicKeySerializer;
        public IDynamicParser[] keyParsers;
        public IReadString cursorOrderbyMaxFiles;
        public IReadString cursorOrderbyMaxObjects;
        public IReadString cursorOrderbyTempDir;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorOrderBy2 m = new MCursorOrderBy2();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            foreach (XmlElement elem in me.SelectNodes("./order_by"))
            {
                if (elem.HasAttribute("max_files"))
                {
                    m.cursorOrderbyMaxFiles = mc.ParseSyntax(elem.GetAttribute("max_files"));
                }
                if (elem.HasAttribute("max_objects"))
                {
                    m.cursorOrderbyMaxObjects = mc.ParseSyntax(elem.GetAttribute("max_objects"));
                }
                if (elem.HasAttribute("temp_dir"))
                {
                    m.cursorOrderbyTempDir = mc.ParseSyntax(elem.GetAttribute("temp_dir"));
                }
                string syntax = elem.InnerText;
                string direction = elem.GetAttributeDefault("direction", "asc");
                string type = elem.GetAttributeDefault("type", "string");
                m.orderBysSyntax.Add(syntax);
                m.orderBysParsed.Add(mc.ParseSyntax(syntax));
                m.orderBysType.Add(type);
                m.orderBysDirection.Add(direction);
            }
            // build up the comparers for each of the order by fields and then stuff them
            // in a comparer that looks at each.
            m.orderBysCount = m.orderBysSyntax.Count;

            IDynamicMergeableComparer[] keyComparers = new IDynamicMergeableComparer[m.orderBysCount];
            IDynamicSerializer[] keySerializers = new IDynamicSerializer[m.orderBysCount];
            m.keyParsers = new IDynamicParser[m.orderBysCount];
            for (int i = 0; i < m.orderBysSyntax.Count; i++)
            {
                // setup the key comparer type
                string orderbyType=m.orderBysType[i];
                if (orderbyType.Equals("numeric")){
                    keyComparers[i] = DynamicMergeableComparer<Number>.Default;
                    keySerializers[i] = DynamicSerializer<Number>.Default;
                    m.keyParsers[i] = DynamicParser<Number>.Default;
                }
                else if (orderbyType.Equals("string")){
                    keyComparers[i] = DynamicMergeableComparer<Text>.Default;
                    keySerializers[i] = DynamicSerializer<Text>.Default;
                    m.keyParsers[i] = DynamicParser<Text>.Default;
                }
                else{
                    throw new Exception("unexpected order by type ["+orderbyType+"]");
                }

                // alter the direction if needed
                string orderbyDirection=m.orderBysDirection[i];
                if (orderbyDirection.Equals("desc")){
                    keyComparers[i] = new DynamicDescMergableComparer(keyComparers[i]);
                }

            }
            m.dynamicKeyComparer = new DynamicKeyComparer(keyComparers);
            m.dynamicKeySerializer= new DynamicKeySerializer(keySerializers);

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            int maxFiles = this.cursorOrderbyMaxFiles!=null?this.cursorOrderbyMaxFiles.Read(mc).ToInt():100;
            int maxObjects = this.cursorOrderbyMaxObjects != null ? this.cursorOrderbyMaxObjects.Read(mc).ToInt() : 10000;
            string tempDir = this.cursorOrderbyTempDir != null ? this.cursorOrderbyTempDir.Read(mc) :  System.IO.Path.GetTempPath();
            CursorOrderByCursor2 outputCursor 
                = new CursorOrderByCursor2(
                    mc, 
                    this.cursorSetup, 
                    orderBysParsed,
                    maxFiles,
                    maxObjects,
                    tempDir,
                    this.dynamicKeyComparer,
                    this.dynamicKeySerializer,
                    this.keyParsers
                    );
        }
    }
}
