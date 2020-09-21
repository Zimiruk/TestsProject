using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class QuestionResult
    {
        public int QuestionId { get; set; }

        public bool IsRight { get; set; }

        public bool IsOpen { get; set; }

        public bool NoChoises { get; set; }

        public List<int> WrongAnswerChoises { get; set; }
    }
}
