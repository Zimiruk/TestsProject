using Common.Models;
using Data;
using System;
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

        public bool CheckIfFileExists(string fileName, string fileDirectory, string fileExtention)
        {
            return operations.CheckIfFileExists(fileName, fileDirectory, fileExtention);     
        }

        public CreationReport ValidateCreation(Test test)
        {
            CreationReport executionReport = new CreationReport();
            executionReport.BadQuestions = new List<int>();

            if (test.Questions.Count == 0)
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

        /// TODO Not sure
        private TestResult CreateResult(int rightAmount, List<QuestionResult> questionsResult)
        {
            TestResult testResult = new TestResult();            

            int wrongChoisesCount = 0;
            foreach(QuestionResult questionResult in questionsResult)
            {
                if (!questionResult.IsRight)
                    wrongChoisesCount++;
            }

            testResult.AmountOfRight = questionsResult.Count - wrongChoisesCount;

            if (rightAmount > testResult.AmountOfRight)
            {
                testResult.Passed = false;
            }

            else
            {
                testResult.Passed = true;
            }

            testResult.Message = $"{testResult.AmountOfRight} / {questionsResult.Count}";
            return testResult;
        }


        public TestResult FinishTest(Test finishedTest, Test testToCompare)
        {    
            List<QuestionResult> questionsResult = new List<QuestionResult>();

            for (int i = 0; i < testToCompare.Questions.Count; i++)
            {
                questionsResult.Add(CheckCurrentQuestion(finishedTest.Questions[i], testToCompare.Questions[i], i));
            }

            //TODO RightQuestionsCount
            TestResult testResult = CreateResult(5, questionsResult);
            testResult.QuestionsResult = questionsResult;

            statisticLogic.UpdateTestStatistic(testToCompare.Name, testResult);

            return testResult;
        }

        /// TODO Use this with all questions method
        public QuestionResult CheckCurrentQuestion(Question questionWithChoses, Question questionToCompare, int id)
        {
            QuestionResult questionResult = new QuestionResult {QuestionId = id, IsRight = true, IsOpen = false, NoChoises = false };

            if (!questionWithChoses.Answers.Exists(x => x.IsItRight == true))
            {
                questionResult.NoChoises = true;
                questionResult.IsRight = false;
            }

            if (questionToCompare.IsOpen)
            {
                questionResult.IsOpen = true;

                if (!questionWithChoses.Answers[0].Content.Equals(questionToCompare.Answers[0].Content, StringComparison.OrdinalIgnoreCase))               
                {
                    questionResult.IsRight = false;
                    return questionResult;
                }

                return questionResult;
            }

            for (int i = 0; i < questionToCompare.Answers.Count; i++)
            {
                if (questionToCompare.Answers[i].IsItRight != questionWithChoses.Answers[i].IsItRight && !questionToCompare.Answers[i].IsItRight)
                {
                    questionResult.IsRight = false;

                    if (questionResult.WrongAnswerChoises != null)
                    {
                        questionResult.WrongAnswerChoises.Add(i);
                    }
                    else
                    {
                        questionResult.WrongAnswerChoises = new List<int>();
                        questionResult.WrongAnswerChoises.Add(i);
                    }
                }
            }

            return questionResult;
        }
    }
}
