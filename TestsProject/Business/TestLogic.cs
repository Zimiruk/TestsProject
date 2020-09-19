using Common.Models;
using Data;
using System.Collections.Generic;

namespace Business
{
    public class TestsLogic
    {
        TestFilesOperations operations = new TestFilesOperations();

        public void SaveTest(Test test)
        {
            operations.SaveTest(test);
        }

        public List<string> ShowTestsNames()
        {
            return operations.GetTestsNames();
        }

        public Test GetTest(string testName)
        {
            return operations.GetTest(testName);
        }   
    }
}
