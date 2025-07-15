using System;
using System.Threading;
using UnityEngine;
using ZincFramework.Loop;
using ZincFramework.Pools;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public class WaitForSecondsPromise : CorePromise, ILoopItem
    {
        private float _waitTime;

        private E_LoopType _loopType;

        private bool _isRealTime;

        private CancellationToken _cancellationToken;

        private readonly static DataPool<WaitForSecondsPromise> _promisePool = new DataPool<WaitForSecondsPromise>(() => new WaitForSecondsPromise());

        private WaitForSecondsPromise() { }

        public static ITaskSource Create(float waitTime, E_LoopType loopType, bool isRealTime, CancellationToken cancellationToken)
        {
            var promise = _promisePool.RentValue();

            if (cancellationToken.IsCancellationRequested)
            {
                promise.SetCanceled();
                return promise;
            }


            promise._waitTime = waitTime;
            promise._loopType = loopType;
            promise._isRealTime = isRealTime;
            promise._cancellationToken = cancellationToken;

            ZincTaskLoopHelper.AddLoopItem(loopType, promise);
            return promise;
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

        public bool Tick()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _sourceCore.TrySetCancel();
                return false;
            }

            if (_loopType == E_LoopType.FixedUpdate)
            {
                _waitTime -= _isRealTime ? Time.fixedUnscaledTime : Time.fixedDeltaTime;
            }
            else
            {
                _waitTime -= _isRealTime ? Time.unscaledDeltaTime : Time.deltaTime;
            }

            if(_waitTime <= 0)
            {
                _sourceCore.TrySetResult(null);
                return false;
            }

            return true;
        }

        public override void OnReturn()
        {
            base.OnReturn();
            _isRealTime = false;
            _waitTime = 0;
            _cancellationToken = default;
            _loopType = E_LoopType.Update;
        }
    }
}