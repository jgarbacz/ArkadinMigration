﻿using System;
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
    public interface IKeyValueEnumerator<K, V> : IEnumerator<KeyValuePair<K, V>>
    {
        /// <summary>
        /// Count remaining (does not include <code>this.Current</code>. -1 indicates I do not know.
        /// </summary>
        int Count { get; }
       
    }
}
