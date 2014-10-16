using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IRawMergeEnumerable<K, V> : IEnumerable<RawKeyValuePair<K, V>>
    {
        IRawMergeEnumerator<K, V> GetRawMergeEnumerator();
    }

}
