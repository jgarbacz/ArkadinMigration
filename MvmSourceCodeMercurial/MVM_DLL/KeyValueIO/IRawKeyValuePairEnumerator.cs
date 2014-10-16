using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    /// <summary>
    /// MergeReader works with MergeEnumerators that comply with this interface. For something
    /// to be mergeable both key and value must be serializable and the key must also be
    /// binary comparable. Also, it must support count which indicates the number of rows or 
    /// -1 if that is unknown.
    /// </summary>
    public interface IRawKeyValuePairEnumerator<K, V> : IEnumerator<RawKeyValuePair<K, V>>
    {
        int Count { get; }
    }
}
