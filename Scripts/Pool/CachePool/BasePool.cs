using System;

namespace ZincFramework
{
    namespace DataPools
    {
        public interface IDataPool<in T> : IDisposable
        {
            int MaxCount { get; set; }
        }
    }
}
