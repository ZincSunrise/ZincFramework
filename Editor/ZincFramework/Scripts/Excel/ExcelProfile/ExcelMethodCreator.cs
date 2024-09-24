using System;
using System.Buffers;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.Writer;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelMethodCreator
        {
            public static void WriteAllMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> serialize = new List<string>()
                {
                    "byte[] bytes = new byte[GetBytesLength()];",
                    "int nowIndex = 0;" + Environment.NewLine,
                };
                CreateSerializeMethod(classWriter, classType, excelSheet, count);
                CreateDeserializeMethod(classWriter, classType, excelSheet, count);
                CreateGetBytesLengthMethod(classWriter, classType, excelSheet, count);
                CreateAppendMethod(classWriter, classType, excelSheet, count);
                CreateConvertMethod(classWriter, classType, excelSheet, count);
            }

            private static void CreateSerializeMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "byte[] bytes = new byte[GetBytesLength()];",
                    "int nowIndex = 0;" + Environment.NewLine,
                };

                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    string name = TextUtility.UpperFirstChar(excelSheet[autoWriteConfig.nameLine, i]);

                    string[] strings = type.Split('/');
                    ArraySegment<string> segments;
                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        type = "array";
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];

                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetAppendState(name, genericTypes: elementType);
                    }
                    else
                    {
                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetAppendState(name);
                    }

                    statement.AddRange(segments);
                    ArrayPool<string>.Shared.Return(segments.Array);
                }

                statement.Add("return bytes;");
                classWriter.WriteMethod(2, "Serialize", "byte[]", null, null, statement);
            }

            private static void CreateDeserializeMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "int nowIndex = 0;",
                    "int code;" + Environment.NewLine,
                };


                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    if (excelSheet[autoWriteConfig.typeLine, i].Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        statement.Add("short count;");
                        break;
                    }
                }


                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    string name = TextUtility.UpperFirstChar(excelSheet[autoWriteConfig.nameLine, i]);

                    string[] strings = type.Split('/');

                    ArraySegment<string> segments;
                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        type = "array";
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];

                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetConvertState(name, genericTypes: elementType);
                    }
                    else
                    {
                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetConvertState(name);
                    }

                    statement.AddRange(segments);
                    ArrayPool<string>.Shared.Return(segments.Array);
                }

                classWriter.WriteMethod(2, "Deserialize", "void", null, new string[] { "byte[] bytes" }, statement);
            }

            private static void CreateGetBytesLengthMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "int bytesLength = 0;\r\n",
                    string.Empty,
                };

                classWriter.WriteLine();


                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    string name = TextUtility.UpperFirstChar(excelSheet[autoWriteConfig.nameLine, i]);

                    string[] strings = type.Split("/");

                    ArraySegment<string> segments;
                    ISerializationWriter writer;
                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];
                        type = "array";
                        writer = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = writer.GetLengthState(name, genericTypes: elementType);
                    }
                    else
                    {
                        writer = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = writer.GetLengthState(name);
                    }

                    statement.AddRange(segments);
                    ArrayPool<string>.Shared.Return(segments.Array);
                }

                statement.Add("return bytesLength;" + Environment.NewLine);
                classWriter.WriteMethod(2, "GetBytesLength", "int", null, null, statement);
            }

            private static void CreateConvertMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "int code;" + Environment.NewLine,
                };

                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;
                for (int i = 0; i < count; i++)
                {
                    if (excelSheet[autoWriteConfig.typeLine, i].ToString().Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        statement.Add("short count;");
                        break;
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    string name = TextUtility.UpperFirstChar(excelSheet[autoWriteConfig.nameLine, i]);
                    string[] strings = type.Split('/');

                    ArraySegment<string> segments;
                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        type = "array";
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];
                        
                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetConvertState(name, genericTypes: elementType);
                    }
                    else
                    {
                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetConvertState(name);
                    }

                    statement.AddRange(segments);
                    ArrayPool<string>.Shared.Return(segments.Array);
                }

                classWriter.WriteMethod(2, "Convert", "void", null, new string[] { "byte[] bytes", "ref int nowIndex" }, statement);
            }

            private static void CreateAppendMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> methodStatement = new();


                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;
                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    string name = TextUtility.UpperFirstChar(excelSheet[autoWriteConfig.nameLine, i]);
                    string[] strings = type.Split('/');

                    ArraySegment<string> segments;

                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        type = "array";
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];

                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetAppendState(name, genericTypes: elementType);
                    }
                    else
                    {
                        ISerializationWriter serializationWriter = ISerializationWriter.GetSerializationWriter(classType, type, 1);
                        segments = serializationWriter.GetAppendState(name);
                    }

                    methodStatement.AddRange(segments);
                    ArrayPool<string>.Shared.Return(segments.Array);
                }

                classWriter.WriteMethod(2, "Append", "void", null, new string[] { "byte[] bytes", "ref int nowIndex" }, methodStatement);
            }
        }
    }
}
