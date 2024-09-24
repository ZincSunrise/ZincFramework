using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZincFramework.Events;
using ZincFramework.Writer.Exceptions;
using ZincFramework.Writer.Handle;



namespace ZincFramework
{
    namespace Writer
    {
        public class CSharpWriter
        {
            public static class Modifiers
            {
                public static string[] Virtual { get; } = new string[] { "virtual" };
                public static string[] Abstract { get; } = new string[] { "abstract" };
                public static string[] Override { get; } = new string[] { "override" };
                public static string[] Static { get; } = new string[] { "static" };
                public static string[] Aysnc { get; } = new string[] { "async" };
                public static string[] ReadOnly { get; } = new string[] { "readonly" };
                public static string[] ReadOnlyStatic { get; } = new string[] { "readonly", "static" };
            }

            public static class Accessors
            {
                public const string Public = "public";
                public const string Private = "private";
                public const string Protected = "protected";
                public const string Internal = "internal";
            }

            public StreamWriter StreamWriter => _streamWriter;

            public readonly StreamWriter _streamWriter;

            private static readonly string[] _defaultNamespaces = new string[3]
            {
                "System.Collections",
                "System.Collections.Generic",
                "UnityEngine"
            };


            private readonly StringBuilder _writeHelper = new StringBuilder(64);


            public CSharpWriter(StreamWriter streamWriter)
            {
                _streamWriter = streamWriter;
            }


            public IWriteHandle WriteNamespace(int spaceCount, params string[] namespaces)
            {              
                if (namespaces.Length == 0)
                {
                    namespaces = _defaultNamespaces;
                }

                NamespaceHandle namespaceHandle = new NamespaceHandle(namespaces, spaceCount);
                namespaceHandle.HandleWrite(_streamWriter);

                return namespaceHandle;
            }


            public IWriteHandle WriteNamespace(string[] namespaces, int spaceCount = 2)
            {
                if (namespaces.Length == 0)
                {
                    namespaces = _defaultNamespaces;
                }

                NamespaceHandle namespaceHandle = new NamespaceHandle(namespaces, spaceCount);
                namespaceHandle.HandleWrite(_streamWriter);

                return namespaceHandle;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="namespaces">多命名空间则填入xx.xx.xx</param>
            /// <param name="className"></param>
            /// <param name="classType"></param>
            /// <param name="access"></param>
            /// <param name="parent"></param>
            public IWriteHandle BeginWriteClass(int indentSize, string namespaces, string[] modifiers, string className, string classType = "class", string access = Accessors.Public, string[] parents = null, AttributeHandle? attributeHandle = null)
            {
                if (access == "private")
                {
                    throw new InvaildModifierException("写入一个类的时候不能把访问修饰符写成private");
                }

                ClassHandle classHandle = new ClassHandle(indentSize, className, modifiers, classType, parents, namespaces, access, attributeHandle);
                classHandle.HandleWrite(_streamWriter);
                return classHandle;
            }


            public void EndWriteClass(int indentSize, bool isEndNamespace)
            {
                _streamWriter.WriteLine(WriteUtility.InsertTable('}', indentSize));

                if (isEndNamespace)
                {
                    _streamWriter.WriteLine('}');
                }
            }


            public IWriteHandle WriteAttribute(int indentSize, string attributeType, params string[] arguments)
            {
                AttributeHandle attributeHandle = new AttributeHandle( indentSize, attributeType, arguments);
                attributeHandle.HandleWrite(_streamWriter);

                return attributeHandle;
            }


            public IWriteHandle WriteAttribute(IWriteHandle writeHandle, string attributeType, params string[] arguments)
            {
                AttributeHandle attributeHandle = new AttributeHandle(writeHandle, attributeType, arguments);
                attributeHandle.HandleWrite(_streamWriter);

                return attributeHandle;
            }

            public IWriteHandle WriteField(int indentSize, ReadOnlySpan<char> fieldName, string fieldType, string[] modifiers, bool isInitalized, string access = Accessors.Public)
            {
                FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, modifiers, isInitalized);
                fieldHandle.HandleWrite(_streamWriter);

                return fieldHandle;
            }


            public IWriteHandle WriteField(int indentSize, string fieldName, string fieldType, string access = Accessors.Public)
            {
                FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, null, false);
                fieldHandle.HandleWrite(_streamWriter);

                return fieldHandle;
            }

            public IWriteHandle WriteField(int indentSize, ReadOnlySpan<char> fieldName, string fieldType, string access = Accessors.Public)
            {
                FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, null, false);
                fieldHandle.HandleWrite(_streamWriter);

                return fieldHandle;
            }

            public IWriteHandle WriteGenericField(int indentSize, string fieldName, string fieldType, string[] genericTypes, string access = Accessors.Public, string[] modifiers = null, bool isInitialize = true ,params string[] defaultValue)
            {
                GenericFieldHandle genericFieldHandle = new GenericFieldHandle(indentSize, access, modifiers, fieldType, genericTypes, fieldName, isInitialize, defaultValue);
                genericFieldHandle.HandleWrite(_streamWriter);
                return genericFieldHandle;
            }



