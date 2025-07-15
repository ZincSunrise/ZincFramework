using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ZincFramework.Pools;
using ZincFramework.Serialization;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.ScriptWriter;
using ZincFramework.ScriptWriter.Handle;



namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public static class ProtocolClassCreator
            {
                public static void CreateEnum(XmlNode rootNode, CSharpWriter classWriter)
                {
                    XmlNodeList xmlNodeList = rootNode.SelectNodes("enum");
                    XmlNodeList fieldNodeList;

                    foreach (XmlNode enumNode in xmlNodeList)
                    {
                        using (FileStream fileStream = File.Open(Path.Combine(ProtocolTool.DataPath, enumNode.Attributes["name"].Value + ".cs"), FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            string namespaces = enumNode.Attributes["namespace"].Value;
                            string name = enumNode.Attributes["name"].Value;
                            classWriter.WriteNamespace(2);
                            classWriter.BeginWriteClass(1, namespaces, null, name, classType: "enum");

                            fieldNodeList = enumNode.SelectNodes("field");

                            foreach (XmlNode fieldNode in fieldNodeList)
                            {
                                name = fieldNode.Attributes["name"].Value + (fieldNode.Attributes["value"]?.Value ?? "0") + ',';
                                classWriter.WriteLine(2, name);
                            }

                            classWriter.EndWriteClass(1, !string.IsNullOrEmpty(namespaces));
                            classWriter.WriteAndClear(fileStream);
                        }
                    }
                }

                public static void CreateMessage(XmlNode rootNode, CSharpWriter classWriter)
                {
                    XmlNodeList xmlNodeList = rootNode.SelectNodes("massage");
                    XmlNodeList fieldNodeList;
                    string path = Path.Combine(ProtocolTool.DataPath, "Messages");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (XmlNode massageNode in xmlNodeList)
                    {
                        path = Path.Combine(ProtocolTool.DataPath, "Messages", massageNode.Attributes["name"].Value + ".cs");

                        using (FileStream fileStream = File.Create(path))
                        {
                            string namespaces = massageNode.Attributes["namespace"].Value;
                            string classType = massageNode.Attributes["name"].Value;
                            string messageId = massageNode.Attributes["messageId"].Value;

                            classWriter.WriteNamespace(2, ExcelModel.Instance.ProtocolDefault.usingNamespaces);
                            classWriter.BeginWriteClass(1, namespaces, null, classType, classType: "record", parents: new string[] { "BaseMessage" }, attributeHandle: new AttributeHandle(1, nameof(ZincSerializable), new string[] { messageId }));

                            fieldNodeList = massageNode.SelectNodes("field");

                            classWriter.WriteAttribute(2, nameof(BinaryIgnore));
                            classWriter.WriteQuickProperty(2, "int", "SerializableCode", messageId, CSharpWriter.Modifiers.Override);

                            List<MemberWriteInfo> fieldWriteInfos = GetFieldWriteInfos(fieldNodeList);

                            SerializableClassWriter.WriteMaps(fieldWriteInfos, true, classType, classWriter);
                            SerializableClassWriter.WriteAllFields(fieldWriteInfos, classWriter);

                            ProtocolMethodCreator.WriteAllMethod(classWriter, classType, fieldNodeList, true);
                            classWriter.EndWriteClass(1, !string.IsNullOrEmpty(namespaces));
                            classWriter.WriteAndClear(fileStream);

                            CreateHandler(massageNode, classWriter);
                            //PoolCollector.Collect(massageNode);
                        }
                    }

                    PoolCollector.InsertPool();
                }


                public static void CreateData(XmlNode rootNode, CSharpWriter classWriter)
                {
                    XmlNodeList xmlNodeList = rootNode.SelectNodes("data");
                    string dataPath = Path.Combine(ProtocolTool.DataPath, "Datas") ;

                    if (!Directory.Exists(dataPath))
                    {
                        Directory.CreateDirectory(dataPath);
                    }

                    foreach (XmlNode dataNode in xmlNodeList)
                    {
                        using (FileStream fileStream = File.Open(Path.Combine(dataPath, dataNode.Attributes["name"].Value + ".cs"), FileMode.Create, FileAccess.Write))
                        {
                            string namespaces = dataNode.Attributes["namespace"].Value;
                            string name = dataNode.Attributes["name"].Value;
                            string dataId = dataNode.Attributes["dataId"].Value;

                            //写入命名空间
                            classWriter.WriteNamespace(2, ExcelModel.Instance.ProtocolDefault.usingNamespaces);

                            //写入类名字
                            classWriter.BeginWriteClass(1, namespaces, null, name + "Data");

                            //写入Data类
                            classWriter.WriteGenericField(2, "infoDic", "Dictionary", new string[] { "int", name + "Info" });
                            classWriter.EndWriteClass(1, false);

                            XmlNodeList fieldNodeList = dataNode.SelectNodes("field");
                            SerializableClassWriter.CreateSerializeClass(classWriter, name + "Info", dataId, GetFieldWriteInfos(fieldNodeList), true);

                            ProtocolMethodCreator.WriteAllMethod(classWriter, name + "Info", fieldNodeList, false);
                            classWriter.EndWriteClass(1, !string.IsNullOrEmpty(namespaces));
                            classWriter.WriteAndClear(fileStream);
                        }
                    }
                }


                private static void CreateHandler(XmlNode massageNode, CSharpWriter classWriter)
                {
                    string path = Path.Combine(ProtocolTool.DataPath, "Handlers");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = Path.Combine(path, massageNode.Attributes["name"].Value + "Handler.cs");
                    if (File.Exists(path))
                    {
                        return;
                    }

                    using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        string namespaces = massageNode.Attributes["namespace"].Value;
                        string name = massageNode.Attributes["name"].Value;

                        classWriter.WriteNamespace(2, "ZincFramework.Network.Protocol");

                        classWriter.BeginWriteClass(1, namespaces, null, name + "Handler", parents: new string[] { "IHandleMessage" });

                        classWriter.WriteAutoProperty(2, "BaseMessage", "Message", CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Public);
                        classWriter.WriteLine(2);

                        classWriter.WriteMethod(2, "HandleMessage", "void", null, Array.Empty<string>(), new string[1] { "//填入你对于这个类的处理方法" });

                        classWriter.EndWriteClass(1, !string.IsNullOrEmpty(namespaces));
                        classWriter.WriteAndClear(fileStream);
                    }
                }

                private static List<MemberWriteInfo> GetFieldWriteInfos(XmlNodeList fieldNodeList) 
                {
                    List<MemberWriteInfo> memberWriteInfos = new List<MemberWriteInfo>();
                    int count = 0;

                    foreach (XmlNode fieldNode in fieldNodeList)
                    {
                        string type = fieldNode.Attributes["type"].Value;
                        string name = fieldNode.Attributes["name"].Value;
                        string oridinalNumber = fieldNode.Attributes["oridinalNumber"].Value;
                        memberWriteInfos[count++] = new MemberWriteInfo(0, name, type, oridinalNumber, true);
                    }

                    return memberWriteInfos;
                }
            }
        }
    }
}

