using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class KeyValuePairKeyComparer<K,V>:IComparer<KeyValuePair<K,V>>
    {
        IComparer<K> keyComparer;
        public KeyValuePairKeyComparer(IComparer<K> keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        #region IComparer<KeyValuePair<K,V>> Members

        public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            return this.keyComparer.Compare(x.Key, y.Key);
        }

        #endregion
    }
}
