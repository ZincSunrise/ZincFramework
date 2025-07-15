using System;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.ScriptWriter;



namespace ZincFramework.Excel
{
    public static class ExcelMethodCreator
    {
        public static void WriteAllMethod(string className, CSharpWriter classWriter, List<MemberWriteInfo> memberWriteInfos)
        {
            CreateWriteMethod(className, classWriter, memberWriteInfos);
            CreateReadMethod(className, classWriter, memberWriteInfos);
        }

        private static void CreateWriteMethod(string className, CSharpWriter classWriter, List<MemberWriteInfo> memberWriteInfos)
        {
            AutoWriteConfig autoWriteConfig = ExcelModel.Instance.ExcelDefault;
            MethodWriter methodWriter = new MethodWriter(className, new List<string>());

            for (int i = 0; i < memberWriteInfos.Count; i++)
            {
                methodWriter.PushToWriteWrite(memberWriteInfos[i]);
            }

            classWriter.WriteMethod(2, "Write", "void", null, new[] { "ByteWriter byteWriter, SerializerOption serializerOption" }, methodWriter.MethodStates);
        }

        private static void CreateReadMethod(string className, CSharpWriter classWriter, List<MemberWriteInfo> memberWriteInfos)
        {
            List<string> statement = new List<string>
                {
                    "int code;" + Environment.NewLine,
                };

            AutoWriteConfig autoWriteConfig = ExcelModel.Instance.ExcelDefault;
            MethodWriter methodWriter = new MethodWriter(className, statement);

            for (int i = 0; i < memberWriteInfos.Count; i++)
            {
                methodWriter.PushToWriteRead(memberWriteInfos[i]);
            }

            classWriter.WriteMethod(2, "Read", "void", null, new[] { "ref ByteReader byteReader, SerializerOption serializerOption" }, statement);
        }
    }
}
