using System;
using System.Collections.Generic;

namespace ZincFramework.Loop.Internal
{
    /// <summary>
    /// 线程不安全，触发无序，类型多种，元素需要手动删除的循环数组。
    /// 使用数组+空闲索引队列管理循环元素，支持注册、注销和遍历。
    /// 注销会留下空槽位，达到一定阈值时触发数组紧凑操作，减少碎片。
    /// </summary>
    internal class LoopArray
    {
        public ILoopItem this[int index] => _loopItems[index];

        // 存储所有元素的数组，初始容量为8
        private ILoopItem[] _loopItems = new ILoopItem[8];

        // 空闲槽位索引队列，存放注销后可复用的位置
        private readonly Queue<int> _freeIndices = new Queue<int>();

        // 当前有效元素的尾部索引，指向下一个可用插入位置
        private int _arrayTail;

        // 触发紧凑操作的空闲槽位数量阈值，初始为2
        // 当空闲槽位数量超过此阈值时，会进行数组紧凑（Compact）
        private int _compactThreshold = 2;

        /// <summary>
        /// 注册一个新元素。
        /// 优先复用空闲槽位，否则追加到数组尾部。
        /// 数组容量不足时会进行扩容，并动态调整紧凑阈值为当前容量的25%。
        /// </summary>
        public void Register(ILoopItem loopItem)
        {
            // 有空闲槽位，优先复用
            if (_freeIndices.Count > 0)
            {
                int freeIndex = _freeIndices.Dequeue();
                _loopItems[freeIndex] = loopItem;
                return;
            }

            // 数组满时扩容为当前容量的两倍
            if (_arrayTail >= _loopItems.Length)
            {
                Array.Resize(ref _loopItems, _loopItems.Length * 2);

                // 扩容后，调整紧凑阈值为数组容量的25%
                _compactThreshold = (int)(_loopItems.Length * 0.25f);
            }

            // 在尾部插入新元素，并推进尾部索引
            _loopItems[_arrayTail++] = loopItem;
        }

        /// <summary>
        /// 注销元素，将对应槽位设为空，并将该槽位索引加入空闲队列。
        /// 当空闲槽位数量超过阈值时，触发数组紧凑，整理碎片。
        /// </summary>
        public void Unregister(ILoopItem loopItem)
        {
            // 查找元素索引
            int index = Array.IndexOf(_loopItems, loopItem);

            if (index >= 0)
            {
                // 清空槽位，标记为空
                _loopItems[index] = null;

                // 记录空闲槽位索引以供复用
                _freeIndices.Enqueue(index);
            }

            // 超过阈值触发紧凑操作，清理碎片
            if (_freeIndices.Count > _compactThreshold)
            {
                Compact();
                _freeIndices.Clear();
            }
        }

