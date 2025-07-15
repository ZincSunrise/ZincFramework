using System;
using System.Threading;
using ZincFramework.Pools;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    internal class DelayPromise : CorePromise, IDisposable
    {
        private CancellationToken _cancellationToken;

        private CancellationTokenRegistration _cancellationTokenRegistration;

        private Timer _timer;

        private readonly static DataPool<DelayPromise> _promisePool = new DataPool<DelayPromise>(() => new DelayPromise());

        private DelayPromise()
        {
            _timer = new Timer(OnTimeComplete, null, Timeout.Infinite, Timeout.Infinite);
        }

        public static ITaskSource Create(int millisecondsDelay, CancellationToken cancellationToken)
        {
            DelayPromise delaySource = _promisePool.RentValue();

            if (cancellationToken.IsCancellationRequested)
            {
                delaySource.SetCanceled();
                return delaySource;
            }

            delaySource.Initialize(millisecondsDelay, cancellationToken);
            return delaySource;
        }


        private void Initialize(int millisecondsDelay, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _timer.Change(millisecondsDelay, Timeout.Infinite);

            if (_cancellationToken.CanBeCanceled)
            {
                _cancellationTokenRegistration = _cancellationToken.Register(SetCanceled);
            }
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

        /// <summary>
        /// 计时器到期后的回调，请注意这个state必然为空
        /// </summary>
        /// <param name="state"></param>
        public void OnTimeComplete(object state)
        {
            _sourceCore.TrySetResult(null);
        }


        public override void OnReturn()
        {
            base.OnReturn();
            _cancellationTokenRegistration.Dispose();
            _cancellationTokenRegistration = default;
            _sourceCore.Reset();
        }

        public void Dispose()
        {
            _timer.Dispose();   
        }
    }
}
