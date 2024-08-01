using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZincFramework
{
    namespace Writer
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

                public void HandleWrite(StreamWriter streamWriter)
                {
                    foreach (var str in Namespaces)
                    {
                        streamWriter.WriteLine($"using {str};");
                    }

                    for (int i = 0; i < SpaceCount; i++)
                    {
                        streamWriter.WriteLine();
                    }
                }
            }
        }
    }
}
