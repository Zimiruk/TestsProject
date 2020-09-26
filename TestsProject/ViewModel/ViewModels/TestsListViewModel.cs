using Business;
using Business.BusinessModels;
using Common;
using Common.Models.Others;
using Common.Models.Statistic;
using Common.Models.TestComponents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModel.Utility;

namespace ViewModel.ViewModels
{
    public class TestsListViewModel : BaseViewModel
    {
        private TestsLogic testsLogic = new TestsLogic();
        private StatisticLogic statisticLogic = new StatisticLogic();

        private Test test;

        public TestsListViewModel()
        {            
            UpdateLists();
            Node.SendNode += GetNode;
        }

        public ObservableCollection<string> TestsNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Node> Nodes { get; set; } = new ObservableCollection<Node>();

        private string selectedTestName;
        public string SelectedTestName
        {
            get
            {
                return selectedTestName;
            }
            set
            {
                selectedTestName = value;
                ShowSelectedTest(selectedTestName);
                selectedTestName = null;

                OnPropertyChanged("SelectedTestName");
            }
        }

        /// TODO Delete?
        private BaseViewModel testInformation;

        public BaseViewModel TestInformation
        {
            get { return testInformation; }
            set
            {
                testInformation = value;
                OnPropertyChanged(nameof(TestInformation));
            }
        }

        ///  TODO Send names to class
        public void GetNode(Node node)
        {
            StatisticByTheme statisticByTheme;

            switch (node.NodeType)
            {
                case Constants.Test:
                    ShowSelectedTest(node.Name);
                    break;

                case Constants.Theme:
                    statisticByTheme = statisticLogic.GetTestStatisticByTheme(node.Name);
                    TestInformation = new ThemeInformationViewModel(statisticByTheme);
                    break;

                case Constants.SubTheme:
                    statisticByTheme = statisticLogic.GetTestStatisticByTheme(node.DaddyName, node.Name);
                    TestInformation = new ThemeInformationViewModel(statisticByTheme);
                    break;
            }
        }

        private void ShowSelectedTest(string selectedTestName)
        {
            TestExistsReport testExistsReport = testsLogic.GetTest(selectedTestName);

            if (testExistsReport.Result)
            {
                test = testExistsReport.Test;
                TestInformation = new SelectedTestViewModel(test);
            }

            else
            {
                NotificationService.ShowMessageWindow(testExistsReport.Message);
                UpdateLists();
            }
        }

        /// TODO On property changed?
        private void UpdateLists()
        {
            Nodes.Clear();

            List<Node> nodes = testsLogic.GetListForTree();
            foreach (Node node in nodes)
            {
                Nodes.Add(node);
            }

            TestsNames.Clear();

            List<string> testsNames = testsLogic.ShowTestsNames();
            foreach (string name in testsNames)
            {
                TestsNames.Add(name);
            }
        }
    }
}