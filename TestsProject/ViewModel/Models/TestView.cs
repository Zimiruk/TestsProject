using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class TestView : INotifyPropertyChanged, IDataErrorInfo
    {
        public TestView()
        {
            Questions = new ObservableCollection<QuestionView>();
            SubThemes = new ObservableCollection<string>();
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

        private bool showAnswerAtEnd;
        public bool ShowAnswerAtEnd
        {
            get { return showAnswerAtEnd; }
            set
            {
                showAnswerAtEnd = value;
                OnPropertyChanged("ShowAnswerAtEnd");
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

        private int toPassAmount;
        public int ToPassAmount
        {
            get { return toPassAmount; }
            set
            {
                toPassAmount = value;
                OnPropertyChanged("ToPassAmount");
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "ToPassAmount":
                        if (toPassAmount > this.Questions.Count)
                        {
                            error = $"{toPassAmount} more than that test quesitons amount";
                        }
                        break;
                }
                return error;
            }
        }

        public ObservableCollection<QuestionView> Questions { get; set; }
        public ObservableCollection<string> SubThemes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
