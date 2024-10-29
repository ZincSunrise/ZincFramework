using System;
using System.Text;
using ZincFramework.Binary.Serialization;


namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public readonly struct FieldHandle : IFieldHandle
            {
                public int IndentSize { get; }

                public string Access { get; }

                public string FieldName { get; }

                public string FieldType { get; }

                public string[] Modifiers { get; }

                public bool IsInitialize { get; }


                public FieldHandle(int indentSize, string access, string name, string fieldType, string[] modifiers, bool isInitalized)
                {
                    IndentSize = indentSize;
                    Access = access;
                    FieldName = name;
                    FieldType = fieldType;
                    Modifiers = modifiers;
                    IsInitialize = isInitalized;
                }

                public FieldHandle(int indentSize, string access, ReadOnlySpan<char> name, string fieldType, string[] modifiers, bool isInitalized)
                {
                    IndentSize = indentSize;
                    Access = access;
                    FieldName = new string(name);
                    FieldType = fieldType;
                    Modifiers = modifiers;
                    IsInitialize = isInitalized;
                }

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    string writeStr;
                    if(Modifiers == null)
                    {
                        writeStr = $"{Access} {FieldType} {FieldName};";
                    }
                    else
                    {
                        writeStr = $"{Access}{' ' + string.Join(' ', Modifiers)} {FieldType} {FieldName};";
                    }

                    if (IsInitialize && !WriterFactory.IsSimpleValue(FieldType))
                    {
                        stringBuilder.InsertWriteLine(IndentSize, writeStr.Replace(";", " = new ();"));
                    }
                    else
                    {
                        stringBuilder.InsertWriteLine(IndentSize, writeStr);
                    }
                }
            }
        }
    }
}