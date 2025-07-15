using System;
using System.Collections.Generic;

namespace ZincFramework.Loop.Internal
{
    /// <summary>
    /// 线程不安全的，触发无序的，元素可自动可手动删除，元素类型单一的循环数组。
    /// 每次 Tick 时会遍历所有元素，并移除生命周期已结束的 LoopItem（由 Tick 返回 false 判断）。
    /// 
    /// - 无顺序要求；
    /// - 支持快速注册与删除；
    /// - 遍历时自动压缩空槽位，避免内存碎片；
    /// - 适用于频繁注册、短生命周期、无序处理的场景，例如音效、粒子、临时动画等。
    /// </summary>
    internal class LoopAutoArray<TLoopItem> where TLoopItem : ILoopItem
    {
        public TLoopItem this[int index] => _loopItems[index];

        /// <summary>
        /// 当前数组的尾部索引（即下一个可插入元素的位置）。
        /// Tick 过程中会被更新以缩短有效范围。
        /// </summary>
        private int _arrayTail;

        /// <summary>
        /// 存储所有注册的 LoopItem 实例。
        /// 空槽位（null）将在遍历时被压缩。
        /// </summary>
        private TLoopItem[] _loopItems = new TLoopItem[8];

        private readonly EqualityComparer<TLoopItem> _comparer = EqualityComparer<TLoopItem>.Default;
        /// <summary>
        /// 注册一个新的 LoopItem 到数组尾部。
        /// 若数组已满，则自动扩容至原容量两倍。
        /// </summary>
        /// <param name="loopItem">待注册的循环元素</param>
        public void Register(TLoopItem loopItem)
        {
            if (_arrayTail >= _loopItems.Length)
            {
                Array.Resize(ref _loopItems, _loopItems.Length * 2);
            }

            _loopItems[_arrayTail++] = loopItem;
        }

        /// <summary>
        /// 手动移除指定的 LoopItem。
        /// 将对应位置置为 null，等待下一次 Tick 时自动压缩。
        /// </summary>
        /// <param name="loopItem">要移除的元素</param>
        public void Unregister(TLoopItem loopItem)
        {
            int index = Array.IndexOf(_loopItems, loopItem);
            if (index != -1)
            {
                _loopItems[index] = default;
            }
        }

        /// <summary>
        /// Tick 所有有效元素。
        /// - 若元素的 Tick 返回 false，则移除；
        /// - 若遇到空槽位，则从数组尾部搬移有效元素压缩空洞；
        /// - 若尾部无更多有效元素，则提前结束遍历；
        /// </summary>
        public void Tick()
        {
            var j = _arrayTail - 1;

            for (int i = 0; i < _loopItems.Length; i++)
            {
                var loopItem = _loopItems[i];

                // 当前槽位非空，执行 Tick 判断其生命周期
                if (!_comparer.Equals(loopItem, default))
                {
                    try
                    {
                        // Tick 返回 false 表示应移除
                        if (!loopItem.Tick())
                        {
                            _loopItems[i] = default;
                        }
                        else
                        {
                            continue; // Tick 成功，跳过到下一个 i
                        }
                    }
                    catch (Exception ex)
                    {
                        // 出现异常也立即移除
                        _loopItems[i] = default;
                        UnityEngine.Debug.LogException(ex);
                    }
                }

                // 当前 i 为空，尝试从尾部 j 移动一个有效元素填充空位
                while (i < j)
                {
                    var fromTail = _loopItems[j];

                    if (!_comparer.Equals(fromTail, default))
                    {
                        try
                        {
                            if (!fromTail.Tick())
                            {
                                // 尾部元素不再有效，移除并继续往前找
                                _loopItems[j] = default;
                                j--;
                                continue;
                            }
                            else
                            {
                                // 将尾部有效元素移动到当前位置 i
                                _loopItems[i] = fromTail;
                                _loopItems[j] = default;
                                j--;

                                // i 位置已填充，继续外层循环
                                goto NEXT_LOOP;
                            }
                        }
                        catch (Exception ex)
                        {
                            _loopItems[j] = default;
                            j--;
                            UnityEngine.Debug.LogException(ex);
                            continue;
                        }
                    }
                    else
                    {
                        j--; // 尾部为空，继续向前找
                    }
                }

                // 尾部已无有效元素，更新尾部索引为当前 i，结束遍历
                _arrayTail = i;
                break;

            NEXT_LOOP:
                continue;
            }
        }

        public void Clear()
        {
            Array.Clear(_loopItems, 0, _arrayTail);
            _arrayTail = 0;
        }

        public ReadOnlySpan<TLoopItem> AsSpan()
        {
            return new ReadOnlySpan<TLoopItem>(_loopItems, 0, _arrayTail);
        }
    }

