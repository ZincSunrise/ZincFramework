using ZincFramework.Analysis;
using ZincFramework.DialogueSystem.Analysis.Factory;
using ZincFramework.DialogueSystem.Analysis.SyntaxParse;


namespace ZincFramework.DialogueSystem.Analysis.Service
{
    internal partial class DialogueAnalysisService : BaseSafeSingleton<DialogueAnalysisService>, IAnalysisService<DialogueAnalyzer, DialogueParser, TextAnalysisFactory>
    {
        public TextAnalysisFactory GetAnalysisFactory(string key)
        {
            for (int i = 0; i < _defaultFactories.Count; i++)
            {
                if (_defaultFactories[i].IsTarget(key))
                {
                    return _defaultFactories[i];
                }
            }

            return null;
        }

        public DialogueAnalyzer GetAnalyzer(string key) => GetAnalysisFactory(key).CreateAnalyzer() as DialogueAnalyzer; 

        public DialogueParser GetSyntaxParser(string key) => GetAnalysisFactory(key).CreateParser() as DialogueParser;
    }
}