using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    /// <summary>
    /// һ����˵����ȡ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WaitableEffectParser<T> : DialogueParser<T>, IAsyncEffectParser, ICancelable where T : IParseResult
    {
        public virtual void Cancel() { }

        public abstract Task ExceteAsync(GameObject target, DialogueSyntax textSyntax, CancellationToken cancellationToken);
    }
}