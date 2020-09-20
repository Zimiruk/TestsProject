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

            ///TODO MB change that
            for (int i = 0; i < testToCompare.Questions.Count; i++)
            {
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
