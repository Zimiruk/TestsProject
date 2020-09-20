using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class TestStatisticView : INotifyPropertyChanged
    {
        private int attempsCount;
        public int AttempsCount
        {
            get
            {
                return attempsCount;
            }
            set
            {
                attempsCount = value;
                OnPropertyChanged("AttempsCount");
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
