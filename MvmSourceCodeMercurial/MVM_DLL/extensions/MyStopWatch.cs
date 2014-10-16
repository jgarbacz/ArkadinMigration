using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MVM
{
    /// <summary>
    /// Builds on stopwatch to support splits
    /// </summary>
    public class SplitStopwatch:Stopwatch
    {
        private long lastSplitElapsedMilliseconds = 0;

        /// <summary>
        /// Milliseconds since last call to Start()
        /// </summary>
        public long SplitMilliseconds
        {
            get
            {
                return base.ElapsedMilliseconds - this.lastSplitElapsedMilliseconds;
            }
        }

        private long lastElapsedTicks = 0;

        /// <summary>
        /// Ticks since last call to Start()
        /// </summary>
        public long SplitTicks
        {
            get
            {
                return base.ElapsedTicks - this.lastElapsedTicks;
            }
        }


        /// <summary>
        /// Starts the stopwatch which also starts a new split
        /// </summary>
        public new void Start()
        {
            this.lastSplitElapsedMilliseconds = base.ElapsedMilliseconds;
            this.lastElapsedTicks = base.ElapsedTicks;
            base.Start();
        }

        /// <summary>
        /// Resets the stopwatch to 0
        /// </summary>
        public new void Reset()
        {
            this.lastSplitElapsedMilliseconds = 0;
            this.lastElapsedTicks = 0;
            base.Reset();
        }

        public static void Test()
        {
            SplitStopwatch sw = new SplitStopwatch();
            for (int x = 1; x <= 2; x++)
            {
                for (int i = 1; i <= 5; i++)
                {
                    int sleep = 100 * i;
                    sw.Start();
                    System.Threading.Thread.Sleep(sleep);
                    sw.Stop();

                    Console.WriteLine("sleep_ms=" + sleep + ", total_ms=" + sw.ElapsedMilliseconds + ", split_ms=" + sw.SplitMilliseconds + ", ticks=" + sw.ElapsedTicks + ", split_ticks=" + sw.SplitTicks);
                }
                Console.WriteLine("RESET SW");
                sw.Reset();
            }
            
        }
    }
}
