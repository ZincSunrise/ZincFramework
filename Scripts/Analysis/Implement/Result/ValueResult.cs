namespace ZincFramework.Analysis
{
    public readonly struct ValueResult<T> : IParseResult<T>
    {
        public T Value { get; }

        public ValueResult(T value) 
        { 
            Value = value; 
        }

        public T GetResult() => Value;
    }
}