namespace ZincFramework.DialogueSystem.GraphView
{
    public class ConditionNodeView : MutiTextNodeView
    {
        private ConditionNode _conditionNode;

        public ConditionNodeView(ConditionNode conditionNode) : base(conditionNode)
        {
            _listView.fixedItemHeight = 60;
        }

        protected override string GetDescription() => "条件集合节点,节点结束后会根据\n现有的条件选择权重最高的节点";

        protected override void InitListView()
        {

        }
    }
}