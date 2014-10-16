using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{
    [Module(@"
        <module_config>
            <name>bfs_file_exists</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='file' type='xs:string' datatype='string' mode='in' description='name of the file'/>
                        <xs:element name='exists' type='xs:string' datatype='boolean' mode='out' values='0:file does not exist,1:file exists'/>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>BFS</category>
                <description>Checks for the existence of a BFS file</description>
            </doc>
        </module_config>
    ")]
    public class MBfsFileExists : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string fileSyntax;
        private string existsSyntax;

        // from setup
        private IReadString fileParsed;
        private IWriteString existsParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBfsFileExists m = new MBfsFileExists();
            // xml extraction
            m.fileSyntax = m.SelectSingleNode(me, "./file");
            m.existsSyntax = m.SelectSingleNode(me, "./exists");
            // parsing
            m.fileParsed = mc.ParseSyntax(m.fileSyntax);
            m.existsParsed = mc.ParseWritableSyntax(m.existsSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string file=this.fileParsed.Read(mc);
            bool exists = mc.globalContext.bfs.FileExists(file);
            this.existsParsed.Write(mc, exists ? "1" : "0");
        }
    }
}
