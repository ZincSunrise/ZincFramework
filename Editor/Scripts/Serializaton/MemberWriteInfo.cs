using System;

namespace ZincFramework
{
    namespace Serialization
    {
        public class MemberWriteInfo
        {
            public bool IsWriteMember => !string.IsNullOrEmpty(OrdinalNumber);

            public string OrdinalNumber { get; }

            public string Name { get; }

            public string Type { get; }

            public bool IsArray { get; }

            public bool IsEnum { get; }

            public string[] GenericTypes { get; }

            public int IndentSize { get; set; }


            /// <summary>
            /// ÓÃÓÚExcelµÄWriteInfo
            /// </summary>
            /// <param name="indentSize"></param>
            /// <param name="name"></param>
            /// <param name="type"></param>
            /// <param name="ordinalNumber"></param>
            /// <param name="isProperty"></param>
            public MemberWriteInfo(int indentSize, string name, string type, string ordinalNumber, bool isProperty)
            {
                IndentSize = indentSize;
                Name = isProperty ? TextUtility.UpperFirstChar(name) : name;
                IsArray = type.Contains("Array", StringComparison.OrdinalIgnoreCase);
                IsEnum = type.Contains("E_") && !IsArray;

                Type = IsArray ? type.Replace("Array/", "", StringComparison.OrdinalIgnoreCase) : type;
                GenericTypes = null;
                OrdinalNumber = ordinalNumber;
            }

            public MemberWriteInfo(int indentSize, string name, string type)
            {
                IndentSize = indentSize;
                Name = name;
                Type = type;
                IsEnum = type.Contains("E_");

                GenericTypes = null;
                IsArray = false;
                OrdinalNumber = string.Empty;
            }


            public void Deconstruct(out int indentSize, out string name, out string type)
            {
                indentSize = IndentSize;
                name = Name;
                type = Type;
            }
        }
    }
}