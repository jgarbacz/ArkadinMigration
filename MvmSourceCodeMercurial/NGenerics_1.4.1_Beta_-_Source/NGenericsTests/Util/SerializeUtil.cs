/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/


using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NGenerics.Tests.Util
{
    public static class SerializeUtil
    {        
        /// <summary>
        /// Serializes the object, then deserialize the object and returns the result.
        /// </summary>
        /// <param name="obj">The object to serializer / deserialize.</param>
        public static T BinarySerializeDeserialize<T>(T obj)
        {                                                
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);

                using (var readStream = new MemoryStream(ms.ToArray()))
                {
                    return (T) formatter.Deserialize(readStream);
                }
            }
        }
    }
}
