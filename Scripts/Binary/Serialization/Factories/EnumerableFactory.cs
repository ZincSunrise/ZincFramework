using System;
using System.Collections;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Converters;
using ZincFramework.Binary.Serialization.MetaModule;



namespace ZincFramework.Binary.Serialization.Factory
{
    public class EnumerableFactory : ConvertibleFactory
    {
        internal override BinaryConverter GetConverter(Type type, SerializerOption serializerOption)
        {
            Type genericType;
            Type elementType;
            
            if (type.IsArray)
            {
                if(type.GetArrayRank() > 1)
                {
                    throw new NotSupportedException("不支持二维及以上的数组");
                }

                elementType = type.GetElementType();

                if (DefaultMetaModule.IsBlittable(elementType))
                {
                    genericType = typeof(BlittableArrayConverter<>).MakeGenericType(elementType);
                }
                else
                {
                    genericType = typeof(ArrayConverter<>).MakeGenericType(elementType);
                }
            }
            else if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = type.GetGenericArguments()[0];

                if (DefaultMetaModule.IsBlittable(elementType))
                {
                    genericType = typeof(BlittableListConverter<>).MakeGenericType(elementType);
                }
                else
                {
                    genericType = typeof(ListConverter<>).MakeGenericType(elementType);
                }
            }
            else if (type.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(HashConverter<>).MakeGenericType(elementType);
            }
            else if (type.GetGenericTypeDefinition() == typeof(Queue<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(QueueConverter<>).MakeGenericType(elementType);
            }
            else if (type.GetGenericTypeDefinition() == typeof(Stack<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(StackConverter<>).MakeGenericType(elementType);
            }
            else if (type.GetGenericTypeDefinition() == typeof(LinkedList<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(LinkedListConverter<>).MakeGenericType(elementType);
            }
            else if(typeof(IList).IsAssignableFrom(type))
            {
                if (type.IsGenericType)
                {
                    elementType = type.GetGenericArguments()[0];
                    genericType = typeof(IListConverterOfT<,>).MakeGenericType(type, elementType);
                }
                else
                {
                    genericType = typeof(IListConverter<>).MakeGenericType(type);
                }
            }
            else if (type.TryGetDefination(typeof(ISet<>)))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(ISetConverter<,>).MakeGenericType(type, elementType);
            }
            else if (type.GetGenericTypeDefinition() == typeof(ConcurrentQueueConverter<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(ConcurrentQueueConverter<>).MakeGenericType(elementType);
            }
            else if (type.GetGenericTypeDefinition() == typeof(ConcurrentStackConverter<>))
            {
                elementType = type.GetGenericArguments()[0];
                genericType = typeof(ConcurrentStackConverter<>).MakeGenericType(elementType);
            }
            else
            {
                throw new NotSupportedException($"暂不支持该类{type.Name}");
            }

            
            return Activator.CreateInstance(genericType) as BinaryConverter;
        }

        internal override ConvertStrategy GetStrategy() => ConvertStrategy.Enumerable;

        internal override bool IsSerializable(Type type) => typeof(IEnumerable).IsAssignableFrom(type) && !typeof(IDictionary).IsAssignableFrom(type);
    }
}
