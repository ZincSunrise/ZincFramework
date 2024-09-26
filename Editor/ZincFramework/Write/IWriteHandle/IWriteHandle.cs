using System.IO;
using System.Text;



namespace ZincFramework
{
    namespace Writer
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

                protected static StringBuilder WriteHelper { get; set; } = new StringBuilder(64);

                void HandleWrite(StreamWriter streamWriter);
            }
        }
    }
}
