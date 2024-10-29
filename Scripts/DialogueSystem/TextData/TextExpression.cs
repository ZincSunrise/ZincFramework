using UnityEngine;


namespace ZincFramework.DialogueSystem.TextData
{
    [System.Serializable]
    public class TextExpression
    {
        public string Expression => _expression;

        public BaseTextNode ExpressionNode
        {
            get => _expressionNode;
            set => _expressionNode = value;
        }

        [SerializeField]
        private string _expression;

        [SerializeField]
        private BaseTextNode _expressionNode;

        public TextExpression(string expression, BaseTextNode baseTextNode)
        {
            _expression = expression;
            _expressionNode = baseTextNode;
        }

#if UNITY_EDITOR
        public void SetNode(BaseTextNode baseTextNode)
        {
            _expressionNode = baseTextNode;
        }
#endif
    }
}