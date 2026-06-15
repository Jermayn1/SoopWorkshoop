using SoopWorkshop.Backend.Application.Evaluation.Interfaces;
using SoopWorkshop.Backend.Domain.Entities;
using SoopWorkshop.Backend.Infrastructure.Evaluation.Checkers;

namespace SoopWorkshop.Backend.Infrastructure.Evaluation
{
    public class JavaAnalyzer : IJavaAnalyzer
    {
        private readonly CharacterSetChecker _characterSetChecker;
        private readonly NamingConventionChecker _namingConventionChecker;
        private readonly CompilabilityChecker _compilabilityChecker;
        private readonly TestCaseChecker _testCaseChecker;

        public JavaAnalyzer(
            CharacterSetChecker characterSetChecker,
            NamingConventionChecker namingConventionChecker,
            CompilabilityChecker compilabilityChecker,
            TestCaseChecker testCaseChecker)
        {
            _characterSetChecker = characterSetChecker;
            _namingConventionChecker = namingConventionChecker;
            _compilabilityChecker = compilabilityChecker;
            _testCaseChecker = testCaseChecker;
        }

        public async Task<EvaluationResult> AnalyzeAsync(Submission submission, List<TaskTest> expectedTests)
        {
            var files = submission.Files.ToList();

            var characterSetResult = _characterSetChecker.Check(files);
            var namingConventionResult = _namingConventionChecker.Check(files);
            var (compilabilityResult, compilation) = await _compilabilityChecker.CheckAsync(files);
            var testCaseResult = await _testCaseChecker.CheckAsync(compilation, expectedTests);

            CleanupWorkingDirectory(compilation.WorkingDirectory);

            var categoryResults = new List<CategoryResult>
        {
            characterSetResult,
            namingConventionResult,
            compilabilityResult,
            testCaseResult
        };

            var evaluationResult = new EvaluationResult
            {
                Id = Guid.NewGuid(),
                SubmissionId = submission.Id,
                TotalScore = categoryResults.Sum(c => c.Points),
                MaxScore = categoryResults.Sum(c => c.MaxPoints),
                CategoryResults = categoryResults
            };

            foreach (var categoryResult in categoryResults)
            {
                categoryResult.EvaluationResultId = evaluationResult.Id;
            }

            return evaluationResult;
        }

        // Löscht das temporäre Verzeichnis mit dem kompilierten Dateien damit der Speicher nicht unnötig voll wird
        private static void CleanupWorkingDirectory(string workingDirectory)
        {
            if (string.IsNullOrEmpty(workingDirectory) || !Directory.Exists(workingDirectory))
                return;

            try
            {
                Directory.Delete(workingDirectory, recursive: true);
            }
            catch (IOException)
            {
                // Aufräumfehler sollen die Auswertung nicht fehlschlagen lassen
            }
        }
    }
}