using System;
using ZincFramework.Binary.Serialization;
using ZincFramework.Serialization;


namespace ZincFramework
{
    namespace Binary
    {
        public static class VariableUtility
        {
            #region 深浅拷贝相关
            public static T DeepCopy<T>(T data)
            {
                byte[] bytes = BinarySerializer.Serialize<T>(data);
                return BinarySerializer.Deserialize<T>(bytes);
            }


            public static object DeepCopy(object data, Type type)
            {
                byte[] bytes = BinarySerializer.Serialize(data, type);
                return BinarySerializer.Deserialize(bytes, type);
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
        }
    }
}

