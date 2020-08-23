using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestsProject
{
    class FilesOperator
    {
        public static void SaveTest(Test test)
        {
            Console.WriteLine("Enter test name for saving");
            string testName = Console.ReadLine();

            BinaryFormatter formatter = new BinaryFormatter();

            string path = @"MyAmazingTests";
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            using (FileStream fStream = File.Open($"MyAmazingTests\\{testName}.test", FileMode.Create))

            {
                formatter.Serialize(fStream, test);
                Console.WriteLine($"File {testName}.test saved");
            }
        }

        public static bool CheckIfTestExists(string testName)
        {
            if (File.Exists($"MyAmazingTests\\{testName}.test"))
            {
                return true;
            }

            else return false;
        }

        public static Test OpenTest(string testName)
        { 
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fStream = File.Open($"MyAmazingTests\\{testName}.test", FileMode.Open))
            {
                Test test = (Test)formatter.Deserialize(fStream);
                return test;
            }

        }
    }
}


