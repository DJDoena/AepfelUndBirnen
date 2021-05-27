using System.Collections.Generic;

namespace AepfelUndBirnen
{
    internal sealed class Demo
    {

        internal void Run()
        {
            var grannySmith = new Gattung_Apfel.Art_KulturApfel.Sorte_GrannySmith();

            var goldenDelicious = new Gattung_Apfel.Art_KulturApfel.Sorte_GoldenDelicious();

            var williamsChrist = new Gattung_Birne.Art_KulturBirne.Sorte_WilliamsChrist();

            var korb = new List<Untertribus_Pyrinae>(3)
            {
                williamsChrist,
                goldenDelicious,
                grannySmith,
            };

            korb.Sort();

            foreach (var frucht in korb)
            {
                Output.WriteLine(string.Format("{0} (Kerne: {1})", frucht.Name, frucht.AnzahlKerne));
            }

            Output.WriteLine();

            var arguments = new Dictionary<string, string>(1)
            {
                [ArgsProcessor.Constants.Filter] = null,
            };

            var hierarchyPrinter = new HierarchyPrinter(arguments);

            hierarchyPrinter.PrintDerivationHierarchy(new[]
            {
                typeof(Gattung_Apfel.Art_KulturApfel.Sorte_GrannySmith),
                typeof(Gattung_Birne.Art_KulturBirne.Sorte_WilliamsChrist),
                typeof(Gattung_Apfel.Art_KulturApfel.Sorte_GoldenDelicious)
            });

            Output.WriteLine();

            arguments[ArgsProcessor.Constants.Filter] = "Apfel";

            hierarchyPrinter = new HierarchyPrinter(arguments);

            hierarchyPrinter.PrintDerivationHierarchy(typeof(Program).Assembly);

            Output.WriteLine();

            hierarchyPrinter.PrintNestingHierarchy(typeof(Gattung_Apfel.Art_KulturApfel.Sorte_GoldenDelicious).Assembly);

            Output.WriteLine();

            arguments[ArgsProcessor.Constants.Filter] = null;

            hierarchyPrinter = new HierarchyPrinter(arguments);

            hierarchyPrinter.PrintInterfaceHierarchyForInterfaces(typeof(Program).Assembly);

            Output.WriteLine();

            arguments[ArgsProcessor.Constants.Filter] = "^AepfelUndBirnen";

            hierarchyPrinter = new HierarchyPrinter(arguments);

            hierarchyPrinter.PrintInterfaceHierarchyForClasses(typeof(Program).Assembly);

            Output.WriteLine();
        }
    }
}