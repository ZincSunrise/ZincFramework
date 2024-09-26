using System.Collections.Generic;
using ZincFramework.Writer;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace TypeWriter
        {
            public static class SerializableClassWriter
            {
                private readonly static HashSet<string> _ordinals = new HashSet<string>();

                public static void CreateSerializeClass(CSharpWriter classWriter, string className, string serializableCode, MemberWriteInfo[] fieldWriteInfos, bool isWriteSerializableCode)
                {
                    classWriter.WriteAttribute(1, "ZincSerializable", serializableCode);
                    classWriter.BeginWriteClass(1, "", null, className, parents: new string[] { "ISerializable" });

                    if (isWriteSerializableCode)
                    {
                        classWriter.WriteAttribute(2, nameof(BinaryIgnore));
                        classWriter.WriteQuickProperty(2, "int", "SerializableCode", serializableCode);
                    }

                    WriteMaps(fieldWriteInfos, isWriteSerializableCode, className, classWriter);
                    WriteAllFields(fieldWriteInfos, classWriter);
                }


                public static void WriteMaps(MemberWriteInfo[] fieldWriteInfos, bool isWriteSerializableCode, string classType, CSharpWriter classWriter)
                {
                    List<string> codeMap = new List<string>()
                    {
                        $"_codeMap = new ({fieldWriteInfos.Length})",
                        "{"
                    };

                    string code;
                    for (int i = 0; i < fieldWriteInfos.Length; i++)
                    {
                        code = fieldWriteInfos[i].OrdinalNumber;
                        if (_ordinals.Contains(code))
                        {
                            _ordinals.Clear();
                            throw new System.ArgumentException("不能够填入相同的序列化序列号" + code);
                        }

                        codeMap.Add(WriteUtility.InsertTable($"{{nameof({fieldWriteInfos[i].Name}), {code}}},", 1));
                        _ordinals.Add(code);
                    }

                    codeMap.Add("};");
                    codeMap.Add(string.Empty);
                    codeMap.Add(string.Empty);

                    _ordinals.Clear();

                    List<string> setMap = new List<string>()
                    {
                        $"PropertyInfo[] propertyInfos = typeof({classType}).GetProperties();",
                        $"PropertyInfo propertyInfo;",
                        $"int code;",
                        $"_setMap = new ({fieldWriteInfos.Length});",
                        string.Empty,
                    };

                    for (int i = 0; i < fieldWriteInfos.Length; i++)
                    {
                        string type = fieldWriteInfos[i].IsArray ? fieldWriteInfos[i].Type + "[]" : fieldWriteInfos[i].Type;

                        if (!fieldWriteInfos[i].IsArray && fieldWriteInfos[i].GenericTypes != null)
                        {
                            type = $"{type}<{string.Join(", ", fieldWriteInfos[i].GenericTypes)}>";
                        }

                        setMap.Add($"propertyInfo = propertyInfos[{(isWriteSerializableCode ? i + 1 : i)}];");
                        setMap.Add($"code = _codeMap[propertyInfo.Name];");
                        setMap.Add($"_setMap.Add(code, new SetAction<{classType}, {type}>(SerializationUtility.GetSetAction<{classType}, {type}>(propertyInfo)));");
                        setMap.Add(string.Empty);
                    }
                    codeMap.AddRange(setMap);

                    classWriter.WriteLine();
                    classWriter.WriteLine();
                    classWriter.WriteConstructor(2, classType, codeMap, "static");
                    classWriter.WriteLine();

                    classWriter.WriteGenericField(2, "_codeMap", "Dictionary", new string[] { "string", "int" }, CSharpWriter.Accessors.Private, CSharpWriter.Modifiers.ReadOnlyStatic, false);
                    classWriter.WriteGenericField(2, "_setMap", "Dictionary", new string[] { "int", "SetActionBase" }, CSharpWriter.Accessors.Private, CSharpWriter.Modifiers.ReadOnlyStatic, false);
                    classWriter.WriteLine();
                    classWriter.WriteLine();
                }


                public static void WriteAllFields(MemberWriteInfo[] memberWriteInfos, CSharpWriter classWriter)
                {
                    string type;
                    string code;

                    for (int i = 0; i < memberWriteInfos.Length; i++)
                    {           
                        code = memberWriteInfos[i].OrdinalNumber;
                        classWriter.WriteAttribute(2, "BinaryOrdinal", code);
                        type = memberWriteInfos[i].Type;

                        if (memberWriteInfos[i].IsArray)
                        {
                            classWriter.WriteAutoProperty(2, type + "[]", memberWriteInfos[i].Name, CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Public);
                        }
                        else if (memberWriteInfos[i].GenericTypes != null)
                        {
                            type = $"{type}<{string.Join(", ", memberWriteInfos[i].GenericTypes)}>";
                            classWriter.WriteAutoProperty(2, type, memberWriteInfos[i].Name, CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Public);
                        }
                        else
                        {
                            classWriter.WriteAutoProperty(2, type, memberWriteInfos[i].Name, CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Public);
                        }

                        classWriter.WriteLine();
                    }
                }
            }
        }
    }
}
