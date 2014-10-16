using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{
    [Module(@"
        <module_config>
            <name>bfs_flush_file</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='bfs_flush_file_type'/>
            </xsd>
            <doc>
                <category>BFS</category>
                <description>Flushes a BFS file</description>
            </doc>
        </module_config>
    ")]
    public class MBfsFlushFile : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string fileSyntax;
        private string resultSyntax;

        // from setup
        private IReadString fileParsed;
        private IWriteString resultParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBfsFlushFile m = new MBfsFlushFile();
            // xml extraction
            m.fileSyntax = m.SelectSingleNode(me, "./file");
            m.resultSyntax = m.SelectSingleNode(me, "./result");
            // parsing
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            m.resultParsed = mc.ParseWritableSyntax(m.resultSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file=this.fileParsed.Read(mc);
            bool result = mc.globalContext.bfs.FlushFile(file);
            this.resultParsed.Write(mc, result ? "1" : "0");
            
        }
    }
}
