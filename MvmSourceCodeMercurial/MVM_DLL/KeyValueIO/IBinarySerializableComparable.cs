using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{
    public interface IBinarySerializableComparable<T> : ISerializableComparable<T>, IDynamicSerializable, IBinaryComparable
    {
        IComparer<RawValue<T>> GetRawComparer();
    }

  
}
