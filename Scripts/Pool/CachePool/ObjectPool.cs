using System;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace DataPools
    {
        public class ObjectPool<TValue> : IDataPool<TValue>
        {
            protected Queue<TValue> _unuseValues;

            protected Func<TValue> _factory;

            public int CacheCount => _unuseValues.Count;

            public virtual int MaxCount { get; set; }

#if UNITY_EDITOR
            private readonly bool _isCheckRepeat = true;

            public void CheckRepeat(TValue value)
            {
                foreach(var data in _unuseValues)
                {
                    if(data.Equals(value))
                    {
                        Debug.LogWarning("出现了两个一样的对象");
                    }
                }
            }
#endif
            public ObjectPool()
            {
                _unuseValues = new Queue<TValue>(16);
                MaxCount = -1;
            }

            public ObjectPool(Func<TValue> createAction)
            {
                _factory = createAction;
                _unuseValues = new Queue<TValue>(16);
                MaxCount = -1;
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
#if UNITY_EDITOR
                if (_isCheckRepeat)
                {
                    CheckRepeat(value);
                }
#endif
                if (MaxCount == -1 || _unuseValues.Count <= MaxCount)
                {
                    _unuseValues.Enqueue(value);
                }
            }
        }
    }
}

