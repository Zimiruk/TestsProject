using Common;
using Common.Models.Statistic;
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

            return File.Exists($"{fileDirectory}\\{fileName}.{fileExtention}");
        }

        public TestStatistic OpenTestStatistic(string testName)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"{Constants.StatisticPath}\\{testName}.dat", FileMode.OpenOrCreate))
            {
                TestStatistic testStatistic = (TestStatistic)formatter.Deserialize(fileStream);
                return testStatistic;
            }
        }
        
        public void CreateTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"{Constants.StatisticPath}\\{testStatistic.Name}.dat", FileMode.Create))
            {
                formatter.Serialize(fileStream, testStatistic);
            }
        }

        public void SaveTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open($"{Constants.StatisticPath}\\{testStatistic.Name}.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, testStatistic);
            }
        }

        public void DeleteStatistic(string testName)
        {
            if (File.Exists($"{Constants.StatisticPath}\\{testName}.dat"))
            {
                File.Delete($"{Constants.StatisticPath}\\{testName}.dat");
            }
        }

        /// TODO Check if not Statistic
        public List<TestStatistic> GetAllStatistic()
        {
            List<TestStatistic> statistics = new List<TestStatistic>();

            string path = $"{Constants.StatisticPath}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            BinaryFormatter formatter = new BinaryFormatter();

            foreach (string file in Directory.EnumerateFiles(path, "*.dat"))
            {
                using (FileStream fileStream = File.Open(file, FileMode.OpenOrCreate))
                {
                    TestStatistic testStatistic = (TestStatistic)formatter.Deserialize(fileStream);
                    statistics.Add(testStatistic);
                }
            }

            return statistics;
        }
    }
}