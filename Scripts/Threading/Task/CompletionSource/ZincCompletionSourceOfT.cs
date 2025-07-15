using System;
using System.Threading;
using ZincFramework.Pools;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public class ZincCompletionSource<T> : ITaskSource<T>, IReuseable
    {
        public ZincTask<T> ZincTask => new ZincTask<T>(this);

        private Exception _exception;

        public ZincTaskStatus _status;

        private CancellationTokenRegistration _cancelRegistration;

        private object _state;

        private Action<object> _continuation;

        private T _result;

        private bool _isDisposed = false;

        private readonly object _lockObject = new object();

        private static DataPool<ZincCompletionSource<T>> _dataPool = new DataPool<ZincCompletionSource<T>>(() => new ZincCompletionSource<T>());

        public static ZincCompletionSource<T> Create()
        {
            var source = _dataPool.RentValue();
            return source;
        }

        private ZincCompletionSource() { }

        public void OnComplete(Action<object> continuation, object state)
        {
            lock (_lockObject)
            {
                if(_status == ZincTaskStatus.Executing)
                {
                    _continuation = continuation;
                    _state = state;
                }
                else
                {
                    continuation.Invoke(state);
                }
            }

        }

        public T GetResult()
        {
            lock (_lockObject) 
            {
                switch (_status)
                {
                    case ZincTaskStatus.Succeeded when _isDisposed:
                        throw new ObjectDisposedException("不可以重复获取同一个对象内的资源");
                    case ZincTaskStatus.Succeeded:
                        T result = _result;
                        _isDisposed = true;
                        _dataPool.ReturnValue(this);
                        return result;
                    case ZincTaskStatus.Failed:
                        ZincTaskExeptionPublisher.PublishUnobservedException(_exception);
                        return default;
                    case ZincTaskStatus.Canceled:
                        _isDisposed = true;
                        _dataPool.ReturnValue(this);
                        ZincTaskExeptionPublisher.PublishUnobservedException(new OperationCanceledException("任务已被取消", _cancelRegistration.Token));
                        return default;
                    default:
                        ZincTaskExeptionPublisher.PublishUnobservedException(new InvalidOperationException("不正确的状态"));
                        return default;
                }
            }
        }

        public ZincTaskStatus GetStatus() => _status;

        public void SetResult(T result)
        {
            lock (_lockObject)
            {
                _result = result;
                _status = ZincTaskStatus.Succeeded;
                _continuation?.Invoke(_state);
            }
        }

        public void SetException(Exception exception)
        {
            lock (_lockObject)
            {
                if (exception is OperationCanceledException)
                {
                    SetCanceled();
                    return;
                }

                if (_status == ZincTaskStatus.Executing)
                {
                    return;
                }

                _status = ZincTaskStatus.Failed;
                _exception = exception;
                _continuation?.Invoke(_state);
            }
        }


        public void SetCanceled()
        {
            lock (_lockObject)
            {
                _status = ZincTaskStatus.Canceled;
            }

            _continuation?.Invoke(_state);
        }

        public void OnRent()
        {
            _isDisposed = false;
        }

        public void OnReturn()
        {
            _exception = null;
            _result = default;
            _continuation = null;
            _cancelRegistration.Dispose();
            _cancelRegistration = default;
            _status = ZincTaskStatus.Executing;
            _state = null;
        }

        void ITaskSource.GetResult()
        {
            GetResult();
        }
    }
}
