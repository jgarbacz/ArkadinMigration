using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    static class MyDirectoryInfo
    {
        /// <summary>
        /// Creats the dir if it doesn't exist.
        /// Returns true if it need to create the dir
        /// </summary>
        /// <param name="thisDirectoryInfo"></param>
        /// <returns></returns>
        public static bool CreateIfNotThere(this DirectoryInfo thisDirectoryInfo)
        {
            if (!thisDirectoryInfo.Exists)
            {
                thisDirectoryInfo.Create();
                return true;
            }
            return false;
        }
    }
}
