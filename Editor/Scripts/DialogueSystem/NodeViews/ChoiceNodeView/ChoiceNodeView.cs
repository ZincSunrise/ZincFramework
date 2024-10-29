using System.Collections;

namespace ZincFramework.DialogueSystem.GraphView
{
    public class ChoiceNodeView : MutiTextNodeView
    {
        protected ChoiceNode _choiceNode;

        public ChoiceNodeView(ChoiceNode choiceNode) : base(choiceNode)
        {

        }

        protected override void InitListView()
        {
            _choiceNode = DisplayNode as ChoiceNode;

            _listView.itemsSource = _choiceNode.ChoiceInfos;
            _listView.makeItem = () => new ChoiceField();

            _listView.bindItem = (x, i) =>
            {
                ChoiceField choiceField = x as ChoiceField;

                choiceField.NameLabel.text = "选项" + (i + 1);
                choiceField.TextField.value = _choiceNode.ChoiceInfos[i].ChoiceText;
                choiceField.RegistValueChange(_choiceNode.ChoiceInfos[i]);
            };
        }

        protected override string GetDescription() => "选择节点,节点结束后会出现选择分支";
    }
}
