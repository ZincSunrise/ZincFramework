using System;

namespace ZincFramework
{
    namespace DataPool
    {
        public abstract class BasePool : IDisposable
        {
            public int MaxCount { get; set; } = -1;

            public abstract void Dispose();
        }
    }
}
