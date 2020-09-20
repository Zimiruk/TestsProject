using Common.Models;
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

            using (FileStream fStream = File.Open($"TestsStatistic\\{testName}.dat", FileMode.Open))
            {
                TestStatistic testStatistic = (TestStatistic)formatter.Deserialize(fStream);
                return testStatistic;
            }
        }

        public void CreateTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fStream = File.Open($"TestsStatistic\\{testStatistic.TestName}.dat", FileMode.Create))
            {
                formatter.Serialize(fStream, testStatistic);
            }
        }

        public void SaveTestStatistic(TestStatistic testStatistic)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fStream = File.Open($"TestsStatistic\\{testStatistic.TestName}.dat", FileMode.Open))
            {
                formatter.Serialize(fStream, testStatistic);
            }
        }

        public void DeleteStatistic (string testName)
        {
            if (File.Exists($"TestsStatistic\\{testName}.dat"))
            {
                File.Delete($"TestsStatistic\\{testName}.dat");
            }
        }
    }
}

