namespace SoopWorkshop.Shared.Constants
{
    // Maximale Punktzahl pro Bewertungskategorie
    // Später werden weitere Bewerungskategorien hinzugefügt
    public static class EvaluationCategoryPoints
    {
        public const int CharacterSet = 5;
        public const int NamingConventions = 10;
        public const int Compilability = 20;
        public const int TestCases = 65;

        public const int Total =
            CharacterSet +
            NamingConventions +
            Compilability +
            TestCases;
    }
}