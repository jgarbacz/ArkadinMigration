using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 * <print_record> 
 * <file>"some_dir/ACCOUNTS."~threadId~".data"</file>
 * <field_delim>","</field_delim>
 * <record_delim>"notused"</record_delim>
 * <data>
 * <field>OBJECT.first</field>
 * <field>OBJECT.second * 22</field>
 * <data>
 * </print_record>
 */
namespace MVM
{
    class MPrintRecord : IModuleSetup, IModuleRun
    {
        private string fileNameSyntax;
        private string outFileNameSyntax;
        private string encodingSyntax;
        private string fieldDelimSyntax;
        private string recordDelimSyntax;
        private List<string> fieldsSyntax = new List<string>();
        private IReadString encodingParsed;
        private IReadString fileNameParsed;
        private IWriteString outFileNameParsed;
        private IReadString fieldDelimParsed;
        private IReadString recordDelimParsed;
        private List<IReadString> fieldsParsed = new List<IReadString>();
        private Encoding encoding;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MPrintRecord m = new MPrintRecord();
            // xml extraction
            m.fileNameSyntax = me.SelectNodeInnerText("./file");
            m.outFileNameSyntax = me.SelectNodeInnerText("./output_filename");
            m.encodingSyntax = me.SelectNodeInnerText("./encoding");
            m.fieldDelimSyntax = me.SelectNodeInnerText("./field_delim");
            m.recordDelimSyntax = me.SelectNodeInnerText("./record_delim");
            //Console.WriteLine("PRINT_RECORDx WITH RD=[" + m.recordDelimSyntax + "]");

            m.fieldsSyntax = me.SelectNodesInnerText("./data/field");
            // parsing
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            m.outFileNameParsed = mc.ParseWritableSyntax(m.outFileNameSyntax);
            m.encodingParsed = mc.ParseSyntax(m.encodingSyntax);
            m.fieldDelimParsed = mc.ParseSyntax(m.fieldDelimSyntax);
            m.recordDelimParsed = mc.ParseSyntax(m.recordDelimSyntax);
            m.fieldsParsed = mc.ParseSyntax(m.fieldsSyntax);

            m.encoding = System.Text.Encoding.UTF8;
            if (m.encodingParsed != null)
            {
                string encodingName = m.encodingParsed.Read(mc);
                m.encoding = System.Text.Encoding.GetEncoding(encodingName);
                if (m.encoding == null)
                {
                    throw new Exception("Cannot print record with unknown encoding [" + encodingName + "]");
                }
            }
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string fileName = this.fileNameParsed.Read(mc);
            string fieldDelim = this.fieldDelimParsed.Read(mc).InterpolateEscapes();
            string recordDelim = this.recordDelimParsed.Read(mc).InterpolateEscapes();

            //Console.WriteLine("PRINT_RECORD WITH RD=[" + recordDelim.InterpolateEscapesReverse() + "]");
            //Console.WriteLine("open:" + fileName);
            StreamWriter w = mc.threadContext.GetStreamWriter(fileName, this.encoding);
            if (this.fieldsParsed.Count > 0)
            {
                w.Write(this.fieldsParsed[0].Read(mc));
                for (int i = 1; i < this.fieldsParsed.Count; i++)
                {
                    w.Write(fieldDelim);
                    w.Write(this.fieldsParsed[i].Read(mc));
                }
            }
            if (outFileNameParsed != null)
            {
                outFileNameParsed.Write(mc, fileName);
            }
            w.Write(recordDelim);
            //w.FlushToFile();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("print:");
        }
    }
}
