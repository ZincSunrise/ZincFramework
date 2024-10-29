namespace ZincFramework.Analysis
{
    public interface IParseResult
    {
        public static IParseResult Empty { get; } = new EmptyResult();

        object GetResult();
    }

    public interface IParseResult<out T> : IParseResult
    {
        object IParseResult.GetResult() => GetResult();

        new T GetResult();
    }
}