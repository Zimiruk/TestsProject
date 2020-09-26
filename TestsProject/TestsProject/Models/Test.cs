using System;
using System.Collections.Generic;

namespace TestsProject
{
    [Serializable]
    public class Test
    {
        public string Theme { get; set; }
        List<string> SubThemes { get; set; }
        public List<Question> Questions { get; set; }

        public Test(string theme)
        {
            Theme = theme;
            Questions = new List<Question>();
        }
    }
}