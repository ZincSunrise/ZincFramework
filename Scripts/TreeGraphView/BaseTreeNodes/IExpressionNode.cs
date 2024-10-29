using System.Collections.Generic;
using ZincFramework.DialogueSystem;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.TreeService
{
    public interface IExpressionNode
    {
        List<TextExpression> Expressions { get; }

        void AddChild(BaseTextNode child, string expression);
    }
}
