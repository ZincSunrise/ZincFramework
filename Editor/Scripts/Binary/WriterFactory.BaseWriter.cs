using System;
using System.Reflection;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.ProfileWriters;


namespace ZincFramework.Binary.Serialization
{
    public partial class WriterFactory
    {
        private Dictionary<string, StrProfileWriter> _baseProfileWriters;

        private void InitialDictionary()
        {
            _baseProfileWriters ??= new Dictionary<string, StrProfileWriter>(StringComparer.OrdinalIgnoreCase)
            {
                { "byte", new ProfileWriters.ByteWriter()},
                { "sbyte", new SByteWriter()},
                { "bool", new BooleanWriter()},
                { "short", new Int16Writer()},
                { "ushort", new UInt16Writer()},
                { "char", new CharWriter()},
                { "int", new Int32Writer()},
                { "uint", new UInt32Writer()},
                { "float", new SingleWriter()},
                { "long", new Int64Writer()},
                { "ulong", new UInt64Writer()},
                { "double", new DoubleWriter()},
                { "string", new StringWriter()},
                { "ignore", new IgnoreWriter()}
            };
        }

        public static bool IsSimpleValue(string type) => type switch
        {
            "int" or "uint" or "float" => true,
            "long" or "ulong" or "double" => true,
            "short" or "ushort" or "char" => true,
            "bool" or "byte" or "sbyte" => true,
            nameof(Int32) or nameof(UInt32) or nameof(Single) => true,
            nameof(Int64) or nameof(UInt64) or nameof(Double) => true,
            nameof(Int16) or nameof(UInt16) or nameof(Char) => true,
            nameof(Boolean) or nameof(SByte) or nameof(Byte) => true,
            _ => false,
        };

        public static bool IsBittable(Type type)
        {
            if(type != null)
            {
                if (!type.IsValueType)
                {
                    return false;
                }

                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

                FieldInfo[] fieldInfos = type.GetFields(bindingFlags);
                foreach (FieldInfo field in fieldInfos) 
                {
                    if (!field.FieldType.IsValueType)
                    {
                        return false;
                    }
                }


                PropertyInfo[] propertyInfos = type.GetProperties(bindingFlags);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (!propertyInfo.PropertyType.IsValueType)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public static string GetFullName(string type) => type switch
        {
            "int" => typeof(Int32).FullName,
            "uint" => typeof(UInt32).FullName,
            "float" => typeof(Single).FullName,
            "long" => typeof(Int64).FullName,
            "ulong" => typeof(UInt64).FullName,
            "double" => typeof(Double).FullName,
            "short" => typeof(Int16).FullName,
            "ushort" => typeof(UInt16).FullName,
            "char" => typeof(Char).FullName,
            "bool" => typeof(Boolean).FullName,
            "byte" => typeof(Byte).FullName,
            "sbyte" => typeof(SByte).FullName,
            _ => "找不到对应的基础类型名称"
        };
    }
}