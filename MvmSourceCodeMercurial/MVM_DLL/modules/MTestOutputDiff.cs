using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
namespace MVM
{
    [Module(@"
        <module_config>
            <name>test_output_diff</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:all>
                        <xs:element name='proc_name' type='xs:string' datatype='string' mode='in' description='name of proc'/>
                        <xs:element name='exception_name' type='xs:string' datatype='string' mode='out' description='name of exception'/>
                        <xs:element name='exception_message' type='xs:string' datatype='string' mode='out' description='message of exception'/>
                        <xs:element name='exception_trace' type='xs:string' datatype='string' mode='out' description='trace of exception'/>
                    </xs:all>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Unit Testing</category>
                <description>Diffs current and blessed unit test output</description>
            </doc>
        </module_config>
    ")]
    class MTestOutputDiff: BaseModuleSetup,IModuleRun
    {
        private string procNameSyntax;
        private IReadString procNameParsed;
        private string exceptionNameSyntax;
        private IWriteString exceptionNameParsed;
        private string exceptionMessageSyntax;
        private IWriteString exceptionMessageParsed;
        private string exceptionTraceSyntax;
        private IWriteString exceptionTraceParsed;
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MTestOutputDiff m = new MTestOutputDiff();
            m.SetupReadString(me, mc, "./proc_name", out m.procNameSyntax, out m.procNameParsed);
            m.SetupWriteString(me, mc, "./exception_name", out m.exceptionNameSyntax, out m.exceptionNameParsed);
            m.SetupWriteString(me, mc, "./exception_message", out m.exceptionMessageSyntax, out m.exceptionMessageParsed);
            m.SetupWriteString(me, mc, "./exception_trace", out m.exceptionTraceSyntax, out m.exceptionTraceParsed);
            run.Add(m);
        }

        /// <summary>
        /// Returns the file name for the passed proc_name
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="proc_name"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("get_proc_file_name")]
        public static string GetProcFileName(ModuleContext mc,string proc_name)
        {
            ProcInfo procInfo = mc.GetProcInfo(proc_name);
            string procFileName = new FileInfo(procInfo.location.GetLocation()).FullName;
            return procFileName;
        }

        /// <summary>
        /// Returns a proc's test directory
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="proc_name"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("get_proc_test_dir")]
        public static string GetProcTestDir(ModuleContext mc, string proc_name)
        {
            ProcInfo procInfo = mc.GetProcInfo(proc_name);
            string procDir=new FileInfo(procInfo.location.GetLocation()).Directory.FullName;
            DirectoryInfo procTestDir = new DirectoryInfo(Path.Combine(procDir,proc_name));
            //procTestDir.CreateIfNotThere();
            return procTestDir.FullName;
        }

        /// <summary>
        /// Returns a proc's current test directory
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="proc_name"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("get_proc_test_current_dir")]
        public static string GetProcTestCurrentDir(ModuleContext mc, string proc_name)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(GetProcTestDir(mc,proc_name),"current"));
            //dir.CreateIfNotThere();
            return dir.FullName;
        }

        /// <summary>
        /// Returns a proc's blessed test directory
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="proc_name"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("get_proc_test_blessed_dir")]
        public static string GetProcTestBlessedDir(ModuleContext mc, string proc_name)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(GetProcTestDir(mc, proc_name), "blessed"));
            //dir.CreateIfNotThere();
            return dir.FullName;
        }

        /// <summary>
        /// This function returns the test output file
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="proc_name"></param>
        /// <returns></returns>
        /// <category>Unit Testing</category>
        [MvmExport("get_proc_test_out_file")]
        public static string GetProcTestOutputFile(ModuleContext mc, string proc_name)
        {
            return Path.Combine(GetProcTestCurrentDir(mc, proc_name), "output.txt");
        }

        public void Run(ModuleContext mc)
        {

            // flush any files to disk so we can diff them
            mc.threadContext.DisposeStreamWriters();

            string procName = this.procNameParsed.Read(mc);
            ProcInfo procInfo = mc.GetProcInfo(procName);
            string procFileNameDir = GetProcTestDir(mc,procName);

            DirectoryInfo currentDir = new DirectoryInfo(GetProcTestCurrentDir(mc, procName));
            DirectoryInfo blessedDir = new DirectoryInfo(GetProcTestBlessedDir(mc, procName));

            List<string> currentFiles = currentDir.GetFiles().Select(fi=>fi.Name).Where(f=>!f.EqualsIgnoreCase("log.txt")&&f.EndsWith(".txt")).OrderBy(f=>f).ToList();
            List<string> blessedFiles = blessedDir.GetFiles().Select(fi => fi.Name).Where(f => !f.EqualsIgnoreCase("log.txt") && f.EndsWith(".txt")).OrderBy(f => f).ToList(); 

            if (blessedFiles.Count() > 0)
            {
                if (!currentFiles.IsEqualTo(blessedFiles))
                {
                    var extraOutput = currentFiles.Except(blessedFiles).ToList();
                    var missingOutput = blessedFiles.Except(currentFiles).ToList();
                    if (extraOutput.Count > 0 && missingOutput.Count == 0)
                    {
                        this.exceptionNameParsed.Write(mc, "nunit_fail");
                        this.exceptionMessageParsed.Write(mc, "Error, current output has extra file(s):" + extraOutput.JoinStrings(","));
                    }
                    else if (extraOutput.Count == 0 && missingOutput.Count > 0)
                    {
                        this.exceptionNameParsed.Write(mc, "nunit_fail");
                        this.exceptionMessageParsed.Write(mc, "Error, current output is missing blessed file(s):" + missingOutput.JoinStrings(","));
                    }
                    else if (extraOutput.Count > 0 && missingOutput.Count > 0)
                    {
                        this.exceptionNameParsed.Write(mc, "nunit_fail");
                        this.exceptionMessageParsed.Write(mc, "Error, current output current output has extra file(s):" + extraOutput.JoinStrings(",") + " and is missing blessed file(s):" + missingOutput.JoinStrings(","));
                    }
                    else
                    {
                        throw new Exception("unexpected diff exception");
                    }
                }
                else
                {
                    // files line up need to diff the contents one by one.
                    foreach (string fileName in blessedFiles)
                    {
                        FileInfo blessedFile = new FileInfo(Path.Combine(blessedDir.FullName, fileName));
                        FileInfo currentFile = new FileInfo(Path.Combine(currentDir.FullName, fileName));
                        List<FileInfo> diffs = new List<FileInfo>();
                        if(!FileUtils.FilesAreEqual(blessedFile,currentFile)){
                            diffs.Add(currentFile);
                        }
                        if (diffs.Count > 0)
                        {
                            this.exceptionNameParsed.Write(mc, "nunit_fail");
                            this.exceptionMessageParsed.Write(mc, "Error, these output file(s) differ from blessed: " + diffs.Select(fi=>fi.Name).JoinStrings(","));
                        }
                        else
                        {
                            // otherwise this is a pass
                            this.exceptionNameParsed.Write(mc, "nunit_pass");
                            this.exceptionMessageParsed.Write(mc, "all output matches!");
                    
                        }
                    }
                }
            }
            else if (blessedFiles.Count() == 0 && currentFiles.Count() > 0)
            {
                // if there is current output but no blessed output at all, then the test is inconclusive.
                this.exceptionNameParsed.Write(mc, "nunit_inconclusive");
                this.exceptionMessageParsed.Write(mc, "There is current test output in dir [" + currentDir.FullName + "], but no blessed output in dir [" + blessedDir.FullName + "]");
            }
            else
            {
                // no current output and not blessed output so do nothing.
            }
        }
    }
}
