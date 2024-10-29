using System.Collections.Generic;
using ZincFramework.DialogueSystem.Analysis.Factory;

namespace ZincFramework.DialogueSystem.Analysis.Service
{
    internal partial class DialogueAnalysisService
    {
        private static List<TextAnalysisFactory> _defaultFactories;

        private static void InitialFactory()
        {
            _defaultFactories ??= new List<TextAnalysisFactory>() 
            {

            };
        }
    }
}
