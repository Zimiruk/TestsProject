using Common;
using Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    public class StatisticFilesOperator
    {
        /// TODO Check all methods here
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

        public TestStatistic OpenTestStatistic(string testName)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"TestsStatistic\\{testName}.dat", FileMode.Open))
            {
                TestStatistic testStatistic = (TestStatistic)formatter.Deserialize(fileStream);
                return testStatistic;
            }
        }

        public void CreateTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"TestsStatistic\\{testStatistic.TestName}.dat", FileMode.Create))
            {
                formatter.Serialize(fileStream, testStatistic);
            }
        }

        public void SaveTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"TestsStatistic\\{testStatistic.TestName}.dat", FileMode.Open))
            {
                formatter.Serialize(fileStream, testStatistic);
            }
        }

        public void DeleteStatistic(string testName)
        {
            if (File.Exists($"TestsStatistic\\{testName}.dat"))
            {
                File.Delete($"TestsStatistic\\{testName}.dat");
            }
        }

        /// TODO Check if not Statistic
        public List<TestStatistic> GetAllStatistic()
        {
            List<TestStatistic> statistics = new List<TestStatistic>();

            string path = $"{Constants.StatisticPath}";
            BinaryFormatter formatter = new BinaryFormatter();

            foreach (string file in Directory.EnumerateFiles(path, "*.dat"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.Open))
                {
                    TestStatistic testStatistic = (TestStatistic)formatter.Deserialize(fileStream);
                    statistics.Add(testStatistic);
                }
            }

            return statistics;
        }
    }
}

