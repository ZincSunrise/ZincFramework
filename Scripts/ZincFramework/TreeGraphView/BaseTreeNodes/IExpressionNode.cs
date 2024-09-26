using System.Collections.Generic;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        public interface IExpressionNode<T> where T : BaseNode
        {
            IList<IExpressionInfo<T>> Expressions { get; }

            void AddChild(T child, string expression);
        }

        public interface IExpressionInfo<T> where T : BaseNode
        {
            public string Expression { get; }

            public T ExpressionNode { get; }
        }
    }
}
