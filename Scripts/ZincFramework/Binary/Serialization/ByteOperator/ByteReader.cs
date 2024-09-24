using System;
using System.Text;
using System.Runtime.InteropServices;


namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public ref struct ByteReader
            {
                public readonly bool IsReadEnd => !Bytes.IsEmpty && _position >= Bytes.Length || !Memory.IsEmpty && _position >= Memory.Length;

                public BinaryReaderOption BinaryReaderOption { get; set; }

                public ReadOnlySpan<byte> Bytes { get; private set; }

                public Memory<byte> Memory { get; private set; }

                private int _position;

                public ByteReader(Memory<byte> memory, in BinaryReaderOption binaryReaderOption)
                {
                    Memory = memory;
                    Bytes = memory.Span;
                    _position = 0;
                    BinaryReaderOption = binaryReaderOption;
                }

                public ByteReader(ReadOnlySpan<byte> bytes, in BinaryReaderOption binaryReaderOption)
                {
                    Bytes = bytes;
                    _position = 0;
                    BinaryReaderOption = binaryReaderOption;

                    Memory = default;
                }


                public byte ReadByte()
                {
                    return Bytes[_position++];
                }

                public ReadOnlySpan<byte> ReadBytes(int length)
                {
                    ReadOnlySpan<byte> bytes = Bytes.Slice(_position, length);
                    _position += length;
                    return bytes;
                }

                public sbyte ReadSByte()
                {
                    return (sbyte)Bytes[_position++];
                }

                public short ReadInt16() 
                {                 
                    short value = MemoryMarshal.Read<short>(Bytes[_position..]);
                    _position += 2;
                    return value;
                }

                public ushort ReadUInt16()
                {
                    ushort value = MemoryMarshal.Read<ushort>(Bytes[_position..]);
                    _position += 2;
                    return value;
                }

                public bool ReadBoolean()
                {
                    bool value = MemoryMarshal.Read<bool>(Bytes[_position..]);
                    _position += 1;
                    return value;
                }

                public char ReadChar()
                {
                    byte count = ReadByte();
                    char value;
                    if (BinaryReaderOption.Encoding == Encoding.Unicode)
                    {
                        value = MemoryMarshal.Read<char>(Bytes.Slice(_position, count));

                        _position += count;
                        return value;
                    }
                    else
                    {
                        Span<char> chars = stackalloc char[1];
                        BinaryReaderOption.Encoding.GetChars(Bytes.Slice(_position, count), chars);

                        _position += count;
                        return chars[0];
                    }
                }

                public int ReadVarInt32()
                {
                    int value = 0;
                    int shift = 0;

                    while (true)
                    {
                        byte currentByte = Bytes[_position++];
                        value |= (currentByte & 0x7F) << shift;

                        if ((currentByte & 0x80) == 0)
                        {
                            break;
                        }

                        shift += 7;
                    }

                    return value;
                }

                public int ReadInt32()
                {
                    int value = MemoryMarshal.Read<int>(Bytes[_position..]);
                    _position += 4;
                    return value;
                }

                public uint ReadUInt32()
                {
                    uint value = MemoryMarshal.Read<uint>(Bytes[_position..]);
                    _position += 4;
                    return value;
                }

                public float ReadSingle()
                {
                    float value = MemoryMarshal.Read<float>(Bytes[_position..]);
                    _position += 4;
                    return value;
                }

                public long ReadInt64()
                {
                    long value = MemoryMarshal.Read<long>(Bytes[_position..]);
                    _position += 8;
                    return value;
                }

                public ulong ReadUInt64()
                {
                    ulong value = MemoryMarshal.Read<ulong>(Bytes[_position..]);
                    _position += 8;
                    return value;
                }

                public double ReadDouble()
                {
                    double value = MemoryMarshal.Read<double>(Bytes[_position..]);
                    _position += 8;
                    return value;
                }

                public string ReadString()
                {
                    int length = ReadInt32();
                    ReadOnlySpan<byte> sliceSpan = Bytes.Slice(_position, length);
                    _position += length;

                    if (BinaryReaderOption.Encoding == Encoding.Unicode)
                    {
                        ReadOnlySpan<char> str = MemoryMarshal.Cast<byte, char>(sliceSpan);                 
                        return new string(str);
                    }
                    else
                    {
                        return BinaryReaderOption.Encoding.GetString(sliceSpan);
                    }
                }

                public DateTime ReadDateTime()
                {
                    return new DateTime(ReadInt64() + TimeUtility.UTCFirstYear.Ticks);
                }

                public TimeSpan ReadTimeSpan()
                {
                    return new TimeSpan(ReadInt64());
                }
            }
        }
    }
}