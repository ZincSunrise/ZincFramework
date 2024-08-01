using System.IO;


namespace ZincFramework
{
    namespace Writer
    {
        namespace Handle
        {
            public readonly struct AttributeHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string AttributeType { get; }

                public string[] Arguments { get; }

                public AttributeHandle(int indentSize, string attributeType, string[] arguments)
                {
                    IndentSize = indentSize;
                    AttributeType = attributeType;
                    Arguments = arguments;
                }

                public AttributeHandle(IWriteHandle writeHandle, string attributeType, string[] arguments)
                {
                    IndentSize = writeHandle.IndentSize;
                    AttributeType = attributeType;
                    Arguments = arguments;
                }

                public void HandleWrite(StreamWriter streamWriter)
                {
                    string attributeStr;

                    if (Arguments != null && Arguments.Length != 0)
                    {
                        attributeStr = $"[{AttributeType}({string.Join(" ,", Arguments)})]";
                    }
                    else
                    {
                        attributeStr = $"[{AttributeType}]";
                    }
                    

                    IFieldHandle.WriteHelper.Append(attributeStr);
                    streamWriter.WriteLine(WriteUtility.InsertTable(IFieldHandle.WriteHelper.ToString(), IndentSize));
                    IFieldHandle.WriteHelper.Clear();
                }
            }
        }
    }
}
