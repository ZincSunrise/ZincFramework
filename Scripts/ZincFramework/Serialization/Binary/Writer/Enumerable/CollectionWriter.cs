
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZincFramework.Binary;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct CollectionWriter : ICollectionWrite<IEnumerable>
            {
                public void Write(IEnumerable enumerable, Stream stream, Type genericType)
                {
                    if (enumerable is ICollection collection)
                    {
                        ByteWriter.WriteInt16((short)collection.Count, stream);
                    }
                    else
                    {
                        short count = 0;
                        foreach (var item in enumerable)
                        {
                            count++;
                        }
                        
                        ByteWriter.WriteInt16(count, stream);
                    }
                    
                    WriteValueImpl(enumerable, stream, genericType);
                }

                public async Task WriteAsync(IEnumerable enumerable, Stream stream, Type genericType)
                {
                    if (enumerable is ICollection collection)
                    {
                        ByteWriter.WriteInt16((short)collection.Count, stream);
                    }
                    else
                    {
                        short count = 0;
                        foreach (var item in enumerable)
                        {
                            count++;
                        }
                        
                        ByteWriter.WriteInt16(count, stream);
                    }
                    
                    await WriteValueImplAsync(enumerable, stream, genericType);
                }
                
                private void WriteValueImpl(IEnumerable tempCollection, Stream stream, Type genericType)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempCollection)
                        {
                            case IEnumerable<int> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt32(item, stream);
                                }
                                break;
                            case IEnumerable<float> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteFloat(item, stream);
                                }
                                break;
                            case IEnumerable<bool> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteBoolean(item, stream);
                                }
                                break;
                            case IEnumerable<long> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt64(item, stream);
                                }
                                break;
                            case IEnumerable<double> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteDouble(item, stream);
                                }
                                break;
                            case IEnumerable<short> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt16(item, stream);
                                }
                                break;
                            case IEnumerable<ushort> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt16(item, stream);
                                }
                                break;
                            case IEnumerable<uint> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt32(item, stream);
                                }
                                break;
                            case IEnumerable<ulong> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt64(item, stream);
                                }
                                break;
                            case IEnumerable<byte> collection:
                                foreach (var item in collection)
                                {
                                    stream.WriteByte(item);
                                }
                                break;
                            case IEnumerable<sbyte> collection:
                                foreach (var item in collection)
                                {
                                    stream.WriteByte((byte)item);
                                }
                                break;
                        }
                    }
                    else if (tempCollection is IEnumerable<string> collection)
                    {
                        foreach (var item in collection)
                        {
                            ByteWriter.WriteString(item, stream);
                        }
                    }
                    else if (tempCollection is IEnumerable<TimeSpan> timeList)
                    {
                        foreach (var item in timeList)
                        {
                            ByteWriter.WriteInt64(item.Ticks, stream);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        foreach (var item in tempCollection)
                        {
                            ByteWriter.WriteInt32((int)item, stream);
                        }
                    }
                    else
                    {
                        ISerializeWrite writer = WriterFactory.Shared.CreateBuilder(genericType);
                        foreach (var item in tempCollection)
                        { 
                            writer.Write(item, stream, genericType);
                        }
                    }
                }


                private async Task WriteValueImplAsync(IEnumerable tempCollection, Stream stream, Type genericType)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempCollection)
                        {
                            case IEnumerable<int> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt32(item, stream);
                                }
                                break;
                            case IEnumerable<float> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteFloat(item, stream);
                                }
                                break;
                            case IEnumerable<bool> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteBoolean(item, stream);
                                }
                                break;
                            case IEnumerable<long> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt64(item, stream);
                                }
                                break;
                            case IEnumerable<double> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteDouble(item, stream);
                                }
                                break;
                            case IEnumerable<short> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteInt16(item, stream);
                                }
                                break;
                            case IEnumerable<ushort> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt16(item, stream);
                                }
                                break;
                            case IEnumerable<uint> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt32(item, stream);
                                }
                                break;
                            case IEnumerable<ulong> collection:
                                foreach (var item in collection)
                                {
                                    ByteWriter.WriteUInt64(item, stream);
                                }
                                break;
                            case IEnumerable<byte> collection:
                                foreach (var item in collection)
                                {
                                    stream.WriteByte(item);
                                }
                                break;
                            case IEnumerable<sbyte> collection:
                                foreach (var item in collection)
                                {
                                    stream.WriteByte((byte)item);
                                }
                                break;
                        }
                    }
                    else if (tempCollection is IEnumerable<string> collection)
                    {
                        foreach (var item in collection)
                        {
                            ByteWriter.WriteString(item, stream);
                        }
                    }
                    else if (tempCollection is IEnumerable<TimeSpan> timeList)
                    {
                        foreach (var item in timeList)
                        {
                            ByteWriter.WriteInt64(item.Ticks, stream);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        foreach (var item in tempCollection)
                        {
                            ByteWriter.WriteInt32((int)item, stream);
                        }
                    }
                    else
                    {
                        ISerializeWrite writer = WriterFactory.Shared.CreateBuilder(genericType);
                        foreach (var item in tempCollection)
                        {
                            await writer.WriteAsync(item, stream, genericType);
                        }
                    }
                }
                
                
                void ISerializeWrite.Write(object obj, Stream stream, Type type)
                {
                    throw new NotImplementedException();
                }

                Task ISerializeWrite.WriteAsync(object obj, Stream stream, Type type)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