            public IWriteHandle WriteArray(int indentSize, string fieldName, string elementType, string[] modifiers = null, string access = Accessors.Public)
            {
                ArrayHandle arrayHandle = new ArrayHandle(indentSize, access, modifiers, fieldName, elementType);
                arrayHandle.HandleWrite(_streamWriter);

                return arrayHandle;
            }

            public IWriteHandle WriteArray(int indentSize, string fieldName, string elementType, string defaultCapacity , string[] modifiers = null, string access = Accessors.Public)
            {
                ArrayHandle arrayHandle = new ArrayHandle(indentSize, access, modifiers, fieldName, elementType, defaultCapacity);
                arrayHandle.HandleWrite(_streamWriter);

                return arrayHandle;
            }

            public IWriteHandle WriteQuickProperty(int indentSize, string returnType, string propertyName, string defaultValue, string[] modifiers = null, string access = Accessors.Public)
            {
                PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, defaultValue);
                propertyHandle.HandleWrite( _streamWriter);

                return propertyHandle;
            }

            public IWriteHandle WriteConstructor(int indentSize, string type, IEnumerable<string> methodStatements, string access = Accessors.Public, params string[] arguments)
            {
                ConstructorHandle constructorHandle = new ConstructorHandle(indentSize, type, arguments, access, methodStatements);
                constructorHandle.HandleWrite(StreamWriter);

                return constructorHandle;
            }

            public MethodHandle WriteMethod(int indentSize, string methodName, string returnType, string[] modifiers, string[] arguments, IEnumerable<string> methodStatements, ZincAction writeCallback = null, string access = Accessors.Public)
            {
                MethodHandle methodHandle = new MethodHandle(indentSize, access, returnType, methodName, modifiers, arguments, methodStatements, writeCallback);
                methodHandle.HandleWrite(StreamWriter);

                return methodHandle;
            }


            public IWriteHandle WriteAutoProperty(int indentSize, string returnType, string propertyName, string getAccess, string setAccess, string[] modifiers = null, string access = Accessors.Public, string defaultValue = null)
            {
                GetterHandle getterHandle = new GetterHandle(indentSize, getAccess, access);
                SetterHandle setterHandle = new SetterHandle(indentSize, setAccess, access);

                PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, getterHandle, setterHandle, defaultValue);
                propertyHandle.HandleWrite(StreamWriter);

                return propertyHandle;
            }


            public IWriteHandle WriteProperty(int indentSize, string returnType, string propertyName, string defaultValue, string[] getterStatement, string[] setterStatement, string[] modifiers = null, string access = Accessors.Public, string getAccess = Accessors.Public, string setAccess = Accessors.Public)
            {
                GetterHandle getterHandle = new GetterHandle(indentSize, getAccess, access, defaultValue, getterStatement);
                SetterHandle setterHandle = new SetterHandle(indentSize, setAccess, access, defaultValue, setterStatement);

                PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, defaultValue, getterHandle, setterHandle);
                propertyHandle.HandleWrite(StreamWriter);

                return propertyHandle;
            }


            public void WriteBlock(int indentSize, string blockTitle, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                _streamWriter.WriteLine(WriteUtility.InsertTable(blockTitle, indentSize));
                _streamWriter.WriteLine(WriteUtility.InsertTable('{', indentSize));

                foreach (var str in statements)
                {
                    _streamWriter.WriteLine(WriteUtility.InsertTable(str, indentSize + 1));
                }

                writeInnerCallback?.Invoke();
                _streamWriter.WriteLine(WriteUtility.InsertTable('}', indentSize));
            }

            public void WriteArrayLoop(int indentSize, string arrayName, string lengthType, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                WriteLoop(indentSize, $"{arrayName}.{lengthType}", statements, writeInnerCallback);
            }

            public void WriteLoop(int indentSize, string lengthName, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                WriteBlock(indentSize, $"for(int i = 0;i < {lengthName}; i++)", statements, writeInnerCallback);
            }

            public void WriteForeach(int indentSize, string containerName, string itemName, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                WriteBlock(indentSize, $"foreach(var {itemName} in {containerName})", statements, writeInnerCallback);
            }

            public void WriteCondition(int indentSize, string condition, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                WriteBlock(indentSize, $"if({condition})", statements, writeInnerCallback);
            }

            public void WriteElse(int indentSize, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
            {
                WriteBlock(indentSize, "else", statements, writeInnerCallback);
            }



            public void WriteLine(int repeatCount = 1)
            {
                for (int i = 0; i < repeatCount; i++) 
                {
                    _streamWriter.WriteLine();
                }
            }

            public void WriteLine(int indentSize, string content)
            {
                _streamWriter.WriteLine(WriteUtility.InsertTable(content, indentSize));
            }

            public void WriteLines(int indentSize, IEnumerable<string> contents)
            {
                foreach(string line in contents)
                {
                    _streamWriter.WriteLine(WriteUtility.InsertTable(line, indentSize));
                }
            }
        }
    }
}

