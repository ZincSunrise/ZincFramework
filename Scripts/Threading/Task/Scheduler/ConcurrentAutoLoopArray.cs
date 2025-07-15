using System;
using System.Collections.Generic;


namespace ZincFramework.Loop.Internal
{
    /// <summary>
    /// 线程安全的，自动移除的循环数组
    /// </summary>
    internal class ConcurrentAutoLoopArray
    {
        //队列和是否正在运行中的锁
        private readonly object _runningLock = new object();

        //数组锁
        private readonly object _arrayLock = new object();

        //是否正在运行中
        private bool _isRunning = false;

        private int _arrayTail;


        private ILoopItem[] _loopItems = new ILoopItem[8];


        private readonly Queue<ILoopItem> _waitingItems = new Queue<ILoopItem>();

        /// <summary>
        /// 注册一个LoopItem
        /// </summary>
        /// <typeparam name="TLoopItem"></typeparam>
        /// <param name="loopItem"></param>
        public void Register(ILoopItem loopItem)
        {
            lock (_runningLock) 
            {
                if (_isRunning)
                {
                    _waitingItems.Enqueue(loopItem);
                    return;
                }
            }

            lock (_arrayLock)
            {
                AddLoopItem(loopItem);
            }
        }

        public void Tick()
        {
            lock (_runningLock)
            {
                _isRunning = true;
            }

            lock (_arrayLock)
            {
                var j = _arrayTail - 1;

                for (int i = 0; i < _loopItems.Length; i++)
                {
                    var loopItem = _loopItems[i];

                    //如果LoopItem不为空
                    if (loopItem != null)
                    {
                        try
                        {
                            //如果不能留在循环，那么直接置为空
                            if (!loopItem.Tick())
                            {
                                _loopItems[i] = null;
                            }
                            else
                            {
                                continue; // next i 
                            }
                        }
                        catch (Exception ex)
                        {
                            _loopItems[i] = null;
                            UnityEngine.Debug.LogException(ex);
                        }
                    }


                    //如果LoopItem为空，开始搬运并且压缩空间
                    while (i < j)
                    {
                        var fromTail = _loopItems[j];

                        if (fromTail != null)
                        {
                            try
                            {
                                //如果不允许留在循环中，直接移除
                                if (!fromTail.Tick())
                                {
                                    _loopItems[j] = null;
                                    j--;
                                    continue; // next j
                                }
                                //如果允许留在循环中，将其和当前的i（空位置）交换顺序
                                else
                                {
                                    _loopItems[i] = fromTail;
                                    _loopItems[j] = null;
                                    j--;
                                    goto NEXT_LOOP; //直接前往下一个i
                                }
                            }
                            catch (Exception ex)
                            {
                                _loopItems[j] = null;
                                j--;

                                UnityEngine.Debug.LogException(ex);
                                continue;//前往下一个J
                            }
                        }
                        else
                        {
                            j--;
                        }
                    }

                    _arrayTail = i;
                    break; //循环结束

                NEXT_LOOP:
                    continue;
                }
            }

            lock (_runningLock)
            {
                _isRunning = false;

                while(_waitingItems.TryDequeue(out var loopItem))
                {
                    AddLoopItem(loopItem);
                }
            }
        }

        private void AddLoopItem(ILoopItem loopItem)
        {
            if (_arrayTail >= _loopItems.Length)
            {
                Array.Resize(ref _loopItems, _loopItems.Length * 2);
            }

            _loopItems[_arrayTail++] = loopItem;
        }

        public void Clear()
        {
            lock (_arrayLock)
            {
                Array.Clear(_loopItems, 0, _loopItems.Length);
                _arrayTail = 0;
            }

            lock (_runningLock) 
            {
                _waitingItems.Clear();
            }
        }
    }
}
