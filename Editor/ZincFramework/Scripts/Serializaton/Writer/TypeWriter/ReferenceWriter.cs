using System;
using System.Buffers;
using ZincFramework.Serialization.Handlers;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace TypeWriter
        {
            public readonly struct ReferenceWriter : ISerializationWriter
            {
                public string ClassType { get; }

                public string Type { get; }

                public int RelativeIndentSize { get; }


                public ReferenceWriter(string classType, string type, int relativeIndentSize)
                {
                    ClassType = classType;
                    Type = type;
                    RelativeIndentSize = relativeIndentSize;
                }

                public readonly ArraySegment<string> GetAppendState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);

                    int nowIndex = 0;
                    statement[nowIndex++] = SerializeWriteTool.AppendNullCondition(name);
                    statement[nowIndex++] = "{";
                    statement[nowIndex++] = WriteUtility.InsertTable(SerializeWriteTool.GetAppendCode(name), RelativeIndentSize);


                    if (Type == "string")
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"ByteAppender.AppendString({name}, bytes, ref nowIndex);", RelativeIndentSize);
                    }
                    else if (SerializeWriteTool.IsCollection(Type))
                    {
                        IEnumerableHandler enumerableHandler = IEnumerableHandler.GetEnumerableHandler(ClassType, Type, RelativeIndentSize);
                        enumerableHandler.GetAppend(name, statement, ref nowIndex, genericTypes);
                    }
                    else
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"{name}.Append(bytes, ref nowIndex);", RelativeIndentSize);
                    }

                    statement[nowIndex++] = "}";

                    statement[nowIndex++] = SerializeWriteTool.Else;
                    statement[nowIndex++] = "{";
                    statement[nowIndex++] = WriteUtility.InsertTable(SerializeWriteTool.AppendNullCode, RelativeIndentSize);
                    statement[nowIndex++] = '}' + Environment.NewLine;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }

                public readonly ArraySegment<string> GetConvertState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);
                    int nowIndex = 0;

                    statement[nowIndex++] = SerializeWriteTool.ConvertMemberCodeStr;
                    statement[nowIndex++] = SerializeWriteTool.ConvertNullCondition;
                    statement[nowIndex++] = "{";

                    if (Type == "string")
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"(_setMap[code] as SetAction<{ClassType}, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));", RelativeIndentSize);
                    }
                    else if (SerializeWriteTool.IsCollection(Type))
                    {
                        IEnumerableHandler enumerableHandler = IEnumerableHandler.GetEnumerableHandler(ClassType, Type, RelativeIndentSize);
                        enumerableHandler.GetConvert(name, statement, ref nowIndex, genericTypes);
                    }
                    else
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"{Type} data = new {Type}();", RelativeIndentSize);
                        statement[nowIndex++] = WriteUtility.InsertTable("data.Convert(bytes, ref nowIndex);", RelativeIndentSize);
                        statement[nowIndex++] = WriteUtility.InsertTable($"(_setMap[code] as SetAction<{ClassType}, {Type}>).Invoke(this, data);", RelativeIndentSize);
                    }


                    statement[nowIndex++] = '}' + Environment.NewLine;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }

                public readonly ArraySegment<string> GetLengthState(string name, params string[] genericTypes)
                {
                    string[] statement = ArrayPool<string>.Shared.Rent(32);

                    int nowIndex = 0;
                    statement[nowIndex++] = SerializeWriteTool.OridinalLengthStr;
                    statement[nowIndex++] = SerializeWriteTool.AppendNullCondition(name);
                    statement[nowIndex++] = "{";

                    if (Type == "string")
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"bytesLength += ByteUtility.GetStringLength({name});", RelativeIndentSize);
                    }
                    else if (SerializeWriteTool.IsCollection(Type))
                    {
                        IEnumerableHandler enumerableHandler = IEnumerableHandler.GetEnumerableHandler(ClassType, Type, RelativeIndentSize);
                        enumerableHandler.GetLength(name, statement, ref nowIndex, genericTypes);
                    }
                    else
                    {
                        statement[nowIndex++] = WriteUtility.InsertTable($"bytesLength += {name}.GetTypeLength();", RelativeIndentSize);
                    }
                    statement[nowIndex++] = '}' + Environment.NewLine;

                    return new ArraySegment<string>(statement, 0, nowIndex);
                }
            }
        }
    }
}

