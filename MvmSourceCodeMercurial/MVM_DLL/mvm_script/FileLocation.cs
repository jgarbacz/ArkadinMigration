using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvmScript
{
    class FileLocation:ILocation
    {
        public string fileName;
        public FileLocation(string fileName){
            this.fileName=fileName;
        }
        public string Location
        {
            get { 
                return this.fileName; 
            }
        }
    }
}
