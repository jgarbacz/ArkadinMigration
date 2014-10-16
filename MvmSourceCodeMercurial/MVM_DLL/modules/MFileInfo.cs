using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
    <file_info>
      <filename>TEMP.filename</filename>
      <basename>TEMP.basename</basename>
      <size>TEMP.size</size>
      <readonly>TEMP.readonly</readonly>
      <extension>TEMP.extension</extension>
      <ctime>TEMP.ctime</ctime>
      <atime>TEMP.atime</atime>
      <wtime>TEMP.wtime</wtime>
    </file_info>
    */

    class MFileInfo : IModuleSetup, IModuleRun
    {
        // from xml
        private string filenameSyntax;
        private string basenameSyntax;
        private string sizeSyntax;
        private string readonlySyntax;
        private string extensionSyntax;
        private string ctimeSyntax;
        private string atimeSyntax;
        private string wtimeSyntax;

        // from setup
        private IReadString filenameParsed;
        private IWriteString basenameParsed;
        private IWriteString sizeParsed;
        private IWriteString readonlyParsed;
        private IWriteString extensionParsed;
        private IWriteString ctimeParsed;
        private IWriteString atimeParsed;
        private IWriteString wtimeParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileInfo m = new MFileInfo();
            // xml extraction
            m.filenameSyntax = me.SelectNodeInnerText("./filename");
            m.basenameSyntax = me.SelectNodeInnerText("./basename");
            m.sizeSyntax = me.SelectNodeInnerText("./size");
            m.readonlySyntax = me.SelectNodeInnerText("./readonly");
            m.extensionSyntax = me.SelectNodeInnerText("./extension");
            m.ctimeSyntax = me.SelectNodeInnerText("./ctime");
            m.atimeSyntax = me.SelectNodeInnerText("./atime");
            m.wtimeSyntax = me.SelectNodeInnerText("./wtime");
            // parsing
            m.filenameParsed = mc.ParseSyntax(m.filenameSyntax);
            m.basenameParsed = mc.ParseWritableSyntax(m.basenameSyntax);
            m.sizeParsed = mc.ParseWritableSyntax(m.sizeSyntax);
            m.readonlyParsed = mc.ParseWritableSyntax(m.readonlySyntax);
            m.extensionParsed = mc.ParseWritableSyntax(m.extensionSyntax);
            m.ctimeParsed = mc.ParseWritableSyntax(m.ctimeSyntax);
            m.atimeParsed = mc.ParseWritableSyntax(m.atimeSyntax);
            m.wtimeParsed = mc.ParseWritableSyntax(m.wtimeSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string filename = this.filenameParsed.Read(mc);
            try
            {
                FileInfo file = new FileInfo(filename);
                if (basenameParsed != null)
                {
                    basenameParsed.Write(mc, file.Name);
                }
                if (sizeParsed != null)
                {
                    sizeParsed.Write(mc, file.Length.ToString());
                }
                if (readonlyParsed != null)
                {
                    readonlyParsed.Write(mc, file.IsReadOnly ? "1" : "0");
                }
                if (extensionParsed != null)
                {
                    extensionParsed.Write(mc, file.Extension);
                }
                if (ctimeParsed != null)
                {
                    ctimeParsed.Write(mc, file.CreationTime.ToString());
                }
                if (atimeParsed != null)
                {
                    atimeParsed.Write(mc, file.LastAccessTime.ToString());
                }
                if (wtimeParsed != null)
                {
                    wtimeParsed.Write(mc, file.LastWriteTime.ToString());
                }
            }
            catch 
            {
                ;   // Don't return any values if the file isn't found
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("file_info:");
        }
    }
}
