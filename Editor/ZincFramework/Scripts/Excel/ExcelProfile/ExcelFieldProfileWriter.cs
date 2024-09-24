using System;
using System.IO;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelFieldProfileWriter
        {
            public static void WriteField(string fieldType, string code, string data, Stream stream)
            {
                BytesWriter.WriteInt32(int.Parse(code.ToString()), stream);

                switch (fieldType)
                {
                    case "int":
                        BytesWriter.WriteInt32(int.TryParse(data, out var intResult) ? intResult : 0, stream);
                        break;
                    case "float":
                        BytesWriter.WriteFloat(float.TryParse(data, out var floatResult) ? floatResult : 0, stream);
                        break;
                    case "bool":
                        BytesWriter.WriteBoolean(bool.TryParse(data, out var boolResult) && boolResult, stream);
                        break;
                    case "string":
                        BytesWriter.WriteString(data, stream);
                        break;
                    case "double":
                        BytesWriter.WriteDouble(double.Parse(data), stream);
                        break;
                    case "long":
                        BytesWriter.WriteInt64(long.Parse(data), stream);
                        break;
                    case "short":
                        BytesWriter.WriteInt16(short.Parse(data), stream);
                        break;
                    case string type when type.Contains("Array", StringComparison.OrdinalIgnoreCase):
                        WriteArray(type, data, stream);
                        break;
                    case string type when (type.IndexOf("E_") != -1):
                        BytesWriter.WriteInt32(int.Parse(data), stream);
                        break;
                }
            }

            public static void WriteArray(string type, string arrayData, Stream stream)
            {
                string[] strs = TextUtility.Split(type, '/');
                string elementType = string.Empty;

                elementType = Array.Find(strs, x => !"Array".Equals(x, StringComparison.OrdinalIgnoreCase));

                if (arrayData == string.Empty)
                {
                    BytesWriter.WriteInt16(0, stream);
                    return;
                }

                string[] datas = TextUtility.Split(arrayData, '/');
                BytesWriter.WriteInt16((short)datas.Length, stream);

                for (int k = 0; k < datas.Length; k++)
                {
                    WriteSingleVariable(elementType, datas[k], stream);
                }
            }

            public static void WriteSingleVariable(string rowType, string dataStr, Stream stream)
            {
                switch (rowType)
                {
                    case "int":
                        BytesWriter.WriteInt32(int.Parse(dataStr), stream);
                        break;
                    case "float":
                        BytesWriter.WriteFloat(float.Parse(dataStr), stream);
                        break;
                    case "bool":
                        BytesWriter.WriteBoolean(bool.Parse(dataStr), stream);
                        break;
                    case "string":
                        BytesWriter.WriteString(dataStr, stream);
                        break;
                    case "double":
                        BytesWriter.WriteDouble(double.Parse(dataStr), stream);
                        break;
                    case "long":
                        BytesWriter.WriteInt64(long.Parse(dataStr), stream);
                        break;
                    case "short":
                        BytesWriter.WriteInt16(short.Parse(dataStr), stream);
                        break;
                    case string type when type.Contains("E_"):
                        BytesWriter.WriteInt32(int.Parse(dataStr), stream);
                        break;
                }
            }
        }
    }
}
