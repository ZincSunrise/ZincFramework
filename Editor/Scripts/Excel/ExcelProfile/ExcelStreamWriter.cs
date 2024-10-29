using System.IO;
using UnityEngine;
using ZincFramework.Binary.Serialization;
using ZincFramework.LoadServices;
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
                    ByteWriter bytesWriter = ByteWriterPool.GetCachedWriter(SerializerOption.Default, out var pooledBufferWriter);
                    WriteTableBytes(bytesWriter, excelSheet);
                    pooledBufferWriter.WriteInStream(fileStream);
                    fileStream.Close();

                    ByteWriterPool.ReturnWriterAndBuffer(bytesWriter, pooledBufferWriter);
                }
            }

            public static byte[] CreateBytes(ExcelSheet excelSheet)
            {
                ByteWriter bytesWriter = ByteWriterPool.GetCachedWriter(SerializerOption.Default, out var pooledBufferWriter);
                WriteTableBytes(bytesWriter, excelSheet);
                byte[] bytes = pooledBufferWriter.WrittenMemory.ToArray();

                ByteWriterPool.ReturnWriterAndBuffer(bytesWriter, pooledBufferWriter);
                return bytes;
            }


            private static void WriteTableBytes(ByteWriter byteWriter, ExcelSheet excelSheet)
            {
                int keyLine = ExcelReader.GetContainerKey(excelSheet);

                //先写容器中存储了多少元素
                byteWriter.WriteInt32(excelSheet.RowCount - ExcelTool.StartLine);

                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;
                string keyType = null;
                for (int i = ExcelTool.StartLine; i < excelSheet.RowCount; i++)
                {
                    if (keyLine != -1)
                    {
                        keyType ??= excelSheet[1, keyLine];
                        WriterFactory.Instance.GetWriter(keyType).WriteBinary(excelSheet[i, keyLine], byteWriter, SerializerOption.Default);
                    }

                    for (int j = 0; j < excelSheet.ColCount; j++)
                    {
                        string type = excelSheet[autoWriteConfig.typeLine, j];
                        string code = excelSheet[autoWriteConfig.numberLine, j];

                        if (string.IsNullOrEmpty(type) || type.Equals("ignore", System.StringComparison.OrdinalIgnoreCase) 
                            || string.IsNullOrEmpty(code))
                        {
                            Debug.LogWarning($"{excelSheet.TableName}的第{j}列为空,将不会记载数据");
                            continue;
                        }

                        //写入序列码
                        byteWriter.WriteInt32(int.Parse(code));
                        WriterFactory.Instance.GetWriter(type).WriteBinary(excelSheet[i, j], byteWriter, SerializerOption.Default);
                    }
                }
            }
        }
    }
}
