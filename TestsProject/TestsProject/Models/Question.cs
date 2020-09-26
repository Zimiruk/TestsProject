using System;
using System.Collections.Generic;

namespace TestsProject
{
    [Serializable]
    public class Question
    {
        public string QuestionContent { get; set; }
        public List<Answer> Answers { get; set; }

        public Question(string questionContent)
        {
            QuestionContent = questionContent;
            Answers = new List<Answer>();
        }
    }
}