using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace MVM
{

    public interface ISerializableComparable<T> : ISerializable<T>, IComparable<T>
    {
    }
}
