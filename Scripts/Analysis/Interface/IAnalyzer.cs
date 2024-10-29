namespace ZincFramework.Analysis
{
    public interface IAnalyzer
    {
        ISyntax Analyze(string text);
    }

    public interface IAnalyzer<T> : IAnalyzer where T : ISyntax 
    {
        ISyntax IAnalyzer.Analyze(string text) => Analyze(text);

        new T Analyze(string text);
    }
}
