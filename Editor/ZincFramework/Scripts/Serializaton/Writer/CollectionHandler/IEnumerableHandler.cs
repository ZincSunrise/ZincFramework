using System;
using ZincFramework.Writer;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public interface IEnumerableHandler
            {
                string Method { get; }

                string Type { get;}

                int RelativeIndentSize { get; }

                void GetConvert(string name, string[] statement, ref int index, params string[] genericType);

                void GetAppend(string name, string[] statement, ref int index, params string[] genericType);

                void GetLength(string name, string[] statement, ref int index, params string[] genericType);


                public static IEnumerableHandler GetEnumerableHandler(string classType, string type, int indentSize) => type switch
                {
                    "linkedList" or "list" or "queue" or "hashSet" or "stack" => new CollecionHandler(GetMethod(type), classType, TextUtility.UpperFirstString(type), indentSize),
                    "array" => new ArrayHandler(GetMethod(type), classType, TextUtility.UpperFirstString(type), indentSize),
                    "dictionary" => new DictionaryHandler(GetMethod(type), classType, TextUtility.UpperFirstString(type), indentSize),
                    _ => throw new NotSupportedException(type + "该类不受支持！")
                };

                private static string GetMethod(string type) => type switch
                {
                    string when string.Compare(type, "list", true) == 0 => "Add",
                    string when string.Compare(type, "linkedList", true) == 0 => "Add",
                    string when string.Compare(type, "hashSet", true) == 0 => "Add",
                    string when string.Compare(type, "stack", true) == 0 => "Push",
                    string when string.Compare(type, "queue", true) == 0 => "Enqueue",
                    string when string.Compare(type, "dictionary", true) == 0 => "Add",
                    string when string.Compare(type, "array" , true) == 0 => "SetValue",
                    _ => "Add",
                };
            }
        }
    }
}
