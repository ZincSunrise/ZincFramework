namespace ZincFramework.DialogueSystem.Analysis
{
    public class PlayAnimationSyntax : DialogueSyntax
    {
        public string AnimationName => Argument;

        public override string ToSyntaxString() => $"Animation({AnimationName},{Argument})";
    }
}
