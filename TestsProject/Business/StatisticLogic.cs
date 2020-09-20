using Common.Models;
using Data;
using System.Collections.Generic;

namespace Business
{
    public class StatisticLogic
    {
        private StatisticFilesOperator statisticFiles = new StatisticFilesOperator();

        /// TODO Right answers, success count etc
        public TestStatistic GetTestStatistic(string testName)
        {
            if (statisticFiles.CheckIfFileExists(testName, "TestsStatistic", "dat"))
            {
                TestStatistic testStatistic = statisticFiles.OpenTestStatistic(testName);
                return testStatistic;
            }

            else
            {
                TestStatistic testStatistic = new TestStatistic() { TestName = testName, AttempsCount = 0, RightAnswersCount = new List<int> { 0 }, SuccessCount = 0 };
                statisticFiles.CreateTestStatistic(testStatistic);
                return testStatistic;
            }
        }

        /// TODO Right answers, success count etc
        public void UpdateTestStatistic(string testName)
        {
            if (statisticFiles.CheckIfFileExists(testName, "TestsStatistic", "dat"))
            {
                TestStatistic testStatistic = statisticFiles.OpenTestStatistic(testName);
                testStatistic.AttempsCount++;
                statisticFiles.SaveTestStatistic(testStatistic);
            }

            else
            {
                TestStatistic testStatistic = new TestStatistic() { TestName = testName, AttempsCount = 1, RightAnswersCount = new List<int> { 0 }, SuccessCount = 0 };
                statisticFiles.CreateTestStatistic(testStatistic);
            }
        }

        public void DeleteStatistic(string testName)
        {
            statisticFiles.DeleteStatistic(testName);
        }
    }
}
