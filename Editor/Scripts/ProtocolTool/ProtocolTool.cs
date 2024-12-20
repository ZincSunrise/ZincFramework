using System.IO;
using System.Xml;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZincFramework.ScriptWriter;


namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public static class ProtocolTool
            {
                public static string XmlPath { get; } = Application.dataPath + "/ArtRes/XmlProtocols/";
                public static string DataPath { get; } = Application.dataPath + "/Scripts/Protocols/";


                [MenuItem("GameTool/Protocol/GenerateCSharpData")]
                private static void GenerateCSharpData()
                {
                    if (!Directory.Exists(DataPath))
                    {
                        Directory.CreateDirectory(DataPath);
                    }


                    if (Directory.Exists(XmlPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(XmlPath);
                        XmlDocument xmlDocument = new XmlDocument();

                        var fileInfos = from data in directoryInfo.GetFiles()
                                        where data.Extension == ".xml"
                                        select data;

                        foreach (var data in fileInfos)
                        {
                            using (FileStream fileStream = data.OpenRead())
                            {
                                xmlDocument.Load(fileStream);

                                XmlNode rootNode = xmlDocument.SelectSingleNode("Protocols");
                                var writer = CSharpWriter.RentWriter();

                                ProtocolClassCreator.CreateEnum(rootNode, writer);
                                ProtocolClassCreator.CreateMessage(rootNode, writer);
                                ProtocolClassCreator.CreateData(rootNode, writer);

                                writer.Return();
                            }
                        }
                    }
                    else
                    {
                        LogUtility.LogError("不存在名字为XmlProtocols的文件夹");
                    }

                    AssetDatabase.Refresh();
                }
            }
        }
    }
}

