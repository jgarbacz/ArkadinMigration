using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    class ReadNull:IReadable
    {
        #region IReadable Members

        public object ReadObject(ModuleContext mc)
        {
            return null;
        }

        public Type GetReadType()
        {
            return typeof(object);
        }

        #endregion
    }
}
