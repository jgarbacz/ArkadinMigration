using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessTemp : ISetupSyntax,ISetupWritable
    {
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            string fieldName = tree.GetChild(0).Text;
                return new ReadTemp(fieldName);
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            IReadString right = syntaxMaster.SetupRead(rightTree);
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteTemp(fieldName, right);         
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, IReadString right)
        {
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteTemp(fieldName, right);
        }

        #endregion

        #region IModuleRecurse Members

        class WriteTemp : ReadStringBase
        {
            private readonly string fieldName;
            private readonly IReadString fieldValue;
            public WriteTemp(string fieldName, IReadString fieldValue)
            {
                this.fieldName = fieldName;
                this.fieldValue = fieldValue;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.tempContext[fieldName]= fieldValue.Read(mc);
            }
        }

        class ReadTemp : ReadStringBase
        {
            private readonly string fieldName;
            public ReadTemp(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.tempContext[fieldName];
            }
        }
        #endregion
        
        #region ISetupWritable Members

        public IWriteString SetupWritable(SyntaxMaster syntaxMaster, ITree leftTree)
        {
           
            string fieldName = leftTree.GetChild(0).Text;
            return new WritableTemp(fieldName);
        }
        #endregion

        #region IModuleWritable Members
          class WritableTemp : WriteStringBase
        {
            private readonly string fieldName;
            public WritableTemp(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Write(ModuleContext mc, string value)
            {
                return mc.tempContext[fieldName]=value;
            }
            public override string Write(ModuleContext mc, IReadString value)
            {
                return mc.tempContext[fieldName] = value.Read(mc);
            }
        }
        #endregion
    }
}
