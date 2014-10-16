using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using NLog;
namespace MVM
{
    /*
     * <select>
     *  <db_select>
     *  <query> select T_id from T WHERE ID=OBJECT.id</query>
     *  </db_select>
     *  -- allow db style ops in a forced order.
     *  
     *  -- pipeline allows the user to do whatever they want in the order they want
     *  <pipeline>
     *      <execute>
     *       <do>OBJECT.x=OBJECT.y+OBJECT.z</do>
     *      </execute>
     *      <where>OBJECT.x==OBJECT.y</where>
     *      <order_by>OBJECT.x</order_by>
     *      <order_by>OBJECT.y</order_by>
     *      <group_by>OBJECT.x</group_by>
     *      <select_fields>
     *          <field name='name'>OBJECT.x</field>
     *          <aggregate type='avg' name='myavg'>OBJECT.y</field>
     *      <select_fields>
     *   </pipeline>
     * <select>
     */
    public class MPipeline : IModuleSetup, IModuleRun
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public List<string> cursorInstIdsSyntax = new List<string>();
        public List<IReadString> cursorInstIdsParsed;

        public string cursorInstIdSyntax;
        public IReadString cursorInstIdParsed;


        public List<string> cursorOpInstIdsSyntax = new List<string>();
        public List<IReadString> cursorOpInstIdsParsed;


        private CursorSetupCommon cursorSetup;
       // private CursorSetupCommon unionAllCursorSetup;

        public static string[] IteratorNames = new string[] { "cursor", "cursor_inst_id" };

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // this module needs to be smart enough to know when you do not need a pipeline.
            // so it can automatically skip that bit. So selecting a cursor with nothing else
            // is the same as working right not that cursor.

            // custom dude says:
            // if i have any pipeline funcs, then generate a generic select.
            // otherwise just do my thing.

            // general dude says:
            // case 1: no union, no pipeline -> generate custom
            // case 2: union, no pipeline
            // case 3: no union, pipeline
            // case 4: union, pipeline

            // union is one of many things we are going to do...
            // 
            // union is simple a union_select
            // only key here is who owns final setup.
            //
            // So just need to keep track of who own the final setup.
            //

            // Go thru all the child elements and see what we have
            List<XmlElement> inputCursorElems = new List<XmlElement>();
            List<XmlElement> pipelineCursorElems = new List<XmlElement>();
            List<XmlElement> iteratorElems = new List<XmlElement>();
            foreach (var elem in me.GetChildElems())
            {
                string elemName = elem.LocalName;
                if (elemName.Equals("input_cursor_inst_id") || elemName.EndsWith("select"))
                {
                    inputCursorElems.Add(elem);
                }
                else if (elemName.In(CursorSetupCommon.pipelineFuncs))
                {
                    pipelineCursorElems.Add(elem);
                }
                else if (elemName.Equals("pipeline"))
                {
                    foreach (var plElem in elem.GetChildElems())
                    {
                        pipelineCursorElems.Add(plElem);
                    }
                }
                else if (elemName.In(IteratorNames) || elemName.In(CursorSetupCommon.cursorFuncs))
                {
                    iteratorElems.Add(elem);
                }
                else
                {
                    throw new Exception("unexpected element in select [" + elem.OuterXml + "]");
                }
            }

            bool usePipeline = pipelineCursorElems.Count > 0;
            bool useUnionAll = inputCursorElems.Count > 1;

            XmlElement inputCursorElem;

            // if we have multiple input cursors stuff them into a union all so we
            // can deal with a single input elem from here on.
            if (useUnionAll)
            {
                inputCursorElem = me.CreateElement("union_all_select");
                foreach (var elem in inputCursorElems)
                {
                    inputCursorElem.AppendChildElement(elem);
                }
            }
            else
            {
                inputCursorElem = inputCursorElems[0];
            }

            // if we do not have any pipeline functions, then stuff stuff the iterator
            // work into the input cursor
            if (!usePipeline)
            {
                foreach (var elem in iteratorElems)
                {
                    inputCursorElem.AppendChildElement(elem);
                }
                run.Add(mc.GetModuleRun(inputCursorElem));
                return;
            }

            // otherwise we need a pipeline.
            MPipeline m = new MPipeline();
            m.cursorSetup = new CursorSetupCommon(me, mc);

