using System.Collections.Generic;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        public interface IExpressionParser<T> where T : BaseNode
        {
            T ParseExpression(IList<IExpressionInfo<T>> expressions);
        }
    }
}