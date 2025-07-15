using UnityEditor.Experimental.GraphView;

namespace ZincFramework.DialogueSystem.GraphView
{
    public class RandomNodeView : TextNodeView
    {
        private RandomNode _randomNode;

        public RandomNodeView(RandomNode randomNode) : base(randomNode)
        {
            _randomNode = randomNode;
        }

        protected override void CreateOutPort()
        {
            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            SetPortColor(OutputPort, Direction.Output);
        }
    }
}