using System.Runtime.CompilerServices;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    [AsyncMethodBuilder(typeof(ZincAsyncVoidMethodBuilder))]
    public readonly partial struct ZincTask
    {
        public ITaskSource TaskSource => _taskSource;


        private readonly ITaskSource _taskSource;

        public ZincTask(ITaskSource taskSource)
        {
            _taskSource = taskSource;
        }

        public ZincAwaiter GetAwaiter() => new ZincAwaiter(this);

        /// <summary>
        /// 不可以使用async void，必须用这种方法来代替
        /// </summary>
        public void Forget() => GetAwaiter().OnForget();

        #region 等值方法重写
        public bool Equals(ZincTask other)
        {
            return other._taskSource == _taskSource;
        }

        public override bool Equals(object obj)
        {
            return obj is ZincTask task && Equals(task);
        }

        public static bool operator ==(ZincTask left, ZincTask right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ZincTask left, ZincTask right)
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
