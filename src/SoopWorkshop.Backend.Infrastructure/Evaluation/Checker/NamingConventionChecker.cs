using System.Text.RegularExpressions;
using SoopWorkshop.Backend.Domain.Entities;
using SoopWorkshop.Shared.Constants;
using SoopWorkshop.Shared.Enums;

namespace SoopWorkshop.Backend.Infrastructure.Evaluation.Checker
{
    // Prüft auf Nameconventions
    // Klassennamen in PascalCase
    // Variablen und Methodennamen in lowerCamelCase
    public class NamingConventionChecker
    {
        private const int PointsPerCheck = EvaluationCategoryPoints.NamingConventions / 2;

        private static readonly Regex ClassDeclaration = new(@"class\s+([A-Za-z_][A-Za-z0-9_]*)", RegexOptions.Compiled);

        // Erkennt Identifier wie "mein_wert". SCREAMING_SNAKE_CASE Konstanten (z.B. MAX_VALUE)
        // werden bewusst nicht erfasst, da diese in Java üblich und korrekt sind.
        private static readonly Regex SnakeCaseIdentifier = new(@"\b[a-z][a-z0-9]*_[a-z0-9_]*\b", RegexOptions.Compiled);

        public CategoryResult Check(List<SubmissionFile> files)
        {
            var allContent = string.Join("\n", files.Select(f => f.Content));

            var classNamesValid = CheckClassNames(allContent);
            var noSnakeCase = !SnakeCaseIdentifier.IsMatch(allContent);

            var points = 0;
            if (classNamesValid) points += PointsPerCheck;
            if (noSnakeCase) points += PointsPerCheck;

            var result = new CategoryResult
            {
                Id = Guid.NewGuid(),
                Category = EvaluationCategory.NamingConventions,
                MaxPoints = EvaluationCategoryPoints.NamingConventions,
                Points = points,
                Passed = points == EvaluationCategoryPoints.NamingConventions,
                ErrorTip = points < EvaluationCategoryPoints.NamingConventions
                    ? "Klassen werden in PascalCase benannt (z.B. 'MeineKlasse'), Variablen und Methoden in camelCase (z.B. 'meineVariable')."
                    : string.Empty
            };

            result.TestCaseResults.Add(new TestCaseResult
            {
                Id = Guid.NewGuid(),
                Description = "Klassennamen in PascalCase",
                Passed = classNamesValid
            });

            result.TestCaseResults.Add(new TestCaseResult
            {
                Id = Guid.NewGuid(),
                Description = "Variablen- und Methodennamen in camelCase",
                Passed = noSnakeCase
            });

            return result;
        }

        private static bool CheckClassNames(string content)
        {
            var matches = ClassDeclaration.Matches(content);

            if (matches.Count == 0)
                return true;

            return matches.All(m => char.IsUpper(m.Groups[1].Value[0]));
        }
    }
}