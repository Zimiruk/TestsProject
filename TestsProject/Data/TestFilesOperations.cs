using Common;
using Common.Models.Others;
using Common.Models.TestComponents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    public class TestFilesOperations
    {
        public List<TestForList> GetAllTestsNames()
        {
            List<TestForList> tests = new List<TestForList>();

            string path = $"{Constants.TestPath}";

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (string file in Directory.EnumerateFiles(path, $"*.{Constants.TestExtenstion}"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.OpenOrCreate))
                {
                    Test test = (Test)formatter.Deserialize(fileStream);
                    tests.Add(new TestForList
                    {
                        Name = test?.Name,
                        Theme = test?.Theme,
                        SubThemes = test?.SubThemes
                    });
                }
            }
            return tests;
        }

        public List<Node> GetListForTree()
        {
           List<Node> nodes = new List<Node>();

            List<TestForList> tests = GetAllTestsNames();

            IEnumerable<string> themes = tests.Select(x => x.Theme).Distinct();

            foreach (string theme in themes)
            {
                Node node = new Node 
                { 
                    Name = theme, NodeType = Constants.Theme, ViewName = $"Theme: {theme}" 
                };
                node.Nodes = new ObservableCollection<Node>();

                List<TestForList> testsByTheme = tests.FindAll(x => x.Theme == theme);

                List<TestForList> testsWithoutSubtheme = testsByTheme.FindAll(x => x.SubThemes.Count == 0);
                List<TestForList> testsWithSubtheme = testsByTheme.FindAll(x => x.SubThemes.Count > 0);

                IEnumerable<string> subThemes = testsWithSubtheme.SelectMany(x => x.SubThemes).Distinct();

                foreach (TestForList test in testsWithoutSubtheme)
                {
                    node.Nodes.Add(new Node 
                    {
                        Name = test.Name, DaddyName = node.Name, NodeType = Constants.Test, ViewName = $"Test: {test.Name}" 
                    });
                }

                foreach (string subTheme in subThemes)
                {
                    Node subNode = new Node 
                    { 
                        Name = subTheme, DaddyName = node.Name, NodeType = Constants.SubTheme, ViewName = $"SubTheme: {subTheme}" 
                    };
                    subNode.Nodes = new ObservableCollection<Node>();

                    List<TestForList> testsWithThatSubTheme = testsWithSubtheme.FindAll(x => x.SubThemes.Contains(subTheme));

                    foreach (TestForList test in testsWithThatSubTheme)
                    {
                        subNode.Nodes.Add(new Node 
                        { 
                            Name = test.Name, DaddyName = subNode.Name, NodeType = Constants.Test, ViewName = $"Test: {test.Name}" 
                        });
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

            foreach (string file in Directory.EnumerateFiles(path, $"*.{Constants.TestExtenstion}"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.OpenOrCreate))
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

            using (FileStream fileStream = File.Open($"{Constants.TestPath}\\{test.Name}.{Constants.TestExtenstion}", FileMode.Create))

            {
                formatter.Serialize(fileStream, test);
            }

            return Constants.TestAdded;
        }

        public Test GetTest(string testName)
        {
            BinaryFormatter formatter = new BinaryFormatter();            

            using (FileStream fileStream = File.Open($"{Constants.TestPath}\\{testName}.{Constants.TestExtenstion}", FileMode.OpenOrCreate))
            {
                Test test = (Test)formatter.Deserialize(fileStream);
                return test;
            }
        }

        public bool CheckIfFileExists(string fileName, string fileDirectory, string fileExtention)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(fileDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            return File.Exists($"{fileDirectory}\\{fileName}.{fileExtention}");
        }

        public void DeleteTest(string testName)
        {
            if (File.Exists($"{Constants.TestPath}\\{testName}.{Constants.TestExtenstion}"))
            {
                File.Delete($"{Constants.TestPath}\\{testName}.{Constants.TestExtenstion}");
            }
        }
    }
}