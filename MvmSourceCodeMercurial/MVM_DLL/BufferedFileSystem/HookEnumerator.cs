using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NGenerics.DataStructures.Trees;

namespace MVM
{
    /// <summary>
    /// This class provides the ability to loop through all records in the usage hook as well
    /// as add new records on the fly. These records will show up if their sortkey is GTE the 
    /// current sortkey, else they will get written out for the next pass on the usage hook.
    /// </summary>
    public class HookEnumerator : ObjectQueueMerger
    {
        public readonly FutureTree futureTree;
        public bool FutureTreeInserted = false;
        public IObjectData targetObj;
        public string lastSortKey;
        public readonly UsageHookObject usageHook;
       
        /// <summary>
        /// Instanciate a HookEnumerator, for looping through records in the hook and adding records to 
        /// the hook. Added records will be interleaved based on the current hook object.
        /// </summary>
        /// <param name="usageHook"></param>
        /// 
        /// <param name="sortField"></param>
        /// <param name="trackDelta"></param>
        public HookEnumerator(UsageHookObject usageHook, string sortField, bool trackDelta)
            : base(usageHook.mvm, sortField)
        {
            this.usageHook = usageHook;
            this.futureTree = new FutureTree(this);
        }

        /// <summary>
        /// Current sortkey value or null
        /// </summary>
        public string CurrentSortKey
        {
            get
            {
                return this.Current!=null ? this.Current[this.sortField]:null;
            }
        }

        /// <summary>
        /// Adds an object to the usage hook
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(IObjectData obj)
        {
            // Need to determine if the added object is future or in the past.
            bool objectIsFuture=true;
            if (this.HasStarted)
            {
                string passedSortKey = obj[this.usageHook.sortField];
                if (passedSortKey.IsLt(this.CurrentSortKey))
                {
                    objectIsFuture = false;
                }
            }

            // If the object is past write it out for next time.
            if (!objectIsFuture)
            {
                //logger.Info("add object to for next pass:" + obj["dt_session"] + "," + obj["amount"]);
                this.usageHook.AddNextPassObject(obj);
            }

            // Otherwise the object is future so add it to the future tree.
            this.futureTree.AddObject(obj);
        }
    }


    



}
