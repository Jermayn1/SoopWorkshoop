using System.Text.RegularExpressions;
using SoopWorkshop.Backend.Domain.Entities;
using SoopWorkshop.Shared.Constants;
using SoopWorkshop.Shared.Enums;

namespace SoopWorkshop.Backend.Infrastructure.Evaluation.Checker
{
    // Prüft, ob die eingereichten Dateien Umlaute oder ß beinhalten
    public class CharacterSetChecker
    {
        private static readonly Regex ForbiddenCharacters = new(@"[äöüÄÖÜß]", RegexOptions.Compiled);

        public CategoryResult Check(List<SubmissionFile> files)
        {
            var hasForbiddenCharacters = files.Any(file => ForbiddenCharacters.IsMatch(file.Content));

            var result = new CategoryResult
            {
                Id = Guid.NewGuid(),
                Category = EvaluationCategory.CharacterSet,
                MaxPoints = EvaluationCategoryPoints.CharacterSet,
                Points = hasForbiddenCharacters ? 0 : EvaluationCategoryPoints.CharacterSet,
                Passed = !hasForbiddenCharacters,
                ErrorTip = hasForbiddenCharacters
                    ? "Vermeide Umlaute (ä, ö, ü) und das ß-Zeichen im Code. Nutze stattdessen z.B. 'ae', 'oe', 'ue', 'ss'."
                    : string.Empty
            };

            result.TestCaseResults.Add(new TestCaseResult
            {
                Id = Guid.NewGuid(),
                Description = "Kein Umlaut oder Sonderzeichen gefunden",
                Passed = !hasForbiddenCharacters
            });

            return result;
        }
    }
}