using System;
using ZincFramework.Pools;
using ZincFramework.Runtime.CompilerServices;


namespace ZincFramework.Threading.Tasks
{
    public abstract class CorePromise : ITaskSource, IReuseable
    {
        public ZincTask ZincTask => new ZincTask(this);

        protected bool _isDisposed;

        protected ZincCompletionSourceCore<object> _sourceCore;

        public ZincTaskStatus GetStatus() => _sourceCore.GetStatus();

        public virtual void OnComplete(Action<object> continuation, object state)
        {
            _sourceCore.OnComplete(continuation, state);
        }

        public abstract void GetResult();

        public void SetException(Exception exception)
        {
            _sourceCore.TrySetException(exception);
        }

        public void SetCanceled()
        {
            _sourceCore.TrySetCancel();
        }

        public virtual void OnReturn()
        {
            _sourceCore.Reset();
        }

        public virtual void OnRent()
        {
            _isDisposed = false;
        }

/*        public override void GetResult()
        {
            try
            {
                _sourceCore.GetResult();
            }
            finally
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    _promisePool.ReturnValue(this);
                }
                else
                {
                    throw new ObjectDisposedException("不可以重复获取同一个对象内的资源");
                }
            }
        }*/
    }
}
