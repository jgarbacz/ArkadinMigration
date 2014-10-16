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
  <mash_id>
    <next_id>GLOBAL.next_id_acc</next_id>
    <maximum_id>GLOBAL.max_id_acc</maximum_id>
    <minimum_id>GLOBAL.min_id_acc</minimum_id>
    <mashed_id>OBJECT.id_acc</mashed_id>
    <unmashed_id>OBJECT.id_acc</unmashed_id>
  </mash_id>
    */

    class MMashId : IModuleSetup, IModuleRun
    {
        private string nextIdSyntax;
        private IReadString nextIdParsed;
        private IWriteString nextIdWriteParsed;

        private string maximumIdSyntax;
        private IReadString maximumIdParsed;

        private string minimumIdSyntax;
        private IReadString minimumIdParsed;

        private string mashedIdSyntax;
        private IWriteString mashedIdParsed;

        private string unmashedIdSyntax;
        private IWriteString unmashedIdParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MMashId m = new MMashId();
            m.nextIdSyntax = me.SelectNodeInnerText("./next_id");
            m.nextIdParsed = mc.ParseSyntax(m.nextIdSyntax);
            m.nextIdWriteParsed = mc.ParseWritableSyntax(m.nextIdSyntax);
            m.maximumIdSyntax = me.SelectNodeInnerText("./maximum_id", "'-1'");
            m.maximumIdParsed = mc.ParseSyntax(m.maximumIdSyntax);
            m.minimumIdSyntax = me.SelectNodeInnerText("./minimum_id");
            m.minimumIdParsed = mc.ParseSyntax(m.minimumIdSyntax);
            m.mashedIdSyntax = me.SelectNodeInnerText("./mashed_id");
            m.mashedIdParsed = mc.ParseWritableSyntax(m.mashedIdSyntax);
            m.unmashedIdSyntax = me.SelectNodeInnerText("./unmashed_id");
            m.unmashedIdParsed = mc.ParseWritableSyntax(m.unmashedIdSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            int nextId = this.nextIdParsed.Read(mc).ToInt();
            int maximumId = this.maximumIdParsed.Read(mc).ToInt();
            int minimumId = this.minimumIdParsed.Read(mc).ToInt();
            int mashedId;
            int unmashedId;
            NextMashedId(ref nextId, minimumId, out mashedId, out unmashedId);
            if (maximumId > 0 && unmashedId >= maximumId)
            {
                // Signal that we exceeded the max value by returning ""
                this.mashedIdParsed.Write(mc, "");
                this.unmashedIdParsed.Write(mc, "");
                this.nextIdWriteParsed.Write(mc, "");
            }
            else
            {
            this.mashedIdParsed.Write(mc,mashedId.ToString());
                this.unmashedIdParsed.Write(mc, unmashedId.ToString());
                this.nextIdWriteParsed.Write(mc, nextId.ToString());
            }
        }


        public static void NextMashedId(ref int nextId, int minimumId, out int mashedId, out int unmashedId)
        {
            int tmp = 0;
            do
            {
                unmashedId = nextId++;
                tmp = unmashedId;
                tmp += (tmp << 12);
                tmp &= 0x7fffffff;

                tmp ^= (tmp >> 22);

                tmp += (tmp << 4);
                tmp &= 0x7fffffff;

                tmp ^= (tmp >> 9);

                tmp += (tmp << 10);
                tmp &= 0x7fffffff;

                tmp ^= (tmp >> 2);

                tmp += (tmp << 7);
                tmp &= 0x7fffffff;

                tmp ^= (tmp >> 12);

            } while (tmp < minimumId);
            mashedId = tmp;
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("mash_id:");
        }
    }
}
