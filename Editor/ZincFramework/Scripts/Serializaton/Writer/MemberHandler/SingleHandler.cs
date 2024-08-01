using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct SingleHandler : IMemberHandle
            {
                public string Type { get; }

                public int IndentSize { get; }

                public SingleHandler(int indentSize, string type) 
                {
                    IndentSize = indentSize;
                    Type = type;
                }


                public string[] GetAppend(string name)
                {
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


                    return new string[]
                    {
                         WriteUtility.InsertTable(methodStr, IndentSize)
                    };
                }


                public string[] GetConvert(string name)
                {
                    string methodStr = Type switch
                    {
                        "int" => $"{name} = ByteConverter.ToInt32(bytes, ref nowIndex);",
                        "float" => $"{name} = ByteConverter.ToFloat(bytes, ref nowIndex);",
                        "bool" => $"{name} = ByteConverter.ToBoolean(bytes, ref nowIndex);",
                        "long" => $"{name} = ByteConverter.ToInt64(bytes, ref nowIndex);",
                        "short" => $"{name} = ByteConverter.ToInt16(bytes, ref nowIndex);",
                        "double" => $"{name} = ByteConverter.ToDouble(bytes, ref nowIndex);",
                        string when Type.Contains("E_") => $"{name} = ({Type})ByteConverter.ToInt32(bytes, ref nowIndex);",
                        _ => throw new NotSupportedException(Type + "不受支持"!)
                    };

                    return new string[]
                    {
                         WriteUtility.InsertTable(methodStr, IndentSize)
                    };
                }


                public string[] GetLength(string name)
                {
                    string methodStr = Type switch
                    {
                        "int" or "uint" or "float" => "bytesLength += 4;",
                        "long" or "ulong" or "double" => "bytesLength += 8;",
                        "short" or "ushort" => "bytesLength += 2;",
                        "bool" or "byte" or "sbyte" => "bytesLength += 1;",
                        string when Type.Contains("E_") => "bytesLength += 4;",
                        _ => throw new NotSupportedException(Type + "不受支持")
                    };

                    return new string[]
                    {
                         WriteUtility.InsertTable(methodStr, IndentSize)
                    };
                }
            }
        }
    }
}