        /// <summary>
        /// 遍历所有有效元素，调用其 Tick 方法。
        /// 只遍历到有效尾部索引，跳过 null 元素。
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < _arrayTail; i++)
            {
                _loopItems[i]?.Tick();
            }
        }

        /// <summary>
        /// 清除元素
        /// </summary>
        public void Clear()
        {
            _freeIndices.Clear();
            Array.Clear(_loopItems, 0, _arrayTail);
            _arrayTail = 0;
        }

        /// <summary>
        /// 数组压缩算法（紧凑操作）。
        /// 使用双指针法：从前向后扫描遇到空槽，后面指针从尾部向前寻找非空元素交换。
        /// 交换完成后，调整尾部索引 _arrayTail 以缩短遍历范围，减少空闲槽位。
        /// </summary>
        private void Compact()
        {
            int j = _arrayTail - 1;

            for (int i = 0; i < _arrayTail; i++)
            {
                // 如果当前元素为空，则需要从后面寻找非空元素交换
                if (_loopItems[i] == null)
                {
                    // 后指针向前移动，跳过空槽
                    while (i < j && _loopItems[j] == null)
                    {
                        j--;
                    }

                    // 如果前后指针相遇或交错，说明后面全部为空
                    if (i >= j)
                    {
                        // 更新尾部索引为第一个空槽位置，结束压缩
                        _arrayTail = i;
                        break;
                    }

                    // 交换前指针空槽与后指针非空元素，腾出前方连续非空空间
                    (_loopItems[i], _loopItems[j]) = (_loopItems[j], _loopItems[i]);
                    j--;
                }
            }
        }
    }


    /// <summary>
    /// 线程不安全，触发无序，类型单一，元素需要手动删除的循环数组。
    /// 使用数组+空闲索引队列管理循环元素，支持注册、注销和遍历。
    /// 注销会留下空槽位，达到一定阈值时触发数组紧凑操作，减少碎片。
    /// </summary>
    internal class LoopArray<TLoopItem> where TLoopItem : ILoopItem
    {
        public TLoopItem this[int index] => _loopItems[index];

        // 存储所有元素的数组，初始容量为8
        private TLoopItem[] _loopItems = new TLoopItem[8];

        // 空闲槽位索引队列，存放注销后可复用的位置
        private readonly Queue<int> _freeIndices = new Queue<int>();

        // 当前有效元素的尾部索引，指向下一个可用插入位置
        private int _arrayTail;

        // 触发紧凑操作的空闲槽位数量阈值，初始为2
        // 当空闲槽位数量超过此阈值时，会进行数组紧凑（Compact）
        private int _compactThreshold = 2;

        /// <summary>
        /// 注册一个新元素。
        /// 优先复用空闲槽位，否则追加到数组尾部。
        /// 数组容量不足时会进行扩容，并动态调整紧凑阈值为当前容量的25%。
        /// </summary>
        public void Register(TLoopItem loopItem)
        {
            // 有空闲槽位，优先复用
            if (_freeIndices.Count > 0)
            {
                int freeIndex = _freeIndices.Dequeue();
                _loopItems[freeIndex] = loopItem;
                return;
            }

            // 数组满时扩容为当前容量的两倍
            if (_arrayTail >= _loopItems.Length)
            {
                Array.Resize(ref _loopItems, _loopItems.Length * 2);

                // 扩容后，调整紧凑阈值为数组容量的25%
                _compactThreshold = (int)(_loopItems.Length * 0.25f);
            }

            // 在尾部插入新元素，并推进尾部索引
            _loopItems[_arrayTail++] = loopItem;
        }

        /// <summary>
        /// 注销元素，将对应槽位设为空，并将该槽位索引加入空闲队列。
        /// 当空闲槽位数量超过阈值时，触发数组紧凑，整理碎片。
        /// </summary>
        public void Unregister(TLoopItem loopItem)
        {
            // 查找元素索引
            int index = Array.IndexOf(_loopItems, loopItem);

            if (index >= 0)
            {
                // 清空槽位，标记为空
                _loopItems[index] = default;

                // 记录空闲槽位索引以供复用
                _freeIndices.Enqueue(index);
            }

            // 超过阈值触发紧凑操作，清理碎片
            if (_freeIndices.Count > _compactThreshold)
            {
                Compact();
                _freeIndices.Clear();
            }
        }

        /// <summary>
        /// 遍历所有有效元素，调用其 Tick 方法。
        /// 只遍历到有效尾部索引，跳过 null 元素。
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < _arrayTail; i++)
            {
                _loopItems[i]?.Tick();
            }
        }

        /// <summary>
        /// 清除元素
        /// </summary>
        public void Clear()
        {
            _freeIndices.Clear();
            Array.Clear(_loopItems, 0, _arrayTail);
            _arrayTail = 0;
        }

        /// <summary>
        /// 数组压缩算法（紧凑操作）。
        /// 使用双指针法：从前向后扫描遇到空槽，后面指针从尾部向前寻找非空元素交换。
        /// 交换完成后，调整尾部索引 _arrayTail 以缩短遍历范围，减少空闲槽位。
        /// </summary>
        private void Compact()
        {
            int j = _arrayTail - 1;

            for (int i = 0; i < _arrayTail; i++)
            {
                // 如果当前元素为空，则需要从后面寻找非空元素交换
                if (_loopItems[i] == null)
                {
                    // 后指针向前移动，跳过空槽
                    while (i < j && _loopItems[j] == null)
                    {
                        j--;
                    }

                    // 如果前后指针相遇或交错，说明后面全部为空
                    if (i >= j)
                    {
                        // 更新尾部索引为第一个空槽位置，结束压缩
                        _arrayTail = i;
                        break;
                    }

                    // 交换前指针空槽与后指针非空元素，腾出前方连续非空空间
                    (_loopItems[i], _loopItems[j]) = (_loopItems[j], _loopItems[i]);
                    j--;
                }
            }
        }
    }
}