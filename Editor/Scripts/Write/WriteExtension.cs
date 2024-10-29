using System.Text;


namespace ZincFramework.ScriptWriter
{
    public static class WriteExtension
    {
        public static void InsertWriteLine(this StringBuilder stringBuilder, int tableCount, string str)
        {
            stringBuilder.Append('\t', tableCount);
            stringBuilder.AppendLine(str);
        }


        public static void InsertWriteLine(this StringBuilder stringBuilder, int tableCount, char c)
        {
            stringBuilder.Append('\t', tableCount);
            stringBuilder.Append(c);
            stringBuilder.AppendLine();
        }
    }
}
