using Business;
using Business.Models;
using Common;
using Common.Models.TestComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ViewModel.Commands;
using ViewModel.Models;
using ViewModel.Utility;

namespace ViewModel.ViewModels
{
    public class TestRunViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private TestsLogic testsLogic = new TestsLogic();
        private DispatcherTimer timer = new DispatcherTimer();

        private TestResult testResult;

        public TestRunViewModel(Test test)
        {
            TestToRun = test;
            TestDone = false;
            FillTestViewWithContent();

            seconds = TestToRun.TimerCountdown;
            Time = TimeSpan.FromSeconds(seconds);

            if (TestToRun.TimerCountdown != 0)
                LauchTimer();
        }

        public Test TestToRun { get; set; }
        public TestView Test { get; private set; }

        public ObservableCollection<QuestionView> Questions
        {
            get
            {
                return Test.Questions;
            }
        }

        public ObservableCollection<string> SubThemes
        {
            get
            {
                return Test.SubThemes;
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

                ResultVisibility = false;
            }
        }

        private int seconds;

        private TimeSpan time;
        public TimeSpan Time
        {
            get
            {
                return time;

            }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        private string selectedStyle;
        public string SelectedStyle
        {
            get { return selectedStyle; }
            set
            {
                this.selectedStyle = value;
                OnPropertyChanged("SelectedStyle");
            }
        }

        #region [ Style Properties ]

        private bool testDone;
        public bool TestDone
        {
            get
            {
                return testDone;
            }
            set
            {
                testDone = value;
                OnPropertyChanged("TestDone");
            }
        }

        private bool resultVisibility;
        public bool ResultVisibility
        {
            get
            {
                return resultVisibility;
            }
            set
            {
                resultVisibility = value;
                OnPropertyChanged("ResultVisibility");
            }
        }

        #endregion

        private BaseViewModel _testRunContent;

        public BaseViewModel TestRunContent
        {
            get { return _testRunContent; }
            set
            {
                _testRunContent = value;
                OnPropertyChanged(nameof(TestRunContent));
            }
        }

        private void FillTestViewWithContent()
        {
            Test = new TestView();
            Test.Name = TestToRun.Name;
            Test.Theme = TestToRun.Theme;
            Test.ShowAnswerAtEnd = TestToRun.ShowAnswerAtEnd;

            Test.SubThemes = new ObservableCollection<string>();

            if (TestToRun.SubThemes != null)
            {
                foreach (string subTheme in TestToRun.SubThemes)
                {
                    Test.SubThemes.Add(subTheme);
                }
            }

            Test.Questions = new ObservableCollection<QuestionView>();

            for (int i = 0; i < TestToRun.Questions.Count; i++)
            {
                QuestionView questionView = new QuestionView
                {
                    Number = i,
                    Content = TestToRun.Questions[i].Content,
                    Color = Constants.Default,
                    IsOpen = TestToRun.Questions[i].IsOpen,
                    IsCheсked = false
                };
                Test.Questions.Add(questionView);

                foreach (Answer answer in TestToRun.Questions[i].Answers)
                {
                    questionView.Answers.Add(questionView.IsOpen
                        ? new AnswerView { Content = "", IsRight = true }
                        : new AnswerView { Content = answer.Content, IsRight = false });                   
                }
            }

            SelectedQuestion = Test.Questions[0];
        }

        private void UpdateFormColors()
        {
            foreach (QuestionResult questionResult in testResult.QuestionsResult)
            {
                SetColorsToQuestion(questionResult);
            }

            SelectedQuestion = null;
            ResultVisibility = true;
        }

        private void SetColorsToQuestion(QuestionResult questionResult)
        {
            int id = questionResult.Id;

            Questions[id].Color = Constants.Right;

            if (!questionResult.IsRight)
                Questions[id].Color = Constants.Wrong;

            if (questionResult.IsOpen)
            {
                if (questionResult.IsRight)
                {
                    Questions[id].Answers[0].Color = Constants.Right;
                }

                else
                {
                    Questions[id].Answers[0].Color = Constants.Wrong;
                }

                return;
            }

            for (int j = 0; j < Questions[id].Answers.Count; j++)
            {
                if (!questionResult.NoChoises && !questionResult.IsRight)
                {
                    if (questionResult.WrongAnswerChoises.Exists(x => x == j))
                    {
                        Questions[id].Answers[j].Color = Constants.Wrong;
                    }
                }

                else
                {
                    Questions[id].Answers[j].Color = Constants.Wrong;
                }

                if (TestToRun.Questions[id].Answers[j].IsItRight)
                {
                    Questions[id].Answers[j].Color = Constants.Right;
                }
            }

            SelectedQuestion = null;
            SelectedQuestion = Questions[id];
        }

        private RelayCommand finishTest;
        public RelayCommand FinishTest
        {
            get
            {
                return finishTest ??= new RelayCommand(obj =>
                  {
                      EndTest();
                  });
            }
        }

        /// Check if possible to bind without id
        private RelayCommand giveAnswer;
        public RelayCommand GiveAnswer
        {
            get
            {
                return giveAnswer ??= new RelayCommand(obj =>
                  {
                      QuestionView questionView = obj as QuestionView;
                      Question question = ViewToDataConverter.QuestionConverter(questionView);

                      int id = questionView.Number;

                      QuestionResult questionResult = testsLogic.CheckCurrentQuestion(question, TestToRun.Questions[id], id);
                      SetColorsToQuestion(questionResult);

                      SelectedQuestion.IsCheсked = true;

                      SelectedQuestion = null;
                      SelectedQuestion = Questions[id];

                  });
            }
        }

        private void LauchTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(TimerTick);

            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            seconds -= 1;
            Time = TimeSpan.FromSeconds(seconds);

            if (seconds == 0)
            {
                timer.Stop();
                EndTest();
            }
        }

        private void EndTest()
        {
            timer.Stop();

            Test finishedTest = new Test();

            finishedTest.Name = Test.Name;
            finishedTest.Questions = new List<Question>();

            /// TODO Use converter
            foreach (QuestionView question in Test.Questions)
            {
                question.IsCheсked = true;

                Question questionForSaving = new Question();
                questionForSaving.Content = question.Content;
                questionForSaving.Answers = new List<Answer>();

                foreach (AnswerView answer in question.Answers)
                {
                    questionForSaving.Answers.Add(new Answer { Content = answer.Content, IsItRight = answer.IsRight });
                }

                finishedTest.Questions.Add(questionForSaving);
            }

            testResult = testsLogic.FinishTest(finishedTest, TestToRun);
            TestDone = true;
            UpdateFormColors();
            TestRunContent = new TestResultViewModel(testResult);
        }


        private RelayCommand showResult;
        public RelayCommand ShowResult
        {
            get
            {
                return showResult ??= new RelayCommand(obj =>
                  {
                      SelectedQuestion = null;
                      ResultVisibility = true;
                      TestRunContent = new TestResultViewModel(testResult);
                  });
            }
        }
    }
}