using Business;
using Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ViewModel.Commands;
using ViewModel.Models;

namespace ViewModel
{
    /// <summary>
    /// TODO Other way to show messages
    /// </summary>
    public class CreateTestViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private TestsLogic testsLogic = new TestsLogic();

        public CreateTestViewModel()
        {
            test = new TestView();
            test.TestName = "Test";

            test.Questions = new ObservableCollection<QuestionView>();
        }

        private TestView test;
        public TestView Test
        {
            get { return test; }
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

        private RelayCommand addQuestion;
        public RelayCommand AddQuestion
        {
            get
            {
                return addQuestion ??
                  (addQuestion = new RelayCommand(obj =>
                  {
                      QuestionView question = new QuestionView();

                      question.QuestionContent = "Default question";
                      question.IsOpen = false;

                      question.Answers.Add(new AnswerView() { AnswerContent = "Default", IsRight = true });
                      question.Answers.Add(new AnswerView() { AnswerContent = "Default", IsRight = true });

                      test.Questions.Add(question);

                      SelectedQuestion = question;
                  }));
            }
        }

        private RelayCommand addOpenQuestion;
        public RelayCommand AddOpenQuestion
        {
            get
            {
                return addOpenQuestion ??
                  (addOpenQuestion = new RelayCommand(obj =>
                  {
                      QuestionView question = new QuestionView();

                      question.QuestionContent = "Default question";
                      question.IsOpen = true;

                      question.Answers.Add(new AnswerView() { AnswerContent = "Default", IsRight = true });

                      test.Questions.Add(question);

                      SelectedQuestion = question;
                  }));
            }
        }

        private RelayCommand addAnswer;
        public RelayCommand AddAnswer
        {
            get
            {
                return addAnswer ??
                  (addAnswer = new RelayCommand(obj =>
                  {
                      if (selectedQuestion.Answers.Count == 5)
                      {
                          MessageBox.Show("5 answers is enough");
                      }
                      else
                      {
                          selectedQuestion.Answers.Add(new AnswerView() { AnswerContent = "Default", IsRight = true });
                      }
                  }));
            }
        }

        private RelayCommand saveTest;
        public RelayCommand SaveTest
        {
            get
            {
                return saveTest ??
                  (saveTest = new RelayCommand(obj =>
                  {
                      Test testForSaving = new Test();

                      testForSaving.Name = test.TestName;
                      testForSaving.Questions = new List<Question>();

                      foreach (QuestionView question in test.Questions)
                      {
                          Question questionForSaving = new Question();
                          questionForSaving.QuestionContent = question.QuestionContent;
                          questionForSaving.IsOpen = question.IsOpen;
                          questionForSaving.Answers = new List<Answer>();

                          foreach (AnswerView answer in question.Answers)
                          {
                              questionForSaving.Answers.Add(new Answer { Content = answer.AnswerContent, IsItRight = answer.IsRight });
                          }

                          testForSaving.Questions.Add(questionForSaving);
                      }

                      CreationReport creationReport = testsLogic.ValidateCreation(testForSaving);

                      if (creationReport.Result)
                      {
                          testsLogic.SaveTest(testForSaving);
                          MessageBox.Show("Test saved");
                      }

                      else
                      {
                          MessageBox.Show(creationReport.Message);
                          if (creationReport.BadQuestions.Count != 0)
                              selectedQuestion = Questions[creationReport.BadQuestions[0]];
                      }
                  }));
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        public new void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
