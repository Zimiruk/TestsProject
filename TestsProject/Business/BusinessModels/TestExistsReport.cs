using Common.Models.TestComponents;

namespace Business.BusinessModels
{
    public class TestExistsReport
    {
        public string Message { get; set; }

        public bool Result { get; set; }

        public Test Test { get; set; }
    }
}