            // start by setting up the input cursor
            string inputElemName = inputCursorElem.LocalName;
            if (inputElemName.EndsWith("select"))
            {
                // add generated cursor instance id to the mix
                string cursorInst = "TEMP." + mc.GetGenSym(inputElemName + "_is_pipelined");
                inputCursorElem.AppendTextElement("cursor_inst_id", cursorInst);
                m.cursorInstIdSyntax=cursorInst;
                run.Add(mc.GetModuleRun(inputCursorElem));
            }
            else if (inputElemName.EndsWith("input_cursor_inst_id"))
            {
                // add the passed cursor instance id to the mix
                string cursorInst = inputCursorElem.InnerText;
                m.cursorInstIdSyntax = cursorInst;
            }
            else
            {
                throw new Exception("Unexpected");
            }
            m.cursorInstIdParsed = mc.ParseSyntax(m.cursorInstIdSyntax);


            // Now build up the cursor operators
            for (int i = 0; i < pipelineCursorElems.Count; i++)
            {
                XmlElement elem = pipelineCursorElems[i];
                string elemName = elem.LocalName;
                if (elemName.Equals("order_by"))
                {
                    // capture all the concecutive order by elements
                    XmlElement cursorOrderBy = elem.CreateElement("cursor_order_by");
                    cursorOrderBy.AppendChildElement(pipelineCursorElems[i]);
                    while ((i + 1) < pipelineCursorElems.Count && pipelineCursorElems[i + 1].LocalName.Equals("order_by"))
                    {
                        cursorOrderBy.AppendChildElement(pipelineCursorElems[++i]);
                    }
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_pipelined");
                    //logger.Info("Setting up orderby to use cursor_inst_id syntax =" + cursorInst);
                    cursorOrderBy.AppendTextElement("cursor_inst_id", cursorInst);

                    m.cursorOpInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(cursorOrderBy));
                }
                else if (elemName.Equals("where"))
                {
                    XmlElement cursorWhere = elem.CreateElement("cursor_where");
                    cursorWhere.AppendChildElement(elem);
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_pipelined");
                    //logger.Info("Setting up where to use cursor_inst_id syntax =" + cursorInst);
                    cursorWhere.AppendTextElement("cursor_inst_id", cursorInst);
                    m.cursorOpInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(cursorWhere));
                }
                else if (elemName.Equals("pipe_row"))
                {
                    XmlElement cursorPipeRow = elem.CreateElement("cursor_pipe_row");
                    cursorPipeRow.AppendChildElement(elem);
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_pipelined");
                    //logger.Info("Setting up where to use cursor_inst_id syntax =" + cursorInst);
                    cursorPipeRow.AppendTextElement("cursor_inst_id", cursorInst);
                    m.cursorOpInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(cursorPipeRow));
                }
                else if (elemName.Equals("top"))
                {
                    XmlElement cursorTop = elem.CreateElement("cursor_top");
                    cursorTop.AppendChildElement(elem);
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_pipelined");
                    //logger.Info("Setting up where to use cursor_inst_id syntax =" + cursorInst);
                    cursorTop.AppendTextElement("cursor_inst_id", cursorInst);
                    m.cursorOpInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(cursorTop));
                }
                else if (elemName.Equals("execute"))
                {
                    XmlElement cursorExecute = elem.CreateElement("cursor_execute");
                    cursorExecute.AppendChildElement(elem);
                    string cursorInst = "TEMP." + mc.GetGenSym(elemName + "_is_pipelined");
                    //logger.Info("Setting up where to use cursor_inst_id syntax =" + cursorInst);
                    cursorExecute.AppendTextElement("cursor_inst_id", cursorInst);
                    m.cursorOpInstIdsSyntax.Add(cursorInst);
                    run.Add(mc.GetModuleRun(cursorExecute));
                }
                else
                {
                    throw new Exception("unexpected tag in pipeline_select:" + elemName);
                }
            }

            // Parse the operator cursor inst ids
            m.cursorOpInstIdsParsed = mc.ParseSyntax(m.cursorOpInstIdsSyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            // lookup the input cursor
            string cursorInstId = cursorInstIdParsed.Read(mc);
            ICursorBase cursorBase;
            mc.LookupCursorViaInstId(cursorInstId, out cursorBase);
            ICursor cursor=cursorBase as ICursor;

            // lookup the cursor ops
            List<ICursorOp> cursorOps = new List<ICursorOp>();
            foreach (var idParsed in this.cursorOpInstIdsParsed)
            {
                string id = idParsed.Read(mc);
                ICursorOp cursorOp;
                mc.LookupCursorViaInstId(id, out cursorOp);
                cursorOps.Add(cursorOp);
            }

            // instanciate a new pipeline cursor
            CursorPipeline pipeline = new CursorPipeline(mc, cursorSetup, cursor, cursorOps);
        }
    }
}
