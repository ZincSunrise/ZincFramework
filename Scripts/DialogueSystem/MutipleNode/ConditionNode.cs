using System;
using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.DialogueSystem
{
    public class ConditionNode : MutipleTextNode, IExpressionNode
    {
        public override int ChildCount => _expressions.Count;

        public List<TextExpression> Expressions => _expressions;

        [SerializeField]
        private List<TextExpression> _expressions = new List<TextExpression>();

        public override List<BaseTextNode> GetChildren()
        {
            if (ArrayListUtility.IsNullOrEmpty(_expressions))
            {
                return null;
            }

            return _expressions.ConvertAll(x => x.ExpressionNode);
        }

        public override void ClearChild()
        {
            _expressions.Clear();
        }

        public override BaseNode CloneNode()
        {
            ConditionNode conditionNode = ScriptableObject.Instantiate(this);
            for (int i = 0; i < _expressions.Count; i++)
            {
                var node = _expressions[i].ExpressionNode.CloneNode() as BaseTextNode;
                conditionNode.SetChild(i, conditionNode);
            }

            return conditionNode;
        }

        public override void DestroyNode()
        {
            for (int i = 0; i < _expressions.Count; i++)
            {
                _expressions[i].ExpressionNode.DestroyNode();
            }

            base.DestroyNode();
        }

#if UNITY_EDITOR

        public override string InputHtmlColor => "#A755C2";

        public override string OutputHtmlColor => "#E3D3E4";

        public void AddChild(BaseTextNode child, string expression)
        {
            _expressions.Add(new TextExpression(expression, child));
        }

        public override void AddChild(BaseTextNode baseTextNode)
        {
            AddChild(baseTextNode, "null");
        }

        public override void RemoveChild(BaseTextNode baseTextNode)
        {
            _expressions.RemoveAll(x => baseTextNode == x.ExpressionNode);
        }

        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();

            if (_expressions != null)
            {
                var array = _expressions.ToArray();
                dialogueInfo.NextTextId = Array.ConvertAll(array, x => x.ExpressionNode.Index);
                dialogueInfo.ConditionExpressions = Array.ConvertAll(array, x => x.Expression);
            }

            return dialogueInfo;
        }

        public override void SetChild(int index, BaseTextNode node)
        {
            _expressions[index].ExpressionNode = node;
        }
#endif
    }
}
