using System;
using UnityEngine;
using ZincFramework.Serialization;

namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class Vector2Writer : BasicWriter<Vector2>
    {
        public override bool CanWrite(string typeStr) => typeStr == "Vector2";

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            ReadOnlySpan<char> chars = str;
            byteWriter.WriteInt32(8);

            Span<char> trim = stackalloc char[] { ',', '/' , ' '};

            for (int i = 0; i < chars.Length; i++) 
            {
                if (chars[i] == '/' || chars[i] == ',')
                {
                    byteWriter.WriteSingle(float.TryParse(chars[..i].Trim(trim), out var result1) ? result1 : 0);
                    byteWriter.WriteSingle(float.TryParse(chars[i..].Trim(trim), out var result2) ? result2 : 0);
                    break;
                }
            }
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            base.WriteWriteMethod(ref methodWriter);
            methodWriter.WritePrimitiveInList("Blittable");
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            base.WriteReadMethod(ref methodWriter);
            methodWriter.ReadSimpleInList("byteReader.ReadBlittable<UnityEngine.Vector2>()");
        }

        public override string GetExcelString(object obj)
        {
            return obj.ToString()[1..^1];
        }
    }
}