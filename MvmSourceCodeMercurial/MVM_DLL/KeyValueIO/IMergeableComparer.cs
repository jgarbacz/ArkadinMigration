using System;
using System.Collections.Generic;
namespace MVM
{

   public interface IMergeableComparer<T>
    {
        IComparer<T> LiveComparer { get; }
        IComparer<RawValue<T>> RawComparer { get; }
    }


  
}
