using UnityEngine;

namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public interface IEffectParser
    {
        void Execute(GameObject target, DialogueSyntax textSyntax);
    }
}