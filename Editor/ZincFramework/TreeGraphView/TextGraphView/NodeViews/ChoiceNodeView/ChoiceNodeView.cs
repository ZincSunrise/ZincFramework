using System.Collections.Generic;
using System.IO;
using ZincFramework.Events;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ChoiceNodeView : TextNodeView
            {
                private static readonly string _loadPath = Path.Combine("Assets/Editor/Zincframework/TreeGraphView/TextGraphView/NodeViews/ChoiceNodeView/ChoiceNodeView.uxml");

                public List<Port> OutputPorts { get; } = new List<Port>();

                public ListView ChoiceListView { get; }

                private ChoiceNode ChoiceNode => TextNode as ChoiceNode;


                public ZincAction<BaseTextNode> OnNodeDelete;


                public ChoiceNodeView(ChoiceNode choiceNode) : base(choiceNode, _loadPath)
                {             
                    ChoiceListView = new ListView(choiceNode.ChoiceInfos, 40, () => new ChoiceField(), (x, i) => 
                    {
                        ChoiceField choiceField = x as ChoiceField;
                     
                        choiceField.NameLabel.text = "СЎПо" + (i + 1);
                        choiceField.TextField.value = choiceNode.ChoiceInfos[i].ChoiceText;
                        choiceField.RegistValueChange(choiceNode.ChoiceInfos[i]);
                    });

                    ChoiceListView.reorderable = true;
                    ChoiceListView.style.maxHeight = new StyleLength(StyleKeyword.None);
                    this.Q("down").Add(ChoiceListView);
                }


                public void Refrash()
                {
                    ChoiceListView.RefreshItems();
                }
            }
        }
    }
}
