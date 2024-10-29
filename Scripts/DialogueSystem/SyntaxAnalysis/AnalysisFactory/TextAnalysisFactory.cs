using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.Factory
{
    public abstract class TextAnalysisFactory : IAnalysisFactory
    {
        public abstract bool IsTarget(string text);

        public abstract IAnalyzer CreateAnalyzer();

        public abstract ISyntaxParser CreateParser();
    }
}