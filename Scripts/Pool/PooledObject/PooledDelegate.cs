using System;
using System.Runtime.CompilerServices;


namespace ZincFramework.Pools
{
    /// <summary>
    /// 用于当你想把一个Action装进Action<T>里面但是又不想使用lambda表达式捕获闭包时使用
    /// </summary>
    /// <typeparam name="T">你期望的函数type</typeparam>
    public class PooledDelegate<T> : IReuseable
    {
        private readonly static DataPool<PooledDelegate<T>> _delegatePool = new DataPool<PooledDelegate<T>>(() => new PooledDelegate<T>());

        private readonly Action<T> _executeDelegate;

        private Action _action;

        private PooledDelegate()
        {
            _executeDelegate = Execute;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T> Create(Action continuation)
        {
            var deletgate = _delegatePool.RentValue();
            deletgate._action = continuation;
            return deletgate._executeDelegate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Execute(T _)
        {
            if (_action != null)
            {
                _action.Invoke();
                _delegatePool.ReturnValue(this);
            }
        }

        public void OnRent()
        {

        }

        public void OnReturn()
        {

        }
    }
}