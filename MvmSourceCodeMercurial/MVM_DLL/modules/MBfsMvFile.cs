using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{
    [Module(@"
        <module_config>
            <name>bfs_mv_file</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:element type='multi_source_target_type'/>
            </xsd>
            <doc>
                <category>BFS</category>
                <description>Moves a BFS file</description>
            </doc>
        </module_config>
    ")]
    public class MBfsMvFile : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string sourceSyntax;
        private string targetSyntax;

        // from setup
        private IReadString sourceParsed;
        private IReadString targetParsed;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MBfsMvFile m = new MBfsMvFile();
            // xml extraction
            m.sourceSyntax = m.SelectSingleNode(me, "./source");
            m.targetSyntax = m.SelectSingleNode(me, "./target");
            // parsing
            m.sourceParsed = mc.ParseSyntax(m.sourceSyntax);
            m.targetParsed = mc.ParseSyntax(m.targetSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string sourceFile=this.sourceParsed.Read(mc);
            string targetFile = this.targetParsed.Read(mc);
            mc.globalContext.bfs.FlushFile(sourceFile);
            FileInfo sourceFileInfo = new FileInfo(sourceFile);
            FileInfo tgt = new FileInfo(targetFile);
            if (!tgt.Directory.Exists) tgt.Directory.Create();
            try
            {
                sourceFileInfo.MoveTo(targetFile);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot do bfs_mv_file " + sourceFile + " to " + targetFile+":"+e.Message);
            }
        }
    }
}
