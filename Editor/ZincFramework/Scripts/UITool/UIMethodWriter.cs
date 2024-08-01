using System;


namespace ZincFramework
{
    namespace UI
    {
        public static class UIMethodWriter
        {
            private readonly static string _originString = "{0} = transform.Find(\"{1}\").GetComponent<{2}>();";

            public static string GetMethodStatement(ReadOnlySpan<char> fieldName, string typeName)
            {
                return string.Format(_originString, new string(fieldName), new string(fieldName[1..]), typeName);
            }
        } 
    }
}