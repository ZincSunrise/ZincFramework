using System.Collections.Generic;
using System.Text;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace ScriptWriter
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


                public void HandleWrite(StringBuilder stringBuilder)
                {
                    stringBuilder.InsertWriteLine(IndentSize, $"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(", ", Modifiers))} {ReturnType} {MethodName}({(Arguments == null ? string.Empty : string.Join(", ", Arguments))})");
                    stringBuilder.InsertWriteLine(IndentSize, '{');

                    if(MethodStatement != null)
                    {
                        foreach (string statement in MethodStatement)
                        {
                            stringBuilder.InsertWriteLine(IndentSize + 1, statement);
                        }
                    }

                    WriteEvent?.Invoke();

                    stringBuilder.InsertWriteLine(IndentSize, '}');
                }
            }
        }
    }
}