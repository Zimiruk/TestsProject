using Business;
using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ViewModel.Commands;
using ViewModel.Models;

namespace ViewModel
{
    public class TestRunViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private TestsLogic testsLogic = new TestsLogic();
        private DispatcherTimer timer = new DispatcherTimer();

        public TestRunViewModel(Test test)
        {
            TestToRun = test;
            TestDone = false;
            FillTestViewWithContent();
            LauchTimer();
        }

        public Test TestToRun { get; set; }

        private Dictionary<int, List<int>> wrongChoises;

        private Result testResult;
        public Result TestResult
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
      
        private TestView test;
        public TestView Test
        {
            get { return test; }
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

        private double time;
        public double Time
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

        public string SelectedStyle
        {
            get { return this.selectedStyle; }
            set
            {
                this.selectedStyle = value;
                OnPropertyChanged("SelectedStyle");
            }
        }
        private string selectedStyle;

        public ObservableCollection<QuestionView> Questions
        {
            get
            {
                return test.Questions;
            }
        }

        private void FillTestViewWithContent()
        {
            test = new TestView();
            test.TestName = TestToRun.Name;

            test.Questions = new ObservableCollection<QuestionView>();
            foreach (Question question in TestToRun.Questions)
            {
                QuestionView questionView = new QuestionView { QuestionContent = question.QuestionContent, Color = MyEnum.Status.Default };
                test.Questions.Add(questionView);

                foreach (Answer answer in question.Answers)
                {
                    questionView.Answers.Add(new AnswerView { AnswerContent = answer.Content, IsRight = false });
                }
            }

            SelectedQuestion = test.Questions[0];
        }


        private void UpdateFormColors()
        {
            for (int i = 0; i < Questions.Count; i++)
            {
                if (!wrongChoises.ContainsKey(i))
                {
                    Questions[i].Color = MyEnum.Status.Right;
                }

                else
                {
                    Questions[i].Color = MyEnum.Status.Wrong;
                }

                for (int j = 0; j < Questions[i].Answers.Count; j++)
                {
                    if (TestToRun.Questions[i].Answers[j].IsItRight)
                    {
                        Questions[i].Answers[j].Color = MyEnum.Status.Right;
                    }

                    if (wrongChoises.ContainsKey(i))
                    {
                        if (wrongChoises[i].Exists(x => x == j))
                        {
                            Questions[i].Answers[j].Color = MyEnum.Status.Wrong;
                        }
                    }
                }

                SelectedQuestion = null;
                ResultVisibility = true;
            }
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

        private void LauchTimer()
        {     
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(TimerTick);

            ///TODO Set timer time
            Time = 5;

            timer.Start();
        }


        private void TimerTick(object sender, EventArgs e)
        {
            Time -= 1;
            if (Time == 0)
            {
                timer.Stop();
                EndTest();
            }
        }

        private void EndTest()
        {
            Result result;
            Test finishedTest = new Test();

            finishedTest.Name = test.TestName;
            finishedTest.Questions = new List<Question>();

            foreach (QuestionView question in test.Questions)
            {
                Question questionForSaving = new Question();
                questionForSaving.QuestionContent = question.QuestionContent;
                questionForSaving.Answers = new List<Answer>();

                foreach (AnswerView answer in question.Answers)
                {
                    questionForSaving.Answers.Add(new Answer { Content = answer.AnswerContent, IsItRight = answer.IsRight });
                }

                finishedTest.Questions.Add(questionForSaving);
            }

            wrongChoises = testsLogic.FinishTest(finishedTest, TestToRun, out result);
            TestResult = result;
            TestDone = true;
            UpdateFormColors();
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
                      //ContentVisibility = false;
                  }));
            }
        }



    }
}
