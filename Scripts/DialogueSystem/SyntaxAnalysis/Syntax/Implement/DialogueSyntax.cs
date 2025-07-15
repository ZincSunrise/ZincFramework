using ZincFramework.Analysis;
using ZincFramework.Pools;


namespace ZincFramework.DialogueSystem.Analysis
{
    public abstract class DialogueSyntax : ISyntax, IReuseable
    {
        public string TargetKey { get; set; }

        public string Argument { get; set; }


        public abstract string ToSyntaxString();


        public virtual void Reset(string targetKey, string argument)
        {
            TargetKey = targetKey;
            Argument = argument;
        }

        public virtual void OnRent()
        {

        }

        public virtual void OnReturn()
        {

        }
    }
}