using UnityEngine.UIElements;
using System.Collections.Generic;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class EffectNodeView : TextNodeView
    {
        private readonly ListView _listView;

        private readonly List<TextExpression> _expressionInfos;

        public EffectNodeView(EffectNode effectNode) : base(effectNode)
        {
            _expressionInfos = effectNode.Expressions;
            this.Q("down").style.maxWidth = 400;
            _listView = new ListView(_expressionInfos, 45, () => new EffectField(), (x, y) => (x as EffectField).Update(effectNode.Expressions[y], TextNode.VisibleStates));
            
            _listView.showFoldoutHeader = true;
            _listView.showAddRemoveFooter = true;
            _listView.headerTitle = "所有效果";
            _listView.style.maxHeight = Length.None();

            this.Q("down").Add(_listView);
        }
    }
}
