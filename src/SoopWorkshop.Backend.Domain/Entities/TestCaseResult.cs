namespace SoopWorkshop.Backend.Domain.Entities
{
    public class TestCaseResult
    {
        public Guid Id { get; set; }
        public Guid CategoryResultId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ExpectedOutput { get; set; } = string.Empty;
        public string ActualOutput { get; set; } = string.Empty;
        public bool Passed { get; set; }

        public CategoryResult CategoryResult { get; set; } = null!;
    }
}