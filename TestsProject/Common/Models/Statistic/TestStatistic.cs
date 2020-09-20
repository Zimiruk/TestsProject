using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    [Serializable]
    public class TestStatistic
    {
        public string TestName { get; set; }

        public int SuccessCount { get; set; }

        public List<int> RightAnswersCount { get; set; }

        public int AttempsCount { get; set; }
    }
}
