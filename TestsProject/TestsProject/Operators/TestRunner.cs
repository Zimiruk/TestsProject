using System;

namespace TestsProject
{
    class TestRunner
    {
        public void RunTest(Test test)
        {
            Console.Clear();
            Console.WriteLine($"Test theme is {test.Theme}");
            Console.WriteLine("******************************");
            Console.WriteLine();

            bool[] testResult = new bool[test.Questions.Count];

            for (int i = 0; i < test.Questions.Count; i++)
            {
                Console.WriteLine($"Question number: {i + 1}");
                testResult[i] = AskQuestion(test.Questions[i]);
            }

            Showresult(testResult);
        }


        private void Showresult(bool[] testResult)
        {
            Console.WriteLine("Test results");

            for (int i = 0; i < testResult.Length; i++)
            {
                Console.WriteLine($"Question number {i + 1} result: {testResult[i]}");
            }
        }

        private bool AskQuestion(Question question)
        {

            bool[] answerResult = new bool[question.Answers.Count];
            bool result = false;

            while (true)
            {
                Console.WriteLine(question.QuestionContent);

                Console.WriteLine();

                Console.WriteLine("Choose answer:");

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {question.Answers[i].Content}");
                    answerResult[i] = false;
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                int value;

                if (int.TryParse(key.KeyChar.ToString(), out value))
                {
                    if (value <= question.Answers.Count)
                    {
                        Console.WriteLine($"You choose answer: {question.Answers[value - 1].Content}");
                        answerResult[value - 1] = true;

                        result = CheckAnswer(question, answerResult);

                        Console.WriteLine();
                        if (!InputsOperator.ProcessYesNoInput("Choose more right answers for current question? Y/N"))
                        {
                            Console.WriteLine();
                            break;
                        }
                    }
                }

                Console.WriteLine();
            }

            return result;

        }

        private bool CheckAnswer(Question question, bool[] answerResult)
        {
            for (int i = 0; i < question.Answers.Count; i++)
            {
                if (question.Answers[i].IsItRight != answerResult[i])
                    return false;
            }

            return true;
        }
    }
}