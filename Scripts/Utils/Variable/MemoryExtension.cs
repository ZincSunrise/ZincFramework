using System;
using System.Collections.Generic;

namespace ZincFramework
{
    public static class MemoryExtension
    {
        public static void Sort<T>(this Span<T> values, Comparison<T> comparer)
        {
            ArraySortHelper<T>.IntrospectiveSort(values, comparer);
        }

        public static void Sort<T>(this Span<T> values, IComparer<T> comparer)
        {

        }
    }
}