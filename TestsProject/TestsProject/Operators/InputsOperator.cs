using System;

namespace TestsProject
{
    public class InputsOperator
    {

        public static void ProcessStartInputs()
        {
            while (true)
            {
                Console.WriteLine("Choose option \n 1 - Create new test \n 2 - Open existing test \n 3 - Exit");
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.D1)
                {
                    TestConstructor testConstructor = new TestConstructor();
                    testConstructor.CreateTest();
                }

                else if (key.Key == ConsoleKey.D2)
                {
                    Console.WriteLine("Enter name of *.test file");
                    string testName = Console.ReadLine();

                    if (FilesOperator.CheckIfTestExists(testName))
                    {
                        Test test = FilesOperator.OpenTest(testName);
                        TestRunner testRunner = new TestRunner();
                        testRunner.RunTest(test);
                    }
                    else
                    {
                        Console.WriteLine("No such test exists in the working directory");
                    }
                }

                else if (key.Key == ConsoleKey.D3)
                {
                    break;
                }
            }
        }

        public static bool ProcessYesNoInput(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }

                else if (key.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }
    }
}
