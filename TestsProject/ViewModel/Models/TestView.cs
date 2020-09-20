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

        private int timerMinute;
        public int TimerMinute
        {
            get { return timerMinute; }
            set
            {
                timerMinute = value;
                OnPropertyChanged("TimerMinute");
            }
        }

        private int timerSecond;
        public int TimerSecond
        {
            get { return timerSecond; }
            set
            {
                timerSecond = value;

                if (timerSecond >= 60)
                {
                    TimerMinute = timerSecond / 60;
                    TimerSecond = timerSecond % 60;
                }

                OnPropertyChanged("TimerSecond");
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
