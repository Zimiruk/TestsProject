using System.Collections.Generic;

namespace Business.Models
{
    public class CreationReport
    {
        public bool Result { get; set; }

        public string Message { get; set; }

        public List<int> BadQuestions { get; set; }
    }
}