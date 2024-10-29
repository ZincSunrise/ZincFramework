using System.Text;


namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public readonly struct GenericFieldHandle : IFieldHandle
            {
                public int IndentSize { get; }

                public string Access { get; }

                public string FieldName { get; }

                public string FieldType { get; }

                public string[] GenericTypes { get; }

                public string[] DefaultValues { get; }

                public string[] Modifiers { get; }

                public bool IsInitialize { get; }

                public GenericFieldHandle(int indentSize, string access, string[] modifiers, string fieldType, string[] genericTypes, string fieldName, bool isInitalize, string[] defaultValues)
                {
                    IndentSize = indentSize;
                    GenericTypes = genericTypes;
                    FieldName = fieldName;
                    FieldType = fieldType;
                    Access = access;
                    DefaultValues = defaultValues;
                    Modifiers = modifiers;
                    IsInitialize = isInitalize;
                }

                public GenericFieldHandle(int indentSize, string access, string[] modifiers, string fieldName, string fieldType, string[] genericTypes)
                {
                    IndentSize = indentSize;
                    GenericTypes = genericTypes;
                    FieldName = fieldName;
                    FieldType = fieldType;
                    Access = access;
                    Modifiers = modifiers;

                    DefaultValues = null;
                    IsInitialize = false;
                }

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    string tail = $"{FieldType}<{string.Join(" ,", GenericTypes)}>";

                    string modifier = ArrayListUtility.IsNullOrEmpty(Modifiers) ? string.Empty : ' ' + string.Join(' ', Modifiers);

                    string fieldString = $"{Access}{modifier} {tail} {FieldName}{(IsInitialize ? " = new ()" : string.Empty)}";

                    if(DefaultValues == null || DefaultValues.Length == 0)
                    {
                        stringBuilder.InsertWriteLine(IndentSize, fieldString + ';');
                    }
                    else
                    {
                        stringBuilder.InsertWriteLine(IndentSize, fieldString);
                        stringBuilder.InsertWriteLine(IndentSize, '{');
                        for (int i = 0;i < DefaultValues.Length; i++)
                        {
                            stringBuilder.InsertWriteLine(IndentSize + 1, DefaultValues[i] + ',');
                        }

                        stringBuilder.InsertWriteLine(IndentSize, "};");
                    }
                }
            }
        }
    }
}