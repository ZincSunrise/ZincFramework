using System;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace DataPools
    {
        public class ObjectPool<TValue> : IDataPool<TValue>
        {
            protected Queue<TValue> _unuseValues;

            protected Func<TValue> _factory;

            public int CacheCount => _unuseValues.Count;

            public int MaxCount { get ; set; }

            public ObjectPool()
            {
                _unuseValues = new Queue<TValue>(16);
            }

            public ObjectPool(Func<TValue> createAction)
            {
                _factory = createAction;
                _unuseValues = new Queue<TValue>(16);
            }

            public ObjectPool(Func<TValue> createAction, int maxCount)
            {
                MaxCount = maxCount;
                _factory = createAction;
                _unuseValues = new Queue<TValue>(maxCount == -1 ? 30 : maxCount);
            }


            public virtual void Clear()
            {
                _unuseValues.Clear();
            }

            public virtual void Dispose()
            {
                Clear();
                GC.SuppressFinalize(this);
                _unuseValues = null;
                _factory = null;
            }

            public virtual TValue RentValue()
            {
                if (_unuseValues.Count > 0)
                {
                    return _unuseValues.Dequeue();
                }

                return _factory.Invoke();
            }

            public virtual void ReturnValue(TValue value)
            {
                if (MaxCount != -1 && _unuseValues.Count < MaxCount)
                {
                    _unuseValues.Enqueue(value);
                }
            }
        }
    }
}

