using UnityEngine;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public abstract class DialogueParser<T> : DialogueParser where T : IParseResult
    {
        public override IParseResult ParseSyntax(GameObject analysisTarget, DialogueSyntax syntax) => ParseSyntaxTyped(analysisTarget, syntax);

        public abstract T ParseSyntaxTyped(GameObject analysisTarget, DialogueSyntax textSyntax);
    }
}