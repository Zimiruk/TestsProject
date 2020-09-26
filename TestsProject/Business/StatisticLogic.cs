using Business.Models;
using Common;
using Common.Models.Others;
using Common.Models.Statistic;
using Data;
using System.Collections.Generic;

namespace Business
{
    public class StatisticLogic
    {
        private StatisticFilesOperator statisticFiles = new StatisticFilesOperator();
        private TestFilesOperations operations = new TestFilesOperations();

        /// TODO Right answers, success count etc
        public TestStatistic GetTestStatistic(string testName)
        {
            if (statisticFiles.CheckIfFileExists(testName, Constants.StatisticPath, "dat"))
            {
                TestStatistic testStatistic = statisticFiles.OpenTestStatistic(testName);
                return testStatistic;
            }

            else
            {
                TestStatistic testStatistic = new TestStatistic()
                {     
                    Name = testName, 
                    AttempsCount = 0, 
                    RightAnswersCount = new List<int>(), 
                    SuccessCount = 0, 
                    RightAnswersProcent = new List<double>() 
                };

                statisticFiles.CreateTestStatistic(testStatistic);
                return testStatistic;
            }
        }
           
        /// TODO If nothing
        public StatisticByTheme GetTestStatisticByTheme(string testTheme)
        {
            List<TestStatistic> allStatistic = statisticFiles.GetAllStatistic();
            List<TestForList> testsNames = operations.GetAllTestsNames();

            foreach (TestForList test in testsNames)
            {
                GetTestStatistic(test.Name);
            }

            StatisticByTheme statisticByTheme = new StatisticByTheme();
            List<TestStatistic> foundStatistics = new List<TestStatistic>();

            statisticByTheme.ObjectName = testTheme;
            statisticByTheme.ObjectType = Constants.Theme;

            testsNames = testsNames.FindAll(x => x.Theme == testTheme);

            foreach (TestForList test in testsNames)
            {
                foundStatistics.Add(allStatistic.Find(x => x.Name == test.Name));
            }

            statisticByTheme.TestsAmount = foundStatistics.Count;

            foreach (TestStatistic foundStatistic in foundStatistics)
            {
                if(foundStatistic.AttempsCount > 0)
                {
                    double value = 0;
                    double sucessProcent = System.Math.Round((double) foundStatistic.SuccessCount * Constants.Percent / foundStatistic.AttempsCount, 2);

                    foreach (double procentValue in foundStatistic.RightAnswersProcent)
                    {
                        value += procentValue;
                    }

                    statisticByTheme.ProcentOfRight += value / foundStatistic.RightAnswersProcent.Count;
                    statisticByTheme.ProcentOfSuccess += sucessProcent;
                }
            }

            statisticByTheme.ProcentOfRight = System.Math.Round(statisticByTheme.ProcentOfRight / foundStatistics.Count, 2);
            statisticByTheme.ProcentOfSuccess = System.Math.Round(statisticByTheme.ProcentOfSuccess / foundStatistics.Count, 2);

            return statisticByTheme;
        }

        public StatisticByTheme GetTestStatisticByTheme(string testTheme, string testSubTheme)
        {
            List<TestStatistic> allStatistic = statisticFiles.GetAllStatistic();
            List<TestForList> testsNames = operations.GetAllTestsNames();

            StatisticByTheme statisticByTheme = new StatisticByTheme();
            List<TestStatistic> foundStatistics = new List<TestStatistic>();

            statisticByTheme.ObjectName = testSubTheme;
            statisticByTheme.ObjectType = Constants.SubTheme;

            testsNames = testsNames.FindAll(x => x.Theme == testTheme && x.SubThemes.Contains(testSubTheme));

            foreach (TestForList test in testsNames)
            {
                foundStatistics.Add(allStatistic.Find(x => x.Name == test.Name));
            }

            statisticByTheme.TestsAmount = foundStatistics.Count;

            foreach (TestStatistic foundStatistic in foundStatistics)
            {
                if (foundStatistic.AttempsCount > 0)
                {
                    double value = 0;
                    double sucessProcent = System.Math.Round((double)foundStatistic.SuccessCount * Constants.Percent / foundStatistic.AttempsCount, 2);

                    foreach (double procentValue in foundStatistic.RightAnswersProcent)
                    {
                        value += procentValue;
                    }

                    statisticByTheme.ProcentOfRight += value / foundStatistic.RightAnswersProcent.Count;
                    statisticByTheme.ProcentOfSuccess += sucessProcent;
                }
            }

            statisticByTheme.ProcentOfRight = System.Math.Round((double)statisticByTheme.ProcentOfRight / foundStatistics.Count, 2);
            statisticByTheme.ProcentOfSuccess = System.Math.Round((double)statisticByTheme.ProcentOfSuccess / foundStatistics.Count, 2);

            return statisticByTheme;
        }

        /// TODO Procents and mb something else
        public void UpdateTestStatistic(string testName, TestResult testResult, int questionsAmount)
        {
            if (statisticFiles.CheckIfFileExists(testName, Constants.StatisticPath, "dat"))
            {
                TestStatistic testStatistic = statisticFiles.OpenTestStatistic(testName);
                testStatistic.AttempsCount++;

                testStatistic.RightAnswersProcent.Add(System.Math.Round((double)(Constants.Percent * testResult.AmountOfRight / questionsAmount), 2));
                testStatistic.RightAnswersCount.Add(testResult.AmountOfRight);

                if (testResult.Passed)
                {
                    testStatistic.SuccessCount++;
                }

                statisticFiles.SaveTestStatistic(testStatistic);
            }

            else
            {
                TestStatistic testStatistic = new TestStatistic() { 
                    Name = testName, 
                    AttempsCount = 1, 
                    RightAnswersCount = new List<int>() { testResult.AmountOfRight }, 
                    RightAnswersProcent = new List<double> { System.Math.Round((double)(Constants.Percent * testResult.AmountOfRight / questionsAmount), 2) } 
                };

                if (testResult.Passed)
                {
                    testStatistic.SuccessCount++;
                }

                statisticFiles.CreateTestStatistic(testStatistic);
            }
        }

        public void DeleteStatistic(string testName)
        {
            statisticFiles.DeleteStatistic(testName);
        }
    }
}