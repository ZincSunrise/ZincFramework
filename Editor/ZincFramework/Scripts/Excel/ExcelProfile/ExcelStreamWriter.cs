using System.Data;
using System.IO;
using UnityEngine;
using ZincFramework.Binary;
using ZincFramework.Load;
using ZincFramework.Serialization;


namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelStreamWriter
        {
            public static void CreateBinaryFile(ExcelSheet excelSheet)
            {
                if (!Directory.Exists(FrameworkPaths.ProfilePath))
                {
                    Directory.CreateDirectory(FrameworkPaths.ProfilePath);
                }

                string extension = ResourcesManager.Instance.Load<FrameworkData>("FrameWork/FrameWorkData").extension;

                using (FileStream fileStream = File.Open(FrameworkPaths.ProfilePath + '/' + $"{excelSheet.TableName}{extension}", FileMode.Create, FileAccess.Write))
                {
                    TableToStream(fileStream, excelSheet);
                    fileStream.Close();
                }
            }

            public static byte[] CreateBytes(ExcelSheet excelSheet)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    TableToStream(memoryStream, excelSheet);
                    return memoryStream.ToArray();
                }
            }

            private static void TableToStream(Stream stream, ExcelSheet excelSheet)
            {
                int keyLine = ExcelReader.GetContainerKey(excelSheet);

                //��д�����д洢�˶���Ԫ��
                BytesWriter.WriteInt16((short)(excelSheet.RowCount - ExcelTool.StartLine), stream);

                int count = excelSheet.ColCount;

                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;
                string keyName = null;
                for (int i = ExcelTool.StartLine; i < excelSheet.RowCount; i++)
                {
                    if (keyLine != -1)
                    {
                        keyName ??= excelSheet[1, keyLine];
                        ExcelFieldProfileWriter.WriteSingleVariable(keyName, excelSheet[i, keyLine], stream);
                    }

                    for (int j = 0; j < count; j++)
                    {
                        string type = excelSheet[autoWriteConfig.typeLine, j];
                        string code = excelSheet[autoWriteConfig.numberLine, j];

                        if (string.IsNullOrEmpty(type) 
                            || string.IsNullOrEmpty(code))
                        {
                            Debug.LogWarning($"{excelSheet.TableName}�ĵ�{j}��Ϊ��,�������������");
                            break;
                        }

                        string data = excelSheet[i, j];
                        ExcelFieldProfileWriter.WriteField(type, code, data, stream);
                    }
                }
            }
        }
    }
}
