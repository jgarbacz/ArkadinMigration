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
    <sorted_object_file_close>
      <file>TEMP.filename</file>
      <flush>0|1</flush>
    </sorted_object_file_close>
    */

    class MSortedObjectFileClose : IModuleSetup, IModuleRun
    {
        // from xml
        private string fileSyntax;
        private string flushSyntax;

        // from setup
        private IReadString fileParsed;
        private IReadString flushParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSortedObjectFileClose m = new MSortedObjectFileClose();
            // xml extraction
            m.fileSyntax = me.SelectNodeInnerText("./file");
            m.flushSyntax = me.SelectNodeInnerText("./flush", "1");

            // parsing
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            m.flushParsed = mc.ParseSyntax(m.flushSyntax);

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file = this.fileParsed.Read(mc);
            mc.threadContext.CloseMergeSortWriter(file, this.flushParsed.Read(mc).Equals("1"));
        }
    }
}
