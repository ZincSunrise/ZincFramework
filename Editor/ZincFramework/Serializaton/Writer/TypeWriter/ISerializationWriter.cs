using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace TypeWriter
        {
            public interface ISerializationWriter
            {
                static ISerializationWriter()
                {
                    if (string.IsNullOrEmpty(string.IsInterned("{")))
                    {
                        string.Intern("{");
                    }
                    if (string.IsNullOrEmpty(string.IsInterned("}")))
                    {
                        string.Intern("}");
                    }
                }


                string Type { get; }

                string ClassType { get; }

                int RelativeIndentSize { get; }


                ArraySegment<string> GetAppendState(string name, params string[] genericTypes);

                ArraySegment<string> GetConvertState(string name, params string[] genericTypes);

                ArraySegment<string> GetLengthState(string name, params string[] genericTypes);


                public static ISerializationWriter GetSerializationWriter(string classType, string type, int relativeIndentSize) => type switch
                {
                    string when SingleTypeWriter.IsSingleValue(type) => new SingleTypeWriter(classType, type, relativeIndentSize),
                    _ => new ReferenceWriter(classType, type, relativeIndentSize),
                };
            }
        }
    }
}

