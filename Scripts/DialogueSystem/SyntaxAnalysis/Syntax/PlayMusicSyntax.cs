namespace ZincFramework.DialogueSystem.Analysis
{
    public class PlayMusicSyntax : DialogueSyntax
    {
        public override string ToSyntaxString() => $"PlayMusic({TargetKey},{Argument})";
    }
}