﻿using System.Windows;
using ViewModel;
using ViewModel.ViewModels;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
