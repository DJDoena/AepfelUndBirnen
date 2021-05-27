using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AepfelUndBirnen
{
    internal sealed class FileProcessor
    {
        private readonly string _file;

        private readonly string _dir;

        private readonly bool _binRecursive;

        public FileProcessor(Dictionary<string, string> arguments)
        {
            _binRecursive = arguments.ContainsKey(ArgsProcessor.Constants.BinRecursive);

            if (arguments.TryGetValue(ArgsProcessor.Constants.File, out var path))
            {
                _file = path;
            }

            if (arguments.TryGetValue(ArgsProcessor.Constants.Dir, out path))
            {
                _dir = path;
            }
        }

        internal List<Type> GetTypes()
        {
            var typeList = new List<Type>();

            GetFile(typeList);

            GetDirectory(typeList);

            typeList = typeList.Distinct(new TypeEqualityComparer()).ToList();

            return typeList;
        }

        private void GetDirectory(List<Type> typeList)
        {
            if (string.IsNullOrEmpty(_dir) == false)
            {
                List<string> files;
                if (_binRecursive)
                {
                    files = new List<string>();

                    var dirs = Directory.GetDirectories(_dir, "bin", SearchOption.AllDirectories);

                    foreach (var dir in dirs)
                    {
                        files.AddRange(Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly));
                    }
                }
                else
                {
                    files = Directory.GetFiles(_dir, "*.dll", SearchOption.TopDirectoryOnly).ToList();
                }

                var tasks = new Task<Type[]>[files.Count];

                for (var fileIndex = 0; fileIndex < files.Count; fileIndex++)
                {
                    //do not put the indexer files[i] directly into the call because when the task is executed, i can have any value (delayed execution!).
                    var file = files[fileIndex];

                    tasks[fileIndex] = Task.Run(() => GetTypes(file));
                }

                Task.WaitAll(tasks);

                for (var taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
                {
                    var types = tasks[taskIndex].Result;

                    if (types != null)
                    {
                        typeList.AddRange(types);
                    }
                }
            }
        }

        private void GetFile(List<Type> typeList)
        {
            if (string.IsNullOrEmpty(_file) == false)
            {
                var types = GetTypes(_file);

                if (types != null)
                {
                    typeList.AddRange(types);
                }
            }
        }

        private static Type[] GetTypes(string file)
        {
            try
            {
                var ass = Assembly.LoadFrom(file);

                var types = ass.GetTypes();

                Output.WriteLine("Loaded: " + file);

                return types;
            }
            catch (Exception ex)
            {
                Output.WriteLine(string.Format("FAILED: {0} ({1})", file, ex.Message));

                return null;
            }
        }
    }
}