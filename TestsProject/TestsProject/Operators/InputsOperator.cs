using System;

namespace TestsProject
{
    public class InputsOperator
    {
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
