using System;
using System.Collections.Generic;

namespace AepfelUndBirnen
{
    internal sealed class TypeEqualityComparer : IEqualityComparer<Type>
    {
        #region IEqualityComparer<Type>

        /// <summary>
        /// For <see cref="System.Linq.Enumerable.Distinct{T}(IEnumerable{T})"/>
        /// </summary>
        /// <param name="left" />
        /// <param name="right" />
        /// <returns />
        public bool Equals(Type left, Type right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            else if (left == null && right != null)
            {
                return false;
            }
            else if (left != null && right == null)
            {
                return false;
            }
            else
            {
                var leftName = left.Assembly.FullName + " " + left.FullName;

                var rightName = right.Assembly.FullName + " " + right.FullName;

                return leftName == rightName;
            }
        }

        public int GetHashCode(Type obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.FullName.GetHashCode();
            }
        }

        #endregion
    }
}