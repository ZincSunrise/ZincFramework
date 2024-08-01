using System;
using System.Linq.Expressions;
using System.Reflection;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Binary;


namespace ZincFramework
{
    namespace Binary
    {
        public static class VariableUtility
        {

            #region 深浅拷贝相关
            public static T DeepCopy<T>(ISerializable serializable) where T : ISerializable, new()
            {
                T data = new T();
                data.Deserialize(serializable.Serialize());
                return data;
            }

            public static T DeepCopy<T>(T data, SerializeConfig serializeConfig = SerializeConfig.Property)
            {
                byte[] bytes = BinarySerializer.Serialize(data, typeof(T), serializeConfig);
                return BinarySerializer.Deserialize<T>(bytes, serializeConfig);
            }


            public static object DeepCopy(object data, Type type, SerializeConfig serializeConfig = SerializeConfig.Property)
            {
                byte[] bytes = BinarySerializer.Serialize(data, type, serializeConfig);
                return BinarySerializer.Deserialize(bytes, type, serializeConfig);
            }

            public static T ShallowCopy<T>(T data) where T : class
            {
                return data;
            }

            public static T ShallowCopy<T>(ref T data) where T : struct
            {
                return data;
            }
            #endregion

            #region 对象计算相关
            public static bool CompareArray<T>(T[] array1, T[] array2) where T : struct
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
}

