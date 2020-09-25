using Business;
using Common.Models;
using System;
using System.Linq;
using ViewModel.Models;
using ViewModel.Utility;

namespace ViewModel
{
    public class SelectedTestViewModel : BaseViewModel
    {
        private TestsLogic testsLogic = new TestsLogic();
        private StatisticLogic statisticLogic = new StatisticLogic();

        private TestStatistic testStatistic;
        public CommandParameter Execution { get; set; }

        private Test _test;

        public SelectedTestViewModel(Test test)
        {
            _test = test;

            SelectedTest.TestName = _test.Name;
            SelectedTest.TestTheme = _test.Theme;

            QuestionsCount = _test.Questions.Count;

            testStatistic = statisticLogic.GetTestStatistic(_test.Name);

            SelectedTestStatistic.AttempsCount = testStatistic.AttempsCount;
            SelectedTestStatistic.SuccessCount = testStatistic.SuccessCount;

            /// TODO
            if (testStatistic.AttempsCount > 0)
            {
                SelectedTestStatistic.SuccessRate = Math.Round((double) testStatistic.SuccessCount * 100 / testStatistic.AttempsCount, 2);
                SelectedTestStatistic.RightAnswersRate = Math.Round(testStatistic.RightAnswersProcent.AsQueryable().Sum() / testStatistic.AttempsCount, 2);
            }

            else
            {
                SelectedTestStatistic.SuccessRate = 0;
                SelectedTestStatistic.RightAnswersRate = 0;
            }

            Execution = new CommandParameter();
            Execution.Test = test;
        }

        private TestView selectedTest = new TestView();
        public TestView SelectedTest
        {
            get
            {
                return selectedTest;
            }
            set
            {
                selectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }


        private TestStatisticView selectedTestStatistic = new TestStatisticView();
        public TestStatisticView SelectedTestStatistic
        {
            get
            {
                return selectedTestStatistic;
            }
            set
            {
                selectedTestStatistic = value;
                OnPropertyChanged("SelectedTestStatistic");
            }
        }


        private int questionsCount;
        public int QuestionsCount
        {
            get
            {
                return questionsCount;
            }
            set
            {
                questionsCount = value;
                OnPropertyChanged("QuestionsCount");
            }
        }
    }
}

