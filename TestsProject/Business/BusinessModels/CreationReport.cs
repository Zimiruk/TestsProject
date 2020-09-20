using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class CreationReport
    {
        public bool Result { get; set; }

        public string Message { get; set; }

        public List<int> BadQuestions { get; set; }
    }
}
