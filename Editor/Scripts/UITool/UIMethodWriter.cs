using System;
using ZincFramework.UI.Collections;


namespace ZincFramework
{
    namespace UI
    {
        public static class UIMethodWriter
        {
            private readonly static string _originString = "{0} = transform.Find(\"{1}\").GetComponent<{2}>();";

            public static string GetMethodStatement(in UIWriteInfo uIWriteInfo)
            {
                return string.Format(_originString, uIWriteInfo.Name, uIWriteInfo.Path, uIWriteInfo.Type);
            }
        } 
    }
}