using Business;
using Common;
using Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModel.Commands;

namespace ViewModel
{
    public class TestsListViewModel : BaseViewModel
    {
        private TestsLogic testsLogic = new TestsLogic();
        private StatisticLogic statisticLogic = new StatisticLogic();

        private Test test;

        public TestsListViewModel()
        {
            Node.SendNode += GetNode;

            List<string> testsNames = testsLogic.ShowTestsNames();  
            nodes = testsLogic.GetListForTree();

            foreach (string name in testsNames)
            {
                _testsNames.Add(name);
            }
        }

        private ObservableCollection<string> _testsNames = new ObservableCollection<string>();
        public ObservableCollection<string> TestsNames { get { return _testsNames; } }

        private ObservableCollection<Node> nodes = new ObservableCollection<Node>();
        public ObservableCollection<Node> Nodes { get { return nodes; } }


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
                case MyEnum.Nodes.Test:
                    ShowSelectedTest(node.Name);
                    break;

                case MyEnum.Nodes.Theme:
                    statisticByTheme = statisticLogic.GetTestStatisticByTheme(node.Name);
                    TestInformation = new ThemeInformationViewModel(statisticByTheme);
                    break;

                case MyEnum.Nodes.SubTheme:
                    statisticByTheme = statisticLogic.GetTestStatisticByTheme(node.DaddyName, node.Name);
                    TestInformation = new ThemeInformationViewModel(statisticByTheme);
                    break;
            }
        }

        private void ShowSelectedTest(string selectedTestName)
        {
            test = testsLogic.GetTest(selectedTestName);           
            TestInformation = new SelectedTestViewModel(test);
        }
    }
}
