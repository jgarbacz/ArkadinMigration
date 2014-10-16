using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{

    public class EnumerableWrapper<T> : IEnumerable<T>
    {
        private IEnumerator<T> enumerator;
        public EnumerableWrapper(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return enumerator;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumerator;
        }
    }

    public static class MyEnumerator
    {
        /// <summary>
        /// Wraps the Enumerator in a dummy EnumerableWrapper class so it is Enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> item)
        {
            return new EnumerableWrapper<T>(item);
        }
    }
}
