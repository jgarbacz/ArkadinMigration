using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MCursorInstCreateEofIfNone: IModuleSetup,IModuleRun
    {

        private string valueSyntax;
        private IReadString valueParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MCursorInstCreateEofIfNone m = new MCursorInstCreateEofIfNone();
            m.valueSyntax = me.InnerText;
            m.valueParsed = mc.ParseSyntax(m.valueSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string cursorInstId = this.valueParsed.Read(mc);
            PipeCursor cursor = mc.globalContext.GetNamedClassInst(cursorInstId) as PipeCursor;
            cursor.Eof = true;
            //cursor.CreateEofIfNone();
            
        }
    }
}
