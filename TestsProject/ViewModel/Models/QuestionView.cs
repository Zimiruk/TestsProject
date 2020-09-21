﻿using Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.Models
{
    public class QuestionView : INotifyPropertyChanged
    {
        private int questionNumber;
        public int QuestionNumber
        {
            get { return questionNumber; }
            set
            {
                questionNumber = value;
                OnPropertyChanged("QuestionNumber");
            }
        }

        public QuestionView()
        {
            this.Answers = new ObservableCollection<AnswerView>();
        }

        private string questionContent;
        public string QuestionContent
        {
            get { return questionContent; }
            set
            {
                questionContent = value;
                OnPropertyChanged("QuestionContent");
            }
        }

        private bool isOpen;
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            set
            {
                isOpen = value;
                OnPropertyChanged("IsOpen");
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
                OnPropertyChanged("Color");
            }
        }

        private bool isCheсked;
        public bool IsCheсked
        {
            get
            {
                return isCheсked;
            }
            set
            {
                isCheсked = value;
                OnPropertyChanged("IsCheсked");
            }
        }


        public ObservableCollection<AnswerView> Answers { get; set; }       

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
