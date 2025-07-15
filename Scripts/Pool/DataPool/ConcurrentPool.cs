using System;
using System.Collections.Concurrent;


namespace ZincFramework.Pools.Concurrent
{
    public class ConcurrentPool<TValue> : IDataPool<TValue> where TValue : IReuseable
    {
        private Func<TValue> _factory;

        private ConcurrentQueue<TValue> _dataQueue;

        public int MaxCount { get; private set; }


        public ConcurrentPool()
        {
            _dataQueue = new ConcurrentQueue<TValue>();
            MaxCount = -1;
        }

        public ConcurrentPool(Func<TValue> createAction) : this()
        {
            _factory = createAction;
        }

        public ConcurrentPool(Func<TValue> createAction, int maxCount)
        {
            MaxCount = maxCount;
            _factory = createAction;
            _dataQueue = new ConcurrentQueue<TValue>();
        }


        public virtual void Clear()
        {
            _dataQueue.Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
            _dataQueue = null;
            _factory = null;
        }

        public virtual TValue RentValue()
        {
            if (_dataQueue.Count > 0 && _dataQueue.TryDequeue(out var result))
            {
                result.OnRent();
                return result;
            }

            return _factory.Invoke();
        }

        public virtual void ReturnValue(TValue value)
        {
            if (MaxCount != -1 && _dataQueue.Count < MaxCount)
            {
                value.OnReturn();
                _dataQueue.Enqueue(value);
            }
        }
    }
}
