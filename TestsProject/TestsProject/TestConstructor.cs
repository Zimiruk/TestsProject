using System;
using System.Collections.Generic;

namespace TestsProject
{
    class TestConstructor
    {
        public void CreateTest()
        {
            Console.WriteLine("Enter test theme");
            string testTheme = Console.ReadLine();

            Test test = new Test(testTheme);

            while (true)
            {
                if (InputsOperator.ProcessYesNoInput("Add a question for this test Y/N?"))
                {
                    CreateQuestion(test);
                }

                else
                {
                    FilesOperator.SaveTest(test);
                    break;
                }
            }
        }

        private void CreateQuestion(Test test)
        {
            Console.WriteLine("Enter the content of the question");

            string questionContent = Console.ReadLine();
            Question question = new Question(questionContent);

            test.Questions.Add(question);
            AddOptionsToQuestion(question);
        }

        private void AddOptionsToQuestion(Question question)
        {
            Console.WriteLine("Add from 2 to 5 question answers");

            for (int i = 0; i < 5; i++)
            {
                if (i >= 2)
                {
                    if (!InputsOperator.ProcessYesNoInput("Add an answer for current question Y/N?"))
                        break;
                }

                Console.WriteLine("Enter an answer");

                string content = Console.ReadLine();
                bool isItRight = InputsOperator.ProcessYesNoInput("Is it right answer? Y/N?");

                question.Answers.Add(new Answer(content, isItRight));
            }

            if (!CheckForRightAnswers(question.Answers))
            {
                SetRightAnswers(question);
            }

            Console.WriteLine("Question added");
        }

        private bool CheckForRightAnswers(List<Answer> answers)
        {
            foreach (Answer answer in answers)
            {
                if (answer.IsItRight)
                    return true;
            }

            return false;
        }

        private void SetRightAnswers(Question question)
        {
            Console.WriteLine("There no right answers for current question");

            while (true)
            {
                Console.WriteLine("Choose number of answer that should be right");

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (!question.Answers[i].IsItRight)
                        Console.WriteLine($"{i + 1} - {question.Answers[i].Content}");
                }

                while (true)
                {
                    string input = Console.ReadLine();
                    if (AnswerCheck(input, question.Answers))
                    {
                        int i = int.Parse(input);

                        if (InputsOperator.ProcessYesNoInput($"You want answer {i} - {question.Answers[i - 1].Content} to be correct?"))
                        {
                            question.Answers[i - 1].IsItRight = true;
                            break;
                        }
                    }

                    else
                    {
                        Console.WriteLine("Bad input");
                    }
                }

                if (!InputsOperator.ProcessYesNoInput("Add more right answers?"))
                    break;
            }
        }


       private bool AnswerCheck(string input, List<Answer> answers)
        {
            int value;

            if (int.TryParse(input, out value))
            {
                if (answers.Count > value && value > 0)
                    return true;
            }

            return false;
        }

    }
}
