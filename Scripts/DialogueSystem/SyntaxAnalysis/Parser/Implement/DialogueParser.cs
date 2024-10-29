using UnityEngine;
using ZincFramework.Analysis;

namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public abstract class DialogueParser : ISyntaxParser
    {
        public IAssetRepository<Object> AssetRepository => DialogueRepository.GetInstance();

        public virtual IParseResult ParseSyntax(object analysisTarget, ISyntax syntax) => ParseSyntax((GameObject)analysisTarget, (DialogueSyntax)syntax);

        public abstract IParseResult ParseSyntax(GameObject analysisTarget, DialogueSyntax textSyntax);
    }
}