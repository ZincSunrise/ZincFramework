namespace ZincFramework.DialogueSystem.Analysis
{
    public class PlaySoundSyntax : DialogueSyntax
    {
        public override string ToSyntaxString() => $"PlaySound({TargetKey},{Argument})";
    }
}