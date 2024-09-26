using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class EffectNode : SingleTextNode, IExpressionNode<BaseTextNode>
            {
                public IList<IExpressionInfo<BaseTextNode>> Expressions => _expressions;

                [SerializeField]
                private TextExpression[] _expressions;


#if UNITY_EDITOR
                public void AddChild(BaseTextNode child, string expression) => throw new System.NotImplementedException();

                public override void Intialize(DialogueInfo dialogueInfo)
                {
                    base.Intialize(dialogueInfo);
                    _expressions = Array.ConvertAll(dialogueInfo.EffectName, x => new TextExpression(x, _child));
                }

                public override DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = base.GetDialogueInfo();
                    if(_expressions != null)
                    {
                        dialogueInfo.EffectName = Array.ConvertAll(_expressions, x => x.Expression);
                    }
                    
                    return dialogueInfo;
                }

                public override string InputHtmlColor => "#F2F230";

                public override string OutputHtmlColor => "#61F2C2";
#endif
            }
        }
    }
}