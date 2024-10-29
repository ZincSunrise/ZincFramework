namespace ZincFramework.Analysis
{
    public interface IAnalysisService
    {
        IAnalyzer GetAnalyzer(string key);

        ISyntaxParser GetSyntaxParser(string key);

        IAnalysisFactory GetAnalysisFactory(string key);
    }

    public interface IAnalysisService<TAnalyzer, TParser, TFactory> where TAnalyzer : IAnalyzer where TParser : ISyntaxParser where TFactory : IAnalysisFactory
    {
        TAnalyzer GetAnalyzer(string key);

        TParser GetSyntaxParser(string key);

        TFactory GetAnalysisFactory(string key);
    }
}
