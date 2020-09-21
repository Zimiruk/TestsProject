﻿using System;
using System.Collections.Generic;

namespace Common.Models
{
    [Serializable]
    public class Test
    {
        public string Name { get; set; }

        public string Theme { get; set; }

        List<string> SubThemes { get; set; }

        public List<Question> Questions { get; set; }

        public int TimerCountdown { get; set; }

        public bool ShowAnswerAtEnd;
    }
}
