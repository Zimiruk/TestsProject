using System;
using System.Collections.Generic;

namespace Common.Models
{
    [Serializable]
    public class Question
    {
        public string QuestionContent { get; set; }

        public List<Answer> Answers { get; set; }

        public bool IsOpen { get; set; }
    }
}
