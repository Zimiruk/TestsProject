using Business;
using Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModel.Commands;

namespace ViewModel
{
    public class TestsListViewModel : BaseViewModel
    {
        private Test test;
        private TestsLogic testsLogic = new TestsLogic();

        public TestsListViewModel()
        {
            List<string> testsNames = testsLogic.ShowTestsNames();

            foreach (string name in testsNames)
            {
                _testsNames.Add(name);
            }

            Execution = new Parameter();
        }

        public Parameter Execution { get; set; }

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

                

                OnPropertyChanged("SelectedTestName");
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

        public ObservableCollection<string> TestsNames { get { return _testsNames; } }
        private ObservableCollection<string> _testsNames = new ObservableCollection<string>();

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

        private RelayCommand showTestStatistic;
        public RelayCommand ShowTestStatistic
        {
            get
            {
                return showTestContent ??
                  (showTestContent = new RelayCommand(obj =>
                  {               
                  }));
            }
        }
    }
}
