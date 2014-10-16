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
    /// binary comparable. To be mergeable you must be able to say the next value and if there
    /// is one.
    /// </summary>
    public interface IMergeEnumerator<K, V> : IKeyValueEnumerator<K,V>
    {
        /// <summary>
        /// This gets the next value as if you were to call MoveNext. This lets you peek
        /// at the next value without consuming it.
        /// </summary>
        KeyValuePair<K, V> Next { get; }
        /// <summary>
        /// True if Next is valid. Same as saying true if this.MoveNext() will return true.
        /// </summary>
        /// <returns></returns>
        bool HasNext{get;}
    }
}
