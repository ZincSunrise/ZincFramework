using System.Text;


namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public readonly struct ArrayHandle : IFieldHandle
            {
                public int IndentSize { get; }

                public string Access { get; }

                public string ElementType { get; }

                public string FieldName { get; }

                public string FieldType => ElementType;

                public string[] Modifiers { get; }

                public string DefaultCapacity { get; }

                public bool IsInitialize => DefaultCapacity != null;

                public ArrayHandle(int indentSize, string access, string[] modifiers, string fieldName, string elementType)
                {
                    Access = access;
                    Modifiers = modifiers;
                    IndentSize = indentSize;
                    FieldName = fieldName;
                    ElementType = elementType;
                    DefaultCapacity = null;
                }

                public ArrayHandle(int indentSize, string access, string[] modifiers, string fieldName, string elementType, string defaultCapacity)
                {
                    Access = access;
                    Modifiers = modifiers;
                    IndentSize = indentSize;
                    FieldName = fieldName;
                    ElementType = elementType;
                    DefaultCapacity = defaultCapacity;
                }

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    stringBuilder.InsertWriteLine(IndentSize, $"{Access} {ElementType}[] {FieldName}{(IsInitialize ? $"= new {ElementType}[{DefaultCapacity}]" : string.Empty)};");
                }
            }
        }
    }
}