using System;
using System.Collections.Generic;

namespace AepfelUndBirnen
{
    /// <summary />
    public static class Program
    {
        private static Dictionary<string, string> Arguments { get; set; }

        /// <summary />
        public static void Main(string[] args)
        {
            try
            {
                Arguments = (new ArgsProcessor()).GetArgs(args);

                if (Arguments != null)
                {
                    Output.WriteLine("Running with:");

                    foreach (var kvp in Arguments)
                    {
                        Output.WriteLine("/" + kvp.Key + " " + kvp.Value);
                    }

                    Console.WriteLine();

                    if (Arguments.ContainsKey(ArgsProcessor.Constants.Demo))
                    {
                        (new Demo()).Run();
                    }
                    else
                    {
                        var typeList = (new FileProcessor(Arguments)).GetTypes();

                        PrintHierarchies(typeList);
                    }

                }

                Output.ReadLine();
            }
            catch (Exception ex)
            {
                do
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine();

                    ex = ex.InnerException;
                } while (ex != null);

                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }
        }

        private static void PrintHierarchies(List<Type> typeList)
        {
            var hierarchyPrinter = new HierarchyPrinter(Arguments);

            if (Arguments.ContainsKey(ArgsProcessor.Constants.ClassInheritance))
            {
                hierarchyPrinter.PrintDerivationHierarchy(typeList.ToArray());

                Output.WriteLine();
            }

            if (Arguments.ContainsKey(ArgsProcessor.Constants.InterfaceInheritance))
            {
                hierarchyPrinter.PrintInterfaceHierarchyForInterfaces(typeList.ToArray());

                Output.WriteLine();
            }

            if (Arguments.ContainsKey(ArgsProcessor.Constants.InterfaceImplementation))
            {
                hierarchyPrinter.PrintInterfaceHierarchyForClasses(typeList.ToArray());

                Output.WriteLine();
            }

            if (Arguments.ContainsKey(ArgsProcessor.Constants.ClassNesting))
            {
                hierarchyPrinter.PrintNestingHierarchy(typeList.ToArray());

                Output.WriteLine();
            }
        }
    }
}