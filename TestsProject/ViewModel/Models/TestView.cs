using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class TestView : INotifyPropertyChanged
    {
        public TestView()
        {
            this.Questions = new ObservableCollection<QuestionView>();
        }

        private string testName;   
        public string TestName
        {
            get { return testName; }
            set
            {
                testName = value;
                OnPropertyChanged("TestName");
            }
        }

        private string testTheme;
        public string TestTheme
        {
            get { return testTheme; }
            set
            {
                testTheme = value;
                OnPropertyChanged("TestTheme");
            }
        }

        public ObservableCollection<QuestionView> Questions { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
