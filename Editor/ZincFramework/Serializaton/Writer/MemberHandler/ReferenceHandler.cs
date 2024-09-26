using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct ReferenceHandler : IMemberHandle
            {
                public string Type { get; }

                public int IndentSize { get; }

                public ReferenceHandler(int indentSize, string type)
                {
                    Type = type;
                    IndentSize = indentSize;
                }

                public string[] GetWrite(string name)
                {
                    if (SerializeWriteUtility.IsCollection(Type))
                    {
                        throw new ArgumentException("集合类不能存在嵌套结构!");
                    }

                    return new string[] 
                    { 
                        WriteUtility.InsertTable(Type == "string" ? $"byteWriter.WriteString({name});" : $"{name}.Write(bytes);", IndentSize) 
                    };
                }


                public string[] GetRead(string name)
                {
                    if (Type == "string")
                    {
                        return new [] 
                        { 
                            WriteUtility.InsertTable($"{name} = byteReader.ReadString();", IndentSize) 
                        };
                    }
                    else if (SerializeWriteUtility.IsCollection(Type))
                    {
                        throw new ArgumentException("集合类不能存在嵌套结构!");
                    }
                    else
                    {
                        return new [] 
                        { 
                            WriteUtility.InsertTable($"{name} = new {Type}();", 1),
                            WriteUtility.InsertTable($"{name}.Read(bytes);", 1)
                        };
                    }
                }
 

                public string[] GetLength(string name)
                {
                    if (SerializeWriteUtility.IsCollection(Type))
                    {
                        throw new ArgumentException("集合类不能存在嵌套结构!");
                    }

                    return new string[]
                    {
                        WriteUtility.InsertTable(Type == "string" ? $"bytesLength += ByteUtility.GetStringLength({name});" : $"bytesLength += {name}.GetTypeLength();" , 1)
                    };
                }
            }
        }
    }
}