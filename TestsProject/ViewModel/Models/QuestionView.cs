using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class QuestionView : INotifyPropertyChanged
    {
        private int number;
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public QuestionView()
        {
            this.Answers = new ObservableCollection<AnswerView>();
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        private bool isOpen;
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            set
            {
                isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        private string color;
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        private bool isCheсked;
        public bool IsCheсked
        {
            get
            {
                return isCheсked;
            }
            set
            {
                isCheсked = value;
                OnPropertyChanged("IsCheсked");
            }
        }

        public ObservableCollection<AnswerView> Answers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}