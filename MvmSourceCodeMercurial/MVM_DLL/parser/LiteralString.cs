using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    class LiteralString:ReadStringBase
    {
        private readonly string literalString;
        public LiteralString(string literalString)
        {
            this.literalString = literalString;
        }
        
        #region IModuleRecurse Members

        public override string Read(ModuleContext mc)
        {
            return this.literalString;
        }

        #endregion
    }
}
