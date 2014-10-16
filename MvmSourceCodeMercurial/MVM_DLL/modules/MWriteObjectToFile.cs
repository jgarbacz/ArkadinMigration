using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

/*
 * copy object to file
 * 
 * object_file_select... (current cursors assume the object id). 
 * 
 * 
 */
namespace MVM
{
    class MWriteObjectToFile : IModuleSetup, IModuleRun
    {
        private string fileNameSyntax;
        private IReadString fileNameParsed;
        private string objectIdSyntax;
        private IReadString objectIdParsed;
        private string isTemporarySyntax;
        private IReadString isTemporaryParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MWriteObjectToFile m = new MWriteObjectToFile();
            m.fileNameSyntax = me.SelectNodeInnerText("./file");
            m.fileNameParsed = mc.ParseSyntax(m.fileNameSyntax);
            m.isTemporarySyntax = me.SelectNodeInnerText("./is_temporary","0");
            m.isTemporaryParsed = mc.ParseSyntax(m.isTemporarySyntax);
            m.objectIdSyntax = me.SelectNodeInnerText("./object_id");
            m.objectIdParsed = mc.ParseSyntax(m.objectIdSyntax);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            string fileName = this.fileNameParsed.Read(mc);
            string objectId = this.objectIdParsed.Read(mc);
            string isTemporary = this.isTemporaryParsed.Read(mc);
            using(IObjectData obj=mc.objectCache.CheckOut(objectId)){
                using (var f = mc.globalContext.bfs.GetObjectQueue(fileName, !isTemporary.Equals("1")))
                {
                    var bqueue = (f.value as ObjectQueueBufferedFile);
                    bqueue.WriteObject(obj);
                }
            }
        }
    }
}
