using System;
using System.Collections.Generic;
using ZincFramework.Writer;


namespace ZincFramework
{
    namespace Serialization
    {
        public static class SerializeWriteTool
        {
            public static string NullLengthStr => "bytesLength += 8;";

            public static string ConvertMemberCodeStr => "code = ByteConverter.ToInt32(bytes, ref nowIndex);";

            public static string ConvertCollectionLengthStr => $"count = ByteConverter.ToInt16(bytes, ref nowIndex);";


            public static string ConvertNullCondition => $"if(code != int.MinValue)";


            public static string OridinalLengthStr => $"bytesLength += 4;";


            public static string AppendNullCode => "ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);";

            public static string GetCodeStr => $"code = ByteConverter.ToInt32(bytes, ref nowIndex);";

            public static string Else => "else";

            private readonly static string[] _collectionName = new string[]
            {
                    "array",
                    "list",
                    "dictionary",
                    "hashSet",
                    "queue",
                    "stack",
                    "linkedList",
            };

            public static string AppendLength(string name, string LengthType)
            {
                return $"ByteAppender.AppendInt16((short){name}.{LengthType}, bytes, ref nowIndex);";
            }

            public static string GetSingleTypeLength(string name, string elementType, string lengthType)
            {
                var number = elementType switch
                {
                    "int" or "float" or "uint" => '4',
                    "long" or "ulong" or "double" => '8',
                    "bool" or "byte" or "sbyte" => '1',
                    string when elementType.Contains("E_") => '4',
                    _ => '4',
                };

                return $"bytesLength += {name}.{lengthType} * {number};";
            }

            public static bool IsCollection(string type)
            {
                for (int i = 0; i < _collectionName.Length; i++)
                {
                    if (string.Compare(_collectionName[i], type, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }

            public static string AppendNullCondition(string name)
            {
                return $"if({name} != null)";
            }

            public static string GetAppendCode(string name)
            {
                return $"ByteAppender.AppendInt32(_codeMap[nameof({name})], bytes, ref nowIndex);";
            }

            public static string ArrayLoopHead(string containerName, string lengthName)
            {
                return LoopHead($"{containerName}.{lengthName}");
            }

            public static string LoopHead(string lengthName)
            {
                return $"for(int i = 0;i < {lengthName}; i++)";
            }

            public static string ForeachHead(string containerName, string itemName)
            {
                return $"foreach(var {containerName} in {itemName})";
            }
        }
    }
}
