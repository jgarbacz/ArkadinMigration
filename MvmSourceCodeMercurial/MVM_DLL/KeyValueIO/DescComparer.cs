//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MVM
//{
//    public class DescLiveComparer<T>:IComparer<T>
//        where T : BinarySerializableComparable<T>
//    {

//        public readonly IComparer<T> inputComparer;
//        public DescLiveComparer(IComparer<T> inputComparer){
//            this.inputComparer = inputComparer;
//        }

//        #region IComparer<T> Members

//        public int Compare(T x, T y)
//        {
//            return 0-this.inputComparer.Compare(x,y);
//        }

//        #endregion
//    }
//}
