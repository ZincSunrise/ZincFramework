using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using ZincFramework.Pools;
using ZincFramework.Events;
using ZincFramework.ScriptWriter.Exceptions;
using ZincFramework.ScriptWriter.Handle;



namespace ZincFramework.ScriptWriter
{
    public partial class CSharpWriter : IReuseable
    {
        public static CSharpWriter RentWriter()
        {
            return DataPoolManager.RentInfo(() => new CSharpWriter(new StringBuilder()));
        }

        private static readonly string[] _defaultNamespaces = new string[3]
        {
            "System.Collections",
            "System.Collections.Generic",
            "UnityEngine"
        };

        private CSharpWriter(StringBuilder writeHelper)
        {
            _writeHelper = writeHelper;
        }

        private readonly StringBuilder _writeHelper;

        public IWriteHandle WriteNamespace(int spaceCount, params string[] namespaces)
        {
            if (namespaces.Length == 0)
            {
                namespaces = _defaultNamespaces;
            }

            NamespaceHandle namespaceHandle = new NamespaceHandle(namespaces, spaceCount);
            namespaceHandle.HandleWrite(_writeHelper);

            return namespaceHandle;
        }


        public IWriteHandle WriteNamespace(string[] namespaces, int spaceCount = 2)
        {
            if (namespaces.Length == 0)
            {
                namespaces = _defaultNamespaces;
            }

            NamespaceHandle namespaceHandle = new NamespaceHandle(namespaces, spaceCount);
            namespaceHandle.HandleWrite(_writeHelper);

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
            classHandle.HandleWrite(_writeHelper);
            return classHandle;
        }


        public void EndWriteClass(int indentSize, bool isEndNamespace)
        {
            _writeHelper.InsertWriteLine(indentSize, '}');

            if (isEndNamespace)
            {
                _writeHelper.AppendLine("}");
            }
        }


        public IWriteHandle WriteAttribute(int indentSize, string attributeType, params string[] arguments)
        {
            AttributeHandle attributeHandle = new AttributeHandle(indentSize, attributeType, arguments);
            attributeHandle.HandleWrite(_writeHelper);

            return attributeHandle;
        }


        public IWriteHandle WriteAttribute(IWriteHandle writeHandle, string attributeType, params string[] arguments)
        {
            AttributeHandle attributeHandle = new AttributeHandle(writeHandle, attributeType, arguments);
            attributeHandle.HandleWrite(_writeHelper);

            return attributeHandle;
        }

        public IWriteHandle WriteField(int indentSize, ReadOnlySpan<char> fieldName, string fieldType, string[] modifiers, bool isInitalized, string access = Accessors.Public)
        {
            FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, modifiers, isInitalized);
            fieldHandle.HandleWrite(_writeHelper);

            return fieldHandle;
        }


        public IWriteHandle WriteField(int indentSize, string fieldName, string fieldType, string access = Accessors.Public)
        {
            FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, null, false);
            fieldHandle.HandleWrite(_writeHelper);

