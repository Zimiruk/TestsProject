using System;
using System.Collections.Generic;

namespace Common.Models.TestComponents
{
    [Serializable]
    public class Question
    {
        public string Content { get; set; }

        public List<Answer> Answers { get; set; }

        public bool IsOpen { get; set; }
    }
}