using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
    class MObjectRefGet: IModuleSetup,IModuleRun
    {
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string targetSyntax;
        private IWriteString targetParsed;
        private string refCountSyntax;
        private IWriteString refCountParsed;
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectRefGet m = new MObjectRefGet();
            // xml extraction
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.refCountSyntax = me.SelectNodeInnerText("./ref_count");
            m.targetSyntax = me.SelectNodeInnerText("./target");
            // parsing
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            m.refCountParsed = mc.ParseWritableSyntax(m.refCountSyntax);
            m.targetParsed = mc.ParseWritableSyntax(m.targetSyntax);
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            var oid=this.objectIdParsed.Read(mc);
            using (IObjectData obj = mc.objectCache.CheckOut(oid))
            {
               obj.RefGet();
               var refCount= obj.RefCount;
               if(this.refCountParsed!=null) 
                   this.refCountParsed.Write(mc, refCount.ToString());
               if (this.targetParsed != null)
                   this.targetParsed.Write(mc, obj.objectId);
            }
        }
    }
}
