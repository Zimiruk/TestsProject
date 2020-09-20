using Common.Models;
using Data;
using System.Collections.Generic;

namespace Business
{
    public class TestsLogic
    {
        private TestFilesOperations operations = new TestFilesOperations();
        private StatisticLogic statisticLogic = new StatisticLogic();

        public void SaveTest(Test test)
        {
            operations.SaveTest(test);
        }

        public List<string> ShowTestsNames()
        {
            return operations.GetTestsNames();
        }

        public Test GetTest(string testName)
        {
            return operations.GetTest(testName);
        }

        public CreationReport ValidateCreation(Test test)
        {
            CreationReport executionReport = new CreationReport();
            executionReport.BadQuestions = new List<int>();

            if(test.Questions.Count == 0)
            {
                executionReport.Result = false;
                executionReport.Message += "No questions at all";

                return executionReport;
            }

            for (int i = 0; i < test.Questions.Count; i++)
            {
                if (!test.Questions[i].Answers.Exists(x => x.IsItRight == true))
                {
                    executionReport.BadQuestions.Add(i);
                    executionReport.Message += $"Question number {i + 1} does not have any right answer \n";
                }
            }

            if (executionReport.BadQuestions.Count == 0)
            {
                executionReport.Message += "All fine";
                executionReport.Result = true;
            }

            else
            {
                executionReport.Result = false;
            }

            return executionReport;
        }



        private Result CreateResult(int rightAmount, Dictionary<int, List<int>> wrongChoises, int questionsAmount)
        {
            Result result = new Result();
            result.AmountOfRight = rightAmount;
            int rightAnswersCount = questionsAmount - wrongChoises.Count;

            if (rightAmount > rightAnswersCount)
            {
                result.Passed = false;
            }

            else
            {
                result.Passed = true;
            }

            result.Message = $"{rightAnswersCount} / {questionsAmount}";
            return result;
        }


        public Dictionary<int, List<int>> FinishTest(Test finishedTest, Test testToCompare, out Result result)
        {
            Dictionary<int, List<int>> wrongChoises = new Dictionary<int, List<int>>();

            ///TODO Change that 4x if 
            for (int i = 0; i < testToCompare.Questions.Count; i++)
            {                
                if (testToCompare.Questions[i].IsOpen)
                {
                    if(!(testToCompare.Questions[i].Answers[0].Content == finishedTest.Questions[i].Answers[0].Content))
                        wrongChoises.Add(i, new List<int> { 0 });

                    continue;
                }       

                if (!finishedTest.Questions[i].Answers.Exists(x => x.IsItRight == true))
                {
                    wrongChoises.Add(i, new List<int>());
                }

                else
                {
                    for (int j = 0; j < testToCompare.Questions[i].Answers.Count; j++)
                    {
                        if (testToCompare.Questions[i].Answers[j].IsItRight != finishedTest.Questions[i].Answers[j].IsItRight && !testToCompare.Questions[i].Answers[j].IsItRight)
                        {
                            if (!wrongChoises.ContainsKey(i))
                            {
                                wrongChoises.Add(i, new List<int>());
                                wrongChoises[i].Add(j);
                            }
                            else
                            {
                                wrongChoises[i].Add(j);
                            }
                        }
                    }

                }
            }

            result = CreateResult(5, wrongChoises, testToCompare.Questions.Count);
            statisticLogic.UpdateTestStatistic(testToCompare.Name);

            return wrongChoises;
        }
    }
}
