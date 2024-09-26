using System;
using System.Buffers;
using ZincFramework.Serialization.TypeWriter;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct ArrayHandler : IEnumerableHandler
            {
                public string Method { get; }
                public string Type { get; }

                public string ClassType { get; }

                public int RelativeIndentSize { get; }

                public ArrayHandler(string method, string classType, string type, int relativeIndentSize) 
                {
                    Method = method;
                    Type = type;
                    ClassType = classType;
                    RelativeIndentSize = relativeIndentSize;
                }

                public void GetAppend(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.AppendLength(name, "Length"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ArrayLoopHead(name, "Length"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);


                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] array = memberHandle.GetWrite(name + "[i]");

                    for (int i = 0; i < array.Length; i++) 
                    {
                        statement[index++] = array[i];
                    }

                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                }


                public void GetConvert(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ConvertCollectionLengthStr, RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable($"{genericType[0]}[] tempArray = new {genericType[0]}[count];", RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ArrayLoopHead("tempArray", "Length"), RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);

                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] array = memberHandle.GetRead("tempArray[i]");

                    for (int i = 0; i < array.Length; i++)
                    {
                        statement[index++] = array[i];
                    }

                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable($"(_setMap[code] as SetAction<{ClassType}, {genericType[0]}[]>).Invoke(this, tempArray);", RelativeIndentSize);
                }

                public void GetLength(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable("bytesLength += 2;", RelativeIndentSize);

                    if (SingleTypeWriter.IsSingleValue(genericType[0]))
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.GetSingleTypeLength(name, genericType[0], "Length"), RelativeIndentSize);
                    }
                    else
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ArrayLoopHead(name, "Length"), RelativeIndentSize);
                        statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);
                        IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                        string[] array = memberHandle.GetLength(name + "[i]");

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

