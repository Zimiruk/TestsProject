using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View.Views
{
    public partial class CreateTestView
    {
        public CreateTestView()
        {
            InitializeComponent();
        }
        /*
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsValid(sender as DependencyObject);
        }

        private bool IsValid(DependencyObject obj)
        {
            return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }
        */
    }
}