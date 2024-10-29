using System;


namespace ZincFramework.DialogueSystem.Analysis
{
    public class FadeInAnalyzer : DialogueAnalyzer<FadeInSyntax>
    {
        private FadeInSyntax _fadeInSyntax = new FadeInSyntax();

        public bool CanAnalyze(string text) => text.Contains("FadeIn", StringComparison.OrdinalIgnoreCase);

        public override FadeInSyntax AnalyzeTyped(string text)
        {
            return _fadeInSyntax;
        }
    }
}