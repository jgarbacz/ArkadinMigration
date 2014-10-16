using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using MSjogren.Samples.ShellLink;

namespace MVM
{
     [Module(@"
        <module_config>
            <name>create_shortcut</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:all>
                        <xs:element name='link' type='xs:string' datatype='string' mode='in' description='Name of shortcut with or without .lnk suffix'/>
                        <xs:element name='target' type='xs:string' datatype='string' mode='in' description='Full path to shortcut target'/>
                    </xs:all>
                </xs:complexType>
            </xsd>
            <doc>
                <category>File System</category>
                <description>Creates or replaces a windows shortcut</description>
            </doc>
        </module_config>
    ")]
    class MCreateShortcut : BaseModuleSetup, IModuleRun
    {
        // from xml
        private string linkSyntax;
        private string targetSyntax;

        // from setup
        private IReadString linkParsed;
        private IReadString targetParsed;


        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCreateShortcut m = new MCreateShortcut();
            m.SetupReadString(me, mc, "./link", out m.linkSyntax, out m.linkParsed);
            m.SetupReadString(me, mc, "./target", out m.targetSyntax, out m.targetParsed);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string linkValue = this.linkParsed.Read(mc);
            string targetValue = this.targetParsed.Read(mc);
            string shortcutPath = linkValue;
            if (!shortcutPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                shortcutPath = shortcutPath + ".lnk";
            }

            FileInfo shortcutFileInfo = new FileInfo(shortcutPath);
            shortcutFileInfo.Directory.CreateIfNotThere();

            //if (File.Exists(targetValue))
            //{
            //    File.Delete(targetValue);
            //}

            ShellShortcut sc = new ShellShortcut(shortcutPath);
            sc.Path = targetValue;
            sc.Save();
        }
    }
}
