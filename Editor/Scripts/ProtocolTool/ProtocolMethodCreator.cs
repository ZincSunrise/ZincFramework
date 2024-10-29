using System;
using System.Xml;
using System.Collections.Generic;
using ZincFramework.ScriptWriter;


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

                    }
                    methodStatement.Add("return bytesLength;" + Environment.NewLine);
                    classWriter.WriteMethod(2, "GetBytesLength", "int", isOverride ? new string[] { "override" } : null, Array.Empty<string>(), methodStatement);
                    classWriter.WriteLine();
                }
            }
        }
    }
}

