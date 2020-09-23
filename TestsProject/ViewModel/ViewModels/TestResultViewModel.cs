﻿using Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class TestResultViewModel : BaseViewModel
    {
        public TestResultViewModel(TestResult testResult)
        {
            this.testResult = testResult;
        }

        private TestResult testResult;
        public TestResult TestResult
        {
            get
            {
                return testResult;
            }
            set
            {
                testResult = value;
                OnPropertyChanged("TestResult");
            }
        }
    }
}