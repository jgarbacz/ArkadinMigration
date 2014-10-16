using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>index_drop</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:all>
                        <xs:element name='index' type='xs:string' datatype='string' mode='in' description='name of the index'/>
                    </xs:all>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Index Access</category>
                <description>Drops an index and releases any resources it is holding</description>
            </doc>
        </module_config>
    ")]
    class MIndexDrop : BaseModuleSetup, IModuleRun
    {
        private string indexNameSyntax;
        private string indexName;
        private IIndex index;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MIndexDrop m = new MIndexDrop();
            m.SetupReadStringLiteral(me, mc, "./index", out m.indexNameSyntax, out m.indexName);
            m.index = (IIndex)mc.globalContext.GetNamedClassInst(m.indexName);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            this.index.IndexDrop(mc);
        }
    }
}
