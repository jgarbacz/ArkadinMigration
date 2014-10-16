using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MVM;
namespace move_bridge_to_feature
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MVM.MvmEngine.RunFromConsole(args);
            }
            finally
            {
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }

        }
    }
}
