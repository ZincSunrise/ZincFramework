using System.Collections.Generic;
using System.IO;


namespace ZincFramework
{
    namespace Writer
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

                public void HandleWrite(StreamWriter streamWriter)
                {
                    IFieldHandle.WriteHelper.Append
                    ($"{Access} {Type}({(Arguments == null ? string.Empty : string.Join(", ", Arguments))})");

                    streamWriter.WriteLine(WriteUtility.InsertTable(IFieldHandle.WriteHelper.ToString(), IndentSize));
                    IFieldHandle.WriteHelper.Clear();

                    streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));

                    if (MethodStatement != null)
                    {
                        foreach (string statement in MethodStatement)
                        {
                            streamWriter.WriteLine(WriteUtility.InsertTable(statement, IndentSize + 1));
                        }
                    }

                    streamWriter.WriteLine(WriteUtility.InsertTable('}', IndentSize));
                }
            }
        }
    }
}

