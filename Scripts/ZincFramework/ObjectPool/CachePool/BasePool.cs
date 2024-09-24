using System;

namespace ZincFramework
{
    namespace DataPool
    {
        public interface IDataPool<in T> : IDisposable
        {
            int MaxCount { get; set; }
        }
    }
}
