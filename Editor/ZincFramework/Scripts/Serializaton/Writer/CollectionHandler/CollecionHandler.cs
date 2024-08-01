using System;
using System.Buffers;
using ZincFramework.Serialization.TypeWriter;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct CollecionHandler : IEnumerableHandler
            {
                public string Method { get; }
                public string Type { get; }

                public string ClassType { get; }
                public int RelativeIndentSize { get; }

                public CollecionHandler(string method, string classType, string type, int relativeIndentSize)
                {
                    Method = method;
                    Type = type;
                    ClassType = classType;
                    RelativeIndentSize = relativeIndentSize;
                }


                public void GetAppend(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.AppendLength(name, "Count"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.ForeachHead("item", name), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);

                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] array = memberHandle.GetAppend("item");

                    for (int i = 0; i < array.Length; i++) 
                    {
                        statement[index++] = array[i];
                    }

                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                }


                public void GetConvert(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.ConvertCollectionLengthStr, RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable($"{genericType[0]} item;", RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable($"{Type}<{genericType[0]}> tempCollection = new {Type}<{genericType[0]}>();" + Environment.NewLine, RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.LoopHead("count"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);


                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] array = memberHandle.GetConvert("item");

                    for (int i = 0; i < array.Length; i++)
                    {
                        statement[index++] = array[i];
                    }

                    statement[index++] = WriteUtility.InsertTable($"tempCollection.{Method}(item);", RelativeIndentSize + 1);
                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable($"(_setMap[code] as SetAction<{ClassType}, {Type}<{genericType[0]}>>).Invoke(this, tempCollection);", RelativeIndentSize);
                }


                public void GetLength(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable("bytesLength += 2;", RelativeIndentSize);

                    if (SingleTypeWriter.IsSingleValue(genericType[0]))
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.GetSingleTypeLength(name, genericType[0], "Count"), RelativeIndentSize);
                    }
                    else
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteTool.ForeachHead("item", name), RelativeIndentSize);
                        statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);

                        IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                        string[] array = memberHandle.GetLength("item");

                        for (int i = 0; i < array.Length; i++)
                        {
                            statement[index++] = array[i];
                        }

                        statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                    }
                }
            }
        }
    }
}
