using System;
using System.Collections.Generic;

namespace Common.Models.TestComponents
{
    [Serializable]
    public class Test
    {
        public string Name { get; set; }

        public string Theme { get; set; }

        public List<string> SubThemes { get; set; }

        public List<Question> Questions { get; set; }

        public int TimerCountdown { get; set; }

        public bool ShowAnswerAtEnd { get; set; }

        public int ToPassAmount { get; set; }
    }
}