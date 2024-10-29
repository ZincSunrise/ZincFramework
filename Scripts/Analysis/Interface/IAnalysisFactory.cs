namespace ZincFramework.Analysis
{
    public interface IAnalysisFactory
    {
        IAnalyzer CreateAnalyzer();

        ISyntaxParser CreateParser();
    }
}
