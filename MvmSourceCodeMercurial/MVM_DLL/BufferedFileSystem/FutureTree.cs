using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NGenerics.DataStructures.Trees;
namespace MVM
{
    /// <summary>
    /// Future tree represents objects that should be returned in sorted order when the
    /// passed HookEnumerator is iterated. This tree has the ability to cut sorted files if it grows 
    /// too large. I needs a reference to the FileMerger.
    /// 
    /// 
    /// Need the concept of sorted output and unsorted...
    /// where sorted means you will always add in sorted order.
    /// unsorted means you will add in any order.
    /// 
    /// when adding in 'offline' mode it can add unsorted.
    /// when adding in online mode it must add 
    /// 
    /// hook needs to know its mode so it can write objects to the appropriate location.
    /// 
    /// need some ctrl over buffer sizes. complication is if they need to be shared.
    /// 
    /// do we ever need to keep an unsorted buffer? 
    /// 
    /// sorted object buffer
    /// </summary>
    public class FutureTree : IObjectTreeNode
    {
        private HookEnumerator looper;
        private IObjectData nextObjectData;
        private BinarySearchTree<string, LinkedList<IObjectData>> futureBtree = new BinarySearchTree<string, LinkedList<IObjectData>>();

        /// <summary>
        /// The number of objects currently in the future tree.
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// Instanciates the future tree. Has a link to the HookEnumerator because it needs to be able
        /// to remove and reinsert itself as well as split itself into real files when it grows too large.
        /// </summary>
        /// <param name="looper"></param>
        public FutureTree(HookEnumerator looper)
        {
            this.looper = looper;
        }

        /// <summary>
        /// Adds an object. 
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(IObjectData obj)
        {
            this.Count++;
            string passedSortKey = obj[this.looper.sortField];

            // if no next object, this is the next object and this tree
            // is not in the looper.
            if (this.nextObjectData == null)
            {
                this.nextObjectData = obj;
                this.looper.InsertTree(this);
                return;
            }
            // if this object can go after the next object, just add it to the btree
            // no need to reinsert in the looper.
            if (passedSortKey.IsGte(this.nextObjectData[this.looper.sortField]))
            {
                this.futureBtree.Enqueue(obj[this.looper.sortField], obj);
                return;
            }

            // otherwise, this object needs to replace the next object, which involves
            // removing the FutureTree and re-adding it.
            this.looper.RemoveTree(this);
            this.futureBtree.Enqueue(obj[this.looper.sortField], this.nextObjectData);
            this.nextObjectData = obj;
            this.looper.InsertTree(this);
        }

        /// <summary>
        /// Move to the next object in this tree, returning this or null if no more objects
        /// in the tree.
        /// </summary>
        /// <returns></returns>
        public IObjectTreeNode BackFill()
        {
            string key;
            if (this.futureBtree.TryDequeue(out key, out this.nextObjectData))
            {
                this.Count--;
                return this;
            }
            // if you backfill and there are not objects in the future tree then the 
            // future tree is not inserted.
            this.looper.FutureTreeInserted = false;
            return null;
        }

        /// <summary>
        /// Returns the current object.
        /// </summary>
        public IObjectData ObjectData
        {
            get
            {
                return this.nextObjectData;
            }
        }
    }
}
