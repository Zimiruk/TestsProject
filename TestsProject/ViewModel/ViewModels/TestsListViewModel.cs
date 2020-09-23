using Business;
using ViewModel.Utility;
using Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModel.Commands;
using ViewModel.Models;

namespace ViewModel
{
    public class TestsListViewModel : BaseViewModel
    {
        private TestsLogic testsLogic = new TestsLogic();
        private StatisticLogic statisticLogic = new StatisticLogic();

        private Test test;
        private TestStatistic testStatistic;

        public TestsListViewModel()
        {
            List<string> testsNames = testsLogic.ShowTestsNames();

            foreach (string name in testsNames)
            {
                _testsNames.Add(name);
            }

            Execution = new CommandParameter();
        }

        public CommandParameter Execution { get; set; }

        public ObservableCollection<string> TestsNames { get { return _testsNames; } }
        private ObservableCollection<string> _testsNames = new ObservableCollection<string>();

        private string selectedTestName;
        public string SelectedTestName
        {
            get
            {
                return selectedTestName;
            }
            set
            {
                selectedTestName = value;
                ContentVisibility = true;

                test = testsLogic.GetTest(selectedTestName);
                Execution.Test = test;

                SelectedTest.TestName = test.Name;
                SelectedTest.TestTheme = test.Theme;
                
                QuestionsCount = test.Questions.Count;

                testStatistic = statisticLogic.GetTestStatistic(test.Name);
                SelectedTestStatistic.AttempsCount = testStatistic.AttempsCount;
                SelectedTestStatistic.SuccessCount = testStatistic.SuccessCount;

                OnPropertyChanged("SelectedTestName");
            }
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

        private bool contentVisibility = false;
        public bool ContentVisibility
        {
            get
            {
                return contentVisibility;
            }

            set
            {
                if (value != contentVisibility)
                {
                    contentVisibility = value;
                    OnPropertyChanged("ContentVisibility");
                }
            }
        }

        private RelayCommand showTestContent;
        public RelayCommand ShowTestContent
        {
            get
            {
                return showTestContent ??
                  (showTestContent = new RelayCommand(obj =>
                  {
                      ContentVisibility = true;
                  }));
            }
        }        
    }
}
