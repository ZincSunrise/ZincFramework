using UnityEngine;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public abstract class CancelableEffectParser<T> : DialogueParser<T> , IEffectParser, ICancelable where T : IParseResult
    {
        public abstract void Cancel();

        public abstract void Execute(GameObject target, DialogueSyntax textSyntax);
    }
}
