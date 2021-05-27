using System;
using System.Collections.Generic;
using System.IO;

namespace AepfelUndBirnen
{
    internal class TypeInfo : IEquatable<TypeInfo>, IComparable<TypeInfo>
    {
        internal Type Type { get; }

        internal string Name { get; }

        private int HashCode { get; }

        private string FullName => this.Type.Assembly.ToString() + " " + this.Name;

        internal string PrintName => string.Format("{0}          [{1}]", this.Name, (new DirectoryInfo(this.Type.Assembly.Location)).Name);

        internal TypeInfo(Type type)
        {
            this.Type = type;
            this.Name = type.ToString();
            this.HashCode = this.Name.GetHashCode();
        }

        internal TypeInfo(TypeInfo typeInfo) : this(typeInfo.Type)
        {
        }

        #region IEquatable<TypeInfo>

        /// <summary>
        /// For <see cref="List{T}.Contains(T)"/>.
        /// </summary>
        /// <param name="other" />
        /// <returns />
        public bool Equals(TypeInfo other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.FullName == other.FullName;
            }
        }

        #endregion

        #region IComparable<TypeInfo>

        /// <summary>
        /// For <see cref="List{T}.Sort()"/>.
        /// </summary>
        /// <param name="other" />
        /// <returns />
        public int CompareTo(TypeInfo other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (this.Name == null)
            {
                if (other.Name == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return this.FullName.CompareTo(other.FullName);
            }
        }

        #endregion

        public override int GetHashCode() => this.HashCode;

        public override bool Equals(object obj)
        {
            var other = obj as TypeInfo;

            if (other == null)
            {
                return false;
            }
            else
            {
                return this.Equals(other);
            }
        }

        public override string ToString() => this.Name;
    }
}