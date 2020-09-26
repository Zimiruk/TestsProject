using Common;
using System;
using System.Windows.Input;
using ViewModel.Utility;
using ViewModel.ViewModels;

namespace ViewModel.Commands
{
    class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is CommandParameter param)
            {
                if (param.Direction == Constants.Edit)
                {
                    viewModel.SelectedViewModel = new TestConstructorViewModel(param.Test);
                }

                else
                {
                    viewModel.SelectedViewModel = new TestRunViewModel(param.Test);
                }
            }

            else
            {
                if (parameter.ToString() == Constants.Create)
                {
                    viewModel.SelectedViewModel = new TestConstructorViewModel();
                }

                else
                {
                    viewModel.SelectedViewModel = new TestsListViewModel();
                }
            }
        }
    }
}