using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{

    [Module(@"
        <module_config>
            <name>test_output_clear</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:all>
                        <xs:element name='proc_name' type='xs:string' datatype='string' mode='in' description='name of proc'/>
                    </xs:all>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Clears the output for a unit test</description>
            </doc>
        </module_config>
    ")]
    class MTestOutputClear: BaseModuleSetup,IModuleRun
    {
        private string procNameSyntax;
        private IReadString procNameParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestOutputClear m = new MTestOutputClear();
            m.SetupReadString(me, mc, "./proc_name", out m.procNameSyntax, out m.procNameParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string procName = this.procNameParsed.Read(mc);
            ProcInfo procInfo = mc.GetProcInfo(procName);
            string procFileNameDir = new FileInfo(procInfo.location.GetLocation()).FullName.Replace(".xml", "");

            
            DirectoryInfo currentDir = new DirectoryInfo(MTestOutputDiff.GetProcTestCurrentDir(mc, procName));
            DirectoryInfo blessedDir = new DirectoryInfo(MTestOutputDiff.GetProcTestBlessedDir(mc, procName));
            blessedDir.CreateIfNotThere();

            // TestProcFullFilePathStripDotXml\\current\output.txt
            FileInfo currentFile = new FileInfo(Path.Combine(currentDir.FullName,"output.txt"));
            if (currentFile.Directory.Exists)
            {
                mc.mvm.Log("log_test_output", "[TEST_OUTPUT] clearing output dir: " + currentFile.Directory.ToString());
                //currentFile.Directory.Delete(true);
                foreach (var f in currentFile.Directory.GetFiles("*.txt", SearchOption.AllDirectories))
                {
                    f.Delete();
                }
            }
            else
            {
                mc.mvm.Log("log_test_output", "[TEST_OUTPUT] creating output dir: " + currentFile.Directory.ToString());
                currentFile.Directory.CreateIfNotThere();
            }

        }
    }
}
