using ZincFramework.DialogueSystem.Analysis.Factory;
using ZincFramework.DialogueSystem.Analysis.SyntaxParse;



namespace ZincFramework.DialogueSystem.Analysis
{
    public static class TextAnalysisSystem
    {
        public static DialogueAnalyzer GetDialogueAnalyzer(string text, TextAnalyzeConfig syntaxConfig = null) 
        {
            return (syntaxConfig ?? TextAnalyzeConfig.DefaultConfig).GetDialogueAnalyzer(text);
        }

        public static DialogueSyntax GetSyntax(string text, TextAnalyzeConfig syntaxConfig = null)
        {
            return (syntaxConfig ?? TextAnalyzeConfig.DefaultConfig).GetDialogueSyntax(text);
        }

        public static DialogueParser GetSyntaxParser(string text, TextAnalyzeConfig syntaxConfig = null)
        {
            return (syntaxConfig ?? TextAnalyzeConfig.DefaultConfig).GetDialogueParser(text);
        }

        public static TextAnalysisFactory GetFactory(string text, TextAnalyzeConfig syntaxConfig = null)
        {
            return (syntaxConfig ?? TextAnalyzeConfig.DefaultConfig).GetAnalysisFactory(text);
        }
    }
}