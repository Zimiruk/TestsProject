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
            ///TODO 
            if (parameter is CommandParameter)
            {
                CommandParameter param = (CommandParameter)parameter;

                viewModel.SelectedViewModel = new TestRunViewModel(param.Test);
            }

            else
            {
                if (parameter.ToString() == "Create")
                {
                    viewModel.SelectedViewModel = new CreateTestViewModel();
                }

                else
                {
                    viewModel.SelectedViewModel = new TestsListViewModel();
                }
            }
        }
    }
}