using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public interface IWriteString:IWritable
    {
        string Write(ModuleContext mc, string value);
        string Write(ModuleContext mc, IReadString value);
    }
}
