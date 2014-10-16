using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    /// <summary>
    /// This class represents the compiled proc. Since procs are allowed to 
    /// edit themselves there are many definitions for a single logical proc.
    /// </summary>
    public class ProcDefinition
    {
        // This is our special newModule newOrder that means no more orders to process.
        public static readonly ModuleOrder DONE_ORDER = ModuleOrder.CreateDoneOrder();

        // Version, gets update anytime we make changes to the proc.
        public long version = 0;

        # region procContext information that does not change

        public readonly ProcInfo procInfo;
        public readonly ProcType procType;

        // Name of the procContext
        public string procName
        {
            get
            {
                return procInfo.procName;
            }
        }
        // Local name of the procContext
        public string localName
        {
            get
            {
                return procInfo.localName;
            }
        }
        // Namespace of the procContext
        public string nameSpace
        {
            get
            {
                return procInfo.nameSpace;
            }
        }
        // Namespace of the procContext
        public string structuralNameSpace
        {
            get
            {
                return procInfo.structuralNameSpace;
            }
        }
        // id assigned to this procContext
        public int procId
        {
            get
            {
                return this.procInfo.procId;
            }
        }

        #endregion

       

        // Ordered list of newModules we need to run
        // moduleList[moduleIdx]=IModuleRun
        public List<IModuleRun> moduleList = new List<IModuleRun>();

        // Parallel array to moduleList, so we can get the newOrder using moduleList index
        // moduleOrders[moduleIdx]=ModuleOrder
        public List<ModuleOrder> moduleOrders = new List<ModuleOrder>();

        // When we start our procInst, we start resume an newOrder. This tell us what index into moduleList
        // that it corresponds to.
        // moduleOrderToIdx{moduleOrder}=moduleIdx
        public Dictionary<ModuleOrder, int> moduleOrderToIdx = new Dictionary<ModuleOrder, int>();

        // Modules that implement IModuleLabel go in this structure so given a text label
        // we can to a newModule.
        public Dictionary<string, int> moduleLabelToIdx = new Dictionary<string, int>();

        // Add a single newModule
        public void AddModule(IModuleRun newModule, ModuleOrder newOrder)
        {
            int moduleIdx = this.moduleOrders.InsertSorted(newOrder);
            this.moduleList.Insert(moduleIdx, newModule);
            this.moduleOrderToIdx[newOrder] = moduleIdx;
            this.RebuildIndexes(moduleIdx);
        }

        // adds multiple modules to the procdef
        public void AddModules(IEnumerable<IModuleRun> modules)
        {
            int implicitOrder = 0;
            foreach (IModuleRun newModule in modules)
            {
                ModuleOrder newOrder = ModuleOrder.CreateDuringOrder(implicitOrder++);
                int idx = this.moduleOrders.InsertSorted(newOrder);
                this.moduleList.Insert(idx, newModule);
                this.moduleOrderToIdx[newOrder] = idx;
            }
            this.RebuildIndexes();
        }

        // Adds multiple newModules
        public void AddModules(IEnumerable<IModuleRun> newModules, ModuleOrder curOrder)
        {
            if (newModules == null) throw new Exception("unexpected 93434");

            // start by getting the current module index because we'll need to rebuild
            int curModuleIdx = this.moduleOrderToIdx[curOrder];
            
            // Use binary search for the first module but assume subsequent module will be right
            // after the first one so just increment the index.
            int implicitOrder = 0;
            var newModuleEnum = newModules.GetEnumerator();
            if (!newModuleEnum.MoveNext())
            {
                throw new Exception("Expecting new modules to have rows");
            }
            IModuleRun moduleRun = newModuleEnum.Current;
            ModuleOrder newOrder = ModuleOrder.CreateDuringOrder(curOrder, implicitOrder++);
            int moduleIdx = this.moduleOrders.BinarySearchForInsertIdx(newOrder);
            this.moduleOrders.Insert(moduleIdx, newOrder);
            this.moduleList.Insert(moduleIdx, moduleRun);
            this.moduleOrderToIdx[newOrder] = 0;
            while(newModuleEnum.MoveNext()){
                moduleRun = newModuleEnum.Current;
                newOrder = ModuleOrder.CreateDuringOrder(curOrder, implicitOrder++);
                moduleIdx += 1; //assume contiguous indexes...
                this.moduleOrders.Insert(moduleIdx, newOrder);
                this.moduleList.Insert(moduleIdx, moduleRun);
                this.moduleOrderToIdx[newOrder] = 0;
            }
            this.RebuildIndexes(curModuleIdx);
        }

        // replaces the newModule at the passed index with a new one, cheaper 
        // then the ReplaceModule with more then one newModule.
        private void ReplaceModuleInplace(int atModuleIdx, IModuleRun newModule)
        {
            this.moduleList[atModuleIdx] = newModule;
        }

        /// <summary>
        /// Removes the module at the passed module index.
        /// </summary>
        /// <param name="atModuleIdx"></param>
        public void RemoveModule(int atModuleIdx)
        {
            // Remove the current module
            ModuleOrder curOrder = this.moduleOrders[atModuleIdx];
            this.moduleOrders.RemoveAt(atModuleIdx);
            this.moduleList.RemoveAt(atModuleIdx);
            // Rebuild all indexes for all modules after the current module
            this.RebuildIndexes(atModuleIdx);
        }

        private void RebuildIndexes(int fromModuleIdx)
        {
            // we need to update moduleOrderToIdx (really substract 1) for all modules 
            // that happen after the current module. Note that we do not clear out
            // old entries in moduleOrderToIdx because there could be proc that are 
            // paused on old orders and they need to be able to pickup in the right
            // spot. MAYBE RETHINK THIS...
            for (int moduleIdx = fromModuleIdx; moduleIdx < this.moduleOrders.Count; moduleIdx++)
            {
                ModuleOrder moduleOrder = this.moduleOrders[moduleIdx];

                // update module order to module index mapping
                this.moduleOrderToIdx[moduleOrder] = moduleIdx;

                // update module Label to module index mapping
                IModuleLabel moduleLabel = this.moduleList[moduleIdx] as IModuleLabel;
                if (moduleLabel != null)
                {
                    string label = moduleLabel.GetLabel();
                    if (label != null)
                        this.moduleLabelToIdx[label] = moduleIdx;
                }
            }
        }

        // Replaces a newModule with the passed generated newModules
        public void ReplaceModule(int curModuleIdx, params IModuleRun[] newModules)
        {
            if (newModules != null && newModules.Length == 1)
            {
                this.ReplaceModuleInplace(curModuleIdx, newModules[0]);
                return;
            }
            if (newModules == null || newModules.Length == 0)
            {
                this.RemoveModule(curModuleIdx);
                return;
            }
            ModuleOrder curOrder = this.moduleOrders[curModuleIdx];
            this.RemoveModule(curModuleIdx);
            this.AddModules(newModules, curOrder);
        }

        // returns the next implicit newOrder for the passed explicitOrder
        private int GetNextImplicitOrder(StringDecimal whenOrder, StringDecimal explicitOrder)
        {
            // no before newModules, implicitIdx is 0
            if (this.moduleOrders.Count == 0) return 0;

            List<ModuleOrder> explicitOrders = new List<ModuleOrder>();
            foreach (ModuleOrder mo in this.moduleOrders)
            {
                if (mo.orders[0].Equals(whenOrder))
                {
                    if (mo.orders[1].Equals(explicitOrder))
                    {
                        explicitOrders.Add(mo);
                    }
                }
            }

            // no explicit index newModules, implicit idx is 0
            if (explicitOrders.Count == 0) return 0;
            // otherwise return the max implicit newOrder for the explicit orders
            StringDecimal maxImplicitOrder = null;
            foreach (var mo in explicitOrders)
            {
                StringDecimal order = mo.orders[2];
                if ((maxImplicitOrder == null) || order.IsGreaterThan(maxImplicitOrder))
                {
                    maxImplicitOrder = order;
                }
            }
            int nextImplicitOrder = int.Parse(maxImplicitOrder.ToString()) + 1;
            return nextImplicitOrder;
        }

        // Does a push before on the procContext
        public void PushBefore(IModuleRun module, StringDecimal explicitOrder)
        {
            int nextImplicitOrder = this.GetNextImplicitOrder(ModuleOrder.SD_BEFORE, explicitOrder);
            ModuleOrder mo = ModuleOrder.CreateBeforeOrder(
                explicitOrder.ToString(),
                nextImplicitOrder.ToString());
            this.AddModule(module, mo);
        }

        // Does a push after on the procContext
        public void PushAfter(IModuleRun module, StringDecimal explicitOrder)
        {
            int nextImplicitOrder = this.GetNextImplicitOrder(ModuleOrder.SD_AFTER, explicitOrder);
            ModuleOrder mo = ModuleOrder.CreateAfterOrder(
                explicitOrder.ToString(),
                nextImplicitOrder.ToString());
            this.AddModule(module, mo);
        }

        // Given an index into the newModule list it returns the newOrder name for the next newModule.
        // If there are no more newModules, it returns our special value "done"
        public ModuleOrder GetNextModuleOrder(int curModuleIdx)
        {
            if ((curModuleIdx + 1) >= this.moduleList.Count) return DONE_ORDER;
            return moduleOrders[curModuleIdx + 1];
        }

        // Given an index into the newModule list it return the newOrder name for the this newModule.
        public ModuleOrder GetCurrModuleOrder(int curModuleIdx)
        {
            if ((curModuleIdx) >= this.moduleList.Count) return DONE_ORDER;
            return moduleOrders[curModuleIdx];
        }

        public ModuleOrder GetPreviousModuleOrder(ModuleOrder referenceModuleOrder)
        {
            int referenceModuleIdx = this.GetIdxForOrder(referenceModuleOrder);
            ModuleOrder previousModuleOrder;

            if (this.moduleOrders.Count == 0)
            {
                return null;
            }
            //if it is less than max value, look it up and take the one before
            if (referenceModuleIdx < int.MaxValue)
            {
                int idx = this.moduleOrders.IndexOf(referenceModuleOrder);
                if (idx <= 0)
                {
                    return referenceModuleOrder;
                }
                previousModuleOrder = this.moduleOrders[idx - 1];
            }
            // if it is max value just take the last one.
            else
            {
                previousModuleOrder = this.moduleOrders[this.moduleOrders.Count - 1];
            }
            return previousModuleOrder;
        }

        // reassigns the indexes for moduleOrderToIdx
        private void RebuildIndexes()
        {
            this.RebuildIndexes(0);
            this.moduleOrderToIdx[DONE_ORDER] = int.MaxValue;
        }

        public int GetLabelModuleIdx(string label)
        {
            int value;
            if (this.moduleLabelToIdx.TryGetValue(label, out value)) return value;
            throw new Exception("Error, requesting module label that does not exist, label=[" + label + "]");
        }

        // Constructor
        public ProcDefinition(ProcInfo procInfo)
        {
            this.procInfo = procInfo;
            this.moduleOrderToIdx[DONE_ORDER] = int.MaxValue;
            this.procType = procInfo.procType;
        }

        // Returns copy of the current but with a new version
        public ProcDefinition GetNextVersion()
        {
            ProcDefinition copy = new ProcDefinition(this.procInfo);
            copy.version = this.version + 1;
            copy.moduleList = this.moduleList.CopyShallow();
            copy.moduleOrders = this.moduleOrders.CopyShallow();
            copy.moduleOrderToIdx = this.moduleOrderToIdx.CopyShallow();
            copy.moduleLabelToIdx = this.moduleLabelToIdx.CopyShallow();
            return copy;
        }

        // Returns the index into this.moduleList for the passed newOrder
        public int GetIdxForOrder(ModuleOrder order)
        {
            return this.moduleOrderToIdx[order];
        }

        // Returns the newOrder for the first newModule in this procContext
        public ModuleOrder GetFirstOrder()
        {
            return this.moduleOrders.Count > 0 ? this.moduleOrders[0] : DONE_ORDER;
        }
    }
}
