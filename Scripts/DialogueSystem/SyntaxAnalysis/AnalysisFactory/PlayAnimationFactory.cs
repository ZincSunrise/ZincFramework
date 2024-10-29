using ZincFramework.Analysis;
using ZincFramework.DialogueSystem.Analysis.SyntaxParse;


namespace ZincFramework.DialogueSystem.Analysis.Factory
{
    public class PlayAnimationFactory : SingletonAnalysisFactory
    {
        public override bool IsTarget(string text) => text.Contains("PlayAnimation", System.StringComparison.OrdinalIgnoreCase);


        public override IAnalyzer CreateAnalyzer() => _analyzer ??= new PlayAnimationAnalyzer();

        public override ISyntaxParser CreateParser() => _syntaxParser ??= new PlayAnimationParser();
    }
}
