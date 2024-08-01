using System.Data;
using System.IO;
using ZincFramework.Binary;



namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelFieldProfileWriter
        {
            public static void WriteField(string fieldType, DataRow codeRow, DataRow dataRow, int index, FileStream fs)
            {
                ByteWriter.WriteInt32(int.Parse(codeRow[index].ToString()), fs);
                switch (fieldType)
                {
                    case "int":
                        ByteWriter.WriteInt32(int.Parse(dataRow[index].ToString()), fs);
                        break;
                    case "float":
                        ByteWriter.WriteFloat(float.Parse(dataRow[index].ToString()), fs);
                        break;
                    case "bool":
                        ByteWriter.WriteBoolean(bool.Parse(dataRow[index].ToString()), fs);
                        break;
                    case "string":
                        ByteWriter.WriteString(dataRow[index].ToString(), fs);
                        break;
                    case "double":
                        ByteWriter.WriteDouble(double.Parse(dataRow[index].ToString()), fs);
                        break;
                    case "long":
                        ByteWriter.WriteInt64(long.Parse(dataRow[index].ToString()), fs);
                        break;
                    case "short":
                        ByteWriter.WriteInt16(short.Parse(dataRow[index].ToString()), fs);
                        break;
                    case string type when (type.IndexOf("Array") != -1):
                        WriteArray(type, index, dataRow, fs);
                        break;
                    case string type when (type.IndexOf("E_") != -1):
                        ByteWriter.WriteInt32(int.Parse(dataRow[index].ToString()), fs);
                        break;
                }
            }

            public static void WriteArray(string type, int index, DataRow dataRow, FileStream fs)
            {
                string[] strs = TextUtility.Split(type, "/");

                string elementType = "";
                for (int k = 0; k < strs.Length; k++)
                {
                    if (strs[k] != "Array")
                    {
                        elementType = strs[k];
                        break;
                    }
                }

                if (dataRow[index].ToString() == "")
                {
                    ByteWriter.WriteInt16(0, fs);
                    return;
                }
                string[] datas = TextUtility.Split(dataRow[index].ToString(), "/");
                ByteWriter.WriteInt16((short)datas.Length, fs);
                for (int k = 0; k < datas.Length; k++)
                {
                    WriteSingleVariable(elementType, datas[k], fs);
                }
            }

            public static void WriteSingleVariable(string rowType, string dataStr, FileStream fs)
            {
                switch (rowType)
                {
                    case "int":
                        ByteWriter.WriteInt32(int.Parse(dataStr), fs);
                        break;
                    case "float":
                        ByteWriter.WriteFloat(float.Parse(dataStr), fs);
                        break;
                    case "bool":
                        ByteWriter.WriteBoolean(bool.Parse(dataStr), fs);
                        break;
                    case "string":
                        ByteWriter.WriteString(dataStr, fs);
                        break;
                    case "double":
                        ByteWriter.WriteDouble(double.Parse(dataStr), fs);
                        break;
                    case "long":
                        ByteWriter.WriteInt64(long.Parse(dataStr), fs);
                        break;
                    case "short":
                        ByteWriter.WriteInt16(short.Parse(dataStr), fs);
                        break;
                    case string type when (type.IndexOf("E_") != -1):
                        ByteWriter.WriteInt32(int.Parse(dataStr), fs);
                        break;
                }
            }
        }
    }
}
