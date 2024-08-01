using System;
using System.Collections;
using System.Collections.Generic;
using ZincFramework.Serialization.Binary;
using ZincFramework.Serialization.Cache;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {
            public abstract class BaseBuilderFactory<T> : BuilderFactory<T> where T : IBuilder
            {
                protected readonly Dictionary<int, Func<T>> _bulidermap = new Dictionary<int, Func<T>>();

                public override T CreateBuilder(Type type) => type switch
                {
                    not null when type.IsPrimitive => _bulidermap[(int)E_Builder_Type.PrimitiveValue].Invoke(),
                    not null when IBuilderFactory.EnumerableType.IsAssignableFrom(type) => _bulidermap[(int)E_Builder_Type.Enumerable].Invoke(),
                    not null when type.IsEnum || type.Assembly == SerializationCachePool.SystemAssembly => _bulidermap[(int)E_Builder_Type.OtherSystemValue].Invoke(),
                    _ => _bulidermap[(int)E_Builder_Type.OtherValue].Invoke(),
                };


                public override T CreateBuilder(object obj) => obj switch
                {
                    IEnumerable => _bulidermap[(int)E_Builder_Type.Enumerable].Invoke(),
                    Enum or TimeSpan => _bulidermap[(int)E_Builder_Type.OtherSystemValue].Invoke(),
                    int or float or bool or long or double or short or ushort or uint or ulong or byte or sbyte => _bulidermap[(int)E_Builder_Type.PrimitiveValue].Invoke(),
                    _ => _bulidermap[(int)E_Builder_Type.OtherValue].Invoke(),
                };


                public override void AddBuilder(int index, Func<T> func)
                {
                    _bulidermap.TryAdd(index, func);
                }


                internal override T CreateBuilder(E_Builder_Type type)
                {
                    return _bulidermap[(int)type].Invoke();
                }
            }
        }
    }
}