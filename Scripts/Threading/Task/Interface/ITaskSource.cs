using System;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.Runtime.CompilerServices
{
    public interface ITaskSource
    {
        void OnComplete(Action<object> continuation, object state);

        void GetResult();

        ZincTaskStatus GetStatus();
    }

    public interface ITaskSource<T> : ITaskSource
    {
        new T GetResult();
    }
}
