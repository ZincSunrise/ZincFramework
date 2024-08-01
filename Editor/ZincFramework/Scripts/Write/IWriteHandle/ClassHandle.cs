using System.IO;


namespace ZincFramework
{
    namespace Writer
    {
        namespace Handle
        {
            public readonly struct ClassHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string ClassName { get; }

                public string ClassType { get; }

                public string[] Parents { get; }

                public string Namespaces { get; }

                public string Access { get; }

                public string[] Modifiers { get; }

                public AttributeHandle? AttributeHandle { get; }

                public ClassHandle(int indentSize, string className, string[] modifiers, string classType, string[] parents, string namespaces, string access, AttributeHandle? attributeHandle)
                {
                    Modifiers = modifiers;
                    IndentSize = indentSize;
                    ClassName = className;
                    ClassType = classType;
                    Parents = parents;
                    Namespaces = namespaces;
                    Access = access;
                    AttributeHandle = attributeHandle;
                }

                public void HandleWrite(StreamWriter streamWriter)
                {
                    if (!string.IsNullOrEmpty(Namespaces))
                    {
                        streamWriter.WriteLine($"namespace {Namespaces}");
                        streamWriter.WriteLine('{');
                    }

                    AttributeHandle?.HandleWrite(streamWriter);
                    if (Parents == null || Parents.Length == 0)
                    {
                        streamWriter.WriteLine(WriteUtility.InsertTable($"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers))} {ClassType} {ClassName}", IndentSize));
                        streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));
                    }
                    else
                    {
                        streamWriter.WriteLine(WriteUtility.InsertTable($"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers))} {ClassType} {ClassName} : {string.Join(", ", Parents)}", IndentSize));
                        streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));
                    }
                }
            }
        }
    }
}