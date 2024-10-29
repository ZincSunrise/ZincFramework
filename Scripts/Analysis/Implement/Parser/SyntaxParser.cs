namespace ZincFramework.Analysis
{
    public abstract class SyntaxParser<T> : ISyntaxParser
    {
        public virtual IParseResult ParseSyntax(object analysisTarget, ISyntax syntax) => ParseSyntax((T)analysisTarget, syntax);

        public abstract IParseResult ParseSyntax(T analysisTarget, ISyntax syntax);
    }
}
