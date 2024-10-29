using System.Text;

namespace ZincFramework
{
    namespace ScriptWriter
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

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    if (!string.IsNullOrEmpty(Namespaces))
                    {
                        stringBuilder.AppendLine($"namespace {Namespaces}");
                        stringBuilder.AppendLine("{");
                    }

                    AttributeHandle?.HandleWrite(stringBuilder);
                    if (Parents == null || Parents.Length == 0)
                    {
                        stringBuilder.InsertWriteLine(IndentSize, $"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers))} {ClassType} {ClassName}");
                        stringBuilder.InsertWriteLine(IndentSize, '{');
                    }
                    else
                    {
                        stringBuilder.InsertWriteLine(IndentSize, $"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers))} {ClassType} {ClassName} : {string.Join(", ", Parents)}");
                        stringBuilder.InsertWriteLine(IndentSize, '{');
                    }
                }
            }
        }
    }
}