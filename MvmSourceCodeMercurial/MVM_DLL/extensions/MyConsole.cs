using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    static class MyConsole
    {
        public static void CountTo(int end)
        {
            for (int i = 1; i < end; i++)
            {
                Console.WriteLine(i);
                System.Threading.Thread.Sleep(1000);
            }
        }


    }
}
