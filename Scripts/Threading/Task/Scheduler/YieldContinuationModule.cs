using System;
using System.Threading;
using ZincFramework.Loop;

namespace ZincFramework.Threading.Tasks.Internal
{
    internal class YieldContinuationModule : ILoopModule
    {
        private readonly static int _intializeCount = 8;

        #region 内部类
        /// <summary>
        /// 用于包裹延续任务的循环物品
        /// </summary>
        internal readonly struct ContinuationItem : ILoopItem, IEquatable<ContinuationItem>
        {
            public readonly Action Continuation { get; }

            public ContinuationItem(Action continuation) 
            {
                Continuation = continuation;
            }

            public bool Tick()
            {
                Continuation?.Invoke();
                return true;
            }

            public bool Equals(ContinuationItem other)
            {
                return Continuation == other.Continuation;
            }
        }

        #endregion
        public E_LoopType LoopType { get; }

        public Type FlagType { get; }


        private bool _isActive = false;

        private int _activeCount = 0;

        private ContinuationItem[] _activeItems = new ContinuationItem[_intializeCount];

        private int _waitingCount = 0;

        private ContinuationItem[] _waitingItems = new ContinuationItem[_intializeCount];

        /// <summary>
        /// 简单自旋锁，防止多线程并发
        /// </summary>
        private SpinLock _spinLock = new SpinLock(false);




        public YieldContinuationModule(E_LoopType loopType, Type flagType)
        {
            LoopType = loopType;
            FlagType = flagType;
        }

        public void Register(Action continuation)
        {
            Register(new ContinuationItem(continuation));
        }

        public void Register(ContinuationItem loopItem)
        {
            bool lockItem = false;
            try
            {
                _spinLock.Enter(ref lockItem);

                if (_isActive)
                {
                    if (_waitingCount >= _waitingItems.Length)
                    {
                        Array.Resize(ref _waitingItems, _waitingItems.Length * 2);
                    }

                    _waitingItems[_waitingCount++] = loopItem;
                }
                else
                {
                    if (_activeCount >= _activeItems.Length)
                    {
                        Array.Resize(ref _activeItems, _activeItems.Length * 2);
                    }

                    _activeItems[_activeCount++] = loopItem;
                }
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
            finally
            {
                if (lockItem)
                {
                    _spinLock.Exit(false);
                }
            }
        }

        public void Unregister(ContinuationItem loopItem)
        {
            try
            {
                if (_waitingCount == 0) 
                {
                    return;
                }

                for (int i = 0; i < _waitingItems.Length; i++)
                {
                    if (_waitingItems[i].Equals(loopItem))
                    {
                        _waitingItems[i] = default;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
        }

        public void Tick()
        {
            bool lockItem1 = false;
            try
            {
                _spinLock.Enter(ref lockItem1);
                if(_activeCount == 0)
                {
                    return;
                }

                _isActive = true;
            }
            finally
            {
                if (lockItem1)
                {
                    _spinLock.Exit(false);
                }
            }

            for (int i = 0; i < _activeCount; i++)
            {
                ref var item = ref _activeItems[i];
                item.Tick();
                item = default;
            }

            bool lockItem2 = false;
            try
            {
                _spinLock.Enter(ref lockItem2);
                _isActive = false;

                //重置ActiveCount
                _activeCount = 0;

                //交换双数组
                (_activeItems, _waitingItems) = (_waitingItems, _activeItems);
                (_activeCount, _waitingCount) = (_waitingCount, _activeCount);
            }
            finally
            {
                if (lockItem2)
                {
                    _spinLock.Exit(false);
                }
            }
        }

        void ILoopModule.Register(ILoopItem loopItem)
        {
            if (loopItem is not ContinuationItem continuationItem)
            {
                throw new ArgumentException("必须是一个ContinuationItem");
            }

            Register(continuationItem);
        }

        void ILoopModule.Unregister(ILoopItem loopItem)
        {
            if (loopItem is not ContinuationItem continuationItem)
            {
                throw new ArgumentException("必须是一个ContinuationItem");
            }

            Unregister(continuationItem);
        }

        public void Clear()
        {
            _activeItems = new ContinuationItem[_intializeCount];
            _waitingItems = new ContinuationItem[_intializeCount];

            _activeCount = _waitingCount = 0;
        }
    }
}