using System;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class EventNode : SingleTextNode, IExpressionNode<BaseTextNode>
            {
                public IList<IExpressionInfo<BaseTextNode>> Expressions => _expressions;

                [SerializeField]
                private TextExpression[] _expressions;

#if UNITY_EDITOR
                void IExpressionNode<BaseTextNode>.AddChild(BaseTextNode child, string expression)
                {
                    if (Child == null)
                    {
                        _child = child;
                    }
                    else if (child != _child)
                    {
                        throw new ArgumentException("传入了异常的子类");
                    }

                    var list = new List<TextExpression>(_expressions);
                    list.Add(new TextExpression(expression, _child));

                    _expressions = list.ToArray();
                }

                public override void Intialize(DialogueInfo dialogueInfo)
                {
                    base.Intialize(dialogueInfo);
                    _expressions = Array.ConvertAll(dialogueInfo.EventExpression, x => new TextExpression(x, Child));
                }

                public override DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = base.GetDialogueInfo();
                    if(dialogueInfo.EventExpression != null)
                    {
                        dialogueInfo.EventExpression = Array.ConvertAll(_expressions, x => x.Expression);
                    }           
                    return dialogueInfo;
                }

                public override void SetChild(BaseTextNode child)
                {
                    base.SetChild(child);
                    _expressions ??= new TextExpression[1];
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
    }
}