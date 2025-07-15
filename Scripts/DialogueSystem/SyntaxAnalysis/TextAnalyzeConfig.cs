using System.Collections.Generic;
using ZincFramework.DialogueSystem.Analysis.Factory;
using ZincFramework.DialogueSystem.Analysis.Service;
using ZincFramework.DialogueSystem.Analysis.SyntaxParse;
using ZincFramework.LoadServices;



namespace ZincFramework.DialogueSystem.Analysis
{
    /// <summary>
    /// 默认使用AssetBundle加载
    /// </summary>
    public class TextAnalyzeConfig
    {
        public static TextAnalyzeConfig DefaultConfig { get; } = new TextAnalyzeConfig();

        private TextAnalysisFactory _lastFactory;

        public IList<TextAnalysisFactory> CustomFactories 
        {
            get => _customFactories ??= new List<TextAnalysisFactory>();    
            set => _customFactories = value;
        }

        private IList<TextAnalysisFactory> _customFactories;

        public TextAnalyzeConfig()
        {

        }

        public TextAnalyzeConfig(IAssetLoader assetLoader)
        {
            DialogueRepository.GetInstance().AssetLoader = assetLoader;
        }

        public DialogueAnalyzer GetDialogueAnalyzer(string text)
        {
            TextAnalysisFactory analysisFactory = GetAnalysisFactory(text);
            return analysisFactory.CreateAnalyzer() as DialogueAnalyzer;
        }

        public DialogueSyntax GetDialogueSyntax(string text)
        {
            TextAnalysisFactory analysisFactory = GetAnalysisFactory(text);
            return analysisFactory.CreateAnalyzer().Analyze(text) as DialogueSyntax;
        }

        public DialogueParser GetDialogueParser(string text)
        {
            TextAnalysisFactory analysisFactory = GetAnalysisFactory(text);
            return analysisFactory.CreateParser() as DialogueParser;
        }

        public TextAnalysisFactory GetAnalysisFactory(string text)
        {
            if (_lastFactory?.IsTarget(text) == true)
            {
                return _lastFactory;
            }

            TextAnalysisFactory analysisFactory = DialogueAnalysisService.Instance.GetAnalysisFactory(text);

            if (analysisFactory == null)
            {
                for (int i = 0; i < CustomFactories.Count; i++)
                {
                    if (CustomFactories[i].IsTarget(text))
                    {
                        analysisFactory = CustomFactories[i];
                        break;
                    }
                }
            }

            _lastFactory = analysisFactory as TextAnalysisFactory;
            return analysisFactory ?? throw ThrowHelper.ThorwNonKeyException(text);
        }

        private static class ThrowHelper
        {
            public static System.ArgumentException ThorwNonKeyException(string key) => throw new System.ArgumentException("没有这样的键" + key);
        }
    }
}