using System;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace DataPool
    {
        public class ObjectPool<TValue> : IDataPool<TValue>
        {
            protected Queue<TValue> _unuseValues;

            protected Func<TValue> _createFunction;

            public int CacheCount => _unuseValues.Count;

            public int MaxCount { get ; set; }

            public ObjectPool()
            {
                _unuseValues = new Queue<TValue>(16);
            }

            public ObjectPool(Func<TValue> createAction)
            {
                _createFunction = createAction;
                _unuseValues = new Queue<TValue>(16);
            }

            public ObjectPool(Func<TValue> createAction, int maxCount)
            {
                MaxCount = maxCount;
                _createFunction = createAction;
                _unuseValues = new Queue<TValue>(MaxCount);
            }


            public virtual void Clear()
            {
                _unuseValues.Clear();
            }

            public virtual void Dispose()
            {
                Clear();
                _unuseValues = null;
                _createFunction = null;
            }

            public virtual TValue RentValue()
            {
                if (_unuseValues.Count > 0)
                {
                    return _unuseValues.Dequeue();
                }

                return _createFunction.Invoke();
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

