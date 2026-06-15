namespace SoopWorkshop.Backend.Infrastructure.Evaluation.Models
{
    // Ergebnis Mode vom Kompilervorgang
    // Wird von TestCaseChecker benötigt
    public class CompilationResult
    {
        public bool Success { get; set; }
        public string WorkingDirectory { get; set; } = string.Empty;
        public string ErrorOutput { get; set; } = string.Empty;
        public string? MainClassName { get; set; }
    }
}