using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZincFramework.DataPools;


namespace ZincFramework
{
    public static class ArrayListUtility
    {
        private readonly static HashSet<int> _indicesIndex = new HashSet<int>();

        public static bool IsNullOrEmpty(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            IEnumerator enumerator = enumerable.GetEnumerator();
            return enumerator == null || !enumerator.MoveNext();
        }

        public static bool ElementEqual<T>(T[] array1, T[] array2) where T : struct, IEquatable<T>
        {
            if (array1?.Length != array2?.Length)
            {

                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                ref T a = ref array1[i];
                ref T b = ref array2[i];

                if (!a.Equals(b))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ElementEqual<T>(List<T> array1, List<T> array2) where T : struct, IEquatable<T>
        {
            if(array1 == null)
            {
                return array2 == null;
            }

            if(array1.Count != array2.Count)
            {
                return false;
            }

            for (int i = 0; i < array1.Count; i++)
            {
                if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        #region 对象池数组相关
        public static void Fill<T>(T[] array, DataPool<T> dataPool) where T : class, IReuseable
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] ??= dataPool.RentValue();
            }
        }

        public static void Fill<T>(T[] array, Func<T> factory) where T : class, IReuseable
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] ??= DataPoolManager.RentInfo(factory);
            }
        }


        /// <summary>
        /// 遵循原则，从哪里来，到哪里去
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="dataPool"></param>
        public static void Clear<T>(T[] array, DataPool<T> dataPool) where T : class, IReuseable
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    dataPool.ReturnValue(array[i]);
                    array[i] = null;
                }
            }
        }

        /// <summary>
        /// 遵循原则，从哪里来，到哪里去
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void Clear<T>(T[] array) where T : class, IReuseable
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    DataPoolManager.ReturnInfo(array[i]);
                    array[i] = null;
                }
            }
        }

        public static void Clear<T>(IList<T> list) where T : class, IReuseable
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                DataPoolManager.ReturnInfo(list[i]);
                list.RemoveAt(i);
            }
        }

        #endregion

        #region 数组顺序相关

        /// <summary>
        /// 随机填充一维数组某一部分的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void RamdomPadding<T>(T[] array, Func<T> factory, int count)
        {
            if (count > array.Length)
            {
                throw new ArgumentException("插入的数量不能大于数组长度");
            }

            while (_indicesIndex.Count < count)
            {
                _indicesIndex.Add(UnityEngine.Random.Range(0, array.Length));
            }

            foreach(int index in _indicesIndex)
            {
                array[index] = factory.Invoke();
            }

            _indicesIndex.Clear();
        }

        public static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1); 
                Swap(ref array[k], ref array[n]);
            }
        }

        public static void Shuffle<T>(Span<T> span)
        {
            int n = span.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                Swap(ref span[k], ref span[n]);
            }
        }

        /// <summary>
        /// 用于将一维数组表示为二维数组时的越界分析
        /// </summary>
        /// <returns></returns>
        public static bool IsOutRange(int index, int colCount, int rowCount)
        {
            int col = index / colCount;
            return col >= rowCount || index < 0;
        }


        public static bool Compare<T>(T[] array1, T[] array2) where T : struct
        {
            if (array1?.Length != array2?.Length)
            {
                return false;
            }

            if (array1 == null && array2 == null || Array.ReferenceEquals(array1, array2))
            {
                return true;

            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static TTo[] Cast<TForm, TTo>(TForm[] forms) where TForm : struct where TTo : struct
        {
            return MemoryMarshal.Cast<TForm, TTo>(forms).ToArray();
        }

        #endregion

        #region 对象交换相关
        public static void Swap<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);
        }

        public static void Swap(ref int a, ref int b)
        {
            a ^= b;
            b = a ^ b;
            a ^= b;
        }

        public static void Swap(ref long a, ref long b)
        {
            a ^= b;
            b = a ^ b;
            a ^= b;
        }
        #endregion
    }
}

