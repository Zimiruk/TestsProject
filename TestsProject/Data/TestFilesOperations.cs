using Common;
using Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    public class TestFilesOperations
    {
        /// TODO Check all methods here
        /// not working
        public List<TestForList> GetAllTestsNames()
        {
            List<TestForList> tests = new List<TestForList>();

            string path = $"{Constants.TestPath}";
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (string file in Directory.EnumerateFiles(path, "*.test"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.Open))
                {
                    Test test = (Test)formatter.Deserialize(fileStream);
                    tests.Add(new TestForList
                    {
                        TestName = test?.Name,
                        TestTheme = test?.Theme,
                        SubThemes = test?.SubThemes
                    });
                }
            }
            return tests;
        }

        /// Not sure
        public ObservableCollection<Node> GetListForTree()
        {
            ObservableCollection<Node> nodes = new ObservableCollection<Node>();

            List<TestForList> tests = GetAllTestsNames();

            IEnumerable<string> themes = tests.Select(x => x.TestTheme).Distinct();

            foreach (string theme in themes)
            {
                Node node = new Node { Name = theme, NodeType = MyEnum.Nodes.Theme, ViewName = $"Theme: {theme}" };
                node.Nodes = new ObservableCollection<Node>();

                List<TestForList> testsByTheme = tests.FindAll(x => x.TestTheme == theme);

                List<TestForList> testsWithoutSubtheme = testsByTheme.FindAll(x => x.SubThemes.Count == 0);
                List<TestForList> testsWithSubtheme = testsByTheme.FindAll(x => x.SubThemes.Count > 0);

                IEnumerable<string> subThemes = testsWithSubtheme.SelectMany(x => x.SubThemes).Distinct();

                foreach (TestForList test in testsWithoutSubtheme)
                {
                    node.Nodes.Add(new Node { Name = test.TestName, DaddyName = node.Name, NodeType = MyEnum.Nodes.Test, ViewName = $"Test: {test.TestName}" });
                }

                foreach (string subTheme in subThemes)
                {
                    Node subNode = new Node { Name = subTheme, DaddyName = node.Name, NodeType = MyEnum.Nodes.SubTheme, ViewName = $"SubTheme: {subTheme}" };
                    subNode.Nodes = new ObservableCollection<Node>();

                    List<TestForList> testsWithThatSubTheme = testsWithSubtheme.FindAll(x => x.SubThemes.Contains(subTheme));

                    foreach (TestForList test in testsWithThatSubTheme)
                    {
                        subNode.Nodes.Add(new Node { Name = test.TestName, DaddyName = subNode.Name, NodeType = MyEnum.Nodes.Test, ViewName = $"Test: {test.TestName}" });
                    }

                    node.Nodes.Add(subNode);
                }

                nodes.Add(node);
            }

            return nodes;
        }
               

        public List<string> GetTestsNames()
        {
            GetListForTree();
            List<string> tests = new List<string>();

            string path = $"{Constants.TestPath}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            BinaryFormatter formatter = new BinaryFormatter();

            ///TODO Exception
            foreach (string file in Directory.EnumerateFiles(path, "*.test"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.Open))
                {
                    Test test = (Test)formatter.Deserialize(fileStream);
                    tests.Add(test.Name);
                }
            }
            return tests;
        }

        public string SaveTest(Test test)
        {
            string path = $"{Constants.TestPath}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"{Constants.TestPath}\\{ test.Name}.test", FileMode.Create))

            {
                formatter.Serialize(fileStream, test);
            }

            return "Test Added";
        }

        public Test GetTest(string testName)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"{Constants.TestPath}\\{testName}.test", FileMode.Open))
            {
                Test test = (Test)formatter.Deserialize(fileStream);
                return test;
            }
        }

        /// TODO DoSomething here
        public bool CheckIfFileExists(string fileName, string fileDirectory, string fileExtention)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(fileDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            if (File.Exists($"{fileDirectory}\\{fileName}.{fileExtention}"))
            {
                return true;
            }

            else return false;
        }

    }
}
