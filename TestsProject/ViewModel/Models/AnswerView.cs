using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class AnswerView : INotifyPropertyChanged
    {      
        private string сontent;
        public string Content
        {
            get { return сontent; }
            set
            {
                сontent = value;
                OnPropertyChanged("Content");
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {           
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}