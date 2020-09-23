using Business;
using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ViewModel.Commands;
using ViewModel.Utility;
using ViewModel.Models;

namespace ViewModel
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

        public ObservableCollection<string> SubThemes
        {
            get
            {
                return test.SubThemes;
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
            test = new TestView();
            test.TestName = TestToRun.Name;
            test.ShowAnswerAtEnd = TestToRun.ShowAnswerAtEnd;

            test.SubThemes = new ObservableCollection<string>();

            if(TestToRun.SubThemes != null)
            {
                foreach (string subTheme in TestToRun.SubThemes)
                {
                    test.SubThemes.Add(subTheme);
                }
            }

            test.Questions = new ObservableCollection<QuestionView>();

            for (int i = 0; i < TestToRun.Questions.Count; i++)
            {
                QuestionView questionView = new QuestionView
                {
                    QuestionNumber = i,
                    QuestionContent = TestToRun.Questions[i].QuestionContent,
                    Color = MyEnum.Status.Default,
                    IsOpen = TestToRun.Questions[i].IsOpen,
                    IsCheсked = false
                };
                test.Questions.Add(questionView);

                foreach (Answer answer in TestToRun.Questions[i].Answers)
                {
                    if (questionView.IsOpen)
                    {
                        questionView.Answers.Add(new AnswerView { AnswerContent = "", IsRight = true });
                    }

                    else
                    {
                        questionView.Answers.Add(new AnswerView { AnswerContent = answer.Content, IsRight = false });
                    }
                }
            }

            SelectedQuestion = test.Questions[0];
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
            int id = questionResult.QuestionId;

            Questions[id].Color = MyEnum.Status.Right;

            if (!questionResult.IsRight)
                Questions[id].Color = MyEnum.Status.Wrong;

            if (questionResult.IsOpen)
            {
                if (questionResult.IsRight)
                {
                    Questions[id].Answers[0].Color = MyEnum.Status.Right;
                }

                else
                {
                    Questions[id].Answers[0].Color = MyEnum.Status.Wrong;
                }

                return;
            }

            for (int j = 0; j < Questions[id].Answers.Count; j++)
            {
                if (!questionResult.NoChoises && !questionResult.IsRight)
                {
                    if (questionResult.WrongAnswerChoises.Exists(x => x == j))
                    {
                        Questions[id].Answers[j].Color = MyEnum.Status.Wrong;
                    }
                }

                else
                {
                    Questions[id].Answers[j].Color = MyEnum.Status.Wrong;
                }

                if (TestToRun.Questions[id].Answers[j].IsItRight)
                {
                    Questions[id].Answers[j].Color = MyEnum.Status.Right;
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
                return finishTest ??
                  (finishTest = new RelayCommand(obj =>
                  {
                      EndTest();
                  }));
            }
        }

        /// Check if possible to bind without id
        private RelayCommand giveAnswer;
        public RelayCommand GiveAnswer
        {
            get
            {
                return giveAnswer ??
                  (giveAnswer = new RelayCommand(obj =>
                  {
                      QuestionView questionView = obj as QuestionView;
                      Question question = ViewToDataConverter.QuestionConverter(questionView);

                      int id = questionView.QuestionNumber;

                      QuestionResult questionResult = testsLogic.CheckCurrentQuestion(question, TestToRun.Questions[id], id);
                      SetColorsToQuestion(questionResult);

                      SelectedQuestion.IsCheсked = true;

                      SelectedQuestion = null;
                      SelectedQuestion = Questions[id];

                  }));
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

            finishedTest.Name = test.TestName;
            finishedTest.Questions = new List<Question>();

            /// TODO Use converter
            foreach (QuestionView question in test.Questions)
            {
                question.IsCheсked = true;

                Question questionForSaving = new Question();
                questionForSaving.QuestionContent = question.QuestionContent;
                questionForSaving.Answers = new List<Answer>();

                foreach (AnswerView answer in question.Answers)
                {
                    questionForSaving.Answers.Add(new Answer { Content = answer.AnswerContent, IsItRight = answer.IsRight });
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
                return showResult ??
                  (showResult = new RelayCommand(obj =>
                  {
                      SelectedQuestion = null;
                      ResultVisibility = true;
                      TestRunContent = new TestResultViewModel(testResult);
                  }));
            }
        }
    }
}
