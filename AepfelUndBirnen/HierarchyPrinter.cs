using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AepfelUndBirnen
{
    internal sealed class HierarchyPrinter
    {
        private readonly bool _showNonDependencies;

        private readonly string _filter;

        internal HierarchyPrinter(Dictionary<string, string> arguments)
        {
            _showNonDependencies = arguments.ContainsKey(ArgsProcessor.Constants.ShowNonDependencies);

            arguments.TryGetValue(ArgsProcessor.Constants.Filter, out _filter);
        }

        #region PrintHierarchy

        #region PrintInterfaceHierarchy

        #region PrintInterfaceHierarchyForInterfaces
        internal void PrintInterfaceHierarchyForInterfaces(Assembly ass)
        {
            var types = ass.GetTypes();

            this.PrintInterfaceHierarchyForInterfaces(types);
        }

        internal void PrintInterfaceHierarchyForInterfaces(Type[] types)
        {
            Output.WriteLine("Interface Hierarchy: ");
            Output.WriteLine();

            if (types != null)
            {
                var list = GetFilteredList(types, type => type.IsInterface);

                if (list.Count > 0)
                {
                    var inverse = GetBaseInterfacesLists(list);

                    Output.WriteLine("Inverse (Derives From): ");
                    Output.WriteLine();

                    this.PrintHierarchy(inverse);

                    Output.WriteLine();

                    var forward = GetHierarchy(inverse, true);

                    Output.WriteLine("Forward (Is Derived By): ");
                    Output.WriteLine();

                    this.PrintHierarchy(forward);
                }
            }
        }

        #endregion

        #region PrintInterfaceHierarchyForClasses
        internal void PrintInterfaceHierarchyForClasses(Assembly ass)
        {
            var types = ass.GetTypes();

            this.PrintInterfaceHierarchyForClasses(types);
        }

        internal void PrintInterfaceHierarchyForClasses(Type[] types)
        {
            Output.WriteLine();
            Output.WriteLine("Interface Implementation: ");
            Output.WriteLine();

            if (types != null)
            {
                var list = GetFilteredList(types, type => type.IsInterface == false);

                var inverse = GetBaseInterfacesLists(list);

                Output.WriteLine("Inverse (Implements): ");
                Output.WriteLine();

                this.PrintHierarchy(inverse);

                Output.WriteLine();

                var forward = GetHierarchy(inverse);

                Output.WriteLine("Forward (Is Implemented By): ");
                Output.WriteLine();

                this.PrintHierarchy(forward);
            }
        }

        #endregion

        private static Dictionary<TypeInfo, TypeHierarchy> GetHierarchy(Dictionary<TypeInfo, TypeHierarchy> inverse, bool isInterfaces = false)
        {
            var forward = new Dictionary<TypeInfo, TypeHierarchy>();

            foreach (var kvp in inverse)
            {
                if (isInterfaces)
                {
                    var type = new TypeInfo(kvp.Key);

                    if (forward.TryGetValue(type, out var types) == false)
                    {
                        types = new TypeHierarchy(type);

                        forward.Add(type, types);
                    }
                }

                foreach (var hierarchy in kvp.Value.TypeInfos.Values)
                {
                    var type = new TypeInfo(hierarchy);

                    if (forward.TryGetValue(type, out var types) == false)
                    {
                        types = new TypeHierarchy(type);

                        forward.Add(type, types);
                    }

                    types.TypeInfos.Add(kvp.Key, new TypeHierarchy(kvp.Key));
                }
            }

            return forward;
        }

        private static Dictionary<TypeInfo, TypeHierarchy> GetBaseInterfacesLists(List<Type> types)
        {
            var inverses = new Dictionary<TypeInfo, TypeHierarchy>(types.Count);

            var tasks = new Task<TypeHierarchy>[types.Count];

            for (var typeIndex = 0; typeIndex < types.Count; typeIndex++)
            {
                //do not put the indexer types[i] directly into the call because when the task is executed, i can have any value (delayed execution!).
                var type = types[typeIndex];

                tasks[typeIndex] = Task.Run(() => GetBaseInterfaceList(type));
            }

            Task.WaitAll(tasks);

            for (var taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
            {
                var hierarchy = tasks[taskIndex].Result;

                inverses.Add(new TypeInfo(hierarchy), hierarchy);
            }

            return inverses;
        }

        private static TypeHierarchy GetBaseInterfaceList(Type type)
        {
            var hierarchy = new TypeHierarchy(type);

            var interfaces = type.GetInterfaces();

            foreach (var intf in interfaces)
            {
                //Interfaces are flattened, no need to recurse the hierarchy upwards
                hierarchy.TypeInfos.Add(new TypeInfo(intf), new TypeHierarchy(intf));
            }

            return hierarchy;
        }

        #endregion

        #region PrintDerivationHierarchy

        internal void PrintDerivationHierarchy(Assembly ass)
        {
            var types = ass.GetTypes();

            this.PrintDerivationHierarchy(types);
        }

        internal void PrintDerivationHierarchy(Type[] types)
        {
            Output.WriteLine("Class Hierarchy: ");
            Output.WriteLine();

            if (types != null)
            {
                var list = GetFilteredList(types, type => type.IsInterface == false);

                if (list.Count > 0)
                {
                    var lists = GetBaseTypeLists(list);

                    var root = GetHierarchy(lists);

                    this.PrintHierarchy(root);
                }
            }
        }

        private static List<List<TypeInfo>> GetBaseTypeLists(List<Type> types)
        {
            var lists = new List<List<TypeInfo>>(types.Count);

            var tasks = new Task<List<TypeInfo>>[types.Count];

            for (var typeIndex = 0; typeIndex < types.Count; typeIndex++)
            {
                //do not put the indexer types[i] directly into the call because when the task is executed, i can have any value (delayed execution!).
                var type = types[typeIndex];

                tasks[typeIndex] = Task.Run(() => GetBaseTypeList(type));
            }

            Task.WaitAll(tasks);

            for (var taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
            {
                var list = tasks[taskIndex].Result;

                lists.Add(list);
            }

            return lists;
        }

        private static List<TypeInfo> GetBaseTypeList(Type type)
        {
            var list = new List<TypeInfo>();

            do
            {
                list.Insert(0, new TypeInfo(type));

                type = type.BaseType;
            } while (type != null);

            return list;
        }

        #endregion

        #region PrintNestingHierarchy

        internal void PrintNestingHierarchy(Assembly ass)
        {
            var types = ass.GetTypes();

            this.PrintNestingHierarchy(types);
        }

        internal void PrintNestingHierarchy(Type[] types)
        {
            Output.WriteLine("Nesting Hierarchy: ");
            Output.WriteLine();

            if (types != null)
            {
                var list = GetFilteredList(types, type => type.IsInterface == false);

                if (list.Count > 0)
                {
                    var lists = GetNestingTypeLists(list);

                    var root = GetHierarchy(lists);

                    this.PrintHierarchy(root);
                }
            }
        }

        private static List<List<TypeInfo>> GetNestingTypeLists(List<Type> types)
        {
            var lists = new List<List<TypeInfo>>(types.Count);

            var tasks = new Task<List<TypeInfo>>[types.Count];

            for (var typeIndex = 0; typeIndex < types.Count; typeIndex++)
            {
                //do not put the indexer types[i] directly into the call because when the task is executed, i can have any value (delayed execution!).
                var type = types[typeIndex];

                tasks[typeIndex] = Task.Run(() => GetNestingTypeList(type));
            }

            Task.WaitAll(tasks);

            for (var taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
            {
                var list = tasks[taskIndex].Result;

                lists.Add(list);
            }

            return lists;
        }

        private static List<TypeInfo> GetNestingTypeList(Type type)
        {
            var list = new List<TypeInfo>();

            do
            {
                list.Insert(0, new TypeInfo(type));

                type = type.DeclaringType;
            } while (type != null);

            return list;
        }

        #endregion

        private void PrintHierarchy(Dictionary<TypeInfo, TypeHierarchy> root)
        {
            var filtered = this.FilterHiararchy(root);

            this.PrintHierarchy(filtered, 0);
        }

        private void PrintHierarchy(Dictionary<TypeInfo, TypeHierarchy> hierarchy, int level)
        {
            var list = new List<TypeInfo>(hierarchy.Keys);

            list.Sort();

            foreach (var typeInfo in list)
            {
                var typeHierarchy = hierarchy[typeInfo];

                if (_showNonDependencies == false && level == 0 && typeHierarchy.TypeInfos.Count == 0)
                {
                    continue;
                }

                Output.WriteLine(typeInfo.PrintName, level);

                this.PrintHierarchy(typeHierarchy.TypeInfos, level + 1);
            }
        }

        private static Dictionary<TypeInfo, TypeHierarchy> GetHierarchy(List<List<TypeInfo>> lists)
        {
            var root = new Dictionary<TypeInfo, TypeHierarchy>();

            foreach (var list in lists)
            {
                var hierarchy = root;

                foreach (var typeInfo in list)
                {
                    if (hierarchy.TryGetValue(typeInfo, out var nextlevel) == false)
                    {
                        nextlevel = new TypeHierarchy(typeInfo);

                        hierarchy.Add(typeInfo, nextlevel);
                    }

                    hierarchy = nextlevel.TypeInfos;
                }
            }

            return root;
        }

        private static List<Type> GetFilteredList(Type[] types, Func<Type, bool> filter) => types.Where(filter).ToList();

        private Dictionary<TypeInfo, TypeHierarchy> FilterHiararchy(Dictionary<TypeInfo, TypeHierarchy> root)
        {
            if (string.IsNullOrEmpty(_filter) == false)
            {
                var filtered = this.ApplyFilter(root);

                return filtered;
            }
            else
            {
                return new Dictionary<TypeInfo, TypeHierarchy>(root);
            }
        }

        private Dictionary<TypeInfo, TypeHierarchy> ApplyFilter(Dictionary<TypeInfo, TypeHierarchy> root)
        {
            var filtered = new Dictionary<TypeInfo, TypeHierarchy>(root.Count);

            foreach (var kvp in root)
            {
                var subFiltered = this.ApplyFilter(kvp);

                if (subFiltered != null)
                {
                    filtered.Add(kvp.Key, subFiltered);
                }
            }

            return filtered;
        }

        private TypeHierarchy ApplyFilter(KeyValuePair<TypeInfo, TypeHierarchy> kvp)
        {
            if (this.MatchesFilter(kvp.Value, out var filteredHierarchy))
            {
                return filteredHierarchy;
            }
            else if (this.MatchesFilter(kvp.Key))
            {
                return new TypeHierarchy(kvp.Key);
            }
            else
            {
                return null;
            }
        }

        private bool MatchesFilter(TypeInfo typeInfo)
        {
            var mustStart = false;

            var mustEnd = false;

            var name = typeInfo.Name;

            var filter = _filter;

            if (filter[0] == '^')
            {
                mustStart = true;

                filter = filter.Substring(1);
            }

            if (filter[filter.Length - 1] == '$')
            {
                mustEnd = true;

                filter = filter.Remove(filter.Length - 1);
            }

            if (mustStart && mustEnd)
            {
                return name.Equals(filter);
            }
            else if (mustStart)
            {
                return name.StartsWith(filter);
            }
            else if (mustEnd)
            {
                return name.EndsWith(filter);
            }
            else
            {
                return name.Contains(filter);
            }
        }

        private bool MatchesFilter(TypeHierarchy typeHierarchy, out TypeHierarchy filteredHierarchy)
        {
            var filtered = this.ApplyFilter(typeHierarchy.TypeInfos);

            filteredHierarchy = new TypeHierarchy(typeHierarchy);

            foreach (var kvp in filtered)
            {
                filteredHierarchy.TypeInfos.Add(kvp.Key, kvp.Value);
            }

            return filtered.Count > 0;
        }

        #endregion
    }
}