using System.Collections.Generic;

namespace Business.Models
{
    public class TestResult
    {
        public bool Passed { get; set; }

        public string Message { get; set; }

        public int AmountOfRight { get; set; }

        public List<QuestionResult> QuestionsResult { get; set; }
    }
}