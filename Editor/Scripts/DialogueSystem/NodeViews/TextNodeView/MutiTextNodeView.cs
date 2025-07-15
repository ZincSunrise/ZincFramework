using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace ZincFramework.DialogueSystem.GraphView
{
    public abstract class MutiTextNodeView : TextNodeView
    {
        protected ListView _listView;

        protected MutiTextNodeView(BaseTextNode baseTextNode) : base(baseTextNode)
        {
            _listView = new ListView();
            _listView.fixedItemHeight = 40;
            _listView.reorderable = true;
            _listView.style.maxHeight = new StyleLength(StyleKeyword.None);
            this.Q("down").Add(_listView);

            InitListView();
        }

        protected abstract void InitListView();

        protected override void CreateOutPort()
        {
            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            SetPortColor(OutputPort, Direction.Output);
        }

        public void Refresh()
        {
            _listView.RefreshItems();
        }
    }
}