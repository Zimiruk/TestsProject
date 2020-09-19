using System;

namespace TestsProject
{
    [Serializable]
    public class Answer
    {
        public string Content { get; set; }
        public bool IsItRight { get; set; }

        public Answer(string content, bool isItRight)
        {
            Content = content;
            IsItRight = isItRight;
        }
    }
}