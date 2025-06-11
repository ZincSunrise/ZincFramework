using System;

namespace ZincFramework
{
    namespace DataPools
    {
        public interface IDataPool<T> : IDisposable
        {
            int MaxCount { get; }

            T RentValue();

            void ReturnValue(T value);
        }
    }
}
