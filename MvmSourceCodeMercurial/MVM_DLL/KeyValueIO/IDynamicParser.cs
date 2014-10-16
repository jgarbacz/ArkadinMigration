using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public interface IDynamicParser
    {
        object Parse(string input);
    }
}
