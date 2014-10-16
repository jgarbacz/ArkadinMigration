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

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using NGenerics.DataStructures.General;

namespace NGenerics.UI.DataStructures.General
{
    /// <summary>
    /// Represents a dynamic data <see cref="Bag{T}"/> that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ObservableBag{T}"/>.</typeparam>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public partial class ObservableBag<T> : Bag<T>
	{

		/// <inheritdoc />
		protected override void AddItem(T item, int amount)
		{
			CheckReentrancy();
			base.AddItem(item, amount);
            OnPropertyChanged("Count", "IsEmpty", "Item[]");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item,-1));
		}


		/// <inheritdoc />
		protected override void RemoveItem(T item, int maximum, int itemCount)
		{
			CheckReentrancy();
			base.RemoveItem(item, maximum, itemCount);
			OnPropertyChanged("Count", "IsEmpty", "Item[]");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item,-1));
		}


		/// <inheritdoc />
		protected override void ClearItems()
		{
			CheckReentrancy();
			base.ClearItems();
			OnPropertyChanged("Count", "IsEmpty", "Item[]");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

	}
}