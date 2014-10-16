using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
  <get_mvm_generated_dir>
    <namespace>'defaults_to_proc_namespace'</namespace>
    <label>'blah'</label>
    <directory>GLOBAL.my_gen_dir</directory>
  </get_mvm_generated_dir>
      */

    class MGetMvmGeneratedDir : IModuleSetup, IModuleRun
    {
        private string nameSpaceSyntax;
        private IReadString nameSpaceParsed;
        private string labelSyntax;
        private IReadString labelParsed;
        private string directorySyntax;
        private IWriteString directoryParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetMvmGeneratedDir m = new MGetMvmGeneratedDir();
            m.nameSpaceSyntax = me.SelectNodeInnerText("./namespace",mc.procDefinition.nameSpace.q());
            m.nameSpaceParsed = mc.ParseSyntax(m.nameSpaceSyntax);
            m.labelSyntax = me.SelectNodeInnerText("./label");
            m.labelParsed = mc.ParseSyntax(m.labelSyntax);
            m.directorySyntax = me.SelectNodeInnerText("./directory");
            m.directoryParsed = mc.ParseWritableSyntax(m.directorySyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string nameSpace = this.nameSpaceParsed.Read(mc);
            string label = this.labelParsed.Read(mc);
         
            this.directoryParsed.Write(mc, mc.schedulerMaster.GetGeneratedDir(nameSpace,label));
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_mvm_generated_dir:");
        }
    }
}
