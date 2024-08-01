using System.Collections;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            public class CachePool<TKey, TValue>
            {
                private readonly Dictionary<TKey, TValue> _dataMap = new Dictionary<TKey, TValue>();

                public CachePool()
                {

                }
            }
        }
    }
}
