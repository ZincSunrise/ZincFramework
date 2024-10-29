namespace ZincFramework.DialogueSystem.Analysis
{
    public class BackgroundSyntax : DialogueSyntax
    {
        public override string ToSyntaxString() => $"ChangeBackground({TargetKey},{Argument})";
    }
}