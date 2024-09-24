using UnityEngine;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            [System.Serializable]
            public class TextExpression : IExpressionInfo<BaseTextNode>
            {
                public string Expression => _expression;

                public BaseTextNode ExpressionNode => _expressionNode;

                [SerializeField]
                private string _expression;

                [SerializeField]
                private BaseTextNode _expressionNode;

                public TextExpression(string expression, BaseTextNode baseTextNode)
                {
                    _expression = expression;
                    _expressionNode = baseTextNode;
                }
            }
        }
    }
}