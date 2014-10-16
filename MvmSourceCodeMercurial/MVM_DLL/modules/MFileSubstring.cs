using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 * <file_substring> 
 * <file>"some_dir/file.data"</file>
 * <offset>42</offset>
 * <length>9999</length>
 * <output>TEMP.output</output>
 * </file_substring>
 */
namespace MVM
{
    class MFileSubstring : IModuleSetup, IModuleRun
    {
        private string fileNameSyntax;
        private IReadString fileNameParsed;
        private string offsetSyntax;
        private IReadString offsetParsed;
        private string lengthSyntax;
        private IReadString lengthParsed;
        private string outputSyntax;
        private IWriteString outputParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileSubstring m = new MFileSubstring();
            m.fileNameSyntax = me.SelectNodeInnerText("./file");
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            m.offsetSyntax = me.SelectNodeInnerText("./offset");
            m.offsetParsed = mc.ParseSyntax(m.offsetSyntax);
            m.lengthSyntax = me.SelectNodeInnerText("./length");
            m.lengthParsed = mc.ParseSyntax(m.lengthSyntax);
            m.outputSyntax = me.SelectNodeInnerText("./output");
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string fileName = this.fileNameParsed.Read(mc);
            StreamReader r = mc.threadContext.GetStreamReader(fileName);
            string offset = this.offsetParsed.Read(mc);
            string length = this.lengthParsed.Read(mc);
            Byte[] mybytes = new Byte[length.ToInt()];
            r.BaseStream.Seek(offset.ToInt(), SeekOrigin.Begin);
            r.BaseStream.Read(mybytes, 0, length.ToInt());
            this.outputParsed.Write(mc, System.Text.UTF8Encoding.UTF8.GetString(mybytes));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_substring:" );
        }
    }
}
