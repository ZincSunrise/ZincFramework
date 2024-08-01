

namespace ZincFramework
{
    namespace Writer
    {
        namespace Handle
        {
            public interface IFieldHandle : IWriteHandle
            {
                public string FieldName { get; }

                public string FieldType { get; }

                public string Access { get; }

                public string[] Modifiers { get; }

                public bool IsInitialize { get; }
            }
        }
    }
}