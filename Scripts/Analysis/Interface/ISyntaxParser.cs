namespace ZincFramework.Analysis
{
    public interface ISyntaxParser
    {
        IParseResult ParseSyntax(object analysisTarget, ISyntax syntax);
    }
}
