using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;

namespace ViewModel.Models
{
    public class AnswerView : INotifyPropertyChanged
    {      
        private string answerContent;
        public string AnswerContent
        {
            get { return answerContent; }
            set
            {
                answerContent = value;
                OnPropertyChanged("AnswerContent");
            }
        }

        private bool isRight;
        public bool IsRight
        {
            get { return isRight; }
            set
            {
                isRight = value;
                OnPropertyChanged("IsRight");
            }
        }

        private MyEnum.Status color;
        public MyEnum.Status Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("SelectedQuestion");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
