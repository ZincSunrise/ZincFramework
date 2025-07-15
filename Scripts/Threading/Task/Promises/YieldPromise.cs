using System;
using System.Threading;
using ZincFramework.Pools;
using ZincFramework.Loop;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public class YieldPromise : CorePromise, ILoopItem
    {
        private CancellationToken _cancellationToken;

        private E_LoopType _loopType;

        private readonly static DataPool<YieldPromise> _promisePool = new DataPool<YieldPromise>(() => new YieldPromise());

        private YieldPromise()
        {

        }

        public static ITaskSource Create(E_LoopType loopType, CancellationToken cancellationToken)
        {
            var promise = _promisePool.RentValue();

            if (cancellationToken.IsCancellationRequested)
            {
                promise.SetCanceled();
                return promise;
            }

            promise.Initialize(loopType, cancellationToken);
            ZincTaskLoopHelper.AddLoopItem(promise._loopType, promise);
            return promise;
        }

        public void Initialize(E_LoopType loopType, CancellationToken cancellationToken)
        {
            _loopType = loopType;
            _cancellationToken = cancellationToken;
        }

        public override void GetResult()
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
        }

        public override void OnReturn()
        {
            _cancellationToken = default;
            base.OnReturn();
        }

        public bool Tick()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _sourceCore.TrySetCancel(_cancellationToken);
                return false;
            }

            _sourceCore.TrySetResult(null);
            return false;
        }
    }
}
