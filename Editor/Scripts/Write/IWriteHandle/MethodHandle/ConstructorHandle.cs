using System.Collections.Generic;
using System.Text;


namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public readonly struct ConstructorHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string Type { get; }


                public string[] Arguments { get; }

                public string Access { get; }

                public IEnumerable<string> MethodStatement { get; }

                public ConstructorHandle(int indentSize, string type, string[] arguments, string access, IEnumerable<string> methodStatement)
                {
                    IndentSize = indentSize;
                    Type = type;
                    Arguments = arguments;
                    Access = access;
                    MethodStatement = methodStatement;
                }

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    stringBuilder.InsertWriteLine(IndentSize, $"{Access} {Type}({(Arguments == null ? string.Empty : string.Join(", ", Arguments))})");

                    stringBuilder.InsertWriteLine(IndentSize, '{');

                    if (MethodStatement != null)
                    {
                        foreach (string statement in MethodStatement)
                        {
                            stringBuilder.InsertWriteLine(IndentSize + 1, statement);
                        }
                    }

                    stringBuilder.InsertWriteLine(IndentSize, '}');
                }
            }
        }
    }
}

