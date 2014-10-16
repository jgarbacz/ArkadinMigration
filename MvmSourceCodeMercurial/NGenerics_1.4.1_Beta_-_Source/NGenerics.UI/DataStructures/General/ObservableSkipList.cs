/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/
#if (!SILVERLIGHT)
using System;
#endif
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using NGenerics.DataStructures.General;

namespace NGenerics.UI.DataStructures.General
{
    /// <summary>
    /// Represents a dynamic data <see cref="SkipList{TKey,TValue}"/> that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public partial class ObservableSkipList<TKey, TValue> : SkipList<TKey, TValue>
	{
        /// <summary>
        /// Adds the item to the collection.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The value to add to the colleciton.</param>
        /// <remarks>
        /// 	<b>Notes to Inheritors: </b>
        /// Derived classes can override this method to change the behavior of the Add(TKey,TValue) method.
        /// </remarks>
		protected override void AddItem(TKey key, TValue value)
		{
			CheckReentrancy();
			base.AddItem(key, value);
			OnPropertyChanged("Count","CurrentListLevel", "IsEmpty", "Values");
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value), -1));
		}


        /// <summary>
        /// Removes the item from the collection.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns></returns>
        /// <remarks>
        /// 	<b>Notes to Inheritors: </b>
        /// Derived classes can override this method to change the behavior of the Remove(TKey) method.
        /// </remarks>
		protected override bool RemoveItem(TKey key)
		{
			CheckReentrancy();
			TValue value;
			if (TryGetValue(key, out value))
			{
				base.RemoveItem(key);
				OnPropertyChanged("Count", "CurrentListLevel", "IsEmpty", "Values");
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value),-1));
				return true;
			}
				return false;
		}


		/// <inheritdoc />
		protected override void ClearItems()
		{
			CheckReentrancy();
			base.ClearItems();
			OnPropertyChanged("Count", "CurrentListLevel", "IsEmpty", "Values");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		
	}
}