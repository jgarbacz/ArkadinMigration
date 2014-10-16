using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public static class MyObject
    {

        /// <summary>
        /// Swaps first with second and second with first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static void SwapValues<T>(ref T first, ref T second)
        {
            T tmp = first;
            first = second;
            second = tmp;
        }

        /// <summary>
        /// Works like sql in operator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="theseObjects"></param>
        /// <returns></returns>
        public static bool In<T>(this T thisObject, params T[] theseObjects)
        {
            for(int i=0;i<theseObjects.Length;i++) 
                if (thisObject.Equals(theseObjects[i])) 
                    return true;
            return false;
        }

        /// <summary>
        /// Works like sql in operator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="theseObjects"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T thisObject, params T[] theseObjects)
        {
            return !thisObject.In(theseObjects);
        }


        /// <summary>
        /// In Oracle/PLSQL, the NVL2 function extends the functionality found in the NVL function. 
        /// It lets you substitutes a value when a null value is encountered as well as when a non-null 
        /// value is encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="X"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="valueIfNotNull"></param>
        /// <param name="valueIfNull"></param>
        /// <returns></returns>
        public static X Nvl2<T, X>(this T thisObject, X valueIfNotNull, X valueIfNull)
        {
            return thisObject != null ? valueIfNotNull : valueIfNull;
        }
        /// <summary>
        /// In Oracle/PLSQL, the NVL function lets you substitute a value when a null value is encountered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="valueIfNull"></param>
        /// <returns></returns>
        public static T Nvl<T>(this T thisObject, T valueIfNull)
        {
            return thisObject != null ? thisObject : valueIfNull;  
        }

        /// <summary>
        /// Returns true if thisObject.Equals(thatObject) or thisObject==null && thatObject==null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="thatObject"></param>
        /// <returns></returns>
        public static bool SafeEquals<T>(this T thisObject, T thatObject)
        {
            if (thisObject == null && thatObject == null) return true;
            if (thisObject == null || thatObject == null) return false;
            return thisObject.Equals(thatObject);
        }
        /// <summary>
        /// Returns true if thisObject.Equals(thatObject). Anything compared to null is false like a database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="thatObject"></param>
        /// <returns></returns>
        public static bool DbEquals<T>(this T thisObject, T thatObject)
        {
            if (thisObject == null || thatObject == null) return false;
            return thisObject.Equals(thatObject);
        }
        /// <summary>
        /// Returns true if thisObject.Equals(thatObject). Anything compared to null is false. "" is treated like null like a database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <param name="thatObject"></param>
        /// <returns></returns>
        public static bool DbStringEquals<T>(this T thisObject, T thatObject)
        {
            if (thisObject == null || thatObject == null) return false;
            if (thisObject.ToString_Safe().Equals("") || thatObject.ToString_Safe().Equals("")) return false;
            return thisObject.Equals(thatObject);
        }

        /// <summary>
        /// Returns thisObject.ToString or "" if thisObject is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisObject"></param>
        /// <returns></returns>
        public static string ToString_Safe<T>(this T thisObject)
        {
            return thisObject!=null? thisObject.ToString(): "";
        }


       
    }
}
