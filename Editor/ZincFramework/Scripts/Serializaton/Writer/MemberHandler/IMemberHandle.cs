using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Serialization.TypeWriter;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Handlers
        {
            public interface IMemberHandle
            {
                string Type { get; }

                int IndentSize { get; }


                string[] GetAppend(string name);

                string[] GetConvert(string name);

                string[] GetLength(string name);

                public static IMemberHandle GetMemberHandle(int indentSize, string type) => type switch
                {
                    string when SingleTypeWriter.IsSingleValue(type) => new SingleHandler(indentSize, type),
                    _ => new ReferenceHandler(indentSize, type),
                };
            }
        }
    }
}