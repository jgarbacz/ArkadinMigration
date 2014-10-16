using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     * Make a new cursor that has a clone of the data of the input cursor, but can't fire next()
  <sterilize_cursor>
    <input>TEMP.csr</input>
    <output>TEMP.sterile_csr</output>
  </sterilize_cursor>
      */

    class MSterilizeCursor: IModuleSetup,IModuleRun
    {
        // from xml
        private string inputSyntax;
        private string outputSyntax;
        
        // from setup
        private IReadString inputParsed;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSterilizeCursor m = new MSterilizeCursor();
            // xml extraction
            m.inputSyntax = me.SelectNodeInnerText("./input");
            m.outputSyntax = me.SelectNodeInnerText("./output");
            // parsing
            m.inputParsed = mc.ParseSyntax(m.inputSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            throw new Exception("not complete");
            //string inputCursorOid = this.inputParsed.Read(mc);
            //SterileCursor sterileCursor = new SterileCursor(mc, inputCursorOid);
            //this.outputParsed.Write(mc, sterileCursor.oid);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("sterilize_cursor:");
        }
    }
}
