namespace Common.Models
{
    public class StatisticByTheme
    {
        public string ObjectName { get; set; }

        public MyEnum.Nodes ObjectType { get; set; }

        public int TestsAmount { get; set; }

        public double ProcentOfRight { get; set; }

        public double ProcentOfSuccess { get; set; }
    }
}
    