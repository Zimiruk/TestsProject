using Business.Models;

namespace ViewModel.ViewModels
{
    public class TestResultViewModel : BaseViewModel
    {
        public TestResultViewModel(TestResult testResult)
        {
            this.testResult = testResult;
        }

        private TestResult testResult;
        public TestResult TestResult
        {
            get
            {
                return testResult;
            }
            set
            {
                testResult = value;
                OnPropertyChanged("TestResult");
            }
        }
    }
}