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
                public IExpressionParser<BaseTextNode> Parser { get; set; }

                public int ChildCount => _expressions.Length;

                public IList<IExpressionInfo<BaseTextNode>> Expressions => _expressions;

                [SerializeField]
                private TextExpression[] _expressions;

                public override BaseTextNode Execute()
                {
                    return Parser.ParseExpression(_expressions);
                }

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
                    Debug.LogWarning("������ʹ��Ĭ����������Ĭ��Ϊnull");
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
                    Debug.LogWarning("������ʹ��Ĭ����������Ĭ��Ϊnull");
                    list.RemoveAll(x => x.ExpressionNode == baseTextNode);

                    _expressions = list.ToArray();
                }
#endif
            }
        }
    }
}
