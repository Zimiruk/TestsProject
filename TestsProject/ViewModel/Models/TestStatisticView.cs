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

        private int successCount;
        public int SuccessCount
        {
            get
            {
                return successCount;
            }
            set
            {
                successCount = value;
                OnPropertyChanged("SuccessCount");
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
