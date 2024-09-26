using System;
using System.Buffers;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Rendering;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.Writer;


namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public static class ProtocolMethodCreator
            {
                public static void WriteAllMethod(CSharpWriter classWriter, string className, XmlNodeList xmlNodeList, bool isOverride)
                {
                    CreateSerializeMethod(classWriter, isOverride);
                    CreateDeserializeMethod(classWriter, isOverride);
                    CreateGetTypeLengthMethod(classWriter, isOverride);
                    CreateGetBytesLengthMethod(classWriter, className, xmlNodeList, isOverride);
                }


                private static void CreateGetTypeLengthMethod(CSharpWriter classWriter, bool isOverride)
                {
                    classWriter.WriteMethod(2, "GetTypeLength", "int", isOverride ? CSharpWriter.Modifiers.Override : null, Array.Empty<string>(), new string[]     
                    {
                        "return GetBytesLength() - 8;",
                    });
                }

                private static void CreateSerializeMethod(CSharpWriter classWriter, bool isOverride)
                {
                    classWriter.WriteMethod(2, "Serialize", "byte[]", isOverride ? CSharpWriter.Modifiers.Override : null, Array.Empty<string>(), new string[] 
                    {
                        "int length = GetBytesLength();",
                        "byte[] bytes = new byte[length];",
                        "int nowIndex = 0;",
                        Environment.NewLine,

                        "//写入文件头",
                        "ByteAppender.AppendInt32(SerializableCode, bytes, ref nowIndex);",
                        "ByteAppender.AppendInt32(length - HeadInfo.HeadLength, bytes, ref nowIndex);" + Environment.NewLine,
                        "Append(bytes, ref nowIndex);",
                        "return bytes;"
                    });
                }

                private static void CreateDeserializeMethod(CSharpWriter classWriter, bool isOverride)
                {
                    classWriter.WriteMethod(2, "Deserialize", "void", isOverride ? CSharpWriter.Modifiers.Override : null, new string[] { "byte[] bytes" }, new string[]
                    {
                        "int nowIndex = HeadInfo.HeadLength;",
                        "Convert(bytes, ref nowIndex);",
                    });
                }


                private static void CreateGetBytesLengthMethod(CSharpWriter classWriter, string classType, XmlNodeList xmlFieldNodes, bool isOverride)
                {
                    List<string> methodStatement = new()
                    {
                        "int bytesLength = HeadInfo.HeadLength;" + Environment.NewLine,
                    };

                    
                    foreach (XmlNode xmlNode in xmlFieldNodes)
                    {
                        string type = xmlNode.Attributes["type"].Value;
                        string name = TextUtility.UpperFirstChar(xmlNode.Attributes["name"].Value);

                        ArraySegment<string> statement;

                        ISerializationWriter writer;
                        if (xmlNode.Attributes["T"] != null)
                        {
                            string genericType = xmlNode.Attributes["T"].Value;

                            if (string.IsNullOrEmpty(genericType))
                            {
                                throw new ArithmeticException("不能传入空泛型");
                            }


                            writer = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                            statement = writer.GetLengthState(name, genericTypes: genericType);
                        }
                        else if (xmlNode.Attributes["T1"] != null && xmlNode.Attributes["T2"] != null)
                        {
                            string keyType = xmlNode.Attributes["T1"].Value;
                            string valueType = xmlNode.Attributes["T2"].Value;

                            if (string.IsNullOrEmpty(keyType) || string.IsNullOrEmpty(valueType))
                            {
                                throw new ArithmeticException("不能传入空泛型");
                            }

                            writer = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                            statement = writer.GetLengthState(name, keyType, valueType);
                        }
                        else
                        {
                            writer = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                            statement = writer.GetLengthState(name);
                        }

                        methodStatement.AddRange(statement);
                        ArrayPool<string>.Shared.Return(statement.Array);
                    }


                    methodStatement.Add("return bytesLength;" + Environment.NewLine);
                    classWriter.WriteMethod(2, "GetBytesLength", "int", isOverride ? new string[] { "override" } : null, Array.Empty<string>(), methodStatement);
                    classWriter.WriteLine();
                }
            }
        }
    }
}

