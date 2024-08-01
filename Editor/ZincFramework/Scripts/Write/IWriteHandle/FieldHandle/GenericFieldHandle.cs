using System.IO;



namespace ZincFramework
{
    namespace Writer
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

                public void HandleWrite(StreamWriter streamWriter)
                {
                    IFieldHandle.WriteHelper.Append($"{FieldType}<{string.Join(" ,", GenericTypes)}>");

                    string modifier = Modifiers == null || Modifiers.Length == 0 ? string.Empty : ' ' + string.Join(' ', Modifiers);

                    string fieldString = $"{Access}{modifier} {IFieldHandle.WriteHelper} {FieldName}{(IsInitialize ? " = new ()" : string.Empty)}";

                    if(DefaultValues == null || DefaultValues.Length == 0)
                    {
                        streamWriter.WriteLine(WriteUtility.InsertTable(fieldString + ';', IndentSize));
                    }
                    else
                    {
                        streamWriter.WriteLine(WriteUtility.InsertTable(fieldString, IndentSize));
                        streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));
                        for (int i = 0;i < DefaultValues.Length; i++)
                        {
                            streamWriter.WriteLine(WriteUtility.InsertTable(DefaultValues[i] + ',', IndentSize + 1));
                        }
                        streamWriter.WriteLine(WriteUtility.InsertTable("};", IndentSize));
                    }

                    IFieldHandle.WriteHelper.Clear();
                }
            }
        }
    }
}