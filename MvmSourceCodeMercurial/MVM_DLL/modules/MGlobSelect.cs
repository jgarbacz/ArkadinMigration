using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
    <glob_select>
    <glob>'./*.txt'</glob>
    <cursor>TEMP.csr</cursor>
    <cursor_value>'glob'</cursor_value>
    <loop>
        <print>'OBJECT(TEMP.csr).glob='~OBJECT(TEMP.csr).glob</print>
    </loop>
    </glob_select>
   */

    class MGlobSelect: IModuleSetup,IModuleRun
    {
        private string globSyntax;
        //private string cursorGlobSyntax;
        private IReadString globParsed;
        //private IReadString cursorGlobParsed;
        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGlobSelect m = new MGlobSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.globSyntax = me.SelectNodeInnerText("./glob","'./*'");
            m.globParsed = mc.ParseSyntax(m.globSyntax);
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            //string cursorGlob=this.cursorGlobParsed.Read(mc);
            string glob = this.globParsed.Read(mc);
            List<string> stringList = FileUtils2.GlobToList(glob);
            var csr = new SingleFieldListCursor(mc, this.cursorSetup, stringList);
        }

    }

 

   
}