            return fieldHandle;
        }

        public IWriteHandle WriteField(int indentSize, ReadOnlySpan<char> fieldName, string fieldType, string access = Accessors.Public)
        {
            FieldHandle fieldHandle = new FieldHandle(indentSize, access, fieldName, fieldType, null, false);
            fieldHandle.HandleWrite(_writeHelper);

            return fieldHandle;
        }

        public IWriteHandle WriteGenericField(int indentSize, string fieldName, string fieldType, string[] genericTypes, string access = Accessors.Public, string[] modifiers = null, bool isInitialize = true, params string[] defaultValue)
        {
            GenericFieldHandle genericFieldHandle = new GenericFieldHandle(indentSize, access, modifiers, fieldType, genericTypes, fieldName, isInitialize, defaultValue);
            genericFieldHandle.HandleWrite(_writeHelper);
            return genericFieldHandle;
        }



        public IWriteHandle WriteArray(int indentSize, string fieldName, string elementType, string[] modifiers = null, string access = Accessors.Public)
        {
            ArrayHandle arrayHandle = new ArrayHandle(indentSize, access, modifiers, fieldName, elementType);
            arrayHandle.HandleWrite(_writeHelper);

            return arrayHandle;
        }

        public IWriteHandle WriteArray(int indentSize, string fieldName, string elementType, string defaultCapacity, string[] modifiers = null, string access = Accessors.Public)
        {
            ArrayHandle arrayHandle = new ArrayHandle(indentSize, access, modifiers, fieldName, elementType, defaultCapacity);
            arrayHandle.HandleWrite(_writeHelper);

            return arrayHandle;
        }

        public IWriteHandle WriteQuickProperty(int indentSize, string returnType, string propertyName, string defaultValue, string[] modifiers = null, string access = Accessors.Public)
        {
            PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, defaultValue);
            propertyHandle.HandleWrite(_writeHelper);

            return propertyHandle;
        }

        public IWriteHandle WriteConstructor(int indentSize, string type, IEnumerable<string> methodStatements, string access = Accessors.Public, params string[] arguments)
        {
            ConstructorHandle constructorHandle = new ConstructorHandle(indentSize, type, arguments, access, methodStatements);
            constructorHandle.HandleWrite(_writeHelper);

            return constructorHandle;
        }

        public MethodHandle WriteMethod(int indentSize, string methodName, string returnType, string[] modifiers, string[] arguments, IEnumerable<string> methodStatements, ZincAction writeCallback = null, string access = Accessors.Public)
        {
            MethodHandle methodHandle = new MethodHandle(indentSize, access, returnType, methodName, modifiers, arguments, methodStatements, writeCallback);
            methodHandle.HandleWrite(_writeHelper);

            return methodHandle;
        }


        public IWriteHandle WriteAutoProperty(int indentSize, string returnType, string propertyName, string getAccess, string setAccess, string[] modifiers = null, string access = Accessors.Public, string defaultValue = null)
        {
            GetterHandle getterHandle = new GetterHandle(indentSize, getAccess, access);
            SetterHandle setterHandle = new SetterHandle(indentSize, setAccess, access);

            PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, getterHandle, setterHandle, defaultValue);
            propertyHandle.HandleWrite(_writeHelper);

            return propertyHandle;
        }


        public IWriteHandle WriteProperty(int indentSize, string returnType, string propertyName, string defaultValue, string[] getterStatement, string[] setterStatement, string[] modifiers = null, string access = Accessors.Public, string getAccess = Accessors.Public, string setAccess = Accessors.Public)
        {
            GetterHandle getterHandle = new GetterHandle(indentSize, getAccess, access, defaultValue, getterStatement);
            SetterHandle setterHandle = new SetterHandle(indentSize, setAccess, access, defaultValue, setterStatement);

            PropertyHandle propertyHandle = new PropertyHandle(indentSize, access, modifiers, returnType, propertyName, defaultValue, getterHandle, setterHandle);
            propertyHandle.HandleWrite(_writeHelper);

            return propertyHandle;
        }


        public void WriteBlock(int indentSize, string blockTitle, IEnumerable<string> statements, ZincAction writeInnerCallback = null)
        {
            _writeHelper.InsertWriteLine(indentSize, blockTitle);
            _writeHelper.InsertWriteLine(indentSize, '{');

            foreach (var str in statements)
            {
                _writeHelper.InsertWriteLine(indentSize + 1, str);
            }

            writeInnerCallback?.Invoke();
            _writeHelper.InsertWriteLine(indentSize, '}');
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
                _writeHelper.AppendLine();
            }
        }

        public void WriteLine(int indentSize, string content)
        {
            _writeHelper.InsertWriteLine(indentSize, content);
        }

        public void WriteLines(int indentSize, IEnumerable<string> contents)
        {
            foreach (string line in contents)
            {
                _writeHelper.InsertWriteLine(indentSize, line);
            }
        }

        public string[] ReadAllLines()
        {
            return _writeHelper.ToString().Split(Environment.NewLine);
        }

        public void Clear()
        {
            _writeHelper.Clear();
        }

        public void OnReturn()
        {
            Clear();
        }

        public void OnRent()
        {

        }

        public void Return()
        {
            DataPoolManager.ReturnInfo(this);
        }

        public void WriteAndReturn(Stream stream)
        {
            using TextWriter writer = new StreamWriter(stream);
            writer.Write(_writeHelper);
            DataPoolManager.ReturnInfo(this);
        }

        public void WriteAndClear(Stream stream)
        {
            using TextWriter writer = new StreamWriter(stream);
            writer.Write(_writeHelper);
            Clear();
        }
    }
}

