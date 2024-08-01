using System.Collections;
using ZincFramework.Binary;
using System.Reflection;
using ZincFramework.Serialization.Runtime;
using System;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct EnumerableConverter : ISerializeConvert
            {                
                private static readonly Dictionary<Type, Func<int, IEnumerable>> _listConstructorMap = new Dictionary<Type, Func<int, IEnumerable>>();

                public static Func<int, IEnumerable> GetCapacityConstructorMap(Type type)
                {
                    if (!_listConstructorMap.TryGetValue(type, out var result))
                    {
                        result = ExpressionTool.GetConstructor<int, IEnumerable>(type, BindingFlags.Instance | BindingFlags.Public);
                        _listConstructorMap.Add(type, result);
                    }
                    return result;
                }
                
                
                public object Convert(byte[] bytes, ref int nowIndex, Type type)
                {
                    if(type == typeof(string))
                    {
                        return ByteConverter.ToString(bytes, ref nowIndex);
                    }
                    
                    return SelectConverter(type).Convert(bytes, ref nowIndex, type);
                }

                public object Convert(ref ReadOnlySpan<byte> bytes, Type type)
                {                   
                    if(type == typeof(string))
                    {
                        return ByteConverter.ToString(ref bytes);
                    }
                    
                    return SelectConverter(type).Convert(ref bytes, type);
                }

                private ISerializeConvert SelectConverter(Type type)
                {
                    if (type.IsArray || typeof(IList).IsAssignableFrom(type))
                    {
                        return new ArrayListConverter();
                    }
                    else if (typeof(IDictionary).IsAssignableFrom(type))
                    {
                        return new DictionaryConverter();
                    }
                    else if (typeof(ICollection).IsAssignableFrom(type))
                    {
                        string name = type.Name;
                        if (name.Contains("Queue"))
                        {
                            return new QueueConverter();
                        }
                        else if (name.Contains("Stack"))
                        {
                            return new StackConverter();
                        }
                        else if (name.Contains("LinkedList"))
                        {
                            return new LinkedListConverter();
                        }
                    }
                    return new HashSetConverter();
                }
            }
        }
    }
}