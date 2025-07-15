using ZincFramework.DialogueSystem;


namespace ZincFramework.TreeService
{
    public interface IExpressionNode
    {
        void AddChild(BaseTextNode child, string expression);
    }
}
