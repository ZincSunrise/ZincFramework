using System.Threading;
using System;
using ZincFramework.Loop;
using ZincFramework.Pools;
using ZincFramework.Runtime.CompilerServices;
using UnityEngine;

namespace ZincFramework.Threading.Tasks
{
    public class NextFramePromise : CorePromise, ILoopItem
    {
        private CancellationToken _cancellationToken;

        private E_LoopType _loopType;

        private int _frameCount = 0;

        private readonly static DataPool<NextFramePromise> _promisePool = new DataPool<NextFramePromise>(() => new NextFramePromise());

        private NextFramePromise()
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
            _frameCount = Time.frameCount;
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
            _frameCount = 0;
            base.OnReturn();
        }

        public bool Tick()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _sourceCore.TrySetCancel(_cancellationToken);
                return false;
            }

            if(_frameCount == Time.frameCount)
            {
                _sourceCore.TrySetResult(null);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
