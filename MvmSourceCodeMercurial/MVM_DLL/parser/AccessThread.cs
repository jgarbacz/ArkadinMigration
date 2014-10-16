using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessThread : ISetupSyntax,ISetupWritable
    {
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            string fieldName = tree.GetChild(0).Text;
                return new ReadThread(fieldName);
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            IReadString right = syntaxMaster.SetupRead(rightTree);
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteThread(fieldName, right);         
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, IReadString right)
        {
            string fieldName = leftTree.GetChild(0).Text;
            return new WriteThread(fieldName, right);
        }

        #endregion

        #region IModuleRecurse Members

        class WriteThread : ReadStringBase
        {
            private readonly string fieldName;
            private readonly IReadString fieldValue;
            public WriteThread(string fieldName, IReadString fieldValue)
            {
                this.fieldName = fieldName;
                this.fieldValue = fieldValue;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.threadContext[fieldName]= fieldValue.Read(mc);
            }
        }

        class ReadThread : ReadStringBase
        {
            private readonly string fieldName;
            public ReadThread(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.threadContext[fieldName];
            }
        }
        #endregion
        
        #region ISetupWritable Members

        public IWriteString SetupWritable(SyntaxMaster syntaxMaster, ITree leftTree)
        {
           
            string fieldName = leftTree.GetChild(0).Text;
            return new WritableThread(fieldName);
        }
        #endregion

        #region IModuleWritable Members
          class WritableThread : WriteStringBase
        {
            private readonly string fieldName;
            public WritableThread(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Write(ModuleContext mc, string value)
            {
                return mc.threadContext[fieldName]=value;
            }
            public override string Write(ModuleContext mc, IReadString value)
            {
                return mc.threadContext[fieldName] = value.Read(mc);
            }
        }
        #endregion
    }
}
