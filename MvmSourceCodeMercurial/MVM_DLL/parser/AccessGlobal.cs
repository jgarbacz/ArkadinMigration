using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessGlobal : ISetupSyntax,ISetupWritable
    {
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            string fieldName = tree.GetChild(0).Text;
                return new ReadGlobal(fieldName);
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            IReadString right = syntaxMaster.SetupRead(rightTree);
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteGlobal(fieldName, right);         
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, IReadString right)
        {
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteGlobal(fieldName, right);
        }

        #endregion

        #region IModuleRecurse Members

        class WriteGlobal : ReadStringBase
        {
            private readonly string fieldName;
            private readonly IReadString fieldValue;
            public WriteGlobal(string fieldName, IReadString fieldValue)
            {
                this.fieldName = fieldName;
                this.fieldValue = fieldValue;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.globalContext[fieldName]= fieldValue.Read(mc);
            }
        }

        class ReadGlobal : ReadStringBase
        {
            private readonly string fieldName;
            public ReadGlobal(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.globalContext[fieldName];
            }
        }
        #endregion
        
        #region ISetupWritable Members

        public IWriteString SetupWritable(SyntaxMaster syntaxMaster, ITree leftTree)
        {
           
            string fieldName = leftTree.GetChild(0).Text;
            return new WritableGlobal(fieldName);
        }
        #endregion

        #region IModuleWritable Members
          class WritableGlobal : WriteStringBase
        {
            private readonly string fieldName;
            public WritableGlobal(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Write(ModuleContext mc, string value)
            {
                return mc.globalContext[fieldName]=value;
            }
            public override string Write(ModuleContext mc, IReadString value)
            {
                return mc.globalContext[fieldName] = value.Read(mc);
            }
        }
        #endregion
    }
}
