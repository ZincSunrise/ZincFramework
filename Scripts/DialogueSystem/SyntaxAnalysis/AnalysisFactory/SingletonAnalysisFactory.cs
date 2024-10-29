using System;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.Factory
{
    public abstract class SingletonAnalysisFactory : TextAnalysisFactory, IDisposable
    {
        protected IAnalyzer _analyzer;

        protected ISyntaxParser _syntaxParser;

        public void Dispose()
        {
            
        }
    }
}