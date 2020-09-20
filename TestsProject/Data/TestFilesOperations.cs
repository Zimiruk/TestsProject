using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Models;

namespace Data
{
    public class TestFilesOperations
    {
        /// TODO Check all methods here
        public List<TestForList> GetAllTests()
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
                        TestTheme = test?.Theme
                    });
                }
            }
            return tests;
        }

        public List<string> GetTestsNames()
        {
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

    }
}
