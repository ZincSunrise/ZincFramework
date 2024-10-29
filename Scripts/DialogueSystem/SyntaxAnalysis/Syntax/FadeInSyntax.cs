namespace ZincFramework.DialogueSystem.Analysis
{
    public class FadeInSyntax : DialogueSyntax
    {
        public override string ToSyntaxString() => $"FadeIn({TargetKey},{Argument})";
    }
}