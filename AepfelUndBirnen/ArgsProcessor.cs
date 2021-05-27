using System;
using System.Collections.Generic;
using System.IO;

namespace AepfelUndBirnen
{
    internal sealed class ArgsProcessor
    {
        #region Nested Classes

        internal static class Constants
        {
            internal const string Demo = "demo";

            internal const string Dir = "dir";

            internal const string File = "file";

            internal const string Filter = "filter";

            internal const string ShowNonDependencies = "shownondependencies";

            internal const string InterfaceInheritance = "interfaceinheritance";

            internal const string ClassInheritance = "classinheritance";

            internal const string ClassNesting = "classnesting";

            internal const string InterfaceImplementation = "interfaceimplementation";

            internal const string BinRecursive = "binrecursive";
        }

        #endregion

        internal Dictionary<string, string> GetArgs(string[] inArgs)
        {
            var outArgs = new Dictionary<string, string>();

            if (inArgs != null && inArgs.Length > 0)
            {
                for (var argIndex = 0; argIndex < inArgs.Length; argIndex++)
                {
                    if (inArgs[argIndex].StartsWith("/"))
                    {
                        var argKey = inArgs[argIndex].Substring(1).ToLower();

                        switch (argKey)
                        {
                            #region Demo
                            case Constants.Demo:
                                {
                                    outArgs = new Dictionary<string, string>(1)
                                    {
                                        [Constants.Demo] = string.Empty,
                                    };

                                    return (outArgs);
                                }
                            #endregion
                            #region Dir
                            case Constants.Dir:
                                {
                                    if ((argIndex + 1) < inArgs.Length)
                                    {
                                        var dir = inArgs[argIndex + 1];

                                        if (Directory.Exists(dir) == false)
                                        {
                                            Console.WriteLine("Invalid /dir path!");

                                            PrintHelp();

                                            return null;
                                        }
                                        else
                                        {
                                            outArgs[Constants.Dir] = dir;

                                            argIndex++;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid /dir path!");

                                        PrintHelp();

                                        return null;
                                    }

                                    break;
                                }
                            #endregion
                            #region File
                            case Constants.File:
                                {
                                    if ((argIndex + 1) < inArgs.Length)
                                    {
                                        var file = inArgs[argIndex + 1];

                                        if (File.Exists(file) == false)
                                        {
                                            Console.WriteLine("Invalid /file path!");

                                            PrintHelp();

                                            return null;
                                        }
                                        else
                                        {
                                            outArgs[Constants.File] = file;

                                            argIndex++;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid /file path!");

                                        PrintHelp();

                                        return null;
                                    }

                                    break;
                                }
                            #endregion
                            #region Filter
                            case Constants.Filter:
                                {
                                    if ((argIndex + 1) < inArgs.Length)
                                    {
                                        var filter = inArgs[argIndex + 1];

                                        outArgs[Constants.Filter] = filter;

                                        argIndex++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid /file path!");

                                        PrintHelp();

                                        return null;
                                    }

                                    break;
                                }
                            #endregion
                            #region InterfaceInheritance
                            case Constants.InterfaceInheritance:
                                {
                                    outArgs[Constants.InterfaceInheritance] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region ClassInheritance
                            case Constants.ClassInheritance:
                                {
                                    outArgs[Constants.ClassInheritance] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region ClassNesting
                            case Constants.ClassNesting:
                                {
                                    outArgs[Constants.ClassNesting] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region InterfaceImplementation
                            case Constants.InterfaceImplementation:
                                {
                                    outArgs[Constants.InterfaceImplementation] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region ShowNonDependencies
                            case Constants.ShowNonDependencies:
                                {
                                    outArgs[Constants.ShowNonDependencies] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region BinRecursive
                            case Constants.BinRecursive:
                                {
                                    outArgs[Constants.BinRecursive] = string.Empty;

                                    break;
                                }
                            #endregion
                            #region Help
                            case "help":
                                {
                                    PrintHelp();

                                    return null;
                                }
                            #endregion
                            #region Default
                            default:
                                {
                                    Console.WriteLine(string.Format("Invalid argument /{0}!", argKey));

                                    PrintHelp();

                                    return null;
                                }
                                #endregion
                        }
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid argument {0}!", inArgs[argIndex]));

                        PrintHelp();

                        return null;
                    }

                }

            }
            else
            {
                PrintHelp();

                return null;
            }

            return outArgs;
        }

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine();
            Console.WriteLine("/help                    --- shows this help, all other arguments are ignored");
            Console.WriteLine();
            Console.WriteLine("/demo                    --- starts the demo, all other arguments are ignored");
            Console.WriteLine();
            Console.WriteLine("/dir <directory>         --- specifies the folder from which to search for DLLs");
            Console.WriteLine();
            Console.WriteLine("/file <file>             --- specifies the DLL");
            Console.WriteLine();
            Console.WriteLine("/filter <filter>         --- applies a name filter on all entries");
            Console.WriteLine("                               Example:");
            Console.WriteLine("                                 /filter Exception --- includes all type names containing \"Exception\"");
            Console.WriteLine("                                 /filter ^System   --- includes all type names starting with \"System\"");
            Console.WriteLine("                                 /filter Manager$  --- includes all type names ending with \"Manager\"");
            Console.WriteLine("                                 /filter ^Helper$  --- includes all type names being equal to \"Helper\"");
            Console.WriteLine();
            Console.WriteLine("/interfaceDependencies   --- shows the inheritance of interfaces to one another");
            Console.WriteLine();
            Console.WriteLine("/classDependencies       --- shows the inheritance of classes to one another");
            Console.WriteLine();
            Console.WriteLine("/classNesting            --- shows the nesting of classes to one another");
            Console.WriteLine();
            Console.WriteLine("/interfaceImplementation --- shows the implemention of interfaces by classes");
            Console.WriteLine();
            Console.WriteLine("/showNonDependencies     --- shows entries that have no dependencies");
            Console.WriteLine();
        }
    }
}