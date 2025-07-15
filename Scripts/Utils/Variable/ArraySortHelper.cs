using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZincFramework
{
    /// <summary>
    /// 别问，问就是抄的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ArraySortHelper<T>
    {
        public const int IntrosortSizeThreshold = 16;

        public static ArraySortHelper<T> Default { get; } = new ArraySortHelper<T>();

        public void Sort(Span<T> keys, IComparer<T> comparer)
        {
            // Add a try block here to detect IComparers (or their
            // underlying IComparables, etc) that are bogus.
            comparer ??= Comparer<T>.Default;
            IntrospectiveSort(keys, comparer.Compare);
        }

        internal static void IntrospectiveSort(Span<T> keys, Comparison<T> comparer)
        {
            Debug.Assert(comparer != null);

            if (keys.Length > 1)
            {
                IntroSort(keys, 2 * (Log2((uint)keys.Length) + 1), comparer);
            }

            int Log2(uint n)
            {
                if (n <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than zero.");
                }

                int result = 0;

                // Step 1: 先处理大于等于 65536 的数字
                if (n >= 1 << 16)  // 65536
                {
                    n >>= 16;
                    result += 16;
                }

                // Step 2: 处理大于等于 256 的数字
                if (n >= 1 << 8)  // 256
                {
                    n >>= 8;
                    result += 8;
                }

                // Step 3: 处理大于等于 16 的数字
                if (n >= 1 << 4)  // 16
                {
                    n >>= 4;
                    result += 4;
                }

                // Step 4: 处理大于等于 4 的数字
                if (n >= 1 << 2)  // 4
                {
                    n >>= 2;
                    result += 2;
                }

                // Step 5: 处理大于等于 2 的数字
                if (n >= 1)  // 2
                {
                    result += 1;
                }

                return result;
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void IntroSort(Span<T> keys, int depthLimit, Comparison<T> comparer)
        {
            Debug.Assert(!keys.IsEmpty);
            Debug.Assert(depthLimit >= 0);
            Debug.Assert(comparer != null);

            int partitionSize = keys.Length;
            while (partitionSize > 1)
            {
                if (partitionSize <= IntrosortSizeThreshold)
                {

                    if (partitionSize == 2)
                    {
                        SwapIfGreater(keys, comparer, 0, 1);
                        return;
                    }

                    //快速排序
                    if (partitionSize == 3)
                    {
                        SwapIfGreater(keys, comparer, 0, 1);
                        SwapIfGreater(keys, comparer, 0, 2);
                        SwapIfGreater(keys, comparer, 1, 2);
                        return;
                    }

                    //插入排序
                    InsertionSort(keys.Slice(0, partitionSize), comparer);
                    return;
                }

                if (depthLimit == 0)
                {
                    HeapSort(keys.Slice(0, partitionSize), comparer);
                    return;
                }
                depthLimit--;

                int p = PickPivotAndPartition(keys.Slice(0, partitionSize), comparer);

                // Note we've already partitioned around the pivot and do not have to move the pivot again.
                IntroSort(keys[(p + 1)..partitionSize], depthLimit, comparer);
                partitionSize = p;
            }
        }

        /// <summary>
        /// 快排节点选择
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        private static int PickPivotAndPartition(Span<T> keys, Comparison<T> comparer)
        {
            Debug.Assert(keys.Length >= IntrosortSizeThreshold);
            Debug.Assert(comparer != null);

            int hi = keys.Length - 1;

            // Compute median-of-three.  But also partition them, since we've done the comparison.
            int middle = hi >> 1;

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            SwapIfGreater(keys, comparer, 0, middle);  // swap the low with the mid point
            SwapIfGreater(keys, comparer, 0, hi);   // swap the low with the high
            SwapIfGreater(keys, comparer, middle, hi); // swap the middle with the high

            T pivot = keys[middle];
            Swap(keys, middle, hi - 1);
            int left = 0, right = hi - 1;  // We already partitioned lo and hi and put the pivot in hi - 1.  And we pre-increment & decrement below.

            while (left < right)
            {
                while (comparer(keys[++left], pivot) < 0) ;
                while (comparer(pivot, keys[--right]) < 0) ;

                if (left >= right)
                    break;

                Swap(keys, left, right);
            }

            // Put pivot in the right location.
            if (left != hi - 1)
            {
                Swap(keys, left, hi - 1);
            }
            return left;
        }

        /// <summary>
        /// 快排交换算法
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="comparer"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void SwapIfGreater(Span<T> keys, Comparison<T> comparer, int i, int j)
        {
            Debug.Assert(i != j);

            if (comparer(keys[i], keys[j]) > 0)
            {
                T key = keys[i];
                keys[i] = keys[j];
                keys[j] = key;
            }
        }


        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="comparer"></param>
        private static void InsertionSort(Span<T> keys, Comparison<T> comparer)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                T t = keys[i + 1];

                int j = i;
                while (j >= 0 && comparer(t, keys[j]) < 0)
                {
                    keys[j + 1] = keys[j];
                    j--;
                }

                keys[j + 1] = t;
            }
        }


        /// <summary>
        /// 堆排序
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="comparer"></param>
        private static void HeapSort(Span<T> keys, Comparison<T> comparer)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(!keys.IsEmpty);

            int n = keys.Length;
            for (int i = n >> 1; i >= 1; i--)
            {
                DownHeap(keys, i, n, comparer);
            }

            for (int i = n; i > 1; i--)
            {
                Swap(keys, 0, i - 1);
                DownHeap(keys, 1, i - 1, comparer);
            }
        }

        private static void DownHeap(Span<T> keys, int i, int n, Comparison<T> comparer)
        {
            Debug.Assert(comparer != null);

            T d = keys[i - 1];
            while (i <= n >> 1)
            {
                int child = 2 * i;
                if (child < n && comparer(keys[child - 1], keys[child]) < 0)
                {
                    child++;
                }

                if (!(comparer(d, keys[child - 1]) < 0))
                    break;

                keys[i - 1] = keys[child - 1];
                i = child;
            }

            keys[i - 1] = d;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(Span<T> a, int i, int j)
        {
            Debug.Assert(i != j);

            T t = a[i];
            a[i] = a[j];
            a[j] = t;
        }
    }
}
