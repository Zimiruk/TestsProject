using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TestsProject.Utility;

namespace TestsProject
{
    public class FilesOperator
    {
        public static void SaveTest(Test test)
        {
            Console.WriteLine("Enter test name for saving");
            string testName = Console.ReadLine();

            BinaryFormatter formatter = new BinaryFormatter();

            string path = $"{Constants.TestPath}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            using (FileStream fileStream = File.Open($"{Constants.TestPath}\\{ testName}.test", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, test);
                Console.WriteLine($"File {testName}.test saved");
            }
        }

        public static bool CheckIfTestExists(string testName)
        {
            if (File.Exists($"{Constants.TestPath}\\{testName}.test"))
            {
                return true;
            }

            else return false;
        }

        public static Test OpenTest(string testName)
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