namespace ZincFramework.DialogueSystem.Analysis
{
    public abstract class DialogueAnalyzer<T> : DialogueAnalyzer where T : DialogueSyntax
    {
        public override DialogueSyntax Analyze(string text) => AnalyzeTyped(text);

        public abstract T AnalyzeTyped(string text);
    }
}