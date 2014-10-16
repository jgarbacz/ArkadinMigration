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
    public class MCursorOrderBy : IModuleSetup, IModuleRun
    {
        private List<string> orderBysSyntax = new List<string>();
        private List<IReadString> orderBysParsed = new List<IReadString>();
        private List<string> orderBysDirection = new List<string>();
        private List<string> orderBysType = new List<string>();
        private int orderBysCount;
        private IComparer<object[]> comparer;
        private List<IReadString> cursorValuesParsed = new List<IReadString>();
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCursorOrderBy m = new MCursorOrderBy();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            foreach (XmlElement elem in me.SelectNodes("./order_by"))
            {
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
            IComparer<object>[] cmp = new IComparer<object>[m.orderBysCount];
            for (int i = 0; i < m.orderBysSyntax.Count; i++)
            {
                if (m.orderBysType[i].Equals("numeric") && m.orderBysDirection[i].Equals("asc")) cmp[i] = new MyComparers.ObjectDoubleCompareAsc();
                else if (m.orderBysType[i].Equals("numeric") && m.orderBysDirection[i].Equals("desc")) cmp[i] = new MyComparers.ObjectDoubleCompareDesc();
                else if (m.orderBysType[i].Equals("string") && m.orderBysDirection[i].Equals("asc")) cmp[i] = new MyComparers.ObjectStringCompareAsc();
                else if (m.orderBysType[i].Equals("string") && m.orderBysDirection[i].Equals("desc")) cmp[i] = new MyComparers.ObjectStringCompareDesc();
            }
            m.comparer = new MyComparers.ObjectArrayCompare(cmp);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            CursorOrderByCursor outputCursor = new CursorOrderByCursor(mc, this.cursorSetup, orderBysParsed, this.comparer);
           // mc.mvm.Log("created order by output cursor " + outputCursor.CursorInstId);
        }
    }
                }
