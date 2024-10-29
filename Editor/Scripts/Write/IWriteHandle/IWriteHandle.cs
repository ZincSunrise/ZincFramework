using System.IO;
using System.Text;



namespace ZincFramework
{
    namespace ScriptWriter
    {
        namespace Handle
        {
            public interface IWriteHandle
            {
                static IWriteHandle()
                {
                    string.Intern(" ,");
                }

                int IndentSize { get; }

                void HandleWrite(StringBuilder stringBuilder);
            }
        }
    }
}
