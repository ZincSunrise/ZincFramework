using System;

namespace ZincFramework
{
    namespace Pools
    {
        public interface IDataPool<T> : IDisposable
        {
            int MaxCount { get; }

            T RentValue();

            void ReturnValue(T value);
        }
    }
}
