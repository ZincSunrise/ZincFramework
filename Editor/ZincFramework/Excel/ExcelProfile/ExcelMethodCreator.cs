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

                CreateWriteMethod(classWriter, classType, excelSheet, count);
                CreateReadMethod(classWriter, classType, excelSheet, count);
                CreateGetBytesLengthMethod(classWriter, classType, excelSheet, count);
            }

            private static void CreateWriteMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>();

                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

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

                classWriter.WriteMethod(2, "Write", "void", null, new[] { "ByteWriter byteWriter" }, statement);
            }

            private static void CreateReadMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "int code;" + Environment.NewLine,
                };


                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    if (excelSheet[autoWriteConfig.typeLine, i].Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        statement.Add("int count;");
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

                classWriter.WriteMethod(2, "Read", "void", null, new string[] { "ref ByteReader byteReader" }, statement);
            }

            private static void CreateGetBytesLengthMethod(CSharpWriter classWriter, string classType, ExcelSheet excelSheet, int count)
            {
                List<string> statement = new List<string>()
                {
                    "int bytesLength = 0;\r\n",
                    string.Empty,
                };

                classWriter.WriteLine();


                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

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
        }
    }
}
