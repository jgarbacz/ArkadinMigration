using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <extract_embedded_resource>
    <resource_name>TEMP.in</resource_name>
    <file_name>TEMP.out</file_name>
  </extract_embedded_resource>
      */
    class MExtractEmbeddedFile : IModuleSetup, IModuleRun
    {
        private string resourceNameSyntax;
        private string fileNameSyntax;
        private IReadString resourceNameParsed;
        private IReadString fileNameParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MExtractEmbeddedFile m = new MExtractEmbeddedFile();
            m.resourceNameSyntax = me.SelectNodeInnerText("./resource_name");
            m.fileNameSyntax = me.SelectNodeInnerText("./file_name");
            m.resourceNameParsed = mc.ParseSyntax(m.resourceNameSyntax);
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            //MyReflection.PrintEmbeddedResources();
            string resourceName = this.resourceNameParsed.Read(mc);
            string fileName = this.fileNameParsed.Read(mc);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetEmbeddedFile(resourceName);
            using (FileStream fileStream = new FileStream(fileName,FileMode.CreateNew))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
