using System;
using System.Runtime.InteropServices;
using ZincFramework.Serialization;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.Binary.Serialization.ProfileWriters 
{
    public class VisibleStateWriter : BasicWriter<VisibleState>
    {
        public override bool CanWrite(string typeStr) => typeStr.Contains("VisibleState");

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            string[] strs = str.Split(',');

            int visableId = int.Parse(strs[0]);
            int differential = int.Parse(strs[1]);
            bool isFocus = bool.Parse(strs[2]);
            ReadOnlySpan<char> chars = strs[3].AsSpan()[1..^1];

            int index = chars.IndexOf(':');
            float x = float.Parse(chars[..index]);
            float y = float.Parse(chars[(index + 1)..]);
            VisibleState visibleState = new VisibleState(visableId, differential, isFocus, new UnityEngine.Vector2(x, y));

            Span<byte> bytes = stackalloc byte[Marshal.SizeOf<VisibleState>()];
            MemoryMarshal.Write(bytes, ref visibleState);
            byteWriter.WriteBytes(bytes);
        }

        public override string GetExcelString(object obj)
        {
            VisibleState visibleState = (VisibleState)obj;
            return $"{visibleState.VisableId},{visibleState.Differential},{visibleState.IsFocus},({visibleState.Position.x}:{visibleState.Position.y})";
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            base.WriteReadMethod(ref methodWriter);
            methodWriter.WritePrimitiveInList("Blittable");
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            base.WriteWriteMethod(ref methodWriter);
            methodWriter.ReadSimpleInList($"byteReader.ReadBlittable<{typeof(VisibleState).FullName}>()");
        }
    }
}
