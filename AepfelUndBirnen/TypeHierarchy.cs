using System;
using System.Collections.Generic;

namespace AepfelUndBirnen
{
    [System.Diagnostics.DebuggerDisplay("{Name}, TypeInfos={TypeInfos.Count}")]
    internal sealed class TypeHierarchy : TypeInfo
    {
        internal readonly Dictionary<TypeInfo, TypeHierarchy> TypeInfos;

        internal TypeHierarchy(Type type) : base(type)
        {
            TypeInfos = new Dictionary<TypeInfo, TypeHierarchy>();
        }

        internal TypeHierarchy(TypeInfo typeInfo) : this(typeInfo.Type)
        {
        }
    }
}