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
            public readonly struct ArrayListWriter : ICollectionWrite<IList>
            {                
                public void Write(IList obj, Stream stream, Type genericType)
                {
                    ByteWriter.WriteInt16((short)obj.Count, stream);
                    WriteValueImpl(obj, stream, genericType);
                }

                
                public async Task WriteAsync(IList obj, Stream stream, Type genericType)
                {
                    ByteWriter.WriteInt16((short)obj.Count, stream);
                    await WriteValueImplAsync(obj, stream, genericType);
                }

                void ISerializeWrite.Write(object obj, Stream stream, Type type)
                {
                    throw new NotImplementedException();
                }

                Task ISerializeWrite.WriteAsync(object obj, Stream stream, Type type)
                {
                    throw new NotImplementedException();
                }

                private void WriteValueImpl(IList tempList, Stream stream, Type genericType)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempList)
                        {
                            case IList<int> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt32(list[i], stream);
                                }
                                break;
                            case IList<float> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteFloat(list[i], stream);
                                }
                                break;
                            case IList<bool> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteBoolean(list[i], stream);
                                }
                                break;
                            case IList<long> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt64(list[i], stream);
                                }
                                break;
                            case IList<double> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteDouble(list[i], stream);
                                }
                                break;
                            case IList<short> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt16(list[i], stream);
                                }
                                break;
                            case IList<ushort> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt16(list[i], stream);
                                }
                                break;
                            case IList<uint> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt32(list[i], stream);
                                }
                                break;
                            case IList<ulong> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt64(list[i], stream);
                                }
                                break;
                            case IList<byte> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    stream.WriteByte(list[i]);
                                }
                                break;
                            case IList<sbyte> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    stream.WriteByte((byte)list[i]);
                                }
                                break;
                        }
                    }
                    else if (tempList is IList<string> list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ByteWriter.WriteString(list[i], stream);
                        }
                    }
                    else if (tempList is IList<TimeSpan> timeList)
                    {
                        for (int i = 0; i < timeList.Count; i++)
                        {
                            ByteWriter.WriteInt64(timeList[i].Ticks, stream);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            ByteWriter.WriteInt32((int)tempList[i], stream);
                        }
                    }
                    else
                    {
                        ISerializeWrite writer = WriterFactory.Shared.CreateBuilder(genericType);

                        for (int i = 0; i < tempList.Count; i++)
                        {
                             writer.Write(tempList[i], stream, genericType);
                        }
                    }
                }

                private async Task WriteValueImplAsync(IList tempList, Stream stream, Type genericType)
                {
                    if (genericType.IsPrimitive)
                    {
                        switch (tempList)
                        {
                            case IList<int> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt32(list[i], stream);
                                }
                                break;
                            case IList<float> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteFloat(list[i], stream);
                                }
                                break;
                            case IList<bool> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteBoolean(list[i], stream);
                                }
                                break;
                            case IList<long> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt64(list[i], stream);
                                }
                                break;
                            case IList<double> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteDouble(list[i], stream);
                                }
                                break;
                            case IList<short> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteInt16(list[i], stream);
                                }
                                break;
                            case IList<ushort> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt16(list[i], stream);
                                }
                                break;
                            case IList<uint> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt32(list[i], stream);
                                }
                                break;
                            case IList<ulong> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    ByteWriter.WriteUInt64(list[i], stream);
                                }
                                break;
                            case IList<byte> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    stream.WriteByte(list[i]);
                                }
                                break;
                            case IList<sbyte> list:
                                for (int i = 0; i < list.Count; i++)
                                {
                                    stream.WriteByte((byte)list[i]);
                                }
                                break;
                        }
                    }
                    else if (tempList is IList<string> list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ByteWriter.WriteString(list[i], stream);
                        }
                    }
                    else if (tempList is IList<TimeSpan> timeList)
                    {
                        for (int i = 0; i < timeList.Count; i++)
                        {
                            ByteWriter.WriteInt64(timeList[i].Ticks, stream);
                        }
                    }
                    else if (genericType.IsEnum)
                    {
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            ByteWriter.WriteInt32((int)tempList[i], stream);
                        }
                    }
                    else
                    {
                        ISerializeWrite writer = WriterFactory.Shared.CreateBuilder(genericType);

                        for (int i = 0; i < tempList.Count; i++)
                        {
                            await writer.WriteAsync(tempList[i], stream, genericType);
                        }
                    }
                }    
            }
        }
    }
}
