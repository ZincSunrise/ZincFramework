using System;
using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.DialogueSystem
{
    public class EffectNode : SingleTextNode, IExpressionNode
    {
        public List<TextExpression> Expressions => _expressions;

        [SerializeField]
        private List<TextExpression> _expressions = new List<TextExpression>();


#if UNITY_EDITOR
        public void AddChild(BaseTextNode child, string expression) => throw new System.NotImplementedException();

        public override void Intialize(DialogueInfo dialogueInfo)
        {
            base.Intialize(dialogueInfo);
            _expressions = new List<TextExpression>(Array.ConvertAll(dialogueInfo.EffectNames, x => new TextExpression(x, _child)));
        }

        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();
            if (_expressions != null)
            {
                dialogueInfo.EffectNames = Array.ConvertAll(_expressions.ToArray(), x => x.Expression);
            }

            return dialogueInfo;
        }

        public override string InputHtmlColor => "#F2F230";

        public override string OutputHtmlColor => "#61F2C2";
#endif
    }
}