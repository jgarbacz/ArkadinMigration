using System;
using System.Collections.Generic;

using System.Text;

using Antlr.Runtime.Tree;

namespace MVM
{
    public class AccessObject : ISetupSyntax,ISetupWritable
    {
        #region ISetupSyntax Members

        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            ITree objectId = tree.GetChild(0);
            string fieldName = tree.GetChild(1).Text;
            int ufn = MvmEngine.DefaultMvmEngine.mvmCluster.GetUfn(fieldName);
            if (objectId.Text.Equals("CURRENT_OBJECT"))
            {
                return new ReadCurObj(ufn);
            }
            else
            {
                IReadString parsedObjectId = syntaxMaster.SetupRead(objectId);
                return new ReadObj(parsedObjectId, ufn);
            }
            throw new Exception("not handled:" + objectId.Text);
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            IReadString right = syntaxMaster.SetupRead(rightTree);
            return SetupWrite(syntaxMaster, leftTree, right);
        }
        
        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, IReadString right)
        {
            ITree objectId = leftTree.GetChild(0);
            string fieldName = leftTree.GetChild(1).Text;
            int ufn = MvmEngine.DefaultMvmEngine.mvmCluster.GetUfn(fieldName);
            if (objectId.Text.Equals("CURRENT_OBJECT"))
            {
                return new WriteCurObj(ufn, right);
            }
            else
            {
                IReadString parsedObjectId = syntaxMaster.SetupRead(objectId);
                return new WriteObj(parsedObjectId, ufn, right);
            }
            throw new Exception("not handled:" + objectId.Text);
        }

        #endregion

        #region IModuleRecurse Members
        class WriteCurObj : ReadStringBase
        {
            private readonly int ufn;
            private readonly IReadString fieldValue;
            public WriteCurObj(int ufn, IReadString fieldValue)
            {
                this.ufn = ufn;
                this.fieldValue = fieldValue;
            }
            public override string Read(ModuleContext mc)
            {
                //return mc.WriteObjectField(mc.procInst.objectId, ufn, fieldValue.Read(mc));
                return mc.objectData[ufn] = fieldValue.Read(mc);
            }
        }

        class ReadCurObj : ReadStringBase
        {
            private readonly int ufn;
            public ReadCurObj(int ufn)
            {
                this.ufn = ufn;
            }
            public override string Read(ModuleContext mc)
            {
                return mc.objectData[ufn]; // speed up current object access 
            }
        }
        class WriteObj : ReadStringBase
        {
            private readonly int ufn;
            private readonly IReadString objectId;
            private readonly IReadString fieldValue;
            public WriteObj(IReadString objectId, int ufn, IReadString fieldValue)
            {
                this.ufn = ufn;
                this.objectId = objectId;
                this.fieldValue = fieldValue;
            }
            public override string Read(ModuleContext mc)
            {
                string oid = this.objectId.Read(mc);
                return mc.WriteObjectField(oid, ufn, fieldValue.Read(mc));
            }
        }

        class ReadObj : ReadStringBase
        {
            private readonly int ufn;
            private readonly IReadString objectId;
            public ReadObj(IReadString objectId,int ufn)
            {
                this.objectId = objectId; 
                this.ufn = ufn;
            }
            public override string Read(ModuleContext mc)
            {
                string oid = this.objectId.Read(mc);
                return mc.ReadObjectField(oid, ufn);
            }
        }
        #endregion

        #region ISetupWritable Members

        public IWriteString SetupWritable(SyntaxMaster syntaxMaster, ITree leftTree)
        {
            ITree objectId = leftTree.GetChild(0);
            string fieldName = leftTree.GetChild(1).Text;
            if (objectId.Text.Equals("CURRENT_OBJECT"))
            {
                return new WritableCurObj(fieldName);
            }
            else
            {
                IReadString parsedObjectId = syntaxMaster.SetupRead(objectId);
                return new WritableObj(parsedObjectId, fieldName);
            }
            throw new Exception("not handled:" + objectId.Text);
        }
        #endregion

        #region IModuleWritable Members
        class WritableObj : WriteStringBase
        {
            private readonly string fieldName;
            private readonly IReadString objectId;
            public WritableObj(IReadString objectId, string fieldName)
            {
                this.fieldName = fieldName;
                this.objectId = objectId;
            }
            public override string Write(ModuleContext mc,string value)
            {
                string oid = this.objectId.Read(mc);
                return mc.WriteObjectField(oid, fieldName, value);
            }
            public override string Write(ModuleContext mc, IReadString value)
            {
                string oid = this.objectId.Read(mc);
                return mc.WriteObjectField(oid, fieldName, value.Read(mc));
            }
        }

        class WritableCurObj : WriteStringBase
        {
            private readonly string fieldName;
            public WritableCurObj(string fieldName)
            {
                this.fieldName = fieldName;
            }
            public override string Write(ModuleContext mc, string value)
            {
                //return mc.WriteObjectField(mc.procInst.objectId, fieldName, value);
                return mc.objectData[fieldName] = value;
            }
            public override string Write(ModuleContext mc, IReadString value)
            {
                //return mc.WriteObjectField(mc.procInst.objectId, fieldName, value.Read(mc));
                return mc.objectData[fieldName] = value.Read(mc);
            }
        }

        #endregion
    }
}
