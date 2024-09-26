using System;
using ZincFramework.Binary.Serialization;


namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelFieldProfileWriter
        {
            public static void WriteField(ByteWriter byteWriter, string fieldType, string code, string data)
            {
                byteWriter.WriteInt32(int.Parse(code.ToString()));

                switch (fieldType)
                {
                    case "int":
                        byteWriter.WriteInt32(int.TryParse(data, out var intResult) ? intResult : 0);
                        break;
                    case "float":
                        byteWriter.WriteSingle(float.TryParse(data, out var floatResult) ? floatResult : 0);
                        break;
                    case "bool":
                        byteWriter.WriteBoolean(bool.TryParse(data, out var boolResult) && boolResult);
                        break;
                    case "string":
                        byteWriter.WriteString(data);
                        break;
                    case "double":
                        byteWriter.WriteDouble(double.Parse(data));
                        break;
                    case "long":
                        byteWriter.WriteInt64(long.Parse(data));
                        break;
                    case "short":
                        byteWriter.WriteInt16(short.Parse(data));
                        break;
                    case string type when type.Contains("Array", StringComparison.OrdinalIgnoreCase):
                        WriteArray(byteWriter, type, data);
                        break;
                    case string type when (type.IndexOf("E_") != -1):
                        byteWriter.WriteInt32(int.Parse(data));
                        break;
                }
            }

            public static void WriteArray(ByteWriter byteWriter, string type, string arrayData)
            {
                string[] strs = TextUtility.Split(type, '/');
                string elementType = string.Empty;

                elementType = Array.Find(strs, x => !"Array".Equals(x, StringComparison.OrdinalIgnoreCase));

                if (arrayData == string.Empty)
                {
                    byteWriter.WriteInt32(0);
                    return;
                }

                string[] datas = TextUtility.Split(arrayData, '/');
                byteWriter.WriteInt32(datas.Length);

                for (int k = 0; k < datas.Length; k++)
                {
                    WriteSingleVariable(byteWriter, elementType, datas[k]);
                }
            }

            public static void WriteSingleVariable(ByteWriter byteWriter, string rowType, string dataStr)
            {
                switch (rowType)
                {
                    case "int":
                        byteWriter.WriteInt32(int.Parse(dataStr));
                        break;
                    case "float":
                        byteWriter.WriteSingle(float.Parse(dataStr));
                        break;
                    case "bool":
                        byteWriter.WriteBoolean(bool.Parse(dataStr));
                        break;
                    case "string":
                        byteWriter.WriteString(dataStr);
                        break;
                    case "double":
                        byteWriter.WriteDouble(double.Parse(dataStr));
                        break;
                    case "long":
                        byteWriter.WriteInt64(long.Parse(dataStr));
                        break;
                    case "short":
                        byteWriter.WriteInt16(short.Parse(dataStr));
                        break;
                    case string type when type.Contains("E_"):
                        byteWriter.WriteInt32(int.Parse(dataStr));
                        break;
                }
            }
        }
    }
}
