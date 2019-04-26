/*
    +==========================================+
    +             NetJsonTest v1.0             +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JonasFX.CliShiv;

namespace NetJsonTest
{
    class Program
    {
        /// <summary>
        /// This removes possible control-characters from a string.
        /// </summary>
        /// <param name="String">The input string.</param>
        /// <returns></returns>
        public static string Sanitize(string String)
        {
            StringBuilder Builder = new StringBuilder(String.Length);
            foreach (char Character in String)
            {
                if (Character > 127)
                    continue;
                if (Character < 32)
                    continue;
                Builder.Append(Character);
            }
            return Builder.ToString();
        }
        
        public static List<ParserTest> Tests = new List<ParserTest>();
        public static int IrrelevantTests      = 0;
        public static int RelevantTests        = 0;
        public static int PassedRelevantTest   = 0;
        public static int PassedIrrelevantTest = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("JSON parser tester");
            Console.WriteLine("------------------\n");

            Console.WriteLine("Test source: http://seriot.ch/parsing_json.php");
            Console.WriteLine("             https://github.com/nst/JSONTestSuite\n");

            // Check existence of 'tests' directory
            if (!Directory.Exists("tests"))
            {
                Console.WriteLine("Critical: The 'tests'-directory doesn't exist.");
                Environment.Exit(1);
            }

            // Load the test files and validate them
            Console.WriteLine("[Loading tests, please wait.]");
            foreach (var Filename in Directory.GetFiles("tests"))
            {
                var Name = Path.GetFileNameWithoutExtension(Filename);
                if (!Name.StartsWith("y_") && !Name.StartsWith("n_") && !Name.StartsWith("i_"))
                {
                    Console.WriteLine("Warning: The filename of '" + Name + "' doesn't follow the naming guide.");
                    continue;
                }
                var Content = "";
                try
                {
                    Content = File.ReadAllText(Filename);
                } catch (Exception)
                {
                    Console.WriteLine("Critical: The content of '" + Name + "' couldn't be read.");
                    Environment.Exit(1);
                }
                if (Name.StartsWith("i_"))
                    IrrelevantTests++;
                else
                    RelevantTests++;
                Tests.Add(new ParserTest(
                    Name.Remove(0, 2),
                    Name.StartsWith("y_") ? ValidationResult.Pass : (Name.StartsWith("n_") ? ValidationResult.Failure : ValidationResult.Irrelevant),
                    Content
                ));
            }

            // Dump counts
            Console.WriteLine("\n+====================================+");
            Console.WriteLine("  Relevant tests found:   " + RelevantTests.ToString());
            Console.WriteLine("  Irrelevant tests found: " + IrrelevantTests.ToString());
            Console.WriteLine("+====================================+\n");

            // Validate and store result
            Console.WriteLine("[Testing parser, please wait.]");
            foreach (var Test in Tests)
                Test.Output = Validator.Validate(Test.Content);

            // Prepare and the table
            Console.WriteLine("[Displaying table.]\n");
            var Table = new TextTable(new string[] { "Name", "Expected", "Result", "Message", "Content" }, true);
            ValidationResult? LastResult = null;
            foreach (var Test in Tests)
            {
                // Sort by expected result
                if (LastResult != null)
                {
                    if (LastResult != Test.Expected)
                    {
                        Table.AddBreak();
                        LastResult = Test.Expected;
                    }
                }
                else
                    LastResult = Test.Expected;

                if (Test.Expected == ValidationResult.Irrelevant && Test.Output.Result == ValidationResult.Pass)
                    PassedIrrelevantTest++;
                else if (Test.Expected != ValidationResult.Irrelevant && Test.Expected == Test.Output.Result)
                    PassedRelevantTest++;

                // Add the next row data
                Table.AddRow(new object[] {
                    Test.Name,
                    Test.Expected,
                    Test.Output.Result,
                    Test.Output.Message,

                    // Sanitize the content (we don't want beeps and newlines)
                    (Test.Content.Length > 15) ? (
                        TextTable.Pad(Sanitize(Test.Content), 12) + "..."
                    ) : (
                        TextTable.Pad(Sanitize(Test.Content), 15)
                    )
                });
            }

            // Render and output the table
            Console.WriteLine(Table.GetString(true));

            // Dump pass-count
            Console.WriteLine("\n+====================================+");
            Console.WriteLine("  Passed " + PassedRelevantTest.ToString() + " of " + RelevantTests.ToString() + " relevant tests.");
            Console.WriteLine("  Passed " + PassedIrrelevantTest.ToString() + " of " + IrrelevantTests.ToString() + " irrelevant tests.");
            Console.WriteLine("+====================================+");

            // RFC notice when everything relevant passed.
            if (PassedRelevantTest == RelevantTests)
                Console.WriteLine("\n=> All relevant tests passed, the parser is *most likely* RFC 8259 compliant.");
        }
    }
}