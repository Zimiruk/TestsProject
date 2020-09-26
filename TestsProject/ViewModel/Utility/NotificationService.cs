using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ViewModel.Utility
{
    public static class NotificationService
    {
        public static bool ShowDialogWindow(string message, string messageHeader)
        {
            MessageBoxResult result = MessageBox.Show(message, messageHeader, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return (result == MessageBoxResult.Yes);                       
        }

        public static void ShowMessageWindow(string message)
        {
            MessageBox.Show(message);
        }
    }
}
