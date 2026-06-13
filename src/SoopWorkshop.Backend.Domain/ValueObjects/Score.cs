namespace SoopWorkshop.Backend.Domain.ValueObjects
{
    public class Score
    {
        public int Achieved { get; set; }
        public int Max { get; set; }
        public double Percentage => Max == 0 ? 0 : (double)Achieved / Max * 100;
    }
}
