using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MNothing: IModuleSetup,IModuleRun
    {
        public void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            //Console.WriteLine("start setup nothing");
            //System.Threading.Thread.Sleep(1000);
           
            MNothing m = new MNothing();
            run.Add(m);
            //Console.WriteLine("end setup nothing");
        }

        public void Run(ModuleContext mc)
        {
            //Console.WriteLine("start run nothing");
            //System.Threading.Thread.Sleep(1000);
            //Console.WriteLine("end run nothing");


            //SpeedOfFormatedObjectRead.Run(mc);

        }
    }


    public static class SpeedOfFormatedObjectRead
        {
        public static void Run(ModuleContext mc)
        {
            //Console.WriteLine("start run nothing");
            //System.Threading.Thread.Sleep(1000);
            //Console.WriteLine("end run nothing");
            long cnt = 10000000;
            string fieldName = "a1";
            string fieldValue = "a1";
            int ufn = mc.mvmCluster.GetUfn(fieldName);
            mc.objectData[ufn] = fieldValue;
            {
                Console.WriteLine("WRITE ============== name=" + fieldName + ",fieldValue=" + fieldValue + ",ufn=" + ufn);
                SplitStopwatch sw = new SplitStopwatch();
                sw.Start();
                for (int i = 0; i < cnt; i++)
                {
                    mc.objectData[ufn] = fieldValue;
        }
                sw.Stop();
                long ms = sw.ElapsedMilliseconds;
                double s = ms / (double)1000;
                long tps = (long)(((double)cnt) / s);
                Console.WriteLine("WRITE ============== ms=" + ms + ",cnt=" + cnt + ",tps=" + tps);
    }
            {
                Console.WriteLine("READ ============== name=" + fieldName + ",fieldValue=" + fieldValue + ",ufn=" + ufn);
                SplitStopwatch sw = new SplitStopwatch();
                sw.Start();
                for (int i = 0; i < cnt; i++)
                {
                    fieldValue=mc.objectData[ufn];
}
                sw.Stop();
                long ms = sw.ElapsedMilliseconds;
                double s = ms / (double)1000;
                long tps = (long)(((double)cnt) / s);
                Console.WriteLine("READ ============== ms=" + ms + ",cnt=" + cnt + ",tps=" + tps);
            }
        }
    }
}
