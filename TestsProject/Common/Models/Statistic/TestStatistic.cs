using System;
using System.Collections.Generic;

namespace Common.Models.Statistic
{
    [Serializable]
    public class TestStatistic
    {
        public string Name { get; set; }

        public int SuccessCount { get; set; }

        public int AttempsCount { get; set; }

        public List<int> RightAnswersCount { get; set; }

        public List<double> RightAnswersProcent { get; set; }

        public int QuestionAmount { get; set; }
    }
}