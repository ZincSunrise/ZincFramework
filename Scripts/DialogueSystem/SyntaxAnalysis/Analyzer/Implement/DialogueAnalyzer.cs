using UnityEngine;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis
{
    public abstract class DialogueAnalyzer : IAnalyzer<DialogueSyntax>
    {
        public IAssetRepository<Object> AssetRepository => DialogueRepository.GetInstance();

        public abstract DialogueSyntax Analyze(string text);
    }
}