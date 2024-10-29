using System;


namespace ZincFramework
{
    namespace Serialization
    {
        public static class SerializeWriteUtility
        {
            public static string NullLengthStr => "bytesLength += 8;";

            public static string ConvertMemberCodeStr => "code = byteReader.ReadInt32();";

            public static string ReadLength => $"int count = byteReader.ReadInt32();";


            public static string ReadNullCondition => $"if(code != int.MinValue)";


            public static string OridinalLengthStr => $"bytesLength += 4;";


            public static string WriteNullCode => "byteWriter.WriteInt32(int.MinValue);";

            public static string GetCodeStr => $"code = byteReader.ReadInt32();";

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

            public static string WriteLength(string name, string LengthType)
            {
                return $"byteWriter.WriteInt32({name}.{LengthType});";
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

            public static string WriteNullCondition(string name)
            {
                return $"if({name} != null)";
            }

            public static string GetWriteCode(string name)
            {
                return $"byteWriter.WriteInt32(_codeMap[nameof({name})]);";
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
