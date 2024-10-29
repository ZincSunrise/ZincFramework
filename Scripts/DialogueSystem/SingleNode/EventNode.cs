using System;
using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.DialogueSystem
{
    public class EventNode : SingleTextNode, IExpressionNode
    {
        public List<TextExpression> Expressions => _expressions;

        [SerializeField]
        private List<TextExpression> _expressions = new List<TextExpression>();


#if UNITY_EDITOR
        public override void Intialize(DialogueInfo dialogueInfo)
        {
            base.Intialize(dialogueInfo);
            _expressions = new List<TextExpression>(Array.ConvertAll(dialogueInfo.EventExpression, x => new TextExpression(x, Child)));
        }

        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();
            if (dialogueInfo.EventExpression != null)
            {
                dialogueInfo.EventExpression = Array.ConvertAll(_expressions.ToArray(), x => x.Expression);
            }

            return dialogueInfo;
        }

        public void AddChild(BaseTextNode child, string expression)
        {
            if (_child == null)
            {
                _child = child;
            }
            else if (child == null && _child != null)
            {
                _expressions.Add(new TextExpression(expression, _child));
            }
            else if (child != _child)
            {
                throw new ArgumentException("传入了异常的子类");
            }
        }


        public override void SetChild(BaseTextNode child)
        {
            base.SetChild(child);
            _expressions[0] = new TextExpression(_expressions[0]?.Expression, child);
        }

        public override void DiscardChild()
        {
            base.DiscardChild();
            _expressions = null;
        }

        public override string InputHtmlColor => "#F5E960";

        public override string OutputHtmlColor => "#E7E08B";
#endif
    }
}