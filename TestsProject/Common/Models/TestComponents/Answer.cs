using System;

namespace Common.Models
{
    [Serializable]
    public class Answer
    {
        public string Content { get; set; }

        public bool IsItRight { get; set; }
    }
}
