using System.Diagnostics;
using SoopWorkshop.Backend.Domain.Entities;
using SoopWorkshop.Backend.Infrastructure.Evaluation.Models;
using SoopWorkshop.Shared.Constants;
using SoopWorkshop.Shared.Enums;

namespace SoopWorkshop.Backend.Infrastructure.Evaluation.Checkers
{
    // Führt das kompilierte Programm für jeden Testfall aus und vergleicht es mit dem erwareteten Ergebnis
    public class TestCaseChecker
    {
        private const int TimeoutMilliseconds = 10_000;

        public async Task<CategoryResult> CheckAsync(CompilationResult compilation, List<TaskTest> tests)
        {
            var result = new CategoryResult
            {
                Id = Guid.NewGuid(),
                Category = EvaluationCategory.TestCases,
                MaxPoints = EvaluationCategoryPoints.TestCases
            };

            if (tests.Count == 0)
            {
                result.Passed = true;
                result.Points = EvaluationCategoryPoints.TestCases;
                return result;
            }

            if (!compilation.Success || compilation.MainClassName is null)
            {
                result.ErrorTip = "Da der Code nicht kompiliert, konnten keine Testfaelle ausgefuehrt werden.";

                foreach (var test in tests)
                {
                    result.TestCaseResults.Add(new TestCaseResult
                    {
                        Id = Guid.NewGuid(),
                        Description = test.Description,
                        ExpectedOutput = test.ExpectedOutput,
                        ActualOutput = string.Empty,
                        Passed = false
                    });
                }

                return result;
            }

            foreach (var test in tests)
            {
                var actualOutput = await RunProgramAsync(compilation, test.Input);
                var passed = NormalizeOutput(actualOutput) == NormalizeOutput(test.ExpectedOutput);

                result.TestCaseResults.Add(new TestCaseResult
                {
                    Id = Guid.NewGuid(),
                    Description = test.Description,
                    ExpectedOutput = test.ExpectedOutput,
                    ActualOutput = actualOutput,
                    Passed = passed
                });
            }

            var allPassed = result.TestCaseResults.All(t => t.Passed);
            var pointsPerTest = EvaluationCategoryPoints.TestCases / tests.Count;
            var passedCount = result.TestCaseResults.Count(t => t.Passed);

            // Rundungsverlust (z.B. 65 / 3 = 21) bekommt man bei bestandenen Tests trotzdem die volle Punktzahl
            result.Points = allPassed ? EvaluationCategoryPoints.TestCases : passedCount * pointsPerTest;
            result.Passed = allPassed;

            if (!allPassed)
                result.ErrorTip = "Pruefe deine Ausgabe genau gegen die erwartete Ausgabe - achte auf Gross-/Kleinschreibung, Leerzeichen und Zeilenumbrueche.";

            return result;
        }

        // Entfernt führende und abschließende Leerzeichen und vereinheitlicht Zeilenumbrueche,
        // damit kleine Formatierungsunterschiede nicht zu Fehlern führen.
        private static string NormalizeOutput(string output) =>
            output.Replace("\r\n", "\n").Trim();

        private static async Task<string> RunProgramAsync(CompilationResult compilation, string input)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = compilation.MainClassName!,
                WorkingDirectory = compilation.WorkingDirectory,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using var process = Process.Start(startInfo)!;

            if (!string.IsNullOrEmpty(input))
            {
                await process.StandardInput.WriteAsync(input);
            }
            process.StandardInput.Close();

            var outputTask = process.StandardOutput.ReadToEndAsync();
            var completed = process.WaitForExit(TimeoutMilliseconds);

            if (!completed)
            {
                process.Kill(entireProcessTree: true);
                return string.Empty;
            }

            return await outputTask;
        }
    }
}