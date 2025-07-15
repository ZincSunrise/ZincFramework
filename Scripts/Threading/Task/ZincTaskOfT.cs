using System;
using System.Runtime.CompilerServices;
using ZincFramework.Runtime.CompilerServices;


namespace ZincFramework.Threading.Tasks
{
    public enum ZincTaskStatus
    {
        Executing = 0,
        Succeeded = 1,
        Failed = 2,
        Canceled = 3,
    }


    [AsyncMethodBuilder(typeof(ZincAsyncMethodBuilder<>))]
    public readonly struct ZincTask<T> : IEquatable<ZincTask<T>>
    {
        public ITaskSource<T> TaskSource => _taskSource;

        private readonly ITaskSource<T> _taskSource;

        public ZincTask(ITaskSource<T> taskSource) 
        {
            _taskSource = taskSource;
        }

        public ZincAwaiter<T> GetAwaiter() => new ZincAwaiter<T>(this);
        #region 重写等价逻辑
        public bool Equals(ZincTask<T> other)
        {
            return other._taskSource == _taskSource;
        }

        public override bool Equals(object obj)
        {
            return obj is ZincTask<T> task && Equals(task);
        }

        public static bool operator ==(ZincTask<T> left, ZincTask<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ZincTask<T> left, ZincTask<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return _taskSource.GetHashCode();
        }
        #endregion
    }
}
