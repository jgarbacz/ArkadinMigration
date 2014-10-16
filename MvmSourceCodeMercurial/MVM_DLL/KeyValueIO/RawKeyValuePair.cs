using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// This is the binary equivelent of KeyValuePair
    /// </summary>
    public struct RawKeyValuePair<K,V>
    {
        public RawValue<K> Key;
        public RawValue<V> Value;
    }
}
