using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectRefRemove: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string refCountSyntax;
        private IWriteString refCountParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectRefRemove m = new MObjectRefRemove();
            // xml extraction
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.refCountSyntax = me.SelectNodeInnerText("./ref_count");
            // parsing
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.refCountParsed = mc.ParseWritableSyntax(m.refCountSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            var oid=this.objectIdParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(oid))
            {
                var refCount = obj.RefRemove();
               if(this.refCountParsed!=null) 
                   this.refCountParsed.Write(mc, refCount.ToString());
            }
        }
    }
}
