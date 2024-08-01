using System.Text;
using System;


namespace ZincFramework
{
    namespace Binary
    {
        public static class ByteUtility
        {
            public static int GetStringLength(string str, int type = 0) => type switch
            {
                0 => Encoding.UTF8.GetByteCount(str) + 4,
                1 => Encoding.ASCII.GetByteCount(str) + 4,
                2 => Encoding.Unicode.GetByteCount(str) + 4,
                _ => 0,
            };

            public static int GetStringLength(char[] chars, int type = 0) => type switch
            {
                0 => Encoding.UTF8.GetByteCount(chars) + 4,
                1 => Encoding.ASCII.GetByteCount(chars) + 4,
                2 => Encoding.Unicode.GetByteCount(chars) + 4,
                _ => 0,
            };

            public static int GetCharLength(char c, int type = 0)
            {
                ReadOnlySpan<char> span = stackalloc char[1] { c };
                return type switch
                {
                    0 => Encoding.UTF8.GetByteCount(span),
                    1 => Encoding.ASCII.GetByteCount(span),
                    2 => Encoding.Unicode.GetByteCount(span),
                    _ => 0,
                };
            }

            public static int GetPrimitiveLength(Type type) => type switch
            {
                Type when type == typeof(int) => 4,
                Type when type == typeof(float) => 4,
                Type when type == typeof(bool) => 1,
                Type when type == typeof(long) => 8,
                Type when type == typeof(short) => 2,
                Type when type == typeof(double) => 8,
                Type when type == typeof(byte) => 1,
                Type when type == typeof(sbyte) => 1,
                Type when type == typeof(char) => sizeof(char),
                Type when type == typeof(ushort) => 2,
                Type when type == typeof(uint) => 4,
                Type when type == typeof(ulong) => 8,
                _ => throw new ArgumentException("你传入的不是一个基础数据类型"),
            };

            public static int GetPrimitiveLength(object obj) => obj switch
            {
                int or float or uint  => 4,
                bool or byte or sbyte => 1,
                double or long or ulong => 8,
                short or ushort => 2,
                char character => GetCharLength(character),
                _ => throw new ArgumentException("你传入的不是一个基础数据类型"),
            };
        }
    }
}