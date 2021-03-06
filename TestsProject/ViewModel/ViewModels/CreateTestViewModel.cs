﻿using Business;
using Business.Models;
using Common;
using Common.Models.TestComponents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ViewModel.Commands;
using ViewModel.Models;
using ViewModel.Utility;

namespace ViewModel.ViewModels
{
    public class CreateTestViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private TestsLogic testsLogic = new TestsLogic();
        private StatisticLogic statisticLogic = new StatisticLogic();

        public CreateTestViewModel()
        {
            test = new TestView();
            test.Name =  Constants.Default;
            test.Theme = Constants.Default;
            test.ShowAnswerAtEnd = true;

            test.SubThemes = new ObservableCollection<string>();
            test.Questions = new ObservableCollection<QuestionView>();
        }

        private TestView test;
        public TestView Test
        {
            get { return test; }
        }

        private string subTheme;
        public string SubTheme
        {
            get
            {
                return subTheme;
            }
            set
            {
                subTheme = value;
                OnPropertyChanged("SubTheme");
            }
        }

        public ObservableCollection<string> SubThemes
        {
            get
            {
                return test.SubThemes;
            }
        }

        public ObservableCollection<QuestionView> Questions
        {
            get
            {
                return test.Questions;
            }
        }

        private QuestionView selectedQuestion;
        public QuestionView SelectedQuestion
        {
            get
            {
                return selectedQuestion;
            }
            set
            {
                selectedQuestion = value;
                OnPropertyChanged("SelectedQuestion");
            }
        }

        /// TODO Converter
        private RelayCommand saveTest;
        public RelayCommand SaveTest
        {
            get
            {
                return saveTest ??= new RelayCommand(obj =>
                  {
                      Test testForSaving = new Test();

                      testForSaving.Name = test.Name;
                      testForSaving.Theme = test.Theme;
                      testForSaving.Questions = new List<Question>();
                      testForSaving.TimerCountdown = test.TimerMinute * 60 + test.TimerSecond;
                      testForSaving.ShowAnswerAtEnd = test.ShowAnswerAtEnd;
                      testForSaving.ToPassAmount = test.ToPassAmount;

                      testForSaving.SubThemes = new List<string>();

                      foreach (string subTheme in test.SubThemes)
                      {
                          testForSaving.SubThemes.Add(subTheme);
                      }

                      foreach (QuestionView question in test.Questions)
                      {
                          Question questionForSaving = new Question();
                          questionForSaving.Content = question.Content;
                          questionForSaving.IsOpen = question.IsOpen;
                          questionForSaving.Answers = new List<Answer>();

                          foreach (AnswerView answer in question.Answers)
                          {
                              questionForSaving.Answers.Add(new Answer 
                              { 
                                  Content = answer.Content, IsItRight = answer.IsRight 
                              });
                          }

                          testForSaving.Questions.Add(questionForSaving);
                      }

                      CreationReport creationReport = testsLogic.ValidateCreation(testForSaving);

                      /// TODO 4x if                 
                      if (creationReport.Result)
                      {
                          ///TODO Fix that    
                          if (testsLogic.CheckIfFileExists(testForSaving.Name, Constants.TestPath, "test"))
                          {
                              if (NotificationService.ShowDialogWindow(Constants.TestExistsNotification, Constants.TestExistsNotificationHeading))
                              {
                                  statisticLogic.DeleteStatistic(testForSaving.Name);
                                  testsLogic.SaveTest(testForSaving);
                                  NotificationService.ShowMessageWindow(Constants.TestAdded);
                              }                             
                          }

                          else
                          {                              
                              testsLogic.SaveTest(testForSaving);
                              NotificationService.ShowMessageWindow(Constants.TestAdded);                           
                          }
                      }

                      else
                      {
                          NotificationService.ShowMessageWindow(creationReport.Message);
                          if (creationReport.BadQuestions.Count != 0)
                          {
                              SelectedQuestion = Questions[creationReport.BadQuestions[0]];
                          }
                              
                      }
                  });
            }
        }

        private bool addAnswerButtonVisibility = false;
        public bool AddAnswerButtonVisibility
        {
            get
            {
                return addAnswerButtonVisibility;
            }
            set
            {
                addAnswerButtonVisibility = value;
                OnPropertyChanged("AddAnswerButtonVisibility");
            }
        }

        private RelayCommand addQuestion;
        public RelayCommand AddQuestion
        {
            get
            {
                return addQuestion ??= new RelayCommand(obj =>
                  {
                      AddAnswerButtonVisibility = true;

                      QuestionView question = new QuestionView();

                      question.Content = Constants.Default;
                      question.IsOpen = false;

                      question.Answers.Add(new AnswerView 
                      { 
                          Content = Constants.Default, IsRight = true 
                      });
                      question.Answers.Add(new AnswerView 
                      { 
                          Content = Constants.Default, IsRight = true 
                      });

                      test.Questions.Add(question);

                      SelectedQuestion = question;
                  });
            }
        }

        private RelayCommand addOpenQuestion;
        public RelayCommand AddOpenQuestion
        {
            get
            {
                return addOpenQuestion ??= new RelayCommand(obj =>
                  {
                      AddAnswerButtonVisibility = false;

                      QuestionView question = new QuestionView();

                      question.Content = Constants.Default;
                      question.IsOpen = true;

                      question.Answers.Add(new AnswerView 
                      { 
                          Content = Constants.Default, IsRight = true 
                      });

                      test.Questions.Add(question);

                      SelectedQuestion = question;
                  });
            }
        }

        private RelayCommand removeQuestion;
        public RelayCommand RemoveQuestion
        {
            get
            {
                return removeQuestion ??= new RelayCommand(obj =>
                  {
                      Questions.Remove(SelectedQuestion);

                      if (Questions.Count > 0)
                      {
                          SelectedQuestion = Questions[0];
                      }
                  });
            }
        }

        private RelayCommand addAnswer;
        public RelayCommand AddAnswer
        {
            get
            {
                return addAnswer ??= new RelayCommand(obj =>
                  {
                      if (selectedQuestion.Answers.Count == 6)
                      {
                          MessageBox.Show("6 answers is enough");
                      }
                      else
                      {
                          selectedQuestion.Answers.Add(new AnswerView 
                          {
                              Content = Constants.Default, IsRight = true 
                          });
                      }
                  });
            }
        }

        private RelayCommand removeAnswer;
        public RelayCommand RemoveAnswer
        {
            get
            {
                return removeAnswer ??= new RelayCommand(obj =>
                  {
                      SelectedQuestion.Answers.Remove(obj as AnswerView);
                  });
            }
        }

        private RelayCommand addSubtheme;
        public RelayCommand AddSubtheme
        {
            get
            {
                return addSubtheme ??= new RelayCommand(obj =>
                  {
                      bool found = false;

                      foreach (string item in SubThemes)
                      {
                          if (item == SubTheme)
                          {
                              found = true;
                              break;
                          }
                      }

                      if (!found)
                      {
                          SubThemes.Add(SubTheme);
                          SubTheme = "";
                      }

                  });
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        public new void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }}