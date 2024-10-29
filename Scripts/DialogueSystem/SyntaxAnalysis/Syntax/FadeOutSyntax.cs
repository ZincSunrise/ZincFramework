namespace ZincFramework.DialogueSystem.Analysis
{
    public class FadeOutSyntax : DialogueSyntax
    {
        public override string ToSyntaxString() => $"FadeOut({TargetKey},{Argument})";
    }
}