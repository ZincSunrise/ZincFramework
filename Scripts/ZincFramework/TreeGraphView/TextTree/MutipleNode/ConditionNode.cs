using System;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ConditionNode : BaseTextNode, IMutipleNode<BaseTextNode>, IExpressionNode<BaseTextNode>
            {
                public int ChildCount => _expressions.Length;

                public IList<IExpressionInfo<BaseTextNode>> Expressions => _expressions;

                [SerializeField]
                private TextExpression[] _expressions;

                public BaseTextNode[] GetChildren()
                {
                    if(ArrayListUtility.IsNullOrEmpty(_expressions))
                    {
                        return Array.Empty<BaseTextNode>();
                    }

                    return Array.ConvertAll(_expressions, x => x.ExpressionNode);
                }

                public override void ClearChild()
                {
                    _expressions = null;
                }
#if UNITY_EDITOR

                public override string InputHtmlColor => "#A755C2";

                public override string OutputHtmlColor => "#E3D3E4";

                public void AddChild(BaseTextNode child, string expression)
                {
                    var list = new List<TextExpression>(_expressions ?? Array.Empty<TextExpression>());
                    Debug.LogWarning("你正在使用默认条件，它默认为null");
                    list.Add(new TextExpression(expression, child));

                    _expressions = list.ToArray();
                }

                public void AddChild(BaseTextNode baseTextNode)
                {
                    AddChild(baseTextNode, "null");
                }

                public void RemoveChild(BaseTextNode baseTextNode)
                {
                    var list = new List<TextExpression>(_expressions);
                    Debug.LogWarning("你正在使用默认条件，它默认为null");
                    list.RemoveAll(x => x.ExpressionNode == baseTextNode);

                    _expressions = list.ToArray();
                }

                public override DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = base.GetDialogueInfo();

                    if(_expressions != null)
                    {
                        dialogueInfo.NextTextId = Array.ConvertAll(_expressions, x => x.ExpressionNode.Index);
                        dialogueInfo.ConditionExpressions = Array.ConvertAll(_expressions, x => x.Expression);
                    }
                  
                    return dialogueInfo;

                }
#endif
            }
        }
    }
}
