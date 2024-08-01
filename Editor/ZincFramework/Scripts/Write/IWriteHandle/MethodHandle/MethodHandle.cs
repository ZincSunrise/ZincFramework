using System.Collections.Generic;
using System.IO;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace Writer
    {
        namespace Handle
        {
            public readonly struct MethodHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string Access { get; }

                public string ReturnType { get; }

                public string MethodName { get; }

                public string[] Modifiers { get; }

                public string[] Arguments { get; }

                public IEnumerable<string> MethodStatement { get; }

                public ZincEvent WriteEvent { get; }


                public MethodHandle(int indentSize, string access, string returnType, string methodName, string[] modifiers, string[] arguments, IEnumerable<string> methodStatement, ZincAction writeCallback)
                {
                    IndentSize = indentSize;
                    Access = access;
                    ReturnType = returnType;
                    MethodName = methodName;
                    Modifiers = modifiers;
                    Arguments = arguments;
                    MethodStatement = methodStatement;
                    WriteEvent = new ZincEvent();
                    WriteEvent.AddListener(writeCallback);
                }


                public void HandleWrite(StreamWriter streamWriter)
                {
                    IFieldHandle.WriteHelper.Append
                        ($"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(", ", Modifiers))} {ReturnType} {MethodName}({(Arguments == null ? string.Empty : string.Join(", ", Arguments))})");

                    streamWriter.WriteLine(WriteUtility.InsertTable(IFieldHandle.WriteHelper.ToString(), IndentSize));
                    IFieldHandle.WriteHelper.Clear();

                    streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));

                    if(MethodStatement != null)
                    {
                        foreach (string statement in MethodStatement)
                        {
                            streamWriter.WriteLine(WriteUtility.InsertTable(statement, IndentSize + 1));
                        }
                    }

                    WriteEvent?.Invoke();

                    streamWriter.WriteLine(WriteUtility.InsertTable('}', IndentSize));
                }
            }
        }
    }
}