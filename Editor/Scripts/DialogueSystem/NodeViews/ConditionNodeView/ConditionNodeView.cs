namespace ZincFramework.DialogueSystem.GraphView
{
    public class ConditionNodeView : MutiTextNodeView
    {
        private ConditionNode _conditionNode;

        public ConditionNodeView(ConditionNode conditionNode) : base(conditionNode)
        {
            _listView.fixedItemHeight = 60;
        }

        protected override string GetDescription() => "�������Ͻڵ�,�ڵ����������\n���е�����ѡ��Ȩ����ߵĽڵ�";


        protected override void InitListView()
        {
            _conditionNode = DisplayNode
                as ConditionNode;
            _listView.itemsSource = _conditionNode.Expressions;
            _listView.makeItem = () => new ConditionField();
            _listView.bindItem = (x, i) => (x as ConditionField).UpdateItem(i, _conditionNode.Expressions[i]);
        }
    }
}