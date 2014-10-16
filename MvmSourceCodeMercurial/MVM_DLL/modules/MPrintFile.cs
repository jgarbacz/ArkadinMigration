using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 * <print_file> 
 * <file>"some_dir/ACCOUNTS."~threadId~".data"</file>
 * <format fixed_width='true'>
 *      <field name='first'>OBJECT.first</field>
 *      <field name='second'>OBJECT.second * 22</field>
 * <format>
 * </print_file>
 */
namespace MVM
{
    class MPrintFile : IModuleSetup, IModuleRun
    {
        private string fileNameSyntax;
        private IReadString fileNameParsed;
        private XmlSpecifiedFormat xmlSpecifiedFormat;
        private IFormatWriter formatWriter;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MPrintFile m = new MPrintFile();
            m.fileNameSyntax = me.SelectNodeInnerText("./file");
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            m.xmlSpecifiedFormat = new XmlSpecifiedFormat(me.SelectSingleElem("./format"));
            m.formatWriter = m.xmlSpecifiedFormat.GetFormatWriter(mc);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string fileName = this.fileNameParsed.Read(mc);
            StreamWriter w = mc.threadContext.GetStreamWriter(fileName);
            this.formatWriter.Write(mc, w);
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("print:" );
        }
    }
}
