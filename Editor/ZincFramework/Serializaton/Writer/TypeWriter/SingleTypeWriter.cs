using System.Buffers;
using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace TypeWriter
        {
            public readonly struct SingleTypeWriter : ISerializationWriter
            {
                public string ClassType { get; }

                public string Type { get; }

                public int RelativeIndentSize { get; }


                public SingleTypeWriter(string classType, string type, int relativeIndentSize)
                {
                    ClassType = classType;
                    Type = type;
                    RelativeIndentSize = relativeIndentSize;
                }

                public readonly ArraySegment<string> GetAppendState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);

                    string methodStr = Type switch
                    {
                        "int" => $"byteWriter.WriteInt32({name});",
                        "float" => $"byteWriter.WriteSingle({name});",
                        "bool" => $"byteWriter.WriteBoolean({name});",
                        "long" => $"byteWriter.WriteInt64({name});",
                        "short" => $"byteWriter.WriteInt16({name});",
                        "double" => $"byteWriter.WriteDouble({name});",
                        string when Type.Contains("E_") => $"byteWriter.WriteInt32((int){name});",
                        _ => throw new NotSupportedException(Type + "不受支持"!)
                    };

                    int nowIndex = 0;
                    statement[nowIndex++] = SerializeWriteUtility.GetWriteCode(name);
                    statement[nowIndex++] = methodStr;
                    statement[nowIndex++] = string.Empty;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }

                public readonly ArraySegment<string> GetConvertState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);
                    int nowIndex = 0;
                    string methodStr = Type switch
                    {
                        "int" => $"(_setMap[code] as SetAction<{ClassType}, int>).Invoke(this, byteReader.ReadInt32());",
                        "float" => $"(_setMap[code] as SetAction<{ClassType}, float>).Invoke(this, byteReader.ReadSingle());",
                        "bool" => $"(_setMap[code] as SetAction<{ClassType}, bool>).Invoke(this, byteReader.ReadBoolean());",
                        "long" => $"(_setMap[code] as SetAction<{ClassType}, long>).Invoke(this, byteReader.ReadInt64());",
                        "short" => $"(_setMap[code] as SetAction<{ClassType}, short>).Invoke(this, byteReader.ReadInt16());",
                        "double" => $"(_setMap[code] as SetAction<{ClassType}, double>).Invoke(this, byteReader.ReadDouble());",
                        string when Type.Contains("E_") => $"(_setMap[code] as SetAction<{ClassType}, {Type}>).Invoke(this, ({Type})byteReader.ReadInt32());",
                        _ => throw new NotSupportedException(Type + "不受支持"!)
                    };
                    statement[nowIndex++] = SerializeWriteUtility.GetCodeStr;
                    statement[nowIndex++] = methodStr;
                    statement[nowIndex++] = string.Empty;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }

                public readonly ArraySegment<string> GetLengthState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);

                    string methodStr = Type switch
                    {
                        "int" or "uint" or "float" => "bytesLength += 4;",
                        "long" or "ulong" or "double" => "bytesLength += 8;",
                        "short" or "ushort" => "bytesLength += 2;",
                        "bool" or "byte" or "sbyte" => "bytesLength += 1;",
                        string when Type.Contains("E_") => "bytesLength += 4;",
                        _ => throw new NotSupportedException(Type + "不受支持")
                    };

                    int nowIndex = 0;
                    statement[nowIndex++] = SerializeWriteUtility.OridinalLengthStr;
                    statement[nowIndex++] = methodStr;
                    statement[nowIndex++] = string.Empty;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }

                public static bool IsSingleValue(string type) => type switch
                {
                    "int" or "uint" or "float" => true,
                    "long" or "ulong" or "double" => true,
                    "short" or "ushort" => true,
                    "bool" or "byte" or "sbyte" => true,
                    string when type.Contains("E_") => true,
                    _ => false,
                };
            }
        }
    }
}

