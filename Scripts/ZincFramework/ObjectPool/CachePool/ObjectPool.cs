using System;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace DataPool
    {
        public class ObjectPool<TValue> : BasePool
        {
            protected Queue<TValue> _cacheValues;

            protected Func<TValue> _createFunction;

            public int CacheCount => _cacheValues.Count;

            public ObjectPool(Func<TValue> createAction)
            {
                _createFunction = createAction;
                _cacheValues = new Queue<TValue>(16);
            }

            public ObjectPool(Func<TValue> createAction, int maxCount)
            {
                MaxCount = maxCount;
                _createFunction = createAction;
                _cacheValues = new Queue<TValue>(MaxCount);
            }


            public virtual void Clear()
            {
                _cacheValues.Clear();
            }

            public override void Dispose()
            {
                Clear();
                _cacheValues = null;
                _createFunction = null;
            }

            public virtual TValue RentValue()
            {
                if (_cacheValues.Count > 0)
                {
                    return _cacheValues.Dequeue();
                }

                return _createFunction.Invoke();
            }

            public virtual void ReturnValue(TValue value)
            {
                if (MaxCount != -1 && _cacheValues.Count < MaxCount)
                {
                    _cacheValues.Enqueue(value);
                }
            }
        }
    }
}

