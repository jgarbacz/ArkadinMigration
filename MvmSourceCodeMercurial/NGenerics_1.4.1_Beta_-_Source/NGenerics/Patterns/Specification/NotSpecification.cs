﻿/*  
  Copyright 2007-2009 The NGenerics Team
 (http://code.google.com/p/ngenerics/wiki/Team)

 This program is licensed under the GNU Lesser General Public License (LGPL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://www.gnu.org/copyleft/lesser.html.
*/

using NGenerics.Util;

namespace NGenerics.Patterns.Specification
{
    /// <summary>
    /// Performs an OR operation between two specifications.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotSpecification<T> : AbstractSpecification<T>
    {
        #region Globals

        private readonly ISpecification<T> specification;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        public NotSpecification(ISpecification<T> specification)
        {
            #region Validation

            Guard.ArgumentNotNull(specification, "specification");

            #endregion

            this.specification = specification;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the specification.
        /// </summary>
        /// <value>The specification.</value>
        public ISpecification<T> Specification
        {
            get
            {
                return specification;
            }
        }

        #endregion

        #region AbstractSpecification<T> Members

        /// <summary>
        /// Determines whether the specification is satisfied by the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the specified item; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSatisfiedBy(T item)
        {
            return !specification.IsSatisfiedBy(item);
        }

        #endregion
    }
}
