using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{

    /// <summary>
    /// This class keeps maxCount objects in memory and dumps them to a sorted file on disk 
    /// when needed.
    /// 
    /// 
    /// what needs to be decided... make it work for phil too.
    /// 
    /// phil needs to think of the file as a file? true/false. true for now.
    /// 
    /// my stuff by definition needs to be able to not be a file...
    /// 
    /// 
    /// 
    /// got one wiget that merges... at the top level.
    /// got btree member
    /// got queue member
    /// maybe usorted queue member.
    /// 
    /// does the merger deal w/ the sizes? or do the members?
    /// 
    /// 
    /// 
    /// </summary>
    public class BufferedObjectFile
    {
        public BufferedObjectFile(string fileNamePrefix,string fileNameSuffix, int maxCount)
        {
        }
    }
   
  
}
