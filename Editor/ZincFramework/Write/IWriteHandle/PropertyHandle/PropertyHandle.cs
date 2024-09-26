using System;
using System.IO;


namespace ZincFramework
{
    namespace Writer
    {
        namespace Handle
        {
            public readonly struct PropertyHandle : IWriteHandle
            {
                private enum E_Property_Type
                {
                    //普通模式
                    Normal,
                    //快速属性，指public int x => 1;这一种
                    Quick,
                    //自动属性public int x { get; set;}
                    Auto
                }

                public int IndentSize { get; }

                public string Access { get; }

                public string ReturnType { get; }

                public string PropertyName { get; }

                public string DefaultValue { get; }

                public GetterHandle Getter { get; }

                public SetterHandle Setter { get; }

                public string[] Modifiers { get; }

                private E_Property_Type PropertyType { get; }


                public PropertyHandle(int indentSize, string access, string[] modifiers, string returnType, string propertyName, string defaultValue)
                {
                    IndentSize = indentSize;
                    Access = access;
                    PropertyName = propertyName;
                    ReturnType = returnType;
                    DefaultValue = defaultValue;
                    PropertyType = E_Property_Type.Quick;

                    Modifiers = modifiers;
                    Getter = default;
                    Setter = default;
                }

                public PropertyHandle(int indentSize, string access, string[] modifiers, string returnType, string propertyName, GetterHandle getterHandle, SetterHandle setterHandle, string defaultValue)
                {
                    IndentSize = indentSize;
                    Access = access;
                    PropertyName = propertyName;
                    ReturnType = returnType;

                    Modifiers = modifiers;
                    PropertyType = E_Property_Type.Auto;
                    DefaultValue = defaultValue;

                    Getter = getterHandle;
                    Setter = setterHandle;
                }


                public PropertyHandle(int indentSize, string access, string[] modifiers, string returnType, string propertyName, string defaultValue, GetterHandle getterHandle, SetterHandle setterHandle)
                {
                    IndentSize = indentSize;
                    Access = access;
                    PropertyName = propertyName;
                    ReturnType = returnType;

                    Modifiers = modifiers;
                    PropertyType = E_Property_Type.Normal;
                    DefaultValue = defaultValue;

                    Getter = getterHandle;
                    Setter = setterHandle;
                }

                public void HandleWrite(StreamWriter streamWriter)
                {
                    switch (PropertyType) 
                    {
                        case E_Property_Type.Normal:
                            streamWriter.WriteLine(WriteUtility.InsertTable($"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers) )} {ReturnType} {PropertyName}", IndentSize));
                            streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));

                            Getter.HandleWrite(streamWriter);
                            Setter.HandleWrite(streamWriter);

                            streamWriter.WriteLine(WriteUtility.InsertTable('}', IndentSize));
                            break;
                        case E_Property_Type.Quick:
                            streamWriter.WriteLine(WriteUtility.InsertTable($"{Access}{(Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers))} {ReturnType} {PropertyName} => {DefaultValue};", IndentSize));
                            break;
                        case E_Property_Type.Auto:
                            string modifiers = Modifiers == null ? string.Empty : ' ' + string.Join(' ', Modifiers);
                            string body = $" {ReturnType} {PropertyName} {{ {GetGetString()} {GetSetString()} }}";
                            string defaultValue = string.IsNullOrEmpty(DefaultValue) ? string.Empty : $" = {DefaultValue};";

                            streamWriter.WriteLine(WriteUtility.InsertTable(Access + modifiers + body + defaultValue, IndentSize));
                            break;
                    }
                }

                private string GetGetString()
                {
                    if (string.IsNullOrEmpty(Getter.GetterAccess))
                    {
                        return string.Empty;
                    }
                    else 
                    {
                        return (Getter.GetterAccess == Access ? string.Empty : Getter.GetterAccess + ' ') + "get;";
                    }
                }

                private string GetSetString()
                {
                    if (string.IsNullOrEmpty(Setter.SetterAccess))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return (Setter.SetterAccess == Access ? string.Empty : Setter.SetterAccess + ' ') + "set;";
                    }
                }
            }

            public readonly struct GetterHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string GetterAccess { get; }

                public string DefaultValue { get; }

                public string[] MethodStatement { get; }

                public string PropertyAccess { get; }

                public GetterHandle(int indentSize, string getterAccess, string propertyAccess)
                {
                    IndentSize = indentSize;
                    GetterAccess = getterAccess;
                    PropertyAccess = propertyAccess;
                    DefaultValue = string.Empty;
                    MethodStatement = Array.Empty<string>();
                }

                public GetterHandle(int indentSize, string getterAccess, string propertyAccess, string defaultValue, params string[] methodStatement)
                {
                    IndentSize = indentSize;
                    GetterAccess = getterAccess;
                    PropertyAccess = propertyAccess;
                    DefaultValue = defaultValue;
                    MethodStatement = methodStatement;
                }

                public void HandleWrite(StreamWriter streamWriter)
                {
                    streamWriter.WriteLine(WriteUtility.InsertTable(GetterAccess == PropertyAccess || string.IsNullOrEmpty(GetterAccess) ? 
                        "get" : $"{GetterAccess} get", IndentSize));

                    streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));

                    if(MethodStatement != null)
                    {
                        for(int i = 0; i < MethodStatement.Length; i++)
                        {
                            streamWriter.WriteLine(WriteUtility.InsertTable(MethodStatement[i], IndentSize + 1));
                        }
                    }

                    streamWriter.WriteLine(WriteUtility.InsertTable($"return {DefaultValue}", IndentSize + 1));
                    streamWriter.WriteLine(WriteUtility.InsertTable('}', IndentSize));
                }
            }


            public readonly struct SetterHandle : IWriteHandle
            {
                public int IndentSize { get; }

                public string PropertyAccess { get; }

                public string SetterAccess { get; }

                public string DefaultValue { get; }

                public string[] MethodStatement { get; }

                public SetterHandle(int indentSize, string setterAccess, string propertyAccess)
                {
                    IndentSize = indentSize;
                    PropertyAccess = propertyAccess;
                    SetterAccess = setterAccess;
                    DefaultValue = string.Empty;
                    MethodStatement = Array.Empty<string>();
                }

                public SetterHandle(int indentSize, string setterAccess, string propertyAccess, string defaultValue, params string[] methodStatement)
                {
                    IndentSize = indentSize;
                    SetterAccess = setterAccess;
                    PropertyAccess = propertyAccess;
                    DefaultValue = defaultValue;
                    MethodStatement = methodStatement;
                }

                public void HandleWrite(StreamWriter streamWriter)
                {
                    streamWriter.WriteLine(WriteUtility.InsertTable(SetterAccess == PropertyAccess || string.IsNullOrEmpty(SetterAccess) ?
                        "set" : $"{SetterAccess} set", IndentSize));

                    streamWriter.WriteLine(WriteUtility.InsertTable('{', IndentSize));

                    if (MethodStatement != null)
                    {
                        for (int i = 0; i < MethodStatement.Length; i++)
                        {
                            streamWriter.WriteLine(WriteUtility.InsertTable(MethodStatement[i], IndentSize + 1));
                        }
                    }

                    streamWriter.WriteLine(WriteUtility.InsertTable($"{DefaultValue} = value", IndentSize + 1));
                    streamWriter.WriteLine(WriteUtility.InsertTable('}', IndentSize));
                }
            }
        }
    }
}