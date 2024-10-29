using System.Text;



namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public readonly struct NamespaceHandle : IWriteHandle
            {
                public int IndentSize => 0;

                public string[] Namespaces { get; }

                public int SpaceCount { get; }

                public NamespaceHandle(string[] namespaces, int spaceCount)
                {
                    Namespaces = namespaces;
                    SpaceCount = spaceCount;
                }

                public void HandleWrite(StringBuilder stringBuilder)
                {
                    foreach (var str in Namespaces)
                    {
                        stringBuilder.AppendLine($"using {str};");
                    }

                    for (int i = 0; i < SpaceCount; i++)
                    {
                        stringBuilder.AppendLine();
                    }
                }
            }
        }
    }
}
