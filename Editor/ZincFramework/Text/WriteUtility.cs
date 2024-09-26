using System.Collections.Generic;
using System.Text;


namespace ZincFramework
{
    public static class WriteUtility
    {
        private readonly static StringBuilder _stringBuilder = new StringBuilder();

        public static string InsertTable(string str, int tableCount)
        {
            _stringBuilder.Append('\t', tableCount);
            _stringBuilder.Append(str);

            str = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return str;
        }

        public static string InsertTable(char character, int tableCount)
        {
            _stringBuilder.Append('\t', tableCount);
            _stringBuilder.Append(character);

            string str = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return str;
        }
    }
}

