﻿using Business.BusinessModels;
using Business.Models;
using Common;
using Common.Models.Others;
using Common.Models.TestComponents;
using Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    /// TODO
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
            List<string> testsNames = operations.GetTestsNames();

            foreach(string testName in testsNames)
            {
                statisticLogic.GetTestStatistic(testName);
            }

            return testsNames;
        }

        public List<Node> GetListForTree()
        {
            return operations.GetListForTree();
        }

        public TestExistsReport GetTest(string testName)
        {
            TestExistsReport testExistsReport = new TestExistsReport();

            if (operations.CheckIfFileExists(testName, Constants.TestPath, Constants.TestExtenstion))
            {
                testExistsReport.Result = true;
                testExistsReport.Message = Constants.AllFine;
                testExistsReport.Test = operations.GetTest(testName);
            }

            else
            {
                testExistsReport.Result = false;
                testExistsReport.Message = Constants.NotFoundTestMessage;
            }

            return testExistsReport;
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
                if (!test.Questions[i].Answers.Exists(x => x.IsItRight))
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

            testResult.Passed = rightAmount <= testResult.AmountOfRight;  
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
            TestResult testResult = CreateResult(testToCompare.ToPassAmount, questionsResult);
            testResult.QuestionsResult = questionsResult;

            statisticLogic.UpdateTestStatistic(testToCompare.Name, testResult, testToCompare.Questions.Count);

            return testResult;
        }
         
        public QuestionResult CheckCurrentQuestion(Question questionWithChoses, Question questionToCompare, int id)
        {
            QuestionResult questionResult = new QuestionResult 
            {
                Id = id, IsRight = true, IsOpen = false, NoChoises = false 
            };

            if (!questionWithChoses.Answers.Exists(x => x.IsItRight))
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
                        questionResult.WrongAnswerChoises = new List<int>(i);
                    }
                }
            }
            return questionResult;
        }
    }
}