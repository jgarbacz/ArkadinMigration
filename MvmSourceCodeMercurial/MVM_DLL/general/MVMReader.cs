using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MVM
{
    public interface MVMReader : IDisposable
    {
        string ReadLine();
        void Close();
        long Position();
    }
}
