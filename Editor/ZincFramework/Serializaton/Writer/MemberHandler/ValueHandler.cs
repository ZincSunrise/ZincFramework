using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct ValueHandler : IMemberHandle
            {
                public string Type { get; }

                public int IndentSize { get; }

                public ValueHandler(int indentSize, string type) 
                {
                    IndentSize = indentSize;
                    Type = type;
                }


                public string[] GetWrite(string name)
                {
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


                    return new string[]
                    {
                         WriteUtility.InsertTable(methodStr, IndentSize)
                    };
                }


                public string[] GetRead(string name)
                {
                    string methodStr = Type switch
                    {
                        "int" => $"{name} = byteReader.ReadInt32();",
                        "float" => $"{name} = byteReader.ReadSingle();",
                        "bool" => $"{name} = byteReader.ReadBoolean();",
                        "long" => $"{name} = byteReader.ReadInt64();",
                        "short" => $"{name} = byteReader.ReadInt16();",
                        "double" => $"{name} = byteReader.ReadDouble();",
                        string when Type.Contains("E_") => $"{name} = ({Type})byteReader.ReadInt32();",
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