using System;
using System.Runtime.ExceptionServices;
using System.Threading;



namespace ZincFramework.Threading.Tasks
{
    public struct ZincCompletionSourceCore<T>
    {
        private object _state;

        private Action<object> _continuation;

        private int _completedSignal;

        private object _exception;

        private T _result;

        private bool _hasUnreportedException;

        public T GetResult()
        {
            if(_completedSignal == 0)
            {
                _exception = new InvalidOperationException("现在还是未完成的状态！");
            }

            if(_exception != null)
            {
                if (_exception is OperationCanceledException canceledException)
                {
                    ZincTaskExeptionPublisher.PublishUnobservedException(canceledException);
                }
                else if(_exception is ExceptionDispatchInfo dispatchInfo)
                {
                    ZincTaskExeptionPublisher.PublishUnobservedException(dispatchInfo.SourceException);
                }
            }

            return _result;
        }

        public ZincTaskStatus GetStatus()
        {
            if (_continuation == null || _completedSignal == 0)
            {
                return ZincTaskStatus.Executing;
            }
            else if (_exception == null)
            {
                return ZincTaskStatus.Succeeded;
            }
            else if (_exception is OperationCanceledException)
            {
                return ZincTaskStatus.Canceled;
            }
            else
            {
                return ZincTaskStatus.Failed;
            }
        }

        public void OnComplete(Action<object> continuation, object state)
        {
            object oldContinuation = _continuation;

            if (oldContinuation == null)
            {
                _state = state;
                oldContinuation = Interlocked.CompareExchange(ref _continuation, continuation, null);
            }

            if (oldContinuation != null)
            {
                if (!ReferenceEquals(oldContinuation, SharedSentinel.Sentinel))
                {
                    throw new InvalidOperationException("不可以await两次同一个Task");
                }

                continuation.Invoke(state);
            }
        }

        /// <summary>
        /// 任何完成时都调用这个函数来设置完成状态，例如加载完毕
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TrySetResult(T result)
        {
            if (Interlocked.Increment(ref _completedSignal) == 1)
            {
                _result = result;

                var cont = _continuation;
                if (cont != null || Interlocked.CompareExchange(ref _continuation, SharedSentinel.Sentinel, null) != null)
                {
                    cont?.Invoke(_state);
                }

                return true;
            }

            return false;
        }

        public bool TrySetCancel(CancellationToken cancellationToken = default)
        {
            if (Interlocked.Increment(ref _completedSignal) == 1)
            {
                _exception = new OperationCanceledException(cancellationToken);
                _hasUnreportedException = true;

                if (_continuation != null || Interlocked.CompareExchange(ref _continuation, SharedSentinel.Sentinel, null) != null)
                {
                    _continuation.Invoke(_state);
                }

                return true;
            }

            return false;
        }

        public bool TrySetException(Exception exception)
        {
            if (Interlocked.Increment(ref _completedSignal) == 1)
            {
                if(exception is OperationCanceledException)
                {
                    _exception = exception;
                }
                else
                {
                    _exception = ExceptionDispatchInfo.Capture(exception);
                }

                _hasUnreportedException = true;

                if (_continuation != null || Interlocked.CompareExchange(ref _continuation, SharedSentinel.Sentinel, null) != null)
                {
                    _continuation.Invoke(_state);
                }

                return true;
            }

            return false;
        }

        private void ThrowException()
        {
            if(_exception is OperationCanceledException canceledException)
            {
                throw canceledException;
            }
            else if(_exception is ExceptionDispatchInfo exceptionDispatchInfo)
            {
                exceptionDispatchInfo.Throw();
            }
        }

        public void Reset()
        {
            _continuation = null;
            _state = null;
            _completedSignal = 0;
            _result = default;

            if (_hasUnreportedException)
            {
                _hasUnreportedException = false;
                ThrowException();
            }

            _exception = null;
        }
    }

    internal static class SharedSentinel
    {
        public static Action<object> Sentinel { get; } = SentinelFunc;

        private static void SentinelFunc(object state)
        {
            throw new ArgumentException("此函数永远不可能调用");
        }
    }
}
