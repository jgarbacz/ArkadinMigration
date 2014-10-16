using System;
using System.Collections.Generic;
using NLog;

using System.Text;

namespace MVM
{
    class MStopwatchStop:IModuleSetup,IModuleRun
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string swName;
        private string swGlobalName;

        private string msSyntax;
        private IWriteString msWriteParsed;

        private string totalMsSyntax;
        private IWriteString totalMsWriteParsed;

        private IReadString totalMsReadParsed;

        private string counterSyntax;
        private IWriteString counterWriteParsed;

        private IReadString counterReadParsed;

        private string counterIncrSyntax;
        private IReadString counterIncrReadParsed;

        private string skipFirstSyntax;


        private long lastElapsedMilliseconds = 0;
        private long currentDelta = 0;
        private long orig_ctr = 0;
        private long ctr_incr = 0;
        private long skip_first = 0;



        public void Setup(System.Xml.XmlElement me, ModuleContext mc,List<IModuleRun> runModules)
        {
            MStopwatchStop m = new MStopwatchStop();
            m.swName = me.GetAttribute("name");
            m.swGlobalName = "SplitStopWatch:" + m.swName;

            m.msSyntax = me.GetAttribute("ms");
            m.msWriteParsed = mc.ParseWritableSyntax(m.msSyntax);
            
            m.totalMsSyntax = me.GetAttribute("total_ms");          
            m.totalMsWriteParsed = mc.ParseWritableSyntax(m.totalMsSyntax);
            m.totalMsReadParsed = mc.ParseSyntax(m.totalMsSyntax);

            m.counterSyntax = me.GetAttribute("counter");
            m.counterWriteParsed = mc.ParseWritableSyntax(m.counterSyntax);
            m.counterReadParsed = mc.ParseSyntax(m.counterSyntax);

            m.skipFirstSyntax = me.GetAttributeDefaulted("skip_first", "0");
            m.skip_first = m.skipFirstSyntax.ToString().ToInt();


            m.counterIncrSyntax = me.GetAttributeDefaulted("counter_increment","1");
            m.counterIncrReadParsed = mc.ParseSyntax(m.counterIncrSyntax);

            runModules.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            SplitStopwatch sw = mc.globalContext.GetNamedClassInst(this.swGlobalName) as SplitStopwatch;
            sw.Stop();
            if (!long.TryParse(this.totalMsReadParsed.Read(mc), out orig_ctr))
            {
                orig_ctr = 0;
            }
            currentDelta = sw.ElapsedMilliseconds - lastElapsedMilliseconds + orig_ctr;
            //logger.Info("elapsed:" + lastElapsedMilliseconds.ToString() + ", delta:" + currentDelta.ToString() + ", ElapsedNow:" + sw.ElapsedMilliseconds.ToString() + " This:" + this.totalMsWriteParsed.ToString());
            lastElapsedMilliseconds = sw.ElapsedMilliseconds;
            
            if (this.msWriteParsed != null) this.msWriteParsed.Write(mc, sw.SplitMilliseconds.ToString());

            if (this.totalMsWriteParsed != null)
            {
                this.totalMsWriteParsed.Write(mc, currentDelta.ToString());
                //logger.Info("elapsed1:" + this.totalMsWriteParsed.ToString());
            }
            else
                sw.Reset();// if not aggregating,reset since it migh RemoveSpecificItem some rounding error on the split time

            if (this.counterWriteParsed != null)
            {
                if (!long.TryParse(this.counterReadParsed.Read(mc), out orig_ctr))
                {
                    orig_ctr = 0;
                }
                long.TryParse(this.counterIncrReadParsed.Read(mc), out ctr_incr);
                currentDelta = orig_ctr + ctr_incr;
                this.counterWriteParsed.Write(mc, currentDelta.ToString());
            }

            if (this.skip_first == 1)
            {
                this.totalMsWriteParsed.Write(mc, "0");
                this.skip_first = 0;
            }
        }

    }
}
