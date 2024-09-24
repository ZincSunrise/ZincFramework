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

                public string[] GetAppend(string name)
                {
                    if (SerializeWriteTool.IsCollection(Type))
                    {
                        throw new ArgumentException("集合类不能存在嵌套结构!");
                    }

                    return new string[] 
                    { 
                        WriteUtility.InsertTable(Type == "string" ? $"ByteAppender.AppendString({name}, bytes, ref nowIndex);" : $"{name}.Append(bytes, ref nowIndex);", IndentSize) 
                    };
                }


                public string[] GetConvert(string name)
                {
                    if (Type == "string")
                    {
                        return new [] 
                        { 
                            WriteUtility.InsertTable($"{name} = ByteConverter.ToString(bytes, ref nowIndex);", IndentSize) 
                        };
                    }
                    else if (SerializeWriteTool.IsCollection(Type))
                    {
                        throw new ArgumentException("集合类不能存在嵌套结构!");
                    }
                    else
                    {
                        return new [] 
                        { 
                            WriteUtility.InsertTable($"{name} = new {Type}();", 1),
                            WriteUtility.InsertTable($"{name}.Convert(bytes, ref nowIndex);", 1)
                        };
                    }
                }
 

                public string[] GetLength(string name)
                {
                    if (SerializeWriteTool.IsCollection(Type))
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