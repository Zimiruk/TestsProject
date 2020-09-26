using System.Collections.Generic;

namespace Business.Models
{
    public class QuestionResult
    {
        public int Id { get; set; }

        public bool IsRight { get; set; }

        public bool IsOpen { get; set; }

        public bool NoChoises { get; set; }

        public List<int> WrongAnswerChoises { get; set; }
    }
}