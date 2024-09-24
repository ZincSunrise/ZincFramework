namespace ZincFramework
{
    public class Index
    {
        public int MyIndex { get; set; }

        public static implicit operator int(Index index) => index.MyIndex;

        public static Index operator + (Index a, int b)
        {
            a.MyIndex += b;
            return a;
        }

        public static Index operator -(Index a, int b) 
        {
            a.MyIndex -= b;
            return a;
        }

        public static Index operator *(Index a, int b) 
        {
            a.MyIndex *= b;
            return a;
        }

        public static Index operator /(Index a, int b) 
        {
            a.MyIndex /= b;
            return a;
        }
    }
}

