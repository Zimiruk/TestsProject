using Common;
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string theme;
        public string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                OnPropertyChanged("Theme");
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

                if (timerSecond >= Constants.TimeDivision)
                {
                    TimerMinute = timerSecond / Constants.TimeDivision;
                    TimerSecond = timerSecond % Constants.TimeDivision;
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

                if (columnName == "ToPassAmount" && toPassAmount > Questions.Count)
                {
                    error = $"{toPassAmount} more than that test quesitons amount";
                }
                return error;
            }
        }

        public ObservableCollection<QuestionView> Questions { get; set; }
        public ObservableCollection<string> SubThemes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}