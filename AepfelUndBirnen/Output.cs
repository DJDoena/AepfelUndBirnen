using System;
using System.IO;
using System.Text;

namespace AepfelUndBirnen
{
    internal static class Output
    {
        private static readonly StreamWriter _writer;

        static Output()
        {
            _writer = new StreamWriter("output.csv", false, Encoding.UTF8);
        }

        internal static void Write(string text, int level = 0)
        {
            GetSpecificTexts(text, level, out var consoleText, out var fileText);

            Console.Write(consoleText);

            _writer.Write(fileText);
        }

        internal static void WriteLine(string text = null, int level = 0)
        {
            GetSpecificTexts(text, level, out var consoleText, out var fileText);

            Console.WriteLine(consoleText);

            _writer.WriteLine(fileText);
        }

        internal static void ReadLine()
        {
            _writer.WriteLine("---EOF---");
            _writer.Flush();

            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }

        private static void GetSpecificTexts(string text, int level, out string consoleText, out string fileText)
        {
            consoleText = text;
            fileText = text;

            if (level > 0)
            {
                var tempConsole = string.Empty;

                var tempFile = string.Empty;

                for (var levelIndex = 0; levelIndex < level; levelIndex++)
                {
                    tempConsole += "    ";

                    tempFile += ";";
                }

                consoleText = tempConsole + consoleText;

                fileText = tempFile + fileText;
            }
        }
    }
}