using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MIndexClose : IModuleSetup, IModuleRun
    {
        private string indexNameSyntax;
        private string indexName;
        private IIndex globalIndex;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MIndexClose m = new MIndexClose();

            // xml extraction
            m.indexNameSyntax = me.SelectNodeInnerText("./index");

            // For now, only support static index names
            if (!mc.IsLiteralString(m.indexNameSyntax)) throw new Exception("Error, expecting string literal for index name, not [" + m.indexNameSyntax + "]");
            m.indexName = mc.GetLiteralString(m.indexNameSyntax);

            // Since our index is static, lookup and sort out the format now.
            m.globalIndex = (IIndex)mc.globalContext.GetNamedClassInst(m.indexName);

            // Add the runtime select module
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            try
            {
                this.globalIndex.IndexClose(mc);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot index_close index [" + this.globalIndex.IndexName + "]:" + e.Message, e);
            }
        }

    }
}
