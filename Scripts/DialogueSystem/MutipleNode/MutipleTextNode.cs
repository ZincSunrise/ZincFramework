using ZincFramework.TreeService;

namespace ZincFramework.DialogueSystem
{
    public abstract class MutipleTextNode : BaseTextNode, IMutipleNode<BaseTextNode>
    {
        public override bool IsEndNode => false;

        public abstract int ChildCount { get; }


#if UNITY_EDITOR
        public abstract void AddChild(BaseTextNode node);

        public abstract void RemoveChild(BaseTextNode node);

        public abstract void SetChild(int index, BaseTextNode node);
#endif
    }
}
