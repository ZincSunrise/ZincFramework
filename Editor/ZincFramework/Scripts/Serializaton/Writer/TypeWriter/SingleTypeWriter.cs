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
                        "int" => $"ByteAppender.AppendInt32({name}, bytes, ref nowIndex);",
                        "float" => $"ByteAppender.AppendFloat({name}, bytes, ref nowIndex);",
                        "bool" => $"ByteAppender.AppendBoolean({name}, bytes, ref nowIndex);",
                        "long" => $"ByteAppender.AppendInt64({name}, bytes, ref nowIndex);",
                        "short" => $"ByteAppender.AppendInt16({name}, bytes, ref nowIndex);",
                        "double" => $"ByteAppender.AppendDouble({name}, bytes, ref nowIndex);",
                        string when Type.Contains("E_") => $"ByteAppender.AppendInt32((int){name}, bytes, ref nowIndex);",
                        _ => throw new NotSupportedException(Type + "不受支持"!)
                    };

                    int nowIndex = 0;
                    statement[nowIndex++] = SerializeWriteTool.GetAppendCode(name);
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
                        "int" => $"(_setMap[code] as SetAction<{ClassType}, int>).Invoke(this, ByteConverter.ToInt32(bytes, ref nowIndex));",
                        "float" => $"(_setMap[code] as SetAction<{ClassType}, float>).Invoke(this, ByteConverter.ToFloat(bytes, ref nowIndex));",
                        "bool" => $"(_setMap[code] as SetAction<{ClassType}, bool>).Invoke(this, ByteConverter.ToBoolean(bytes, ref nowIndex));",
                        "long" => $"(_setMap[code] as SetAction<{ClassType}, long>).Invoke(this, ByteConverter.ToInt64(bytes, ref nowIndex));",
                        "short" => $"(_setMap[code] as SetAction<{ClassType}, short>).Invoke(this, ByteConverter.ToInt16(bytes, ref nowIndex));",
                        "double" => $"(_setMap[code] as SetAction<{ClassType}, double>).Invoke(this, ByteConverter.ToDouble(bytes, ref nowIndex));",
                        string when Type.Contains("E_") => $"(_setMap[code] as SetAction<{ClassType}, {Type}>).Invoke(this, ({Type})ByteConverter.ToInt32(bytes, ref nowIndex));",
                        _ => throw new NotSupportedException(Type + "不受支持"!)
                    };
                    statement[nowIndex++] = SerializeWriteTool.GetCodeStr;
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
                    statement[nowIndex++] = SerializeWriteTool.OridinalLengthStr;
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

