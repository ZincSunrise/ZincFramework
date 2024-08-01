using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.Writer;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelMethodCreator
        {
            public static void WriteAllMethod(CSharpWriter classWriter, string classType , DataTable dataTable)
            {
                CreateSerializeMethod(classWriter, classType, dataTable);
                CreateDeserializeMethod(classWriter, classType, dataTable);
                CreateGetBytesLengthMethod(classWriter, classType, dataTable);
                CreateAppendMethod(classWriter, classType, dataTable);
                CreateConvertMethod(classWriter, classType, dataTable);
            }

            private static void CreateSerializeMethod(CSharpWriter classWriter, string classType, DataTable dataTable)
            {
                List<string> statement = new List<string>()
                {
                    "byte[] bytes = new byte[GetBytesLength()];",
                    "int nowIndex = 0;" + Environment.NewLine,
                };

                DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                DataRow nameRow = ExcelReader.GetVariableName(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string type = typeRow[i].ToString();
                    string name = TextUtility.UpperFirstString(nameRow[i].ToString());

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

            private static void CreateDeserializeMethod(CSharpWriter classWriter, string classType, DataTable dataTable)
            {
                List<string> statement = new List<string>()
                {
                    "int nowIndex = 0;",
                    "int code;" + Environment.NewLine,
                };

                DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                DataRow nameRow = ExcelReader.GetVariableName(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (typeRow[i].ToString().Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        statement.Add("short count;");
                        break;
                    }
                }

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string type = typeRow[i].ToString();
                    string name = TextUtility.UpperFirstString(nameRow[i].ToString());

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

            private static void CreateGetBytesLengthMethod(CSharpWriter classWriter, string classType, DataTable dataTable)
            {
                List<string> statement = new List<string>()
                {
                    "int bytesLength = 0;\r\n",
                    string.Empty,
                };

                classWriter.WriteLine();

                DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                DataRow nameRow = ExcelReader.GetVariableName(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string type = typeRow[i].ToString();
                    string name = TextUtility.UpperFirstString(nameRow[i].ToString());

                    string[] strings = type.Split("/");

                    ArraySegment<string> segments;
                    ISerializationWriter writer;
                    if (type.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        string elementType = string.Compare(strings[0], "Array", true) == 0 ? strings[1] : strings[0];
                        type = "array";
                        writer = ISerializationWriter.GetSerializationWriter(classType , type, 1);
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

            private static void CreateConvertMethod(CSharpWriter classWriter, string classType, DataTable dataTable)
            {
                List<string> statement = new List<string>()
                {
                    "int code;" + Environment.NewLine,
                };

                DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                DataRow nameRow = ExcelReader.GetVariableName(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (typeRow[i].ToString().Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        statement.Add("short count;");
                        break;
                    }
                }

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string type = typeRow[i].ToString();
                    string name = TextUtility.UpperFirstString(nameRow[i].ToString());
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

            private static void CreateAppendMethod(CSharpWriter classWriter, string classType, DataTable dataTable)
            {
                List<string> methodStatement = new();
                DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                DataRow nameRow = ExcelReader.GetVariableName(dataTable);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string type = typeRow[i].ToString();
                    string name = TextUtility.UpperFirstString(nameRow[i].ToString());
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