    /// <summary>
    /// 线程不安全的，触发无序的，有结构体类型约束的，元素可自动可手动删除，元素类型单一的循环数组。
    /// 每次 Tick 时会遍历所有元素，并移除生命周期已结束的 LoopItem（由 Tick 返回 false 判断）。
    
    /// - 使用结构体，没有内存开销
    /// - 无顺序要求；
    /// - 支持快速注册与删除；
    /// - 遍历时自动压缩空槽位，避免内存碎片；
    /// - 适用于频繁注册、短生命周期、无序处理的场景，例如音效、粒子、临时动画等。
    /// </summary>
    internal class LoopValueAutoArray<TLoopItem> where TLoopItem : struct, ILoopItem
    {
        public TLoopItem this[int index] => _loopItems[index];

        /// <summary>
        /// 当前数组的尾部索引（即下一个可插入元素的位置）。
        /// Tick 过程中会被更新以缩短有效范围。
        /// </summary>
        private int _arrayTail;

        /// <summary>
        /// 存储所有注册的 LoopItem 实例。
        /// 空槽位（null）将在遍历时被压缩。
        /// </summary>
        private TLoopItem[] _loopItems = new TLoopItem[8];

        private readonly EqualityComparer<TLoopItem> _comparer = EqualityComparer<TLoopItem>.Default;
        /// <summary>
        /// 注册一个新的 LoopItem 到数组尾部。
        /// 若数组已满，则自动扩容至原容量两倍。
        /// </summary>
        /// <param name="loopItem">待注册的循环元素</param>
        public void Register(TLoopItem loopItem)
        {
            if (_arrayTail >= _loopItems.Length)
            {
                Array.Resize(ref _loopItems, _loopItems.Length * 2);
            }

            _loopItems[_arrayTail++] = loopItem;
        }

        /// <summary>
        /// 手动移除指定的 LoopItem。
        /// 将对应位置置为 null，等待下一次 Tick 时自动压缩。
        /// </summary>
        /// <param name="loopItem">要移除的元素</param>
        public void Unregister(TLoopItem loopItem)
        {
            int index = Array.IndexOf(_loopItems, loopItem);
            if (index != -1)
            {
                _loopItems[index] = default;
            }
        }

        /// <summary>
        /// Tick 所有有效元素。
        /// - 若元素的 Tick 返回 false，则移除；
        /// - 若遇到空槽位，则从数组尾部搬移有效元素压缩空洞；
        /// - 若尾部无更多有效元素，则提前结束遍历；
        /// </summary>
        public void Tick()
        {
            var j = _arrayTail - 1;

            for (int i = 0; i < _loopItems.Length; i++)
            {
                ref var loopItem = ref _loopItems[i];

                // 当前槽位非空，执行 Tick 判断其生命周期
                if (!_comparer.Equals(loopItem, default))
                {
                    try
                    {
                        // Tick 返回 false 表示应移除
                        if (!loopItem.Tick())
                        {
                            loopItem = default;
                        }
                        else
                        {
                            continue; // Tick 成功，跳过到下一个 i
                        }
                    }
                    catch (Exception ex)
                    {
                        // 出现异常也立即移除
                        loopItem = default;
                        UnityEngine.Debug.LogException(ex);
                    }
                }

                // 当前 i 为空，尝试从尾部 j 移动一个有效元素填充空位
                while (i < j)
                {
                    ref var fromTail = ref _loopItems[j];

                    if (!_comparer.Equals(fromTail, default))
                    {
                        try
                        {
                            if (!fromTail.Tick())
                            {
                                // 尾部元素不再有效，移除并继续往前找
                                fromTail = default;
                                j--;
                                continue;
                            }
                            else
                            {
                                // 将尾部有效元素移动到当前位置 i
                                loopItem = fromTail;
                                fromTail = default;
                                j--;

                                // i 位置已填充，继续外层循环
                                goto NEXT_LOOP;
                            }
                        }
                        catch (Exception ex)
                        {
                            fromTail = default;
                            j--;
                            UnityEngine.Debug.LogException(ex);
                            continue;
                        }
                    }
                    else
                    {
                        j--; // 尾部为空，继续向前找
                    }
                }

                // 尾部已无有效元素，更新尾部索引为当前 i，结束遍历
                _arrayTail = i;
                break;

            NEXT_LOOP:
                continue;
            }
        }

        public void Clear()
        {
            Array.Clear(_loopItems, 0, _arrayTail);
            _arrayTail = 0;
        }

        public ReadOnlySpan<TLoopItem> AsSpan()
        {
            return new ReadOnlySpan<TLoopItem>(_loopItems, 0, _arrayTail);
        }
    }
}
