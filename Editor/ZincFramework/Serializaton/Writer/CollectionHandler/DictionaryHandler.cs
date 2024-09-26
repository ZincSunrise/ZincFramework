using System;
using System.Buffers;
using ZincFramework.Serialization.TypeWriter;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public readonly struct DictionaryHandler : IEnumerableHandler
            {
                public string Method { get; }
                public string Type { get; }
                public int RelativeIndentSize { get; }

                public string ClassType { get; }
                public DictionaryHandler(string method, string classType, string type, int identSize)
                {
                    Method = method;
                    Type = type;
                    RelativeIndentSize = identSize;

                    ClassType = classType;
                }

                public void GetAppend(string name, string[] statement, ref int index, params string[] genericType)
                {
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.AppendLength(name, "Count"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ForeachHead("item", name), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);

                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] keyArray = memberHandle.GetWrite("item.Key");

                    memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[1]);
                    string[] valueArray = memberHandle.GetWrite("item.Value");


                    for (int i = 0; i < keyArray.Length; i++) 
                    {
                        statement[index++] = keyArray[i];
                    }

                    for (int i = 0; i < valueArray.Length; i++)
                    {
                        statement[index++] = valueArray[i];
                    }

                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                }

                public void GetConvert(string name, string[] statement, ref int index, params string[] genericType)
                {
                    string keyType = genericType[0];
                    string valueType = genericType[1];
                    string dictionType = $"{Type}<{keyType}, {valueType}>";

                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ConvertCollectionLengthStr, RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable($"{keyType} key;", RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable($"{valueType} value;", RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable($"{dictionType} tempDictionary = new {dictionType}();" + Environment.NewLine, RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.LoopHead("count"), RelativeIndentSize);
                    statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);


                    IMemberHandle memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[0]);
                    string[] keyArray = memberHandle.GetRead("key");

                    memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, genericType[1]);
                    string[] valueArray = memberHandle.GetRead("value");


                    for (int i = 0; i < keyArray.Length; i++)
                    {
                        statement[index++] = keyArray[i];
                    }

                    for (int i = 0; i < valueArray.Length; i++)
                    {
                        statement[index++] = valueArray[i];
                    }

                    statement[index++] = WriteUtility.InsertTable($"tempDictionary.{Method}(key, value);", RelativeIndentSize + 1);
                    statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);

                    statement[index++] = WriteUtility.InsertTable($"(_setMap[code] as SetAction<{ClassType}, {dictionType}>).Invoke(this, tempDictionary);", RelativeIndentSize);
                }

                public void GetLength(string name, string[] statement, ref int index, params string[] genericType)
                {
                    string keyType = genericType[0];
                    string valueType = genericType[1];

                    bool isValueSingle;   
                    bool isKeySingle;

                    statement[index++] = WriteUtility.InsertTable("bytesLength += 2;", RelativeIndentSize);

                    if (isKeySingle = SingleTypeWriter.IsSingleValue(keyType))
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.GetSingleTypeLength(name, keyType, "Count"), RelativeIndentSize);
                    }
                    if (isValueSingle = SingleTypeWriter.IsSingleValue(valueType))
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.GetSingleTypeLength(name, valueType, "Count"), RelativeIndentSize);
                    }


                    if (!isKeySingle || !isValueSingle)
                    {
                        statement[index++] = WriteUtility.InsertTable(SerializeWriteUtility.ForeachHead("item", name), RelativeIndentSize);
                        statement[index++] = WriteUtility.InsertTable('{', RelativeIndentSize);

                        IMemberHandle memberHandle;
                        if (!isKeySingle)
                        {
                            memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, keyType);
                            string[] keyArray = memberHandle.GetLength("item.Key");
                            for (int i = 0; i < keyArray.Length; i++)
                            {
                                statement[index++] = keyArray[i];
                            }
                        }


                        if (!isValueSingle)
                        {
                            memberHandle = IMemberHandle.GetMemberHandle(RelativeIndentSize + 1, valueType);
                            string[] valueArray = memberHandle.GetLength("item.Value");
                            for (int i = 0; i < valueArray.Length; i++)
                            {
                                statement[index++] = valueArray[i];
                            }
                        }

                        statement[index++] = WriteUtility.InsertTable('}', RelativeIndentSize);
                    }
                }
            }
        }
    }
}
